using UnityEngine;
using System.Collections;

public class SceneRound2Controller : MonoBehaviour {

	private VirionManagerController virionManageScript;
	private InputController inputScript;

	// Use this for initialization
	void Start () 
	{
		virionManageScript = GameObject.Find("Virion Manager").GetComponent<VirionManagerController>();
		//inputScript = GameObject.Find("Input Manager").GetComponent<InputController>();
		InitRound2();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitRound2()
	{
		virionManageScript.MobilizeStationary();
		virionManageScript.ReEnableLifeSpan();
		virionManageScript.initReplication();
		//enable cannons from cannon controller
		GameObject.Find("Cannon Manager").GetComponent<CannonManagerController>().EnableAllCanons(true);
	}

	internal void EndScene()
	{
		Application.LoadLevel(7);
	}
}
