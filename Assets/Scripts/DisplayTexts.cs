using UnityEngine;
using System.Collections;

public class DisplayTexts : MonoBehaviour {

	public PlayerController player;
	public GUIText statusText;
	public GUIText winText;

	// Use this for initialization
	void Start () {
		winText.text = "";
		statusText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		updateTexts();
	}

	void updateTexts() {
		if (player.isDead()) {
			if (!player.isWin ()) {
				winText.text = "You lose :(";
			} else {
				winText.text = "Feeling fat?";
			}
		} else if (player.isWin()) {
			winText.text = "You win!";
		}
		
		if (player.isGod()) {
			statusText.text = "You are a GOD";
		} else {
			statusText.text = "";
		}
	}
	
	void OnGUI() {
		if (player.isWin() || player.isDead()) {
			if (GUI.Button(
					new Rect(
						Screen.width - 75 - 10, 
						Screen.height - 25 - 5, 
						75, 
						25), 
					"Restart")) {
				Application.LoadLevel("MainGame");
			}
		}
	}

}
