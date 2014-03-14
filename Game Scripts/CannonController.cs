using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour {

	public Rigidbody bullet;
	public Rigidbody bulletV2;
	private uint _owner;
	private float coolDownTime = 0.25f;
	private bool weaponHot;
	private bool _playerControl = false;

	// Use this for initialization
	void Start () {
		weaponHot = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void FireOn(Vector3 target)
	{
		if(PlayerManagerController.IsPlayerActive(_owner)){
			if(weaponHot && _playerControl){
				weaponHot = false;
				
				Rigidbody clone;
				if(Application.loadedLevel != 6){
					clone = (Rigidbody)Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
				}else{
					clone = (Rigidbody)Instantiate(bulletV2, gameObject.transform.position, Quaternion.identity);
				}
				Vector3 heading = target - gameObject.transform.position;
				float distance = heading.magnitude;
				Vector3 direction = heading / distance;  // This is now the normalized direction.
				clone.gameObject.GetComponent<BulletController>().SetDirection(direction, target);
				clone.gameObject.GetComponent<BulletController>().Owner = _owner;
				audio.Play();
				Invoke("CoolDown", coolDownTime);
				//Debug.Log("cannon fired");
			}
		}
	}
	private void PracticeShot(uint player)
	{
		if(player == _owner)
			_playerControl = false;
	}
	internal bool PlayerControl
	{
		get{return _playerControl;}
		set{_playerControl = value;}
	}
	private void CoolDown()
	{
		weaponHot = true;
	}

	internal uint Owner
	{
		get { return _owner; }
		set { _owner = value; }
	}
}//class