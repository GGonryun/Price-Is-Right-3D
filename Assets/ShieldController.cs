using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void Bind(GameObject parent){
		gameObject.transform.SetParent(parent.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
