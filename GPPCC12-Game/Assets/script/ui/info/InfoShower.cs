using UnityEngine;

public class InfoShower : MonoBehaviour {

    private GameObject baseGui;

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("LeftMouse"))
        {
            Debug.Log("Try to hit");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
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
