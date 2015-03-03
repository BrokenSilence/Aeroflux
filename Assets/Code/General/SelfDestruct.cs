using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	public float selfDestructTime = 1.0f;
	PhotonView pv;
	
	void Start(){
		pv = GetComponent<PhotonView>();
	}
	

	void Update () {

		selfDestructTime -= Time.deltaTime;
		
		
		
		if (selfDestructTime <= 0){
			if (pv != null && pv.instantiationId != 0){
				PhotonNetwork.Destroy(gameObject);
			} else {
				Destroy (gameObject);
			}
		}
			
	}
	

}
