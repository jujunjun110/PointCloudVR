using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {

    [SerializeField] GameObject PointCloudAnchor;
    [SerializeField] GameObject canvas;
    [SerializeField] Text mainText;
    private PointCloudLoader loader;
    private bool loaded = false;


    private PointCloudLoader getLoader() {
        if (!loader) {
            loader = PointCloudAnchor.GetComponent<PointCloudLoader>();
        }
        return loader;
    }

    private bool IsUIAccessible() {
        if (loaded) {
            return true;
        }

        if (getLoader().IsReady()) {
            // 初めてReadyになった瞬間のみ到達
            canvas.SetActive(false);
            loaded = true;
            return true;
        }

        return false;
    }

    void Update() {
        var loader = getLoader();

        if (!IsUIAccessible()) {
            return;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.X) || Input.GetKeyDown(KeyCode.X)) {
            loader.PointRadius -= 10;
            UpdateCanvas();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.Y) || Input.GetKeyDown(KeyCode.Y)) {
            loader.PointRadius += 10;
            UpdateCanvas();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.B)) {
            UpdateCanvas();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B) || Input.GetKeyDown(KeyCode.B)) {
            canvas.SetActive(false);
        }
    }

    private void UpdateCanvas() {
        loader.ReflectParams();
        canvas.SetActive(true);
        mainText.text = $"Point Size: {loader.PointRadius}\n(Press B to close)";
    }
}
