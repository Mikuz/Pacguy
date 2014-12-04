using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GhostBlockerCollisionController : MonoBehaviour {

	public GhostController ghostController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (!ghostController.isFreeze()) {
			
			if (other.gameObject.tag == "Blocker") {
				BlockerDirectionSwitch(other.gameObject);
			}
		}
	}

	void BlockerDirectionSwitch(GameObject blockerGameObject) {
		BlockerInfo blocker = (BlockerInfo) blockerGameObject.GetComponent("BlockerInfo");
		if (ghostController.direction == blocker.inDirection) {
			List<Direction> randomizable = new List<Direction>();
			foreach (Direction d in Enum.GetValues(typeof(Direction))) {
				if (blocker.outDirections.Contains(d)) randomizable.Add(d);
			}
			Direction randomDirection = GhostController.getRandomDirection(randomizable);
			
			
			if (new System.Random().Next(3) == 0) {
				// 25% chance to not change direction
				ghostController.CounterMove();
				ghostController.direction = randomDirection;
				Debug.Log("Blocker changing to: " + randomDirection);
			} else {
				Debug.Log("Blocker not changing to: " + randomDirection);
			}
		} else {
			Debug.Log("Blocker wrong direction");
		}
	}
}
