using UnityEngine;
using System.Collections;

public class BotBeh : MonoBehaviour {
	public GameObject ground;
	public GameObject end;
	GameObject cur;
	Vector2 gravityVec;
	float cameraAngle=0f;
	private HingeJoint2D hinge;
	private DistanceJoint2D dist;
	
	private ChainElement first=null,last=null,tmp;
	private int chainCount=0;
	
	private GameObject attachedChainElement;
	private float attachedDist=1;
	private LineRenderer line;
	
	private Vector2 test1,test2;
	
	class ChainElement
	{
		public ChainElement prev;
		public ChainElement next;
		public GameObject go;
	}
	
	void Start () {
		last = new ChainElement();
		last.go=ground;
		last.next=null;
		last.prev=null;
		Physics2D.velocityIterations=70;
		Physics2D.positionIterations=70;
		for(int i = 0;i<3;i++)
		{
			cur=(GameObject) Instantiate(Resources.Load("prefabs/chainElement"),new Vector3(0,-0.5f-(i-GameOptions.CHAIN_LEN/2),1),Quaternion.identity);
			cur.transform.localScale=new Vector3(0.1f,GameOptions.CHAIN_LEN,1);
			cur.GetComponent<Rigidbody2D>().mass=20.0f;
			hinge = cur.GetComponent<HingeJoint2D>();
			hinge.anchor=new Vector2(0,0.5f);
			hinge.connectedAnchor=new Vector2(0,-0.5f);
			hinge.connectedBody=last.go.GetComponent<Rigidbody2D>();
			
			dist = cur.GetComponent<DistanceJoint2D>();
			dist.anchor=new Vector2(0,0.5f);
			dist.connectedAnchor=new Vector2(0,1.47f);
			dist.connectedBody=last.go.GetComponent<Rigidbody2D>();
			
			tmp = new ChainElement();
			tmp.go=cur;
			last.next=tmp;
			tmp.prev=last;
			tmp.next=null;
			last=tmp;
			if(first==null)
			{
				first=tmp;
				first.prev=null;
			}
			
			if(tmp.prev!=null)
				tmp.go.GetComponent<ChainELementBeh>().prev=tmp.prev.go;
			
			
			chainCount++;
		}
		
		
		
		hinge = end.GetComponent<HingeJoint2D>();
		hinge.anchor=new Vector2(0,4.4f);
		hinge.connectedAnchor=new Vector2(0,-0.5f);
		hinge.connectedBody=last.go.GetComponent<Rigidbody2D>();
		
		line = end.GetComponent<LineRenderer>();
		dist = first.go.GetComponent<DistanceJoint2D>();
		dist.anchor=new Vector2(0,0.48f);
		dist.connectedAnchor=new Vector2(0,-0.48f);
	}
	
	void Expand()
	{
		attachedDist+=0.08f;
		if(attachedDist>=1)
		{
			Vector2 f = Quaternion.Euler(0,0,end.transform.eulerAngles.z)*new Vector2(0,0.5f);
			cur = (GameObject) Instantiate(Resources.Load("prefabs/chainElement"),end.transform.position+new Vector3(f.x,f.y,0),end.transform.rotation);
			cur.transform.localScale=new Vector3(0.1f,GameOptions.CHAIN_LEN,1);
			cur.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			cur.GetComponent<Rigidbody2D>().mass=20.0f;
			last.go.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.None;
			
			hinge = end.GetComponent<HingeJoint2D>();
			hinge.connectedAnchor=new Vector2(0,0.5f);
			hinge.connectedBody = cur.GetComponent<Rigidbody2D>();
			
			hinge = cur.GetComponent<HingeJoint2D>();
			hinge.connectedBody=last.go.GetComponent<Rigidbody2D>();
			
			tmp = new ChainElement();
			tmp.go=cur;
			last.next=tmp;
			tmp.prev=last;
			tmp.next=null;
			last=tmp;
			
			attachedChainElement=cur;
			attachedDist=0;
			chainCount++;
		}
		else
		{
			attachedChainElement.transform.rotation=end.transform.rotation;
			Vector2 z = Quaternion.Euler(0,0,end.transform.eulerAngles.z)*new Vector2(0,-0.0415f);
			end.transform.position = end.transform.position+new Vector3(z.x,z.y,0);
			hinge=end.GetComponent<HingeJoint2D>();
			hinge.connectedAnchor=new Vector2(0,1-attachedDist-0.5f);
		}
	}
	
	void Revert()
	{
		if(chainCount>1)
		{
			attachedDist-=0.08f;
			if(attachedDist<=0)
			{
				Destroy(last.go);
				last=last.prev;
				last.next=null;
				
				hinge = end.GetComponent<HingeJoint2D>();
				hinge.connectedBody=last.go.GetComponent<Rigidbody2D>();
				hinge.connectedAnchor=new Vector2(0,-0.5f);
				attachedDist=1;
				attachedChainElement=last.go;
				chainCount--;
			}
			else
			{
				last.go.transform.rotation=end.transform.rotation;
				Vector2 f = Quaternion.Euler(0,0,end.transform.eulerAngles.z)*new Vector2(0,0.75f);
				Vector2 z = Quaternion.Euler(0,0,end.transform.eulerAngles.z)*new Vector2(0,attachedDist/4.0f);
				last.go.transform.position = end.transform.position+new Vector3(f.x,f.y,0)+new Vector3(z.x,z.y,0);
				hinge=end.GetComponent<HingeJoint2D>();
				hinge.connectedAnchor=new Vector2(0,1-attachedDist-0.5f);
			}
		}
	}
	
	void DrawLine()
	{
		line.SetPosition(0,new Vector3(end.transform.position.x,end.transform.position.y,2));
		tmp=last;
		int c=1;
		line.SetVertexCount(chainCount+2);
		while(tmp!=null)
		{
			line.SetPosition(c,new Vector3(tmp.go.transform.position.x,tmp.go.transform.position.y,2));
			tmp=tmp.prev;
			c++;
		}
		line.SetPosition(c,new Vector3(ground.transform.position.x,ground.transform.position.y,2));
	}
	
	void FixedUpdate()
	{
		test1=new Vector2(end.transform.position.x,end.transform.position.y);
		Debug.Log("Speed: "+Vector2.Distance(test2,test1));
		test2=test1;
		
		
		DrawLine();

		if(Input.GetKey(KeyCode.DownArrow))
		{
			Expand();
		}
		if(Input.GetKey(KeyCode.UpArrow))
			Revert();
		
		if(Input.GetKey(KeyCode.LeftArrow))
			cameraAngle-=1.5f;
		else if(Input.GetKey(KeyCode.RightArrow))
			cameraAngle+=1.5f;
		
		Camera.main.transform.position=new Vector3(end.transform.position.x,end.transform.position.y,-10);
		cameraAngle=Mathf.Repeat(cameraAngle,360.0f);
		Camera.main.transform.localEulerAngles=new Vector3(0,0,cameraAngle);
		gravityVec = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height*0.5f,0)).x,Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height*0.5f,0)).y)-new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height,0)).x,Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height,0)).y);
		gravityVec/=gravityVec.magnitude;
		gravityVec*=GameOptions.GRAVITY_VALUE;
		Physics2D.gravity=gravityVec;
		
	}
}
