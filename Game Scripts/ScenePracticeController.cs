using UnityEngine;
using System.Collections;

public class ScenePracticeController : MonoBehaviour {
	/*************************************************
	 **	the following are the sections of the practice:
	 **	0)enable reticle
	 **	1)show and split 3 with reticle
	 **	2)enable cannons
	 **  3) exit scene
	**************************************************/

	private int currentSection;
	private VirionManagerController virionManageScript;
	private InputController inputScript;

	// Use this for initialization
	public void Start ()
	{
		currentSection = 0;
		virionManageScript = GameObject.Find("Virion Manager").GetComponent<VirionManagerController>();
		inputScript = GameObject.Find("Input Manager").GetComponent<InputController>();
		inputScript.ReticleVisible = true;
	}
	
	// Update is called once per frame
	public void Update ()
	{

	}

	internal void AdvancePractice()
	{
		Debug.Log("advance practice");
		currentSection++;
		switch(currentSection){
			case 0:
				Debug.Log("enable reticle");
				break;
			case 1:
				Debug.Log("show and split 3 with reticle");
				InitCase1();
				break;
			case 2:
				Debug.Log("enable cannons");
				InitCase2();
				break;
			case 3:
				Debug.Log("exit scene");
				InitCase3();
				break;
			default:
				Debug.Log("something has slipped through");
				break;
		}
	}

	private void InitCase1()
	{
		Vector3 newCamPos = new Vector3(0, 0, -10);
		iTween.MoveTo(Camera.main.gameObject, newCamPos, 1.0f);
		//Camera.main.transform.position = newCamPos;
		virionManageScript.PracCase1();
	}

	private void InitCase2()
	{
		//enable cannons from cannon controller
		GameObject.Find("Cannon Manager").GetComponent<CannonManagerController>().EnableAllCanons(true);
	}

	private void InitCase3()
	{
		//enable cannons from cannon controller
		GameObject.Find("Cannon Manager").GetComponent<CannonManagerController>().EnableAllCanons(true);

		Application.LoadLevel(4);
	}
}//class
