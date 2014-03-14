using UnityEngine;
using System.Collections;

public class SceneRound1Controller : MonoBehaviour {
	
	private VirionManagerController virionManageScript;

	// Use this for initialization
	void Start () 
	{
		virionManageScript = GameObject.Find("Virion Manager").GetComponent<VirionManagerController>();
		InitRound1();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitRound1()
	{
		virionManageScript.MobilizeStationary();
		virionManageScript.ReEnableLifeSpan();
		virionManageScript.initReplication();
	}

	internal void EndScene()
	{
		Application.LoadLevel(5);
	}
}
