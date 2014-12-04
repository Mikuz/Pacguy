using UnityEngine;
using System.Collections;

public class GroundMaterial : MonoBehaviour {

	public Color color = new Color32(0, 0, 0, 1);
	
	// Use this for initialization
	void Start () {
		gameObject.renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
