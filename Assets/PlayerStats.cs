using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public float EnergyMax = 100;
	public float Energy = 100;
	public float EnergyRechargeRate = 0.5f;
	public float Health = 100;
	public float HealthMax = 100;
	public float PlayerSpeed = 15;
	public bool _isUsingEnergy = false;
	
	
	public GameObject EnergyBar;
	public TextMesh EnergyText;
	
	void Update(){
		RechargeEnergy();
		CheckEnergyMinus();
	}
	
	void CheckEnergyMinus(){
		if (Energy < 0){
			Energy = 0;
		}
	}
	
	void RechargeEnergy(){
		if (!_isUsingEnergy){
			if (Energy < EnergyMax){
				Energy += EnergyRechargeRate;
				EnergyText.text = Mathf.Round(Energy).ToString();
				float EnergyScaleCalc = Energy / EnergyMax;
				EnergyBar.transform.localScale = new Vector3(EnergyScaleCalc, 1, 1);
			}
		}
	}
	
	
}
