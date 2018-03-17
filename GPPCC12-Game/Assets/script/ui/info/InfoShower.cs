using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoShower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private GameObject baseGui;

    private List<GameObject> selectedUnits = new List<GameObject>();
    private List<GameObject> hits = new List<GameObject>();

    [SerializeField]
    public Image selectionBoxImage;

    private Vector2 startPosition;
    private Rect selectionRect;

    // Handles what needs to be selected
    void checkHits(GameObject hit)
    {
        foreach (GameObject unit in selectedUnits)
        {
            Renderer renderer = unit.GetComponent<Renderer>();
            renderer.material.shader = Shader.Find("Diffuse");
        }

        selectedUnits.Clear();
        hits.Add(hit);

        foreach (GameObject hittedGo in hits)
        {
            UiReferrer referrer = hittedGo.GetComponent<UiReferrer>();
            if (referrer == null)
            {
                if (baseGui != null)
                {
                    baseGui.SetActive(false);
                }

                foreach (GameObject unit in selectedUnits)
                {
                    Renderer renderer = unit.GetComponent<Renderer>();
                    renderer.material.shader = Shader.Find("Diffuse");
                }

                selectedUnits.Clear();
                return;
            }

            if (referrer.type == UiReferrer.StructureType.Base)
            {
                foreach (GameObject unit in selectedUnits)
                {
                    Renderer renderer = unit.GetComponent<Renderer>();
                    renderer.material.shader = Shader.Find("Diffuse");
                }

                selectedUnits.Clear();
                baseGui = referrer.canvasGo;
                baseGui.SetActive(true);
            }
            else if (referrer.type == UiReferrer.StructureType.HealingUnit ||
                     referrer.type == UiReferrer.StructureType.TankUnit ||
                     referrer.type == UiReferrer.StructureType.DpsUnit)
            {
                Renderer renderer = hittedGo.GetComponent<Renderer>();
                renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");

                selectedUnits.Add(hittedGo);


                Debug.Log(selectedUnits);
            }

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        selectionBoxImage.gameObject.SetActive(true);
        startPosition = eventData.position;
        selectionRect = new Rect();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x < startPosition.x)
        {
            selectionRect.xMin = eventData.position.x;
            selectionRect.xMax = startPosition.x;
        }
        else
        {
            selectionRect.xMin = startPosition.x;
            selectionRect.xMax = eventData.position.x;
        }

        if (eventData.position.y < startPosition.y)
        {
            selectionRect.yMin = eventData.position.y;
            selectionRect.yMax = startPosition.y;
        }
        else
        {
            selectionRect.yMin = startPosition.y;
            selectionRect.yMax = eventData.position.y;
        }

        selectionBoxImage.rectTransform.offsetMin = selectionRect.min;
        selectionBoxImage.rectTransform.offsetMax = selectionRect.max;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBoxImage.gameObject.SetActive(false);
        // TODO: Handle Multiselect (1st hits.Clear(), 2nd foreach hit in selectionRect{checkhits(hit)})
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        float myDistance = 0;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject)
            {
                myDistance = result.distance;
                break;
            }
        }

        GameObject nextObject = null;
        float maxDistance = Mathf.Infinity;

        foreach (RaycastResult result in results)
        {
            if (result.distance > myDistance && result.distance < maxDistance)
            {
                nextObject = result.gameObject;
                maxDistance = result.distance;
            }
        }

        if (nextObject)
        {
            ExecuteEvents.Execute<IPointerClickHandler>(nextObject, eventData, (x, y) => { x.OnPointerClick((PointerEventData)y);});
            // TODO: Handle Leftclick (If leftclick => clear hits + checkHits(), if Leftclick + Control => only checkHits to add new hit possible hit to hits)
        }
    }
}