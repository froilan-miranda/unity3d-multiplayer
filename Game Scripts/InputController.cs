using UnityEngine;
using System.Collections;
using viJoystickLib;

public class InputController : MonoBehaviour 
{
	private JoystickManager jm;
	private float jSerial1;
	private float jSerial2;
	private float jSerial3;
	private SettingsController settingsScript;
	private GameObject virionManager;
	private GameObject cannons;
	private bool settingsSet = false;
	
	private bool _reticleVisible = false;
	private bool _showVersion2;
	public Texture2D ReticleP1V1;
	public Texture2D ReticleP2V1;
	public Texture2D ReticleP3V1;

	public Texture2D ReticleP1V2;
	public Texture2D ReticleP2V2;
	public Texture2D ReticleP3V2;

	private Rect p1Position;
	private Rect p2Position;
	private Rect p3Position;

	private Ray ray2DP1;
	private Ray ray2DP2;
	private Ray ray2DP3;

	private bool raycastActive = false;

	private float rx;				// semi major axis radius
	private float ry;				// semi minor axis radius
	private float h;				//centerpoint x
	private float k;				//center point y

	// Use this for initialization
	void Start ()
	{


	}
	internal void InitInput()
	{
		settingsScript = GameObject.Find("Game Manager").GetComponent<SettingsController>();
		jSerial1 = settingsScript.GetSetting("serial1");
		jSerial2 = settingsScript.GetSetting("serial2");
		jSerial3 = settingsScript.GetSetting("serial3");

		jm = new JoystickManager();
		p1Position = new Rect(0, (Screen.height - ReticleP1V1.height) /2, ReticleP1V1.width, ReticleP1V1.height);
		p2Position = new Rect((Screen.width - ReticleP2V1.width)/2, (Screen.height - ReticleP2V1.height), ReticleP2V1.width, ReticleP2V1.height);
		p3Position = new Rect((Screen.width - ReticleP3V1.width), (Screen.height - ReticleP3V1.height) /2, ReticleP3V1.width, ReticleP3V1.height);

		DontDestroyOnLoad(gameObject);

		cannons = GameObject.Find("Cannon Manager");

		rx = 960;
		ry = 519;
		//h = 960;
		//k = 540;
		h = 960-(ReticleP1V1.width/2);
		k = 540-(ReticleP1V1.height/2);
		//float txtrOffsetX = ReticleP1V1.width/2;
		//float txtrOffsetY = ReticleP1V1.height/2;
		settingsSet = true;
	}
	void OnLevelWasLoaded(int level) {
		switch(level){
			case 2:
				_showVersion2 = false;
				virionManager = GameObject.Find("Virion Manager");
				raycastActive = false;
				break;
			case 3:
				ResetReticlePos();
				raycastActive =  true;
				break;
			case 4:
				raycastActive = true;
				break;
			case 5:
				ResetReticlePos();
				raycastActive = false;
				break;
			case 6:
				raycastActive = true;
				break;
			default:
				raycastActive =  false;
				break;
		}
	}

	// Update is called once per frame
	void Update ()
	{	
		if(settingsSet){
			//Debug.Log("joyStick manager " + jm);
			for (int i = 0; i < jm.Joysticks.Count; i++) {
				Joystick j = jm.Joysticks[i];
				//Debug.Log("Joystick: " + j.Serial + " X: " + j.X.ToString() + " Y: " + j.Y.ToString() + " Thumb: " + j.Thumb.ToString());
				if(j.Serial==jSerial1){
					p1Position = MoveReticle(j.X, j.Y, p1Position);
					if(j.Thumb)AttemptFire(1, ray2DP1);
				}else if(j.Serial==jSerial2){
					p2Position = MoveReticle(j.X, j.Y, p2Position);
					if(j.Thumb)AttemptFire(2, ray2DP2);
				}else if(j.Serial==jSerial3){
					p3Position = MoveReticle(j.X, j.Y, p3Position);
					if(j.Thumb)AttemptFire(3, ray2DP3);
				}else{
					Debug.Log("a joystick serial has fallen through: " + j.Serial.ToString());
				}
			}
			if(raycastActive) DoRayCast();
		}
	}

	public void OnGUI()
	{
		if(_reticleVisible){
			if(!_showVersion2){
				if(PlayerManagerController.IsPlayerActive(1)) GUI.DrawTexture(p1Position, ReticleP1V1);
				if(PlayerManagerController.IsPlayerActive(2))GUI.DrawTexture(p2Position, ReticleP2V1);
				if(PlayerManagerController.IsPlayerActive(3))GUI.DrawTexture(p3Position, ReticleP3V1);
			}else{
				if(PlayerManagerController.IsPlayerActive(1)) GUI.DrawTexture(p1Position, ReticleP1V2);
				if(PlayerManagerController.IsPlayerActive(2)) GUI.DrawTexture(p2Position, ReticleP2V2);
				if(PlayerManagerController.IsPlayerActive(3)) GUI.DrawTexture(p3Position, ReticleP3V2);
			}
		}
	}

	internal bool ReticleVisible
	{
		get{return _reticleVisible;}
		set{_reticleVisible = value;}
	}
	private void DoRayCast()
	{
		Vector2 targetXY;
		RaycastHit hit;

		if(PlayerManagerController.IsPlayerActive(1)){
			targetXY  = new Vector2(p1Position.x +(ReticleP1V1.width/2) ,  Screen.height - p1Position.y - (ReticleP1V1.height/2));
			ray2DP1 = Camera.main.ScreenPointToRay(targetXY);
			Debug.DrawRay (ray2DP1.origin, ray2DP1.direction * 1000, Color.green);
			Physics.Raycast(ray2DP1, out hit, Mathf.Infinity);
			if(hit.collider != null)
				OnRaycast(1, hit.collider.gameObject);
			else
				OnRaycast(1, null);
		}

		if(PlayerManagerController.IsPlayerActive(2)){
			targetXY = new Vector2(p2Position.x +(ReticleP2V1.width/2) ,  Screen.height - p2Position.y - (ReticleP2V1.height/2));
			ray2DP2 = Camera.main.ScreenPointToRay(targetXY);
			Debug.DrawRay (ray2DP2.origin, ray2DP2.direction * 1000, Color.blue);
			Physics.Raycast(ray2DP2, out hit, Mathf.Infinity);
			if(hit.collider != null)
				OnRaycast(2, hit.collider.gameObject);
			else
				OnRaycast(2, null);
		}

		if(PlayerManagerController.IsPlayerActive(3)){
			targetXY = new Vector2(p3Position.x +(ReticleP3V1.width/2) ,  Screen.height - p3Position.y - (ReticleP3V1.height/2));
			ray2DP3 = Camera.main.ScreenPointToRay(targetXY);
			Debug.DrawRay (ray2DP3.origin, ray2DP3.direction * 1000, Color.red);
			Physics.Raycast(ray2DP3, out hit, Mathf.Infinity);
			if(hit.collider != null)
				OnRaycast(3, hit.collider.gameObject);
			else
				OnRaycast(3, null);
		}
	}

	private void OnRaycast(uint player, GameObject hitObject)
	{
		if(virionManager)virionManager.GetComponent<VirionManagerController>().ToggleHitCounters(player, hitObject);
	}

	private Rect MoveReticle(float xOffset, float yOffset, Rect reticlePos)
	{
		float xDeadSpace = 0.2f;
		float yDeadSpace = 0.4f;
		float currentX = reticlePos.x;
		float currentY = reticlePos.y;
		float testX = currentX;
		float testY = currentY;
		Vector2 returnPos;

		xOffset = (xOffset > xDeadSpace || xOffset < -xDeadSpace) ? xOffset : 0;
		yOffset = (yOffset > yDeadSpace || yOffset < -yDeadSpace) ? yOffset : 0;

		testX = currentX + (Mathf.Floor(xOffset * 10));
		testY = currentY + (Mathf.Floor(yOffset * -10));
		returnPos = f_isInEllipse(new Vector2(testX, testY));
		reticlePos.x = returnPos.x;
		reticlePos.y = returnPos.y;
		return reticlePos;
	}

	private void ResetReticlePos()
	{
		p1Position = new Rect(0, (Screen.height - ReticleP1V1.height) /2, ReticleP1V1.width, ReticleP1V1.height);
		p2Position = new Rect((Screen.width - ReticleP2V1.width)/2, (Screen.height - ReticleP2V1.height), ReticleP2V1.width, ReticleP2V1.height);
		p3Position = new Rect((Screen.width - ReticleP3V1.width), (Screen.height - ReticleP3V1.height) /2, ReticleP3V1.width, ReticleP3V1.height);
	}
	public Vector2 f_isInEllipse(Vector2 testPos)
	{
		float testX = testPos.x;	// the x point in question
		float testY = testPos.y;	// the y point in question
		float f1 = Mathf.Pow((testX - h), 2) / Mathf.Pow(rx, 2);
		float f2 = Mathf.Pow((testY - k), 2) /Mathf.Pow(ry, 2);
		if((f1 + f2) <= 1){
			// the mouse position is inside the oval, we can postion reticle x and y
			return new Vector2(testX, testY);
		}else{
			// the mouse position is outside the oval, find the closest point to position reticle 
			return findClosestPoint(new Vector2(testX, testY));
		}
	}

	private Vector2 findClosestPoint(Vector2 testcoords)
	{
		Vector2 shortDistance;
		// a vertical line will throw and error because we divide by zero and that just dosn't sit well 
		if(testcoords.x != 0){
			//find Ellipse-Line Intersection
			Vector2[] interceptPoints = f_ellipseLineIntersection(testcoords);

			//find the closest intercept point
			shortDistance = shortestDistance(interceptPoints);
		}else{
			// the line is verticle so we will check if it's +or- and place the reticle accordingly
			if(Mathf.Sign(testcoords.y) == 1){
				shortDistance = new Vector2(0-(ReticleP1V1.width/2), ry-(ReticleP1V1.height/2));
			}else{
				shortDistance = new Vector2(0-(ReticleP1V1.width/2), (-ry)-(ReticleP1V1.height/2));
			}
		}
		return shortDistance;
	}

	private Vector2[]	f_ellipseLineIntersection( Vector2 testPoint)
	{
		float offSetX = testPoint.x - h;// offset to ellipse origin
		float offSetY = testPoint.y - k;// offset to ellipse origin
		float fx1 = ((rx*ry)/Mathf.Sqrt((Mathf.Pow(rx,2) * Mathf.Pow(offSetY,2)) + (Mathf.Pow(ry,2) * Mathf.Pow(offSetX,2)))) * offSetX;
		float fx2 = -((rx*ry)/Mathf.Sqrt((Mathf.Pow(rx,2) * Mathf.Pow(offSetY,2)) + (Mathf.Pow(ry,2) * Mathf.Pow(offSetX,2)))) * offSetX;
		float fy1 = ((rx*ry)/Mathf.Sqrt((Mathf.Pow(rx,2) * Mathf.Pow(offSetY,2)) + (Mathf.Pow(ry,2) * Mathf.Pow(offSetX,2)))) * offSetY;
		float fy2 = -((rx*ry)/Mathf.Sqrt((Mathf.Pow(rx,2) * Mathf.Pow(offSetY,2)) + (Mathf.Pow(ry,2) * Mathf.Pow(offSetX,2)))) * offSetY;

		//reverse the offset from above
		fx1 += h;
		fx2 += h;
		fy1 += k;
		fy2 += k;

		//package up and send back
		Vector2 intercept1 = new Vector2(fx1, fy1);
		Vector2 intercept2 = new Vector2(fx2, fx2);
		Vector2 testcoords = new Vector2(testPoint.x, testPoint.y);
		Vector2[] interceptPoints = { intercept1, intercept2, testcoords };
		return interceptPoints;
	}

	private Vector2 shortestDistance(Vector2[] interceptPoints)
	{
		Vector2 intercept1 = interceptPoints[0];
		Vector2 intercept2 = interceptPoints[1];
		Vector2 testcoords = interceptPoints[2];

		float distance1 = f_distanceOfPoints(intercept1, testcoords);
		float distance2 = f_distanceOfPoints(intercept2, testcoords);
		Vector2 shortest;
		if(distance1 < distance2){
			//intercept 1 is closer to our point, we are going to use it
			shortest = new Vector2(intercept1.x, intercept1.y);
		}else{
			//intercept 2 is closer to our point, we are going to use it
			shortest = new Vector2(intercept2.x, intercept2.y);
		}
		return shortest;
	}

	private float f_distanceOfPoints(Vector2 coordinates, Vector2 testcoords)
	{
		float x1 = coordinates.x;
		float y1 = coordinates.y;
		float x2 = testcoords.x;
		float y2 = testcoords.y;
		float distance = Mathf.Sqrt(Mathf.Pow((x2 - x1),2) + Mathf.Pow((y2 - y1),2));
		return distance;
	}

	private void AttemptFire (int playerNum, Ray ray2D)
	{
		Debug.Log("player " + playerNum + " attempted fire");
		cannons.GetComponent<CannonManagerController>().RequestFire(playerNum, ray2D);
	}
	internal bool ShowVersion2
	{
		set{_showVersion2 = value;}
	}
	void OnApplicationQuit ()
	{
		Debug.Log("DISPOSING");
		jm.Dispose();
	}
}//class
