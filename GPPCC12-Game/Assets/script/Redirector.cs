using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Redirector : MonoBehaviour
{
	[SerializeField] private string scene;
	// Use this for initialization
	void Start () {
		SceneManager.LoadScene(scene);
	}


}
