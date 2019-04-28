using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class ImpactReceiver : MonoBehaviour {

	// Use this for initialization

	// this is like health
	public float multiplier = 1.01f;
	public float incrementAmount = 10f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnParticleCollision (GameObject col){
		Debug.Log("COLLISION");
		Debug.Log(col.tag);
		if (col.tag == "Spells"){
			Debug.Log("SPELL COLLISION");
			Vector3 moveDirection = transform.position - col.transform.position;
			gameObject.GetComponent<Rigidbody>().AddForce (moveDirection * col.GetComponent<SpellController>().force * multiplier);
			multiplier += col.GetComponent<SpellController>().damage * 0.01f;
		}
	}
}
