using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hitPoints = 100;
	float currentHitPoints;
	PlayerStats PlayerStats;
	bool _hasStats = false;
	bool _hasHealthBar = false;
	
	public GameObject healthBar;


	// Use this for initialization
	void Start () {
		if(gameObject.GetComponent<PlayerStats>()){
			PlayerStats = gameObject.GetComponent<PlayerStats>();
			_hasStats = true;
			currentHitPoints = PlayerStats.HealthMax;
			
			if (healthBar != null){
				_hasHealthBar = true;
			}
		} else {
			currentHitPoints = hitPoints;
			_hasStats = false;
		}
	}
	
	
	[RPC]
	public void TakeDamage(float damage){
		if (_hasStats){
			PlayerStats.Health = PlayerStats.Health - damage;
			currentHitPoints = PlayerStats.Health;
			
			if (_hasHealthBar){
				float healthScaleCalc = PlayerStats.Health / PlayerStats.HealthMax * 1f;
				healthBar.transform.localScale = new Vector3(healthScaleCalc, 1, 1);
			}
		} else {
			currentHitPoints -= damage;
		}
		
		if (currentHitPoints <= 0){
			Die();
		}
	}
	
	void Die(){
		
		if(GetComponent<PhotonView>().instantiationId == 0){ 
			Destroy(gameObject); 
		} else {
			if(GetComponent<PhotonView>() && GetComponent<PhotonView>().isMine) 
				PhotonNetwork.Destroy(gameObject);
		}
		
		if (this.tag == "TestPlayer"){
			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.Instantiate("TestPlayer", new Vector3(10, 10, 0), Quaternion.identity, 0);
			}
		}
		
		
		//FOR TESTING
		if (this.name == "Crate" || this.name == "Crate(Clone)"){
			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.Instantiate("Crate", new Vector3(10, 10, 0), Quaternion.identity, 0);
			}
		}
	}
}
