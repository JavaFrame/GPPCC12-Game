using UnityEngine;

public class InfoShower : MonoBehaviour {


    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("LeftMouse"))
        {
            Debug.Log("Try to hit");
            RaycastHit hit;
            Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                GameObject hittedGo = hit.collider.gameObject;
                UiReferrer referrer = hittedGo.GetComponent<UiReferrer>();
                if (referrer == null) return;

                if (referrer.type == UiReferrer.StructureType.Base)
                {
                    GameObject go = referrer.canvasGo;
                    go.SetActive(true);
                } else
                {
                    GameObject go = referrer.canvasGo;
                    go.SetActive(false);
                }
            }
        }
    }
}
