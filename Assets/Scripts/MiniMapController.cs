using UnityEngine;
using System.Collections;

public class MiniMapController : MonoBehaviour {

	private static readonly float MM_FOV = 30.0f;
	private static readonly float NO_MM_FOV = 55f;
	private static readonly float VIEWPORT_W = 0.40f;
	private static readonly float VIEWPORT_H = 0.35f;

	public GameObject mainCamera;

	private bool mapBeenClosed;
	private bool minimap;
	private float destinyFov;
	private float viewportW;
	private float viewportH;

	// Use this for initialization
	void Start () {
		mapBeenClosed = false;
		minimap = true;
		destinyFov = MM_FOV;
		viewportW = VIEWPORT_W;
		viewportH = VIEWPORT_H;
	}
	
	// Update is called once per frame
	void Update () {
		// Check input
		if (Input.GetKeyDown ("m")) {
			if (minimap) {
				mapBeenClosed = true;
				destinyFov = NO_MM_FOV;
				minimap = false;
			} else {
				destinyFov = MM_FOV;
				minimap = true;
			}
		}

		// Adjust FOV
		if (mainCamera.camera.fieldOfView != destinyFov) {
			float fovChange = 50 * Time.deltaTime * (minimap ? -1 : 1);
			float nextFov = mainCamera.camera.fieldOfView + fovChange;
			if (( minimap && nextFov > destinyFov) ||
			    (!minimap && nextFov < destinyFov)) {
				mainCamera.camera.fieldOfView = nextFov;
			} else {
				mainCamera.camera.fieldOfView = destinyFov;
			}
		}

		// Adjust minimap
		if (minimap) {
			if (Mathf.Abs(mainCamera.camera.fieldOfView - destinyFov) < 10) {
				this.camera.enabled = true;
				viewportW = viewportW + 1.5f * Time.deltaTime;
				viewportH = viewportH + 1.5f * Time.deltaTime;
				if (viewportW > VIEWPORT_W) viewportW = VIEWPORT_W;
				if (viewportH > VIEWPORT_H) viewportH = VIEWPORT_H;
			}
		} else {
			viewportW = viewportW - 1.5f * Time.deltaTime;
			viewportH = viewportH - 1.5f * Time.deltaTime;
			if (viewportW < 0) viewportW = 0;
			if (viewportH < 0) viewportH = 0;
			if (viewportW == 0 && viewportH == 0) {
				this.camera.enabled = false;
			}
		}

		this.camera.rect = new Rect(0.01f, 0.01f, viewportW, viewportH);
	}

	public bool isMapOpenFirstTime() {
		return (minimap && !mapBeenClosed);
	}
}
