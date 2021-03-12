using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    Node[,] nodes;

    //尺寸
    public Vector2Int mapSize = new Vector2Int(8, 8);

    //节点间距
    public float nodeSpacing = 1.2f;

    public LayerMask unwalkableMask;

    public List<Node> path;

    public int maxSize { get { return mapSize.x * mapSize.y; } }
    Vector2 mapWorldSize {
        get {
            return new Vector2(((float)mapSize.x - 1) / 2, ((float)mapSize.y - 1) / 2) * nodeSpacing * 2;
        }
    }

    void Awake() {
        GenerateMap();
    }

    //生成地图
    void GenerateMap() {
        nodes = new Node[mapSize.x, mapSize.y];

        for (int y = 0; y < mapSize.y; y++) {
            for (int x = 0; x < mapSize.x; x++) {
                Vector2 pos = GetNodeWorldPos(x, y);

                //判定旁边是否有不可通行图层物体
                bool walkable = !Physics2D.OverlapCircle(pos, nodeSpacing / 2, unwalkableMask);

                Node node = new Node(x, y, walkable);
                nodes[x, y] = node;
            }
        }
    }

    #region Node

    //获取节点
    public Node GetNode(int x, int y) {
        return nodes[x, y];
    }

    //获取节点世界位置
    Vector2 GetNodeWorldPos(int x, int y) {
        Vector2 startingPos = -mapWorldSize / 2;
        Vector2 pos = startingPos + new Vector2(x, y) * nodeSpacing;
        return pos;
    }

    //从世界坐标获取节点
    public Node GetNodeFromWorldPos(Vector2 worldPos) {
        //转换成百分比
        float percentX = (worldPos.x + mapWorldSize.x / 2) / mapWorldSize.x;
        float percentY = (worldPos.y + mapWorldSize.y / 2) / mapWorldSize.y;

        //限制在0到1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((mapSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((mapSize.y - 1) * percentY);

        return nodes[x, y];
    }

    public HashSet<Node> GetNodes() {
        HashSet<Node> set = new HashSet<Node>();
        for (int x = 0; x < nodes.GetLength(0); x++) {
            for (int y = 0; y < nodes.GetLength(1); y++) {
                set.Add(GetNode(x, y));
            }
        }
        return set;
    }

    #endregion

    //Gizmos
    private void OnDrawGizmos() {
        for (int y = 0; y < mapSize.y; y++) {
            for (int x = 0; x < mapSize.x; x++) {
                Vector2 pos = GetNodeWorldPos(x, y);

                if (nodes != null) {
                    Node node = GetNode(x, y);

                    //可行走区域
                    Gizmos.color = node.walkable ? Color.white : Color.red;

                    //路线
                    if (path != null && path.Contains(node)) {
                        Gizmos.color = Color.blue;
                    }
                }

                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }

}