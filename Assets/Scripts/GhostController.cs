using UnityEngine;
using System.Collections;
using System;

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
		if (freeze <= 0) {

			if (other.gameObject.tag == "Blocker") {
				BlockerInfo blocker = (BlockerInfo) other.gameObject.GetComponent("BlockerInfo");
				if (direction == blocker.inDirection) {
					Direction randomDirection;
					do {
						randomDirection = getRandomDirection();
					} while (!blocker.outDirections.Contains(randomDirection));

					if (new System.Random().Next(3) == 0) {
						// 25% chance to not change direction
						CounterMove();
						direction = randomDirection;
						Debug.Log("Blocker changing to: " + randomDirection);
					} else {
						Debug.Log("Blocker not changing to: " + randomDirection);
					}
				} else {
					Debug.Log("Blocker wrong direction");
				}
			} else if (other.gameObject.tag != "Player" && 
			           other.gameObject.tag != "Enemy" &&
			           other.gameObject.tag != "Ghost" &&
			           other.gameObject.tag != "PickUp" &&
			           freeze <= 0) {
				//Debug.Log("Tag " + other.gameObject.tag);
				SwitchDirection ();
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
	
	void SwitchDirection() {
		Direction randomDirection;
		do {
			randomDirection = getRandomDirection();
		} while (randomDirection == direction || 
		         randomDirection == lockDirection);

		if (randomDirection != getCounterDirection(direction)) {
			// Lock current direction if ghost dosn't back off from it
			lockDirection = direction;
		}

		CounterMove();
		direction = randomDirection;

	}

	void CounterMove() {
		// Move slightly back so that passing a gap sideways
		// doesn't trigger OnTriggerEnter
		Vector3 counterMove = GetMovement(this.direction, 0.1f);
		transform.position = transform.position - counterMove;
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
