using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoShower : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("LeftMouse"))
		{
			Debug.Log("Try to hit");
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.rotation.eulerAngles, out hit))
			{
				GameObject hittedGo = hit.collider.gameObject;
				UiReferrer referrer = hittedGo.GetComponent<UiReferrer>();
				if(referrer == null) return;
				GameObject go = referrer.canvasGo;
				Instantiate(go, gameObject.transform);
			}
		}
	}
}
