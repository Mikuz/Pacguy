using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public GameObject goalObject;
	private int count;
	private bool dead;

	void Start() {
		count = 0;
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		rigidbody.AddForce(movement * speed * Time.deltaTime);

		if (isGod()) {
			gameObject.renderer.material.color = Color.yellow;
		} else {
			gameObject.renderer.material.color = Color.white;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PickUp") {
			other.gameObject.SetActive(false);
			count++;
		} else if (other.gameObject.tag == "Enemy" && !isGod()) {
			this.gameObject.SetActive(false);
			this.dead = true;
		}
	}
	
	public bool isWin() {
		return (count >= 2);
	}

	public bool isDead() {
		return this.dead;
	}

	public bool isGod() {
		return Input.GetKey("g");
	}
}
