using UnityEngine;
using System.Collections;

public class NetworkedPlayer : MonoBehaviour {

	/** PLAYER INDEX ON NETWORK */
	[HideInInspector] public int PlayerIndex;
	
	void Update(){
		if (_PlayerInitCompleted){ PlayerInitComplete(); }
	}
	
	bool _PlayerInitCompleted = false;
	int PlayerInitIndex;
	Vector3 PlayerPosition;
	
	public void UpdatePlayerInitCompleted(int index, Vector3 position){
		PlayerInitIndex = index;
		PlayerPosition = position;
		_PlayerInitCompleted = true;
	}
	
	void PlayerInitComplete(){
		PlayerIndex = PlayerInitIndex;
		transform.position = PlayerPosition;
		_PlayerInitCompleted = false;
	}
	
}
