using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    [SerializeField] GameObject PointCloudAnchor;
    private PointCloudLoader loader;


    private PointCloudLoader getLoader() {
        if (!loader) {
            loader = PointCloudAnchor.GetComponent<PointCloudLoader>();
        }
        return loader;
    }

    void Update() {
        var loader = getLoader();
        if (OVRInput.GetDown(OVRInput.RawButton.X) || Input.GetKeyDown(KeyCode.X)) {
            loader.PointRadius -= 10;
            loader.OnValidate();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.Y) || Input.GetKeyDown(KeyCode.Y)) {
            loader.PointRadius += 10;
            loader.OnValidate();
        }
    }
}
