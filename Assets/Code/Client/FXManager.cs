using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

	public GameObject SecondaryBullet_Basic;
	public AudioClip SecondaryBulletAudio_Basic;
	public float SecondaryBullet_Basic_Speed = 1f;

	[RPC]
	void FX_SecondaryBullet( Vector3 startPos, Vector3 endPos ){
		
		AudioSource.PlayClipAtPoint(SecondaryBulletAudio_Basic, startPos, Random.Range(0.5f, 1f));
		GameObject Bullet = Instantiate(SecondaryBullet_Basic, startPos, Quaternion.LookRotation( startPos - endPos )) as GameObject;
		LineRenderer Line = Bullet.GetComponentInChildren<LineRenderer>();
		Line.SetPosition(0, startPos);
		Line.SetPosition(1, endPos);
		StartCoroutine(MoveObject(Bullet, startPos, endPos, SecondaryBullet_Basic_Speed));
	}
	

	
	
	IEnumerator MoveObject(GameObject ThisOBJ, Vector3 startPos, Vector3 endPos, float overTime)
	{
		float startTime = Time.time;
		while(Time.time < startTime + overTime)
		{
			ThisOBJ.transform.position = Vector3.Lerp(startPos, endPos, (Time.time - startTime)/overTime);
			yield return null;
		}
		transform.position = endPos;
	}

}
