using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class T_Replicate : MonoBehaviour {

	public GameObject virionPrefab;
	private int maxVirions = 100;
	public List<GameObject> list_Wild = new List<GameObject>();
	public List<GameObject> list_Mutant = new List<GameObject>();
	private float  repeatDelay = 0.0f;
	private float repeatTime = 1.0f;

	// Use this for initialization
	void Start () 
	{
		InitVirions();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void InitVirions()
	{
		GameObject instance = (GameObject)Instantiate(virionPrefab, gameObject.transform.position, Quaternion.identity );
		instance.transform.parent = gameObject.transform;
		instance.GetComponent<VirionController>().Stationary = true;
		instance.GetComponent<VirionController>().VType = "wild";
		list_Wild.Add(instance);

		initReplication();
		VirionController.onVirionDeath += RemoveVirion;
	}

	private void initReplication()
	{
		InvokeRepeating("CheckReplication", repeatDelay, repeatTime);
	}

	private void CheckReplication()
	{
		Debug.Log("checking replication");
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

	private void RemoveVirion(GameObject g, string type)
	{
		Debug.Log("here to remove");
		if(type == "wild"){
			int index = list_Wild.IndexOf(g);
			list_Wild.RemoveAt(index);
			Destroy(g);
			//Debug.Log("remove " + index);
		}else if(type == "mutant"){
			int index = list_Mutant.IndexOf(g);
			list_Mutant.RemoveAt(index);
			Destroy(g);
			//Debug.Log("remove " + index);
		}else{
			Debug.Log("virioin state " + g.GetComponent<VirionController>().VState);
		}
	}
}//class
