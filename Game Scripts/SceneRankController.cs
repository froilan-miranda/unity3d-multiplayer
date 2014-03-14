using UnityEngine;
using System.Collections;

public class SceneRankController : MonoBehaviour {

	public GUISkin gameSkin;
	public Texture2D totalRankBG;
	public Texture2D p1ScoreBG;
	public Texture2D p2ScoreBG;
	public Texture2D p3ScoreBG;
	public Texture2D p1MvpBG;
	public Texture2D p2MvpBG;
	public Texture2D p3MvpBG;
	public Texture2D ovalMask;
	//private Rect maskPos;

	// Use this for initialization
	void Start () 
	{
		//maskPos = new Rect(0,0,1920, 1080);
		InitRank();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void OnGUI()
	{
		GUI.skin = gameSkin;
		GUI.DrawTexture(new Rect(679, 0, 566, 538), totalRankBG);
		GUI.Label(new Rect (680, 300, 550, 300), PlayerManagerController.Rank+"%", "total rank");

		if(PlayerManagerController.IsPlayerActive(1)){
			if(PlayerManagerController.TopScore == 1){
				GUI.DrawTexture(new Rect(330, 550, 350, 350), p1MvpBG);
				GUI.Label(new Rect (400, 675, 200, 130), PlayerManagerController.GetName(1), "player initials");
			}else{
				GUI.DrawTexture(new Rect(330, 550, 350, 350), p1ScoreBG);
				GUI.Label(new Rect (400, 645, 200, 130), PlayerManagerController.GetName(1), "player initials");
			}
			GUI.Label(new Rect (400, 720, 200, 130), PlayerManagerController.GetScore(1).ToString(), "player points");
		}

		if(PlayerManagerController.IsPlayerActive(2)){
			if(PlayerManagerController.TopScore == 2){
				GUI.DrawTexture(new Rect(790, 625, 350, 350), p2MvpBG);
				GUI.Label(new Rect (860, 750, 200, 130), PlayerManagerController.GetName(2), "player initials");
			}else{
				GUI.DrawTexture(new Rect(790, 625, 350, 350), p2ScoreBG);
				GUI.Label(new Rect (860, 720, 200, 130), PlayerManagerController.GetName(2), "player initials");
			}
			GUI.Label(new Rect (860, 795, 200, 130), PlayerManagerController.GetScore(2).ToString(), "player points");
		}

		if(PlayerManagerController.IsPlayerActive(3)){
			if(PlayerManagerController.TopScore == 3){
				GUI.DrawTexture(new Rect(1315, 550, 350, 350), p3MvpBG);
				GUI.Label(new Rect (1385, 675, 200, 130), PlayerManagerController.GetName(3), "player initials");	
			}else{
				GUI.DrawTexture(new Rect(1315, 550, 350, 350), p3ScoreBG);
				GUI.Label(new Rect (1385, 645, 200, 130), PlayerManagerController.GetName(3), "player initials");	
			}
			GUI.Label(new Rect (1385, 720, 200, 130), PlayerManagerController.GetScore(3).ToString(), "player points");
		}
		//GUI.DrawTexture(maskPos, ovalMask);
	}

	private void InitRank()
	{

	}

	internal void EndScene()
	{
		GameObject.Find("Game Manager").GetComponent<GameController>().OnReset();;
	}
}
