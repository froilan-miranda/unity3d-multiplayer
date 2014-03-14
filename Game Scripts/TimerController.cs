using UnityEngine;
using System.Collections;

public class TimerController : MonoBehaviour {

	internal delegate void onEndRoundEvent();
	internal static event onEndRoundEvent onEndRound;

	private GameController gameScript;
	public GUISkin gameSkin;
	private  uint seconds = 999;
	private string currentCount;
	public GameObject timeOut;
	//public Texture txtr_timesUp;
	//private textMesh : TextMesh;
	private Rect timerPosition;
	private bool showTime = false;

	public void Awake()
	{
		gameScript = GameObject.Find("Game Manager").GetComponent<GameController>();
		seconds = gameScript.GameTime;
	}

	public void Start () {
		//textMesh = GameObject.Find ("Timer").GetComponent(TextMesh);
		//textMesh.text = seconds.ToString();
		currentCount = seconds.ToString();
		timerPosition = new Rect(910, 30, 200,200);
		InvokeRepeating ("Countdown", 1.0f, 1.0f);
		iTween.MoveTo(gameObject, iTween.Hash("y", 0.265, "time", 0.75,  "oncomplete", "ShowTime", "oncompletetarget", gameObject));
	}

	void OnGUI() {
		GUI.skin = gameSkin;
		if(showTime){
			if(seconds < 10)
				GUI.Label(timerPosition, "00:0"+currentCount);
			else
				GUI.Label(timerPosition, "00:"+currentCount);
		}
		//if(seconds == 0)
			//GUI.DrawTexture(new Rect((Screen.width - txtr_timesUp.width)/2, (Screen.height - txtr_timesUp.height)/2, txtr_timesUp.width, txtr_timesUp.height), txtr_timesUp);
	}

	internal void ShowTime()
	{
		showTime = true;
	}
	private void ShowTimeOut()
	{
		iTween.MoveTo(timeOut, iTween.Hash("z", 0.0, "time", 2.0));
		GameObject.Find("Game Manager").GetComponent<BgAudioController>().InitAudioA();
	}
	public void Countdown () {
		if (--seconds == 0) {
			CancelInvoke ("Countdown");
			onEndRound();
			ShowTimeOut();
		}
		currentCount = seconds.ToString();
	}
}//class