using UnityEngine;
using System.Collections;

public class BotBeh : MonoBehaviour {

	GameObject chainElement;
	GameObject lastElement;
	public GameObject groundElement;
	
	bool mouseReleased=false;
	
	Vector3 lastTouch;
	Vector3 currentTouch;
	Vector3 tmp;
	Vector2 gravityVec;
	float rotateVal;
	
	float cameraAngle=0f;
	
	void Start () {
		int i;
		lastElement=gameObject;
		for(i=0;i<25;i++)
		{
			chainElement=(GameObject) Instantiate(Resources.Load("prefabs/chainElement"),transform.position+new Vector3(0,4.4f+0.25f*(i+1),0),Quaternion.identity);
			lastElement.GetComponent<HingeJoint2D>().connectedBody=chainElement.GetComponent<Rigidbody2D>();
			
			lastElement=chainElement;
		}
		lastElement.GetComponent<HingeJoint2D>().connectedBody=groundElement.GetComponent<Rigidbody2D>();
		Physics2D.gravity=new Vector2(0,-1);
		Camera.main.orthographicSize=GameOptions.IN_GAME_ZOOM;
	}
	
	void Update()
	{
		Camera.main.transform.position=new Vector3(transform.position.x,transform.position.y,-10);
		if(Input.GetMouseButtonDown(0))
			mouseReleased=true;
		if(Input.GetMouseButtonUp(0))
			mouseReleased=false;
		
		currentTouch=Input.mousePosition;
		if(mouseReleased)
		{
			if(currentTouch.x>=Screen.width*0.5f && lastTouch.x >= Screen.width*0.5f)
			{
				tmp=currentTouch-lastTouch;
				if(tmp.magnitude>20) {tmp/=tmp.magnitude*20;}
				if(currentTouch.magnitude>lastTouch.magnitude)
					rotateVal=-tmp.magnitude;	
				else
					rotateVal=tmp.magnitude;
				cameraAngle+=rotateVal*GameOptions.CAMERA_TOUCH_ROTATE_SPEED;
				cameraAngle=Mathf.Repeat(cameraAngle,360.0f);
				Camera.main.transform.localEulerAngles=new Vector3(0,0,cameraAngle);
				
				gravityVec = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height,0)).x,Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height,0)).y)-new Vector2(transform.position.x,transform.position.y);
				gravityVec/=gravityVec.magnitude;
				Physics2D.gravity=-gravityVec;
			}
		}
		lastTouch=currentTouch;
	}
}
