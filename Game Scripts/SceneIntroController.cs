using UnityEngine;
using System.Collections;

public class SceneIntroController : MonoBehaviour {

	/*****************************************************
	 **	the following are the sections of the introduction:
	 **	0) copy 1
	 **	1)remove copy1 show wild virion
	 **	2)show wild virion with copy 2
	 **	3)wild virion with copy 3
	 ** 	4) show mutant
	 **	5) remove mutant
	 ** 	6)exit scene
	******************************************************/

	private int currentSection;
	private GameObject copy1;
	private GameObject copy2;
	private GameObject copy3;
	private GameObject virionManager;
	private VirionManagerController virionManagerScript;
	private Hashtable ht_alphaUp = new Hashtable();
	private Hashtable ht_alphaDown = new Hashtable();

	public void Awake()
	{
		ht_alphaUp.Add("alpha", 1.0);
		ht_alphaUp.Add("time", 0.5);
		ht_alphaUp.Add("delay", 0.5);

		ht_alphaDown.Add("alpha", 0.0);
		ht_alphaDown.Add("time", 0.25);
	}
	// Use this for initialization
	public void Start()
	{
		currentSection = 0;
		copy1 = GameObject.Find("Copy 01/pPlane1");
		ExtensionMethods.SetAlpha(copy1.renderer.material, 0.0f);
		iTween.FadeTo(copy1, ht_alphaUp);

		copy2 = GameObject.Find("Copy 02/pPlane1");
		//copy2.SetActiveRecursively(false);
		ExtensionMethods.SetAlpha(copy2.renderer.material, 0.0f);
		//iTween.FadeTo(copy2, ht_alphaUp);

		copy3 = GameObject.Find("Copy 03/pPlane1");
		//copy3.SetActiveRecursively(false);
		ExtensionMethods.SetAlpha(copy3.renderer.material, 0.0f);
		//iTween.FadeTo(copy3, ht_alphaUp);

		virionManager = GameObject.Find("Virion Manager");
		DontDestroyOnLoad(virionManager);
		virionManagerScript = virionManager.GetComponent<VirionManagerController>();
	}

	// Update is called once per frame
	public void Update()
	{
		
	}

	internal void AdvanceIntro()
	{
		Debug.Log("advance intro");
		currentSection++;
		switch(currentSection){
			case 0:
				Debug.Log("copy 1, this case should not hit");
				break;
			case 1:
				Debug.Log("remove copy1 show wild virion");
				InitCase1();
				break;
			case 2:
				Debug.Log("show wild virion with copy 2");
				InitCase2();
				break;
			case 3:
				Debug.Log("wild virion with copy 3");
				InitCase3();
				break;
			case 4:
				Debug.Log("show mutant");
				InitCase4();
				break;
			case 5:
				Debug.Log("show nothing");
				InitCase5();
				break;
			case 6:
				Debug.Log("exit scene");	//this is now being handled by the tween
				//InitCase6();
				break;
			default:
				Debug.Log("something has slipped through");
				break;
		}
	}

	private void InitCase1()
	{
		//copy1.SetActiveRecursively(false);
		iTween.FadeTo(copy1, ht_alphaDown);
		virionManagerScript.IntroCase1();
	}

	private void InitCase2()
	{
		//copy2.SetActiveRecursively(true);
		iTween.FadeTo(copy2, ht_alphaUp);
	}

	private void InitCase3()
	{
		//copy2.SetActiveRecursively(false);
		//copy3.SetActiveRecursively(true);
		iTween.FadeTo(copy2, ht_alphaDown);
		iTween.FadeTo(copy3, ht_alphaUp);
	}

	private void InitCase4()
	{
		//copy3.SetActiveRecursively(false);
		iTween.FadeTo(copy3, ht_alphaDown);
		Vector3 newCamPos = new Vector3(0, 16, -10);
		iTween.MoveTo(Camera.main.gameObject, newCamPos, 1.0f);
		//Camera.main.transform.position = newCamPos;
	}

	private void InitCase5()
	{
		//Vector3 newCamPos = new Vector3(0, 8, -10);
		//iTween.MoveTo(Camera.main.gameObject, newCamPos, 1.0f);
		iTween.MoveTo(Camera.main.gameObject,iTween.Hash("y",8,"time",1,"oncomplete","EndScene", "oncompletetarget", gameObject));
		//Camera.main.transform.position = newCamPos;
	}
	private void EndScene()
	{
		Application.LoadLevel(3);
	}

}//class
