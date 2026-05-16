using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Camera cam;

    public GameObject gameToken;
    public GameObject displayToken;

    public Board board;

    Token token;
    bool holding = false;
    bool inputMode = false;

    public Transform platform;
    public Transform platformPos;
    public Transform sensorPos;
    public Transform holdPos;

    Vector3 up = new(0.175f, 0.46f, 0.08f);
    Vector3 down = new(0.175f, 0.385f, 0.08f);

    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        Vector3 mousePos = Input.mousePosition;

        float xRatio = Mathf.Clamp((mousePos.y / Screen.height), 0.1f, 0.9f);
        float yRatio = Mathf.Clamp((mousePos.x / Screen.width), 0.1f, 0.9f);

        float xAngle = -(xRatio * 10) + 20;
        float yAngle = (yRatio * 12) - 6;

        transform.localEulerAngles = new Vector3(xAngle, yAngle, 0);

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100)) {
                OnClick(hit.transform);
            }
        }
    }

    void OnClick(Transform other) {
        if (holding) {
            if (other.CompareTag("Platform")) {
                StartCoroutine(token.Move(platformPos));

                holding = false;
            }

            else if (other.CompareTag("Sensor")) {
                StartCoroutine(token.Move(sensorPos));

                StartCoroutine(board.Open());
                board.ToggleMarker();

                holding = false;
                inputMode = true;

            }
        }

        else {
            if (other.CompareTag("Token")) {
                token = other.GetComponent<Token>();

                StartCoroutine(token.Move(holdPos));
                StartCoroutine(board.Close());

                if (inputMode) {
                    board.ToggleMarker();
                }

                holding = true;
                inputMode = false;
            }

            else if (other.CompareTag("Lever") && inputMode && GameState.Instance.CellOpen(board.MarkerIndex())) {
                other.GetComponent<Lever>().Pull();
                StartCoroutine(InputToken());

                inputMode = false;
            }
        }
    }

    IEnumerator InputToken() {
        Destroy(token.gameObject);

        yield return new WaitForSeconds(0.5f);

        GameState.Instance.PlaceToken(board.markerPos, 1, displayToken);
        board.ToggleMarker();
        
        StartCoroutine(board.Close());

        StartCoroutine(GameState.Instance.AiPlay());
    }

    public IEnumerator RespawnToken() {
        Vector3 startPos = platform.transform.localPosition;
        Vector3 targetPos = down;

        float time = 0;
        while (time < 1.0f) {
            float lerpFactor = Mathf.SmoothStep(0, 1, (time / 1.0f));

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, lerpFactor);
            platform.transform.localPosition = newPos;

            time += Time.deltaTime;
            yield return null;
        }

        platform.transform.localPosition = targetPos;

        Instantiate(gameToken, platformPos);

        startPos = platform.transform.localPosition;
        targetPos = up;

        time = 0;
        while (time < 1.0f) {
            float lerpFactor = Mathf.SmoothStep(0, 1, (time / 1.0f));

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, lerpFactor);
            platform.transform.localPosition = newPos;

            time += Time.deltaTime;
            yield return null;
        }

        platform.transform.localPosition = targetPos;
    }
}
