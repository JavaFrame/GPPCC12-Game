using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoShower : MonoBehaviour
{
    private GameObject baseGui;

    private List<GameObject> selectedUnits = new List<GameObject>();

    private List<GameObject> hits = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LeftMouse"))
        {
            if (Input.GetButton("Control"))
            {
                checkHits();
            } else
            {
                foreach (GameObject unit in selectedUnits)
                {
                    Renderer renderer = unit.GetComponent<Renderer>();
                    renderer.material.shader = Shader.Find("Diffuse");
                }

                selectedUnits.Clear();
                hits.Clear();
                checkHits();
            }
        }
    }

    void checkHits()
    {
        Debug.Log("Try to hit");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        {
            if (Physics.Raycast(ray, out hit, 100.0f) && !EventSystem.current.IsPointerOverGameObject())
            {
                hits.Add(hit.collider.gameObject);

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
                        selectedUnits.Add(hittedGo);

                        foreach (GameObject unit in selectedUnits)
                        {
                            if (baseGui != null)
                            {
                                baseGui.SetActive(false);
                            }

                            Renderer renderer = unit.GetComponent<Renderer>();
                            renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                        }
                        Debug.Log(selectedUnits);
                    }
                }
            }
        }
    }
}