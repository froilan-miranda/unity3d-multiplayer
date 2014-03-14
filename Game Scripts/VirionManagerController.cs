using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirionManagerController : MonoBehaviour {

	private GameObject virion1;
	private GameObject virion2;
	private GameObject virion3;
	private GameObject virion4;
	private VirionController virion1Script;
	private VirionController virion2Script;
	private VirionController virion3Script;
	private VirionController virion4Script;

	public GameObject virionPrefab;
	private float maxVirions; // = 100;
	public List<GameObject> list_Wild = new List<GameObject>();
	public List<GameObject> list_Mutant = new List<GameObject>();
	private float  repeatDelay = 0.0f;
	private float repeatTime = 0.75f;

	private GameObject hitObjP1;
	private GameObject hitObjP2;
	private GameObject hitObjP3;

	void Awake()
	{
		maxVirions = GameObject.Find("Game Manager").GetComponent<SettingsController>().GetSetting("maxVirions");
	}
	// Use this for initialization
	void Start () 
	{
		IntroInit();
		VirionController.onVirionDeath += RemoveVirion;
		TimerController.onEndRound += RoundOver;
	}

	// Update is called once per frame
	void Update () {
	}

	internal void MobilizeStationary()
	{
		Debug.Log("mobilize the minions");
		if(virion1) virion1Script.Stationary = false; 
		if(virion2)virion2Script.Stationary = false; 
		if(virion3)virion3Script.Stationary = false; 
	}

	internal void ReEnableLifeSpan()
	{
		//Debug.Log("life span back on");
		if(virion1)virion1Script.SuspendLife(false);
		if(virion2)virion2Script.SuspendLife(false); 
		if(virion3)virion3Script.SuspendLife(false); 
	}
	internal void initReplication()
	{
		InvokeRepeating("CheckReplication", repeatDelay, repeatTime);
	}

	private void CheckReplication()
	{
		//Debug.Log("checking replication");
		//check total count
		int vTotal = list_Wild.Count + list_Mutant.Count;
		if(vTotal < maxVirions){
			//grab random virion to replicate it. Change state and appropriate properties/methods
			GameObject toBeReplicated = list_Wild[Random.Range(0, list_Wild.Count - 1)];
			toBeReplicated.GetComponent<VirionController>().ChangeState("replicating");
			//check population diversity
			//create appropriate virion
			GameObject newGuy = (GameObject)Instantiate(virionPrefab, toBeReplicated.transform.position, Quaternion.identity );
			newGuy.transform.parent = gameObject.transform;
			if( list_Mutant.Count < Mathf.Floor((vTotal +1) / 10)){
				//time for a mutant
				newGuy.GetComponent<VirionController>().VType = "mutant";
				list_Mutant.Add(newGuy);
			}else{
				//time for a wild
				newGuy.GetComponent<VirionController>().VType = "wild";
				list_Wild.Add(newGuy);
			}
		}
	}

	internal void ToggleHitCounters(uint player, GameObject hitObject)
	{
		//Debug.Log("was raycasted");
		GameObject oldObj = null;
		switch(player){
			case 1:
				if(hitObject != hitObjP1){
					oldObj = hitObjP1;
					hitObjP1 = hitObject;
					//show hitcounter
					if(hitObjP1 != null){
						if(hitObjP1.tag == "virion")
							hitObjP1.GetComponent<VirionController>().ShowCounter();
					}
				}
				break;
			case 2:
				if(hitObject != hitObjP2){
					oldObj = hitObjP2;
					hitObjP2 = hitObject;
					//show hitcounter
					if(hitObjP2 != null){
						if(hitObjP2.tag == "virion")
							hitObjP2.GetComponent<VirionController>().ShowCounter();
					}
				}
				break;
			case 3:
				if(hitObject != hitObjP3){
					oldObj = hitObjP3;
					hitObjP3 = hitObject;
					//show hitcounter
					if(hitObjP3 != null){
						if(hitObjP3.tag == "virion")
							hitObjP3.GetComponent<VirionController>().ShowCounter();
					}
				}
				break;
			default:
				Debug.Log("invalid value in ToggleHitCounters: " + player);
				break;
		}
		if(oldObj != null){
			if(oldObj.tag == "virion")
				if(oldObj != hitObjP1 && oldObj != hitObjP2 && oldObj != hitObjP3)
					oldObj.GetComponent<VirionController>().HideCounter();
		}
	}

	private void StopReplication()
	{
		CancelInvoke("CheckReplication");
	}

	private void RoundOver()
	{
		//stop replication
		StopReplication();

		//pause life span
		gameObject.BroadcastMessage("SuspendLife", true);

		//enable cannons from cannon controller
		GameObject.Find("Cannon Manager").GetComponent<CannonManagerController>().EnableAllCanons(false);
	}

	internal void GoHighlander()
	{
		//clear out mutant list and destroy objects
		foreach(GameObject go in list_Mutant) go.GetComponent<VirionController>().ChangeState("end");
		//list_Mutant.Clear();
		//list_Mutant.TrimExcess();


		//there can only be one...wild left
		int remanderWild = list_Wild.Count;
		while(remanderWild > 1){
			GameObject virion = list_Wild[remanderWild - 1];
			virion.GetComponent<VirionController>().ChangeState("end");
			remanderWild--;
		}
		GameObject highlander = list_Wild[0];
		//highlander.SetActiveRecursively(false);
		highlander.GetComponent<VirionController>().Stationary = true;
		highlander.rigidbody.isKinematic = true;
		//highlander.SetActiveRecursively(true);
		iTween.MoveTo(highlander, new Vector3(0,0,0), 1);
		//highlander.transform.position = new Vector3(0,0,0);
		highlander.rigidbody.isKinematic = false;

		//reset var to the last remaining virion
		virion1 = highlander;
		virion1Script = highlander.GetComponent<VirionController>();
		virion1.collider.isTrigger = false;
		virion1Script.SuspendLife(true);
	}

	internal void RemoveAll()
	{
		foreach(GameObject go in list_Wild) go.GetComponent<VirionController>().ChangeState("end");
		foreach(GameObject go in list_Mutant) go.GetComponent<VirionController>().ChangeState("end");
	}
	private void RemoveVirion(GameObject g, string type)
	{
		//Debug.Log("remove");
		if(type == "wild"){
			int index = list_Wild.IndexOf(g);
			list_Wild.RemoveAt(index);
			//iTween.FadeTo(g.transform.Find("Wild").gameObject,iTween.Hash("alpha", 0, "time", 0.25, "oncomplete", "DestroyVirion", "oncompletetarget", gameObject, "oncompleteparams", g ));
			Destroy(g);
			//Debug.Log("remove " + index);
		}else if(type == "mutant"){
			int index = list_Mutant.IndexOf(g);
			Debug.Log(index + " of " + list_Mutant.Count);
			list_Mutant.RemoveAt(index);
			//iTween.FadeTo(g.transform.Find("Mutant").gameObject,iTween.Hash("alpha", 0, "time", 0.25, "oncomplete", "DestroyVirion", "oncompletetarget", gameObject, "oncompleteparams", g ));
			Destroy(g);
			//Debug.Log("remove " + index);
		}else{
			Debug.Log("illegal virioin state " + g.GetComponent<VirionController>().VState);
		}
	}
	internal void DestroyVirion(GameObject g)
	{
		Destroy(g);
	}
	
	/************************************************************************
	 ************************************************************************
	 **	Canned methods to get through
	 **	intro and practice scenes
	 ************************************************************************
	 ************************************************************************/

	internal void IntroInit()
	{
		virion1 = GameObject.Find("Virion 1");
		virion1Script = virion1.GetComponent<VirionController>();
		virion1Script.VType = "wild";
		list_Wild.Insert(0,virion1);
		virion1Script.Stationary = true; 
		virion1Script.SuspendLife(true);

		virion2 = GameObject.Find("Virion 2");
		virion2Script = virion2.GetComponent<VirionController>();
		virion2Script.VType = "wild";
		list_Wild.Insert(1, virion2);
		virion2Script.Stationary = true;
		virion2Script.SuspendLife(true);
		virion2Script.HideVirion();


		virion3 = GameObject.Find("Virion 3");
		virion3Script = virion3.GetComponent<VirionController>();
		virion3Script.VType = "wild";
		list_Wild.Insert(2, virion3);
		virion3Script.Stationary = true;
		virion3Script.SuspendLife(true);
		virion3Script.HideVirion();

		virion4 = GameObject.Find("Mutant 1");
		virion4Script = virion4.GetComponent<VirionController>();
		virion4Script.VType = "mutant";
		list_Mutant.Insert(0, virion4);
		virion4Script.Stationary = true;
		virion4Script.SuspendLife(true);
		Debug.Log("init : " + virion4 + " " + virion4Script);
	}

	internal void IntroCase1()
	{
		//virion1.SetActiveRecursively(true);
		//virion1Script.ShowVirion();
		Vector3 newPos = new Vector3(1.5f, 0.0f, 0.0f);
		iTween.MoveTo(virion1, newPos, 1.0f);
	}

	internal void PracCase1()
	{
		virion1.transform.position = new Vector3(0, 0, 0);

		virion2Script.ShowVirion();
		virion2.transform.position = new Vector3(-2, 0, 0);

		virion3Script.ShowVirion();
		virion3.transform.position = new Vector3(2, 0, 0);

		// remove the mutant. it is outside the playable area
		virion4Script.ChangeState("end");
	}
	internal void ExploreCase01()
	{
		Vector3 newPos = new Vector3(-4.5f, 0.0f, -4.0f);
		iTween.MoveTo(virion1, newPos, 0.5f);
	}
	internal void ExploreCase02()
	{
		Vector3 newPos = new Vector3(0.0f, 0.0f, 0.0f);
		iTween.MoveTo(virion1, newPos, 0.5f);
	}
	public void OnDestroy()
	{
		VirionController.onVirionDeath -= RemoveVirion;
		TimerController.onEndRound -= RoundOver;
	}
}//class
