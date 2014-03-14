using UnityEngine;
using System.Collections;

public class BgVideoController : MonoBehaviour {

	private MovieTexture txtrMovie;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		txtrMovie = renderer.material.mainTexture as MovieTexture;
		txtrMovie.loop = true;
		txtrMovie.Play();
	}
	void OnLevelWasLoaded(int level)
	{
		if(level > 2){
			txtrMovie.loop = true;
			txtrMovie.Play();
		}
	}
	// Update is called once per frame
	void FixedUpdate () {

		//Vector3 followPos = new Vector3(gameObject.transform.position.x,Camera.main.transform.position.y, gameObject.transform.position.z);
		//gameObject.transform.position = followPos;
	}
}
