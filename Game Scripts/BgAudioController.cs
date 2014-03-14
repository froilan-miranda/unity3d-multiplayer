using UnityEngine;
using System.Collections;

public class BgAudioController : MonoBehaviour {
	
	public AudioClip audioA;
	public AudioClip audioB;

	// Use this for initialization
	void Start () {
		TimerController.onEndRound += RoundOver;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded(int level) {
		switch(level){
			case 2:
				PlayAudioA();
				break;
			case 4:
				PlayAudioB();
				break;
			case 6:
				PlayAudioB();
				break;
		}
	}

	internal void InitAudioA()
	{
		PlayAudioA();
	}
	private void PlayAudioA()
	{
        audio.clip = audioA;
        audio.Play();
	}

	private void PlayAudioB()
	{
        audio.clip = audioB;
        audio.Play();
	}

	internal void Volume(float vol)
	{
		audio.volume = vol;
	}
	private void RoundOver()
	{
		PlayAudioA();
	}
	void OnDestroy()
	{
		TimerController.onEndRound -= RoundOver;
	}
}
