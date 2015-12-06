using UnityEngine;
using System.Collections;

public class BotBeh : MonoBehaviour {

	GameObject chainElement;
	GameObject lastElement;
	public GameObject groundElement;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
