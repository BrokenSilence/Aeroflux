using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main){
			transform.LookAt(Camera.main.transform.position, -Vector3.up); 
		}
	}
}
