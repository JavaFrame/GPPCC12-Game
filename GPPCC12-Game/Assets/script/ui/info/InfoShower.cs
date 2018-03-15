using UnityEngine;
using UnityEngine.EventSystems;

public class InfoShower : MonoBehaviour
{

    private GameObject baseGui;

    private int fingerID = -1;

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
            Debug.Log("Try to hit");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            {
                if (Physics.Raycast(ray, out hit, 100.0f) && !EventSystem.current.IsPointerOverGameObject(fingerID))
                {
                    GameObject hittedGo = hit.collider.gameObject;
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
                }
            }
        }
    }
}