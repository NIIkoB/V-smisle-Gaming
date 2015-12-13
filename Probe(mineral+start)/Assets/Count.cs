using UnityEngine;
using System.Collections;

public class Count : MonoBehaviour 
{
	public float speed;
	private int count;
	public GUIText countText;
	public GUIText WinText;
	
	void Start()
	{
		count = 0;
		SetCountText();
		WinText.text = "";
	}
	
	
	void Update()
	{
		
	}
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		
		GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
	}
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "minerals") {
			other.gameObject.SetActive (false);
			count++;
			SetCountText();
		}
	}
	void SetCountText()
	{
		countText.text = "Count: " + count.ToString ();
		if(count>=9)
		{
			WinText.text = "YOU WIN!!!";
		}
	}
}