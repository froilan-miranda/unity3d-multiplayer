using UnityEngine;
using System.Collections;

public class T_EllipseCheck : MonoBehaviour {

	public Texture2D reticle;
	private Rect reticlePos;
	public  Texture2D ellipseTemplate;
	private Rect ellipseTempPos;
	private float testX;		// the x point in question
	private float testY;		// the y point in question
	private float rx;				// semi major axis radius
	private float ry;				// semi minor axis radius
	private float h;				//centerpoint x
	private float k;				//center point y


	// Use this for initialization
	void Start () 
	{
		ellipseTempPos = new Rect(0,0, 800, 800);
		rx = 375;
		ry = 200;
		h = 400;
		k = 400;
	}

	// Update is called once per frame
	void Update () 
	{
		//mousePos_2D = Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y))

	}

	public void OnGUI()
	{
		f_isInEllipse(Event.current.mousePosition);
		GUI.DrawTexture(ellipseTempPos, ellipseTemplate);
		GUI.DrawTexture(reticlePos, reticle);
	}


	public void f_isInEllipse(Vector2 mousePos)
	{
		testX = mousePos.x;
		testY = mousePos.y;
		float f1 = Mathf.Pow((testX - h), 2) / Mathf.Pow(rx, 2);
		float f2 = Mathf.Pow((testY - k), 2) /Mathf.Pow(ry, 2);
		//Debug.Log(f1 + f2);
		if((f1 + f2) <= 1){
			// the mouse position is inside the oval, we can postion reticle x and y
			//Debug.Log(testX + " " +  testY);
			reticlePos = new Rect(testX-(reticle.width/2), testY-(reticle.height/2), reticle.width, reticle.height);
		}else{
			// the mouse position is outside the oval, find the closest point to position reticle 
			findClosestPoint();
		}
	}

	private void findClosestPoint()
	{
		// a vertical line will throw and error because we divide by zero and that just dosn't sit well 
		if(testX != 0){
		//find Ellipse-Line Intersection
		Vector2[] interceptPoints = f_ellipseLineIntersection();

		//find the closest intercept point
		shortestDistance(interceptPoints);
		}else{
			// the line is verticle so we will check if it's +or- and place the reticle accordingly
			if(Mathf.Sign(testY) == 1)
				reticlePos = new Rect(0-(reticle.width/2), ry-(reticle.height/2), reticle.width, reticle.height);
			else if(Mathf.Sign(testY) == -1)
				reticlePos = new Rect(0-(reticle.width/2), (-ry)-(reticle.height/2), reticle.width, reticle.height);
		}
	}

	private Vector2[]	f_ellipseLineIntersection()
	{
		float offSetX = testX - h;// offset to ellipse origin
		float offSetY = testY - k;// offset to ellipse origin
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
		Vector2[] interceptPoints = { intercept1, intercept2 };

		//Debug.Log(fx1 + " " +  fx2 + " so far");
		return interceptPoints;
	}

	private void shortestDistance(Vector2[] interceptPoints)
	{
		Vector2 intercept1 = interceptPoints[0];
		Vector2 intercept2 = interceptPoints[1];

		float distance1 = f_distanceOfPoints(intercept1);
		float distance2 = f_distanceOfPoints(intercept2);

		if(distance1 < distance2){
			//intercept 1 is closer to our point, we are going to use it
			reticlePos = new Rect(intercept1.x-(reticle.width/2), intercept1.y-(reticle.height/2), reticle.width, reticle.height);
		}else{
			//intercept 2 is closer to our point, we are going to use it
			reticlePos = new Rect(intercept2.x-(reticle.width/2), intercept2.y-(reticle.height/2), reticle.width, reticle.height);
		}
	}

	private float f_distanceOfPoints(Vector2 coordinates)
	{
		float x1 = coordinates.x;
		float y1 = coordinates.y;
		float x2 = testX;
		float y2 = testY;

		float distance = Mathf.Sqrt(Mathf.Pow((x2 - x1),2) + Mathf.Pow((y2 - y1),2));
		//Debug.Log("distance is " + distance);
		return distance;
	}
}
