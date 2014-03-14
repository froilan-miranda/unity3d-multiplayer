using UnityEngine;
using System.Collections;

public class VirionController : MonoBehaviour {

	internal delegate void onVirionDeathEvent(GameObject g, string type);
	internal static event onVirionDeathEvent onVirionDeath;

	private enum VirionState {main, replicating, end, killshot};
	private VirionState vState;
	private enum VirionType {wild, mutant};
	private VirionType vType;
	private Transform geomWild;
	private Transform geomMutated;
	private Transform glow;
	private Collider[] hitColliders;
	private float lifeSpan_min = 30.0f;
	private float lifeSpan_max = 50.0f;
	private float lifeSpan;
	public int hitSlots;
	public Transform hitCounter;
	private bool counterVisible;
	private uint[] arr_playerHits;
	private int hitCount = 0;

	private Vector3 speed;
	private float timing;
	private float moveDistance; // = 1;
	private bool _stationary = false;

	public float maxVelocity = 1;
	private Rigidbody rb;
	private float sqrMaxVelocity;

	public AudioClip deathBell;


	// Use this for initialization
	void Awake()
	{
		moveDistance = GameObject.Find("Game Manager").GetComponent<SettingsController>().GetSetting("speed");

		geomWild = transform.Find("Wild");
		geomMutated = transform.Find("Mutated");

		lifeSpan = Random.Range(lifeSpan_min, lifeSpan_max);
		Invoke("EndVirion", lifeSpan);

		rb = rigidbody;
		SetMaxVelocity(maxVelocity);
		speed = new Vector3(Random.Range(-moveDistance, moveDistance),Random.Range(-moveDistance, moveDistance),Random.Range(-moveDistance, moveDistance));
		timing = 0;

		InitHitCounter();
		InitHalos();
	}
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if(collider.isTrigger && vState != VirionState.end)
		{
			hitColliders = Physics.OverlapSphere(gameObject.transform.position, 0.50f);
			if(hitColliders.Length == 1){
				//Debug.Log("Should be empty");
				collider.isTrigger = false;
				ChangeState("main");
			}
		}

		if(hitCounter.gameObject.active == true){
			hitCounter.LookAt(Camera.main.transform);
		}
	}

    internal void FixedUpdate()
    {
    	if(!_stationary){
			timing+=Time.deltaTime;

	    	Vector3 v = rb.velocity;
	    	if(v.sqrMagnitude > sqrMaxVelocity && !rb.isKinematic )
	    		rb.velocity = v.normalized * maxVelocity;

			gameObject.transform.position = gameObject.transform.position + speed * Time.deltaTime;
			if(timing > 3){
				ChangeDirection();
				timing = 0;
			}
		}
    }
	private void SetMaxVelocity(float maxVelocity)
	{
		this.maxVelocity = maxVelocity;
		sqrMaxVelocity = maxVelocity * maxVelocity;
	}

	private void EndVirion()
	{
		Debug.Log("here inside end life:" + IsInvoking("EndVirion"));

		ChangeState("end");
		Debug.Log("called from invoke");
	}

    internal void ChangeState(string state) {
		switch (state){
			case "main":
				vState = VirionState.main;
				//InitWild();		~use this to bring back to a main state
				break;
			case "replicating":
				vState = VirionState.replicating;
				InitReplicate();
				break;
			case "end":
				vState = VirionState.end;
				InitEnd();
				break;
			case "killshot":
				vState = VirionState.killshot;
				InitKillShot();
				break;
			default:
				Debug.Log("Virion State Machine broken(setter) " + state);
				break;
		}
    }

    internal string VState
    {
    	get{
    		string str_state = null; 

    		switch(vState){
    			case VirionState.main:
    				str_state = "main";
    				break;
    			case VirionState.replicating:
    				str_state = "replicating";
    				break;
    			case VirionState.end:
    				str_state = "end";
    				break;
    			default:
    				Debug.Log("Virion State Maching broken(getter) " + vState);
    				break;
    		}
    		return str_state;
 		}//get
    }

	 internal string VType
    {
    	set{
    		switch (value){
    			case "wild":
    				vType = VirionType.wild;
    				InitWild();
    				break;
    			case "mutant":
    				vType = VirionType.mutant;
    				InitMutant();
    				break;
    			default:
    				Debug.Log("error in VType setter");
    				break;
    		}
    	}
    	get{
    		string str_type = null; 
    		switch (vType){
    			case VirionType.wild:
	    			str_type = "wild";
	    			break;
    			case VirionType.mutant:
	    			str_type = "mutant";
	    			break;
	    		default:
	    			Debug.Log("error in VType getter");
	    			break;
    		}
    		return str_type;
    	}
    }

    internal bool Stationary
    {
    	get{return _stationary;}
    	set{_stationary = value;}
    }

	private void InitWild()
	{
		vType = VirionType.wild;
		geomMutated.gameObject.active = false;
	}

	private void InitMutant()
	{
		vType = VirionType.mutant;
		geomWild.gameObject.active = false;
	}

	private void InitReplicate()
	{
		//Debug.Log("Ready to replicate");
		//turn off collider
		collider.isTrigger = true;
	}

	internal void SuspendLife(bool toggle)
	{
		if(toggle){
			CancelInvoke("EndVirion");
			Debug.Log("here inside suspend life true:" + IsInvoking("EndVirion"));
			rigidbody.isKinematic = true;
			_stationary = true;
			collider.isTrigger = true;
		}else if(!toggle && !IsInvoking("EndVirion")){
			Invoke("EndVirion", lifeSpan);
			Debug.Log("here inside suspend life flase");
			collider.isTrigger = false;
		}
	}

	private void InitEnd()
	{
		if(IsInvoking("EndVirion")) CancelInvoke("EndVirion");
		collider.enabled = false;
		//Debug.Log("should start doing some die sequence sturff");
		if(vType == VirionType.wild){
			iTween.FadeTo(geomWild.gameObject,iTween.Hash("alpha", 0, "time", 1,  "oncomplete", "OnFadeOut", "oncompletetarget", gameObject));
		}else if(vType == VirionType.mutant){
			iTween.FadeTo(geomMutated.gameObject,iTween.Hash("alpha", 0, "time", 1, "oncomplete", "OnFadeOut", "oncompletetarget", gameObject));
		}
	}

	private void InitKillShot()
	{
		Debug.Log("kill shot");
		if(IsInvoking("EndVirion")) CancelInvoke("EndVirion");
		collider.enabled = false;
        audio.clip = deathBell;
        audio.Play();
		//Debug.Log("should start doing some die sequence sturff");
		if(vType == VirionType.wild){
			iTween.ColorTo(geomWild.gameObject,iTween.Hash("color", Color.grey, "time", 1));
			iTween.FadeTo(geomWild.gameObject,iTween.Hash("alpha", 0, "time", 1, "delay", 2,  "oncomplete", "OnFadeOut", "oncompletetarget", gameObject));
		}else if(vType == VirionType.mutant){
			iTween.ColorTo(geomMutated.gameObject, iTween.Hash("color", Color.grey, "time", 1));
			iTween.FadeTo(geomMutated.gameObject, iTween.Hash("alpha", 0, "time", 1, "delay", 2, "oncomplete", "OnFadeOut", "oncompletetarget", gameObject));
		}
		iTween.FadeTo(glow.gameObject, iTween.Hash("alpha", 0, "time", 1, "delay", 2));
	}
	internal void OnFadeOut()
	{
		onVirionDeath(gameObject, VType); //call event 
	}

    private void ChangeDirection()
	{
		speed = new Vector3(Random.Range(-moveDistance, moveDistance),Random.Range(-moveDistance, moveDistance),Random.Range(-moveDistance, moveDistance));
	}

	private void InitHalos()
	{
		gameObject.transform.Find("Glows").gameObject.SetActiveRecursively(false);
	}
	private void ShowHalo()
	{
		transform.Find("Glows").gameObject.active= true;
		string glowColor = "Glows/GlowP" + arr_playerHits[arr_playerHits.Length-1];
		glow = transform.Find(glowColor);
		glow.gameObject.active = true;
	}
	private void InitHitCounter()
	{
		hitSlots = Random.Range(2, 6);
		//Debug.Log(hitSlots);

		for(int hit=2; hit<=6; hit++){
			string counterObj = "hit " + hit.ToString();
			 if (hit == hitSlots){
				hitCounter = transform.Find(counterObj);
				//iTween.FadeTo(hitCounter.gameObject, iTween.Hash("alpha", 0, "time", 0.1));
			}else{
				transform.Find(counterObj).gameObject.SetActiveRecursively(false);
			}
		}
		arr_playerHits = new uint[hitSlots];
	}

	internal void OnBulletEnter(GameObject bullet) {
		//Debug.Log("a collision has happended");

		//Test for practice round. don't allow for more than one hitn
		if(Application.loadedLevel == 3 && hitCount ==0){
			//update hitCounter
			UpdateCounter(bullet.GetComponent<BulletController>().Owner);
			GameObject.Find("Cannon Manager").BroadcastMessage("PracticeShot", bullet.GetComponent<BulletController>().Owner);
		}else if(Application.loadedLevel != 3){
			uint bullletCount = (Application.loadedLevel != 6) ? (uint)1 : (uint)4;
			for(uint i = 0; i<bullletCount; i++)
				UpdateCounter(bullet.GetComponent<BulletController>().Owner);
		}
		//play audio
		audio.Play();
		//destroy bullet
		Destroy(bullet);
	}

	private void UpdateCounter(uint bulletOwner)
	{
		if(hitCount < hitSlots){
			//add player number to hit array
			arr_playerHits[hitCount] = bulletOwner;
			//grab approprate slot
			Transform slotPlane = transform.Find(hitCounter.name+"/slot "+(hitCount+1)+"/pPlane1");
			//advance sprite sheet
			float offset=0f;
			switch(bulletOwner){
				case 1:
					offset = 0.25f;
					break;
				case 2:
					offset = 0.50f;
					break;
				case 3:
					offset = 0.75f;
					break;
				default:
					Debug.Log("something is broke in offset:"+ bulletOwner);
					break;
			}
			slotPlane.renderer.material.mainTextureOffset = new Vector2(offset, 0);
			//Debug.Log("the counter: " +slotPlane);
			hitCount++;
			if(hitCount == hitSlots){
				ShowHalo();
				ChangeState("killshot");
				UpdateScore();
			}
		}
	}
	private void UpdateScore()
	{
		uint scorer = arr_playerHits[arr_playerHits.Length-1];
		PlayerManagerController.UpdateScore(scorer);
		GameObject.Find("Game Manager").GetComponent<MessageController>().SendHost("Kill|P"+scorer+"|"+VType, "tcp");
	}

	internal void ShowVirion()
	{
		gameObject.active = true;
		if(vType == VirionType.wild)
			geomWild.gameObject.active = true;
		else if(vType == VirionType.mutant)
			geomMutated.gameObject.active = true;
		hitCounter.gameObject.SetActiveRecursively(true);
	}
	internal void HideVirion()
	{
		gameObject.SetActiveRecursively(false);
	}
	internal void ShowCounter()
	{
		Debug.Log("Ready to show counter");
		//if(!counterVisible){
			iTween.FadeTo(hitCounter.gameObject, iTween.Hash("alpha", 1.0, "time", 0.25));
			//hitCounter.gameObject.SetActiveRecursively(true);
			counterVisible = true;
		//}
	}
	internal void HideCounter()
	{
		//Debug.Log("Ready to hide counter");
		if(counterVisible){
			iTween.FadeTo(hitCounter.gameObject, iTween.Hash("alpha", 0.0, "time", 1.0, "delay", 2.0));
			//hitCounter.gameObject.SetActiveRecursively(false);
			counterVisible = false;
		}
	}
}//class
