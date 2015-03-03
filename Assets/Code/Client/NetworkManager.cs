using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject StandbyCamera;
	
	public bool offlineMode = false;
	bool _connecting = false;

	// Use this for initialization
	void Start () {
		PhotonNetwork.player.name = PlayerPrefs.GetString ("Username", "n00blet");
	}
	

	void Connect () {
		if (offlineMode){
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		} else {
			PhotonNetwork.ConnectUsingSettings("FirstVersion");
		}
	}
	
	void OnDestroy(){
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}
	
	void OnGUI(){
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
		
		
		GUILayout.BeginArea( new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (PhotonNetwork.connected == false){
			GUILayout.Label("Username");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
		}
		GUILayout.EndHorizontal();
		
		//GUI SHIT
		if (PhotonNetwork.connected == false && _connecting == false){
			if ( GUILayout.Button("Connect") ){
				_connecting = true;
				Connect ();
			}
		}
		if (PhotonNetwork.connecting == true){
			if ( GUILayout.Button("Disconnect") ){
				PhotonNetwork.Disconnect();
				_connecting = false;
			}
			
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
	void OnJoinedLobby(){
		PhotonNetwork.JoinRoom("MainLobby", true);
	}
	
	void OnJoinedRoom(){
		SpawnMyPlayer();
	}
	
	void SpawnMyPlayer(){
		GameObject MyPlayerGameObject = PhotonNetwork.Instantiate("PlayerPrefab", Vector3.zero, Quaternion.identity, 0) as GameObject;
		StandbyCamera.SetActive(false);
		MyPlayerGameObject.GetComponent<Jetpack>().enabled = true;
		MyPlayerGameObject.GetComponent<Player>().enabled = true;
		MyPlayerGameObject.GetComponent<PlayerActions>().enabled = true;
		MyPlayerGameObject.transform.FindChild("Main Camera").gameObject.SetActive(true);
		MyPlayerGameObject.transform.FindChild("StatusBars").transform.FindChild("EnergyBarContainer").gameObject.SetActive(true);
		MyPlayerGameObject.transform.FindChild("StatusBars").transform.FindChild("HealthBarContainer").transform.localPosition = new Vector3(7.690732f, 4.327233f, 5.380153f);
		MyPlayerGameObject.transform.FindChild("StatusBars").transform.FindChild("HealthBarContainer").transform.localScale = new Vector3(0.4f, 0.4f, 0.5f);
		MyPlayerGameObject.transform.FindChild("PlayerName").gameObject.SetActive(false);
		MyPlayerGameObject.transform.FindChild("PlayerName").gameObject.GetComponent<TextMesh>().text = PhotonNetwork.player.name;
		
		//PLAYER ATTRIBUTES
	}
}
