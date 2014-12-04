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
			// E.g. (1/0.25)-1=3, Probability of 0 to 3 being 0 is 0.25 
			int probability = (int)Math.Round(1d/blocker.probability)-1;
			if (new System.Random().Next(probability) == 0) {
				ghostController.CounterMove();

				List<Direction> randomizable = new List<Direction>();
				foreach (Direction d in Enum.GetValues(typeof(Direction))) {
					if (blocker.outDirections.Contains(d)) randomizable.Add(d);
				}
				Direction randomDirection = GhostController.getRandomDirection(randomizable);
				ghostController.direction = randomDirection;
				ghostController.lockDirection = randomDirection; // Disable lock
				//Debug.Log("Blocker changing to: " + randomDirection);
			} else {
				//Debug.Log("Blocker not changing to: " + randomDirection);
			}
		} else {
			//Debug.Log("Blocker wrong direction");
		}
	}
}
