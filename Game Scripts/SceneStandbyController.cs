using UnityEngine;
using System.Collections;

public class SceneStandbyController : MonoBehaviour {

	private GameObject videoPlane;
	private MovieTexture txtrMovie;

	// Use this for initialization
	void Start () {
		videoPlane = GameObject.Find("Video Plane/pPlane1");
		txtrMovie = videoPlane.renderer.material.mainTexture as MovieTexture;
		txtrMovie.loop = true;
		txtrMovie.Play();
		PlayerManagerController.ClearPlayers();
	}
	// Update is called once per frame
	void Update ()
	{

	}
	internal void HostSentStart()
	{
		Application.LoadLevel(2);
	}
}
