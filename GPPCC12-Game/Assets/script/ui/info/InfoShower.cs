using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoShower : MonoBehaviour
{
    private GameObject baseGui;

    private int fingerID = -1;

    private List<GameObject> selectedUnits = new List<GameObject>();

    private List<GameObject> hits = new List<GameObject>();

    private void Awake()
    {
        #if !UNITY_EDITOR
            fingerID = 0; 
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LeftMouse"))
        {
            checkHits();
        }
    }

    void OnMouseDrag()
    {
        checkHits();
        Debug.Log(selectedUnits);
    }

    void checkHits()
    {
        Debug.Log("Try to hit");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        {
            if (Physics.Raycast(ray, out hit, 100.0f) && !EventSystem.current.IsPointerOverGameObject(fingerID))
            {
                hits.Clear();
                selectedUnits.Clear();
                hits.Add(hit.collider.gameObject);
                if (baseGui != null) baseGui.SetActive(false);

                foreach (GameObject hittedGo in hits)
                {
                    UiReferrer referrer = hittedGo.GetComponent<UiReferrer>();
                    if (referrer == null)
                    {
                        if (baseGui != null) baseGui.SetActive(false);
                        return;
                    }

                    if (referrer.type == UiReferrer.StructureType.Base)
                    {
                        baseGui = referrer.canvasGo;
                        baseGui.SetActive(true);
                    }
                    else if (referrer.type == UiReferrer.StructureType.HealingUnit ||
                             referrer.type == UiReferrer.StructureType.TankUnit ||
                             referrer.type == UiReferrer.StructureType.DpsUnit)
                    {
                        selectedUnits.Add(hittedGo);
                        Debug.Log(selectedUnits);
                    }
                }
            }
        }
    }
}