using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public GUIText countText;
	public GUIText winText;
	private int count;

	void Start() {
		count = 0;
		SetCountText();
		winText.text = "";
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		rigidbody.AddForce(movement * speed * Time.deltaTime);

		if (isGod()) {
			countText.text = "You are a GOD";
		} else {
			countText.text = "";
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PickUp") {
			other.gameObject.SetActive(false);
			count++;
			SetCountText();
		} else if (other.gameObject.tag == "Enemy" && !isGod()) {
			this.gameObject.SetActive(false);
			SetLoseText();
		}
	}

	void SetCountText() {
		if (isWin()) {
			winText.text = "You win!";
		}
	}

	void SetLoseText() {
		if (!isWin ()) {
			winText.text = "You lose :(";
		} else {
			winText.text = "Feeling fat?";
		}
	}

	bool isWin() {
		return (count >= 1);
	}

	bool isGod() {
		return Input.GetKey("g");
	}
}
