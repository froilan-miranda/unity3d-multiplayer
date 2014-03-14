using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{	
	public Texture2D ovalMask;
	private Rect maskPos;
	private NetworkController  netScript; 
	private InputController inputScript;
	private BgAudioController audioScript;
	private uint _gameTime;

	public void Start()
	{

		Screen.showCursor = false;
		netScript = gameObject.GetComponent<NetworkController>();
		inputScript = GameObject.Find("Input Manager").GetComponent<InputController>();
		audioScript = gameObject.GetComponent<BgAudioController>();
		DontDestroyOnLoad(gameObject);	//Keep this object persistant
		maskPos = new Rect(0,0,1920,1080);
	}

	internal void InitGame()
	{
		netScript.ConnectGameServer();
		inputScript.InitInput();
		audioScript.Volume(gameObject.GetComponent<SettingsController>().GetSetting("volume"));
		PlayerManagerController.RankBase =(uint) gameObject.GetComponent<SettingsController>().GetSetting("rankBase");
		Debug.Log("rank base:" +  PlayerManagerController.RankBase );
		//Grab and set player number from settings to object
		//oPP.SetPlayerNumber(settingsScript.GetSetting("playerNumber"));

		//Load Next Level
		Debug.Log("Ready to exit level");
		Application.LoadLevel(1);
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))Application.Quit();
	}

	public void OnGUI()
	{
		GUI.depth = 0;
		GUI.DrawTexture(maskPos, ovalMask);
	}

	internal void OnReset()
	{
		Destroy(GameObject.Find("Virion Manager"));
		inputScript.ReticleVisible = false;
		GameObject.Find("Cannon Manager").GetComponent<CannonManagerController>().EnableAllCanons(false);
		Application.LoadLevel(1);
	}

	internal uint GameTime
	{
		set{_gameTime = value;}
		get{return _gameTime;}
	}
}//class