using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameState : MonoBehaviour {
    private static GameState _instance;
    public static GameState Instance { get { return _instance; } }

    void Awake() {
        if (_instance != null) {
            Destroy(this);
            return;
        }

        _instance = this;
    }

    List<int> cellsOpen = Enumerable.Range(0, 27).ToList();
    public int[] gameState = new int[27];
    public List<Board> boards;

    public GameObject tempToken;

    public Player player;

    public Face front;
    public Face back;
    public Face top;
    public Face bottom;
    public Face left;
    public Face right;

    public void AddBoard(Board board) {
        boards.Add(board);
    }

    public bool CellOpen(int index) {
        return gameState[index] == 0;
    }

    public void PlaceToken(Vector3Int pos, int player, GameObject token) {
        Face.Cell frontCell = new(pos[1], player);
        front.UpdateCell(pos[0], pos[2], frontCell);

        Face.Cell backCell = new(2 - pos[1], 1);
        back.UpdateCell(pos[0], 2 - pos[2], backCell);

        Face.Cell topCell = new(2 - pos[0], 1);
        top.UpdateCell(pos[1], pos[2], topCell);

        Face.Cell bottomCell = new(pos[0], 1);
        bottom.UpdateCell(2 - pos[1], pos[2], bottomCell);

        Face.Cell leftCell = new(2 - pos[2], 1);
        left.UpdateCell(pos[0], pos[1], leftCell);

        Face.Cell rightCell = new(pos[2], 1);
        right.UpdateCell(pos[0], 2 - pos[1], rightCell);

        int index = (pos[0] * 9) + (pos[1] * 3) + pos[2];

        foreach (Board board in boards) {
            board.PlaceToken(index, token);
        }

        cellsOpen.Remove(index);
        gameState[index] = player;
    }

    public int CheckState() {
        List<int> states = new();

        states.Add(front.CheckFace());
        states.Add(back.CheckFace());
        states.Add(left.CheckFace());
        states.Add(right.CheckFace());
        states.Add(top.CheckFace());
        states.Add(bottom.CheckFace());

        if (states.Count(x => x == 1) > 3) {
            Debug.Log("Player Wins");
            return -1;
        }

        else if (states.Count(x => x == -1) > 3) {
            Debug.Log("CPU Wins");
            return 1;
        }

        else if (cellsOpen.Count == 0) {
            Debug.Log("Draw.");
            return 2;
        }

        else {
            return 0;
        }
    }

    public IEnumerator AiPlay() {
        if (CheckState() != 0) {
            yield break;
        }

        yield return new WaitForSeconds(2.0f);

        int index = cellsOpen[Random.Range(0, cellsOpen.Count)];
        Vector3Int pos = Vector3Int.zero;

        pos[0] = index / 9;
        pos[1] = (index % 9) / 3;
        pos[2] = (index % 9) % 3;

        PlaceToken(pos, -1, tempToken);

        if (CheckState() != 0) {
            yield break;
        }

        StartCoroutine(player.RespawnToken());
    }
}
