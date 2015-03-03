using UnityEngine;
using System.Collections;

public class ClientObject : MonoBehaviour {

	private int serverPort = 9955;
	
	public GameObject PlayerObject;
	public GameObject NetworkedPlayerObject;
	
	public void OnGUI(){
		if (Network.peerType == NetworkPeerType.Disconnected){
			if (GUI.Button(new Rect(100,100,100,30), "Connect to server")){
				Network.Connect("79.177.130.207", serverPort);
			}
		}
		
		if (Network.peerType == NetworkPeerType.Client){
			//GUI.Label(new Rect(100,100,300,30), "Connected to server");
			Application.LoadLevel(1);
		}
	}
}
