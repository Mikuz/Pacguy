using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	private int count;
	private bool dead;
	public AudioSource dyingSound;
	public AudioSource pickupSound;
	public AudioSource winSound;

	void Start() {
		count = 0;
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		rigidbody.AddForce(movement * speed * Time.deltaTime);

		if (isGod()) {
			gameObject.renderer.material.color = Color.red;
		} else {
			gameObject.renderer.material.color = Color.white;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PickUp") {
			other.gameObject.SetActive(false);
			count++;
			pickupSound.Play();
			if (isWin ()) {
				winSound.Play();
			}
		} else if (other.gameObject.tag == "Enemy" && !isGod()) {
			this.gameObject.SetActive(false);
			this.dead = true;
			this.dyingSound.Play();
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
