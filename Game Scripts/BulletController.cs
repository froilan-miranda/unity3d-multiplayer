using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	private uint _owner;
	private bool targetSet = false;
	private Vector3 direction;
	private float speed = 10.0f;

	// Use this for initialization
	public void Start () {
		Invoke("DestroySelf", 5);
	}
	
	// Update is called once per frame
	public void Update () {
		if(targetSet)
			transform.Translate(direction * speed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "virion")
			other.gameObject.GetComponent<VirionController>().OnBulletEnter(gameObject);
	}
	internal void SetDirection(Vector3 dir, Vector3 target)
	{
		direction = dir;
		targetSet = true;
		transform.up = rigidbody.velocity;
		FaceDirection(target);
	}

	private void FaceDirection(Vector3 target)
	{
		transform.LookAt(target, new Vector3(0,1,0));
	}
	internal uint Owner
	{
		get { return _owner; }
		set { _owner = value; }
	}

	private void DestroySelf()
	{
		Destroy(gameObject);
	}
}
