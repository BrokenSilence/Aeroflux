using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public PlayerStats PlayerStats;

	float backforth, strafe;
	
	//public GameObject CrosshairContainer;
	//Animator Crosshair;
		
	void Update() {

		backforth = Input.GetAxis("Vertical") * PlayerStats.PlayerSpeed * Time.deltaTime;
		strafe = Input.GetAxis("Horizontal") * PlayerStats.PlayerSpeed * Time.deltaTime;
		
		//Crosshair = CrosshairContainer.GetComponentInChildren<Animator>();

	}
	
	void FixedUpdate(){
		transform.Translate(strafe, 0, backforth, Space.Self);
	}
}
