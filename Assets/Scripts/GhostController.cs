using UnityEngine;
using System.Collections;
using System;

public class GhostController : MonoBehaviour {

	enum Direction { UP, DOWN, LEFT, RIGHT };

	public float speed;
	private Direction direction;
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
		direction = Direction.UP;
	}
	
	// Update is called once per frame
	void Update () {
		if (grounded > 5.0f) {
			// When lag is really bad a ghost may get lost to the abyss
			Debug.LogWarning("Ghost has not been touching the Ground. Reset position.");
			transform.position = new Vector3(0, 1.5f, 0);
		}
		
		Vector3 movement = GetMovement();
		transform.position = transform.position + (movement * Time.deltaTime);

		freeze = (freeze > 0) ? freeze - Time.deltaTime : 0;
		grounded += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (!(other.gameObject.tag == "Player") && 
		    !(other.gameObject.tag == "Ghost") &&
		    freeze <= 0) {
			SwitchDirection ();
		}
		//Debug.Log("Direction " + direction);
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
	
	void SwitchDirection() {
		//transform.position = transform.position - (GetMovement() * 0.001f);
		Direction randomDirection;
		do {
			randomDirection = getRandomDirection();
		} while (randomDirection == direction || 
		         randomDirection == lockDirection);

		if (randomDirection != getCounterDirection(direction)) {
			// Lock current direction if ghost dosn't back off from it
			lockDirection = direction;
		}

		direction = randomDirection;
	}

	Vector3 GetMovement() {
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

	static Direction getRandomDirection() {
		Array values = Enum.GetValues(typeof(Direction));
		System.Random random = new System.Random();
		int randomDirection = random.Next(values.Length);
		return (Direction)values.GetValue(randomDirection);
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
