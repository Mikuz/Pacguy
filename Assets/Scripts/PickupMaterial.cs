﻿using UnityEngine;
using System.Collections;

public class PickupMaterial : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
