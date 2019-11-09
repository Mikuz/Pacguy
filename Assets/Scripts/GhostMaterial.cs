using UnityEngine;
using System.Collections;

public class GhostMaterial : MonoBehaviour {

	public Color color = new Color32(127, 0, 255, 1);

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
