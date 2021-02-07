using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    AStar aStar;
    Grid grid;

    private void Awake() {
        aStar = GetComponent<AStar>();
        grid = GetComponent<Grid>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Node node = grid.GetNodeFromWorldPos(mouseWorldPos);

            grid.path = aStar.FindPath(grid.nodes[0, 0], node);
        }

        //if (Input.GetKeyDown("d"))
        //{
        //    //FindPath(nodes[0], nodes[499]);

        //    foreach (var item in aStar.FindPath(nodes[0], nodes[499]))
        //    {
        //        item.GetComponent<SpriteRenderer>().color = Color.green;
        //    }
        //}

        //if (Input.GetKeyDown("f"))
        //{
        //    foreach (var item in FindPath(nodes[0], nodes[nodes.Length - 1]))
        //    {
        //        item.GetComponent<SpriteRenderer>().color = Color.green;
        //    }
        //}
    }

}