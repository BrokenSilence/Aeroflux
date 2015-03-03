using UnityEngine;
using System.Collections;

public class Jetpack : MonoBehaviour {
	
	public PlayerStats PlayerStats;
	
	public float JetPackFuelDiminish = 0.5f;
	public TextMesh JetpackFuelText;
	public GameObject EnergyBar;
	public TextMesh EnergyText;
	
	float normalPlayerSpeed;
	public float jetpackPlayerSpeed = 10;
	
	float jumpTimer = 0;
	float jumpFlyInterval = 0.45f;
	float jumpForce = 12;
	float jetpackForce = 20;
	
	bool _jetpackReset = false;
	bool _canJump;
	bool _isFlying;
	bool _canFly;
	

	
	//CHECK IF THE PLAYER IS ON THE GROUND
	bool IsGrounded(){
		return Physics.Raycast(transform.position, Vector3.down, 1f);
	}
	
	
	void Start(){
		EnergyText.text = Mathf.Round(PlayerStats.Energy).ToString();
		normalPlayerSpeed = PlayerStats.PlayerSpeed;
	}
	
	
	void Update(){
	
		
		if (Input.GetKeyDown(Controls.Jump)){
			_jetpackReset = false;
		}
		
		if (!_jetpackReset){
			//PRESS 'JUMP KEY' AND JUMP
			if (Input.GetKey(Controls.Jump) && _canJump){
				Jump();
				jumpTimer = Time.time;
				
			}
			
			//LETS THE PLAYER FLY IF HOLDING JUMP
			if (Input.GetKey(Controls.Jump) && !_canJump && !_canFly && Time.time > (jumpTimer + jumpFlyInterval)){
				_canFly = true;
			}
			
			
			//PRESS 'JUMP KEY' AND FLY
			if (Input.GetKey(Controls.Jump) && _canFly){
				StartJetpack();
			}
		}
			
		//WHEN THE PLAYER RELEASES THE 'JUMP KEY'
		if (Input.GetKeyUp(Controls.Jump) || _jetpackReset){
			StopJetpack();
		}
		
		
		//SET BOOLEANS IF THE PLAYER IS GROUNDED
		if (IsGrounded()){
			_canJump = true;
			_canFly = false;
			Physics.gravity = new Vector3(0, -20, 0);
		}	
	}
	
	
	
	void FixedUpdate(){
		if (_isFlying){
			UseJetpack();
		}
	}
	
	
	void Jump(){
		rigidbody.velocity = Vector3.up * jumpForce;
		_canJump = false;	
	}
	
	void StartJetpack(){
		_isFlying = true;
		Physics.gravity = new Vector3(0, -10, 0);
	}
	
	void UseJetpack(){
		if (rigidbody.velocity.y < -8){
			rigidbody.velocity = new Vector3(0, -8, 0);
		}
		rigidbody.AddForce(Vector3.up * jetpackForce);
		PlayerStats.PlayerSpeed = normalPlayerSpeed + jetpackPlayerSpeed;
		
		
		DepleteEnergy();
	}
	
	void StopJetpack(){
		_isFlying = false;
		if (rigidbody.velocity.y > 20){
			rigidbody.velocity = new Vector3(0, 20, 0);
		}
		Physics.gravity = new Vector3(0, -40, 0);
		PlayerStats.PlayerSpeed = normalPlayerSpeed;
		
	}
	
	void DepleteEnergy(){
		if (PlayerStats.Energy > 0 + JetPackFuelDiminish){
			PlayerStats._isUsingEnergy = true;
			PlayerStats.Energy = PlayerStats.Energy - JetPackFuelDiminish;
			EnergyText.text = Mathf.Round(PlayerStats.Energy).ToString();
			float EnergyScaleCalc = PlayerStats.Energy / PlayerStats.EnergyMax;
			EnergyBar.transform.localScale = new Vector3(EnergyScaleCalc, 1, 1);
		} else {
			PlayerStats._isUsingEnergy = false;
			_jetpackReset = true;
			_canFly = false;
			_isFlying = false;
		}
	}
	
}
