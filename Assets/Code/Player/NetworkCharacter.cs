using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	public TextMesh PlayerName;

	void Start(){
		string PlayerUserName = this.GetComponent<PhotonView>().owner.name;
		this.GetComponentInChildren<TextMesh>().text = PlayerUserName;
	}

	void FixedUpdate(){
		if ( ! photonView.isMine ){
			transform.position = Vector3.Lerp(transform.localPosition, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
			//Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			//player
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			
			//other players
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			
		}
	}
}
