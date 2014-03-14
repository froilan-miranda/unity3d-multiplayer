using UnityEngine;
using System.Collections;

public class SceneExplorationController : MonoBehaviour {
	/*************************************************
	 **	the following are the sections of the practice:
	 **	0) start with a lonely virion
	 **	1) show copy 1 with the loneliest virion
	 **  2) exit scene
	**************************************************/

	private int currentSection;
	private VirionManagerController virionManageScript;
	private InputController inputScript;
	private GameObject copy1;
	private Hashtable ht_alphaUp = new Hashtable();
	private Hashtable ht_scaleUp = new Hashtable();
	private Hashtable ht_alphaDown = new Hashtable();

	public void Awake()
	{
		Vector3 scaleTo = new Vector3(2.32f, 1f ,1.764635f);
		ht_scaleUp.Add("scale", scaleTo);
		ht_scaleUp.Add("time", 0.5);
		ht_scaleUp.Add("delay", 0.5);

		ht_alphaUp.Add("alpha", 1.0);
		ht_alphaUp.Add("time", 0.5);
		ht_alphaUp.Add("delay", 0.5);

		ht_alphaDown.Add("alpha", 0.0);
		ht_alphaDown.Add("time", 0.25);
		//ht_alphaDown.Add("oncomplete", "ExitLevel");
		//ht_alphaDown.Add("oncompletetarget", gameObject);
	}

	// Use this for initialization
	void Start () {
		currentSection = 0;
		copy1 = GameObject.Find("Copy 01");
		ExtensionMethods.SetAlpha(copy1.renderer.material, 0.0f);

		virionManageScript = GameObject.Find("Virion Manager").GetComponent<VirionManagerController>();
		inputScript = GameObject.Find("Input Manager").GetComponent<InputController>();
		InitExplore();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void InitExplore()
	{
		virionManageScript.GoHighlander();
		inputScript.ReticleVisible = false;
		GameObject.Find("Cannon Manager").GetComponent<CannonManagerController>().EnableAllCanons(false);
	}

	internal void AdvanceExplore()
	{
		Debug.Log("advance explore");
		currentSection++;
		switch(currentSection){
			case 0:
				Debug.Log("start with a lonely virion, this case should never hit");
				break;
			case 1:
				Debug.Log("show copy 1 with the loneliest virion");
				InitCase1();
				break;
			case 2:
				Debug.Log("exit scene");
				InitCase2();
				break;
			case 3:
				ExitLevel();
				break;
			default:
				Debug.Log("something has slipped through");
				break;
		}
	}

	private void InitCase1()
	{
		iTween.FadeTo(copy1, ht_alphaUp);
		iTween.ScaleTo(copy1, ht_scaleUp);
		virionManageScript.ExploreCase01();
	}

	private void InitCase2()
	{
		iTween.FadeTo(copy1, ht_alphaDown);
		virionManageScript.ExploreCase02();
		InputController inputScript = GameObject.Find("Input Manager").GetComponent<InputController>();
		inputScript.ShowVersion2 = true;
		inputScript.ReticleVisible = true;
	}

	private void ExitLevel()
	{
		Application.LoadLevel(6);
	}

}//class
