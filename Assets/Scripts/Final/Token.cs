using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {
    float duration = 0.4f;
    Collider col;

    void Start() {
        col = GetComponent<Collider>();
    }

    public IEnumerator Move(Transform parent) {
        col.enabled = !col.enabled;
        transform.SetParent(parent);

        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        float time = 0;
        while (time < duration) {
            float lerpFactor = Mathf.SmoothStep(0, 1, (time / duration));

            Vector3 newPos = Vector3.Lerp(startPos, Vector3.zero, lerpFactor);
            Quaternion newRot = Quaternion.Lerp(startRot, Quaternion.identity, lerpFactor);
            transform.SetLocalPositionAndRotation(newPos, newRot);

            time += Time.deltaTime;
            yield return null;
        }

        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
