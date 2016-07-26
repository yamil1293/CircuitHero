using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    public Camera mainCamera;
    float shakeAmount = 0;

    void Awake() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            Shake(0.1f, 0.2f);
        }
    }

    public void Shake(float amount, float length) {
        shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake() {
        if (shakeAmount > 0) {
            Vector3 cameraPosition = mainCamera.transform.position;
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            cameraPosition.x += offsetX;
            cameraPosition.y += offsetY;
            mainCamera.transform.position = cameraPosition;
        }
    }

    void StopShake() {
        CancelInvoke("DoShake");
        mainCamera.transform.localPosition = Vector3.zero;
    }
}
