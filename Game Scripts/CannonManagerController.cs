using UnityEngine;
using System.Collections;

public class CannonManagerController : MonoBehaviour {

	public GameObject CannonP1;
	public GameObject CannonP2;
	public GameObject CannonP3;

	private float distancePoint = 10f;
	
	// Use this for initialization
	void Start () {
		CannonP1.GetComponent<CannonController>().Owner = 1;
		CannonP2.GetComponent<CannonController>().Owner = 2;
		CannonP3.GetComponent<CannonController>().Owner = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void RequestFire(int whichPlayer, Ray sightLine)
	{
		Vector3 target = sightLine.GetPoint(distancePoint);
		switch(whichPlayer){
			case 1:
				//fire player 1 cannon
				CannonP1.GetComponent<CannonController>().FireOn(target);
				break;
			case 2:
				//fire player 2 cannon
				CannonP2.GetComponent<CannonController>().FireOn(target);
				break;
			case 3:
				//fire player 3 cannon
				CannonP3.GetComponent<CannonController>().FireOn(target);
				break;
			default:
				//something has fallen through
				break;
		}
	}

	internal void EnableAllCanons(bool toggle)
	{
		CannonP1.GetComponent<CannonController>().PlayerControl = toggle;
		CannonP2.GetComponent<CannonController>().PlayerControl = toggle;
		CannonP3.GetComponent<CannonController>().PlayerControl = toggle;
	}
}//class
