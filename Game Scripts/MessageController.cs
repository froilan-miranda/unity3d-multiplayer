using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {

	private NetworkController netScript;
	private GameController gameScript;

	private SceneStandbyController standbyScript;
	private SceneIntroController introScript;
	private ScenePracticeController practiceScript;
	private SceneRound1Controller round1Script;
	private SceneExplorationController exploreScript;
	private SceneRound2Controller round2Script;
	private SceneScoreController scoreScript;
	private SceneRankController rankScript;
	private string[] messageQ;
	private string prefix;

	// Use this for initialization
	void Start ()
	{
		netScript = GetComponent<NetworkController>();
		gameScript = GetComponent<GameController>();
	}
	
	public void FixedUpdate ()
	{
		if(netScript.serverConnected == true)
		SetMessageQ();

	}
	
	// Update is called once per frame
	void Update () {

	}

	/************************************
	 **  code for sending information
	 *************************************/
	internal void SendClient(string message, string protocol)
	{
		if(protocol == "udp")
			netScript. SendClientUDP(prefix + message);
		else if (protocol == "tcp")
			netScript. SendClient(prefix + message);
	}

	internal void SendHost(string message, string protocol)
	{
		if(protocol == "udp")
			netScript.SendHostUDP(prefix + message);
		else if (protocol == "tcp")
			netScript.SendHost(prefix + message);
	}

	internal void SendLeaderboard(string message, string protocol)
	{
		if(protocol == "udp")
			netScript.SendLeaderboardUDP(prefix + message);
		else if (protocol == "tcp")
			netScript.SendLeaderboard(prefix + message);
	}

	internal void SendGlobal(string message, string protocol)
	{
		if(protocol == "udp")
			netScript.SendGlobalUDP(prefix + message);
		else if (protocol == "tcp")
			netScript.SendGlobal(prefix + message);
	}

	internal void InitComm(int playerNumber)
	{
		SetPrefix(playerNumber);
		SendHost("Initiated", "tcp");
	}

	internal void SetPrefix(int playerNumber)
	{
		prefix = "ROV|" + playerNumber + "|";
	}

	/************************************
	 **  code for receiving information
	 *************************************/

	internal void SetMessageQ()
	{
		messageQ = null;
		messageQ = netScript.GetMessages();
		ProccessMessageQ();
	}

	internal void ProccessMessageQ()
	{
		// iterate through the array
		foreach (string mess in messageQ) {
			ReceivedMessage(mess);
		}
	}

	internal IEnumerator OnLevelWasLoaded(int level)
	{
		Debug.Log("on level loaded");
		
		switch(level){
			case 0:
				// Load Start Up Scene Scripts
				break;
			case 1:
				// Load Splash Scene Scripts
				standbyScript = GameObject.Find("Scene Manager").GetComponent<SceneStandbyController>();
				break;
			case 2:
				//  Load Intro Scripts
				introScript = GameObject.Find("Scene Manager").GetComponent<SceneIntroController>();
				break;
			case 3:
				// Load pratice scripts
				practiceScript = GameObject.Find("Scene Manager").GetComponent<ScenePracticeController>(); 
				break;
			case 4:
				//Load round 1 scripts
				round1Script = GameObject.Find("Scene Manager").GetComponent<SceneRound1Controller>(); 
				break;
			case 5:
				//Load exploration scripts
				exploreScript = GameObject.Find("Scene Manager").GetComponent<SceneExplorationController>(); 
				break;
			case 6:
				//Load round 2 scripts
				round2Script = GameObject.Find("Scene Manager").GetComponent<SceneRound2Controller>(); 
				break;
			case 7:
				//Load score scripts;
				scoreScript = GameObject.Find("Scene Manager").GetComponent<SceneScoreController>(); 
				break;
			case 8:
				//Load rank scripts;
				rankScript = GameObject.Find("Scene Manager").GetComponent<SceneRankController>(); 
				break;
			default:
				Debug.Log("An unexpected level was loaded");
				break;
		}
		yield return null;
	}

	/********************************
	 ** Process Received Message
	  *******************************/
	internal void ReceivedMessage(string message)
	{
		string[] messSegs = message.Split("|"[0]);
		
	/* the following is to be executed, reguardless of game state (e.g. wait for next session to begin) */
		if((messSegs[0] == "RESET") || (messSegs[0] == "GO_ISI" && Application.loadedLevel  == 8)) {
			Debug.Log("received message to execute: " + messSegs[0]);
			//gameScript.ResetGame();
		} 

	/* the following will only be executed if client is actively playing */
		//EOG
		//SHOW_RANKING
		//SHOW_SCORES
		//START_ROUND_2
		//SHOW_CROSSHAIRS_2
		//SCAN_VIRION
		//SHOW_VIRION_2
		//START_ROUND_1
		//ENABLE_FIRING
		//DEFINE_PLAYERS
		//SHOW_CROSSHAIRS_1
		//SHOW_FACTOID_3
		//SHOW_FACTOID_2
		//SHOW_VIRION
		//START|10
		//INITIALIZE|P1|HHH
		Debug.Log("received message to execute: " + messSegs[0]);
		switch(messSegs[0]){
			case "INITIALIZE":
				InitPlayer(messSegs[1], messSegs[2]);
				break;
			case "CANCEL_INITIAL":
				CancelPlayer(messSegs[1]);
				break;
			case "START" :
				gameScript.GameTime = uint.Parse(messSegs[1]);
				Debug.Log("here is my uint" + uint.Parse(messSegs[1]));
				standbyScript.HostSentStart();
				break;
			case "SHOW_VIRION":
				introScript.AdvanceIntro();
				break;
			case "SHOW_FACTOID_2":
				introScript.AdvanceIntro();
				break;
			case "SHOW_FACTOID_3":
				introScript.AdvanceIntro();
				break;
			case "SHOW_MUTANT":
				introScript.AdvanceIntro();
				break;
			case "SHOW_CROSSHAIRS_1":
				introScript.AdvanceIntro();
				break;
			case "DEFINE_PLAYERS" :
				practiceScript.AdvancePractice();
				break;
			case "ENABLE_FIRING" :
				practiceScript.AdvancePractice();
				break;
			case "START_ROUND_1" :
				practiceScript.AdvancePractice();
				break;
			case "SHOW_VIRION_2" :
				round1Script.EndScene();
				break;
			case "SCAN_VIRION" :
				exploreScript.AdvanceExplore();
				break;
			case "SHOW_CROSSHAIRS_2" :
				exploreScript.AdvanceExplore();
				break;
			case "START_ROUND_2" :
				exploreScript.AdvanceExplore();
				break;
			case "SHOW_SCORES" :
				round2Script.EndScene();
				break;
			case "SHOW_RANKING" :
				scoreScript.EndScene();
				break;
			case "EOG" :
				rankScript.EndScene();
				break;
			case "RESET" :
				gameScript.OnReset();
				break;
			default :
				Debug.Log("Unrecognized value: " + messSegs[0]);
				break;
		}//switch
	}

	private void InitPlayer(string pos, string name)
	{
		uint posNumber = (uint)pos[1];	//remember this returns a 'char' not a 'string'
		posNumber -= 48;	//The -48 is because 0 is 48 (0x0030) in Unicode value and you need to subtract that value to get the integer representation. 
		PlayerManagerController.InitPlayer(posNumber, name);
	}
	private void CancelPlayer(string pos)
	{
		uint posNumber = (uint)pos[1];	//remember this returns a 'char' not a 'string'
		posNumber -= 48;	//The -48 is because 0 is 48 (0x0030) in Unicode value and you need to subtract that value to get the integer representation. 
		PlayerManagerController.CancelPlayer(posNumber);
	}

}//class
