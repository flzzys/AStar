using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    AStar aStar;
    Grid grid;

    Node currentNode;

    private void Awake() {
        aStar = GetComponent<AStar>();
        grid = GetComponent<Grid>();
    }

    private void Start() {
        currentNode = grid.GetNode(0, 0);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Node node = grid.GetNodeFromWorldPos(mouseWorldPos);

            aStar.FindPath(currentNode, node, path => {
                grid.path = path;

                if(path == null) {
                    print("无路径");
                } 
            });
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Node node = grid.GetNodeFromWorldPos(mouseWorldPos);
            currentNode = node;
        }
    }
}