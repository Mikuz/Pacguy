using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GhostController : MonoBehaviour {

	public bool stop = false;
	public float speed;
	public Direction direction = Direction.UP;
	/// <summary>
	/// Direction that is already being touched
	/// </summary>
	private Direction lockDirection;
	/// <summary>
	/// Blocks direction changing
	/// </summary>
	private float freeze;
	private float grounded;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (grounded > 5.0f) {
			// When lag is really bad a ghost may get lost to the abyss
			Debug.LogWarning("Ghost has not been touching the Ground. Reset position.");
			transform.position = new Vector3(0, 1.5f, 0);
		}

		if (!stop) {
			Vector3 movement = GetMovement (this.direction, this.speed);
			transform.position = transform.position + (movement * Time.deltaTime);
		}

		freeze = (freeze > 0) ? freeze - Time.deltaTime : 0;
		grounded += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (!isFreeze()) {

			if (other.gameObject.tag != "Player" && 
			           other.gameObject.tag != "Enemy" &&
			           other.gameObject.tag != "PickUp" &&
			           other.gameObject.tag != "Blocker") {
				//Debug.Log("Tag " + other.gameObject.tag);
				WallDirectionSwitch();
			}
			//Debug.Log("Direction " + direction);

		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Ground") {
			// Failsafe, if the game lags ghost may leave the ground
			Debug.Log("Ghost left the Ground. Go " + direction);
			direction = getCounterDirection(direction);
			freeze = 1.0f;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Ground") {
			grounded = 0;
		}
	}
	
	void WallDirectionSwitch() {
		List<Direction> randomizable = new List<Direction>();
		foreach (Direction d in Enum.GetValues(typeof(Direction))) {
			if (d != direction && d != lockDirection) randomizable.Add(d);
		}
		Direction randomDirection = getRandomDirection(randomizable);

		if (randomDirection != getCounterDirection(direction)) {
			// Lock current direction if ghost dosn't back off from it
			lockDirection = direction;
		}

		CounterMove();
		direction = randomDirection;

	}

	public void CounterMove() {
		// Move slightly back so that passing a gap sideways
		// doesn't trigger OnTriggerEnter
		Vector3 counterMove = GetMovement(this.direction, 0.1f);
		transform.position = transform.position - counterMove;
	}

	public bool isFreeze() {
		return (this.freeze > 0);
	}

	static Vector3 GetMovement(Direction direction, float speed) {
		Vector3 movement;
		if (direction == Direction.UP) {
			movement = new Vector3(0, 0, speed);
		} else if (direction == Direction.DOWN) {
			movement = new Vector3(0, 0, -speed);
		} else if (direction == Direction.RIGHT) {
			movement = new Vector3(speed, 0, 0);
		} else {
			movement = new Vector3(-speed, 0, 0);
		}
		return movement;
	}

	public static Direction getRandomDirection(List<Direction> directions) {
		System.Random random = new System.Random();
		int index = random.Next(directions.Count);
		return directions[index];
	}

	static bool isDirectionVertical(Direction direction) {
		return direction == Direction.UP || direction == Direction.DOWN;
	}

	static Direction getCounterDirection(Direction direction) {
		if (direction == Direction.UP) return Direction.DOWN;
		else if (direction == Direction.DOWN) return Direction.UP;
		else if (direction == Direction.LEFT) return Direction.RIGHT;
		else return Direction.LEFT;
	}
}
