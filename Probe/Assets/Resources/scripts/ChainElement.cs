using UnityEngine;
using System.Collections;

public class ChainElementBeh : MonoBehaviour {

	public GameObject prev;
	// Use this for initialization
	
	private const float dist = 0.25f;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Vector2.Distance(new Vector2(transform.position.x,transform.position.y),new Vector2(prev.transform.position.x,prev.transform.position.y))>dist)
		{
			Vector2 nv = new Vector2(transform.position.x,transform.position.y)-new Vector2(prev.transform.position.x,prev.transform.position.y);
			nv/=nv.magnitude;
			nv*=dist;
			transform.position=new Vector3(nv.x,nv.y,1);
		}
	}
}
