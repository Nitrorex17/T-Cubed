using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Face : MonoBehaviour {
    public struct Cell {
        public int priority;
        public int state;

        public Cell(int priority, int state) {
            this.priority = priority;
            this.state = state;
        }
    }

    int xWin = 0;
    int oWin = 0;
    Cell[,] board = new Cell[3, 3];

    List<List<(int, int)>> winLines = new() {
        new() { (0, 0), (0, 1), (0, 2) },
        new() { (1, 0), (1, 1), (1, 2) },
        new() { (2, 0), (2, 1), (2, 2) },
        new() { (0, 0), (1, 0), (2, 0) },
        new() { (0, 1), (1, 1), (2, 1) },
        new() { (0, 2), (1, 2), (2, 2) },
        new() { (0, 0), (1, 1), (2, 2) },
        new() { (0, 2), (1, 1), (2, 0) },
    };

    public void UpdateCell(int row, int col, Cell cell) {
        if (cell.priority >= board[row, col].priority) {
            board[row, col] = cell;
        }
    }

    public int CheckFace() {
        xWin = 0;
        oWin = 0;

        foreach (List<(int, int)> line in winLines) {
            var (row1, col1) = line[0];
            var (row2, col2) = line[1];
            var (row3, col3) = line[2];

            if (board[row1, col1].state + board[row2, col2].state + board[row3, col3].state == 3) {
                xWin = 1;
                break;
            }
        }

        foreach (List<(int, int)> line in winLines) {
            var (row1, col1) = line[0];
            var (row2, col2) = line[1];
            var (row3, col3) = line[2];

            if (board[row1, col1].state + board[row2, col2].state + board[row3, col3].state == -3) {
                oWin = -1;
                break;
            }
        }

        return xWin + oWin;
    }
}