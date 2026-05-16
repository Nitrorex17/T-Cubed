using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {
    public float speed = 1f;
    float timer = 0f;
    bool idle = true;

    float maxDis = 1f;
    Vector3 minPos;
    Vector3 midPos;
    Vector3 maxPos;

    Vector3 startPos;
    Vector3 direction;

    Board board;

    void Start() {
        board = transform.parent.GetComponent<Board>();

        startPos = transform.localPosition;
        direction = startPos - Vector3.zero;

        minPos = transform.localPosition;
        midPos = transform.localPosition + (direction * (maxDis / 2));
        maxPos = transform.localPosition + (direction * maxDis);

        //transform.localPosition = midPos;
        timer += Random.Range(0, Mathf.PI);
    }

    void Update() {
        transform.localPosition = startPos + (direction * board.separation * 2);
    }
    
    //void Update() {
    //    if (idle) {
    //        float separation = 0.5f + (Mathf.Sin(timer) * 0.25f);
    //        transform.localPosition = Vector3.Lerp(minPos, maxPos, separation);

    //        timer += Time.deltaTime;
    //    }
    //}
}