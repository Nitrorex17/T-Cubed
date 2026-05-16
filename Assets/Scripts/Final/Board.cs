using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public List<Transform> cells;
    public float separation;
    
    public Transform marker;
    bool active = false;
    public Vector3Int markerPos = Vector3Int.one;

    public float duration = 0.2f;

    // Start is called before the first frame update
    void Start() {
        GameState.Instance.AddBoard(this);
    }

    public void PlaceToken(int index, GameObject token) {
        Instantiate(token, cells[index]);
        StartCoroutine(Close());
    }

    public IEnumerator Open() {
        float time = 0;
        float currSep = separation;

        while (time < 0.5f) {
            float lerpFactor = Mathf.SmoothStep(0, 1, (time / 0.5f));
            separation = Mathf.Lerp(currSep, 0.5f, lerpFactor);

            separation = lerpFactor / 2;

            time += Time.deltaTime;
            yield return null;
        }

        separation = 0.5f;
    }

    public IEnumerator Close() {
        float time = 0;
        float currSep = separation;

        while (time < 0.5f) {
            float lerpFactor = Mathf.SmoothStep(0, 1, (time / 0.5f));
            separation = Mathf.Lerp(currSep, 0f, lerpFactor);

            time += Time.deltaTime;
            yield return null;
        }

        separation = 0f;
    }

    public void ToggleMarker() {
        active = !active;

        if (active) {
            marker.transform.SetParent(cells[13], false);
            markerPos = Vector3Int.one;
            StartCoroutine(ResizeMarker(Vector3.one * 0.8f));
        }

        else {
            StartCoroutine(ResizeMarker(Vector3.zero));
        }
    }

    public int MarkerIndex() {
        return (markerPos[0] * 9) + (markerPos[1] * 3) + markerPos[2];
    }
    
    public IEnumerator MoveMarker(int index, int value) {
        yield return StartCoroutine(ResizeMarker(Vector3.zero));

        markerPos[index] = Mathf.Clamp((markerPos[index] + value), 0, 2);

        int zoneIndex = (markerPos[0] * 9) + (markerPos[1] * 3) + (markerPos[2]);
        marker.transform.SetParent(cells[zoneIndex], false);

        yield return StartCoroutine(ResizeMarker(Vector3.one));
    }

    IEnumerator ResizeMarker(Vector3 targetSize) {
        Vector3 startSize = marker.localScale;

        float time = 0f;
        while (time < duration) {
            marker.localScale = Vector3.Lerp(startSize, targetSize, (time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        marker.localScale = targetSize;
    }
}
