using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {

	PlayerStats PlayerStats;
	
	public GameObject CrosshairContainer;
	Animator Crosshair;
	Transform CrosshairDot;
	
	public float FireRate = 0.1f;
	float GunCooldown;	
	public float GunDamage = 25f;
	bool _shooting = false;
	float ShootingTime = 0f;
	PhotonView fxManager;
	public float maxRange = 100f;
	
	bool _canShoot = true;
	bool _shootReset = false;
	
	public GameObject EnergyBar;
	public TextMesh EnergyText;
	
	float GunEnergyDiminish = 2f;
	
	
	void Start(){
		Crosshair = CrosshairContainer.GetComponentInChildren<Animator>();
		CrosshairDot = Crosshair.transform.FindChild("Crosshair_Secondary_Dot") as Transform;
		fxManager = GameObject.FindObjectOfType<FXManager>().GetComponent<PhotonView>();
		PlayerStats = gameObject.GetComponent<PlayerStats>();
	}
	
	
	
	// Update is called once per frame
	void Update () {
		GunCooldown -= Time.deltaTime;
		
		if(Input.GetKeyDown(Controls.Fire)){
			_shootReset = false;	
		}
	
		if(Input.GetKey(Controls.Fire) && _canShoot && !_shootReset){
			Fire();	
		}
		if(Input.GetKeyUp(Controls.Fire) || _shootReset){
			Crosshair.SetFloat("Shooting", 0);
			_shooting = false;
			PlayerStats._isUsingEnergy = false;
		}
		
		CheckIfTargetingDestructable();
		

		if (PlayerStats.Energy > 0){
			_canShoot = true;
		}
	}
	
	
	
	
	//SETS RED CROSSHAIR WHEN PLAYER IS OVER A HITTABLE PLAYER
	void CheckIfTargetingDestructable(){
		Ray StaticGunRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		Transform StaticGunRayHit;
		Vector3 StaticGunRayHitPoint;
		StaticGunRayHit = FirstHit(StaticGunRay, out StaticGunRayHitPoint);
		if (StaticGunRayHit != null){
			
			Health h = StaticGunRayHit.transform.GetComponent<Health>();
			
			while (h == null && StaticGunRayHit.parent){
				StaticGunRayHit = StaticGunRayHit.parent;
				h = StaticGunRayHit.transform.GetComponent<Health>();
			}
			
			if (h != null){
				CrosshairDot.gameObject.renderer.material.color = Color.red;
			} else {
				CrosshairDot.gameObject.renderer.material.color = Color.white;
			}
			
		}
	}
	
	
	//FIRES
	void Fire(){
		//PLAYER IS SHOOTING
		_shooting = true;
		
		//IF ON COOLDOWN, DONT SHOOT
		if (GunCooldown > 0){
			return;
		}
		
		//USES ENERGY
		DepleteEnergy();
		
		//CROSSHAIR SPLIT
		ShootingTime += 0.1f;
		
		//SET CROSSHAIR SPLIT FLOAT
		Crosshair.SetFloat("Shooting", ShootingTime);
		
		
		//SHOOT RAYCAST AND CHECK IF HIT SOMETHING
		Ray GunRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		Transform GunRayHit;
		Vector3 GunRayHitPoint;
		
		GunRayHit = FirstHit(GunRay, out GunRayHitPoint);
		
		if (GunRayHit != null){			
			Health h = GunRayHit.transform.GetComponent<Health>();
			while (h == null && GunRayHit.parent){
				GunRayHit = GunRayHit.parent;
				h = GunRayHit.transform.GetComponent<Health>();
			}
			if (h != null){
				h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, GunDamage);
			}
			fxManager.RPC("FX_SecondaryBullet", PhotonTargets.All, transform.position, GunRayHitPoint);
		} else {
			GunRayHitPoint = transform.position + (transform.forward * maxRange);
			fxManager.RPC("FX_SecondaryBullet", PhotonTargets.All, transform.position, GunRayHitPoint);
		}

		GunCooldown = FireRate;
	}
	
	Transform FirstHit(Ray ray, out Vector3 GunRayHitPoint){
		RaycastHit[] hits = Physics.RaycastAll(ray);
		Transform closestHit = null;
		float distance = 0;
		GunRayHitPoint = Vector3.zero;
		
		foreach (RaycastHit hit in hits){
			if (hit.transform != this.transform && ( closestHit == null || hit.distance < distance)){
				//Hit something, not us, first thing, or is closer than previous closest thing
				
				closestHit = hit.transform;
				distance = hit.distance;
				GunRayHitPoint = hit.point;
			}
		}
		
		return closestHit;
	}
	

	void DepleteEnergy(){
		if (PlayerStats.Energy > 0 + GunEnergyDiminish){
			PlayerStats._isUsingEnergy = true;
			PlayerStats.Energy = PlayerStats.Energy - GunEnergyDiminish;
			EnergyText.text = Mathf.Round(PlayerStats.Energy).ToString();
			float EnergyScaleCalc = PlayerStats.Energy / PlayerStats.EnergyMax;
			EnergyBar.transform.localScale = new Vector3(EnergyScaleCalc, 1, 1);
		} else {
			_shootReset = true;
			_canShoot = false;
		}
	}
	
}
