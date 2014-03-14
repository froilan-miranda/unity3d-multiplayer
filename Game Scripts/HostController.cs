using UnityEngine;
using System.Collections;

public class HostController : MonoBehaviour {

	private SceneIntroController introScript;
	private ScenePracticeController practiceScript;
	private SceneRound1Controller round1Script;
	private SceneExplorationController exploreScript;
	private SceneRound2Controller round2Script;
	private SceneScoreController scoreScript;
	private SceneRankController rankScript;
   private int ScreenShotInt = 1;

	// Use this for initialization
	public void Start ()
	{
		if (Application.loadedLevel == 2)
			introScript = GameObject.Find("Scene Manager").GetComponent<SceneIntroController>();

		if (Application.loadedLevel == 3)
			practiceScript = GameObject.Find("Scene Manager").GetComponent<ScenePracticeController>();

		if(Application.loadedLevel == 4)
			round1Script = GameObject.Find("Scene Manager").GetComponent<SceneRound1Controller>();

		if(Application.loadedLevel == 5)
			exploreScript = GameObject.Find("Scene Manager").GetComponent<SceneExplorationController>();

		if(Application.loadedLevel == 6)
			round2Script = GameObject.Find("Scene Manager").GetComponent<SceneRound2Controller>();

		if(Application.loadedLevel == 7)
			scoreScript = GameObject.Find("Scene Manager").GetComponent<SceneScoreController>();

		if(Application.loadedLevel == 8)
			rankScript = GameObject.Find("Scene Manager").GetComponent<SceneRankController>();
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if(Input.GetKeyDown(KeyCode.N)){
			switch(Application.loadedLevel){
				case 1:
					Application.LoadLevel(2);
					break;
				case 2:
					introScript.AdvanceIntro();
					break;
				case 3:
					practiceScript.AdvancePractice();
					break;
				case 4:
					round1Script.EndScene();
					break;
				case 5:
					exploreScript.AdvanceExplore();
					break;
				case 6:
					round2Script.EndScene();
					break;
				case 7:
					scoreScript.EndScene();
					break;
				case 8:
					rankScript.EndScene();
					break;				
			}
		}
		if(Input.GetKeyDown(KeyCode.S)){
			 Application.CaptureScreenshot("Screenshot" + ScreenShotInt + ".png");
			ScreenShotInt++;
		}
	}
}//class
