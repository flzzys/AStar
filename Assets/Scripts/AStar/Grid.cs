using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public Node[,] nodes;

    //尺寸
    public Vector2Int mapSize = new Vector2Int(8, 8);

    //节点间距
    float nodeSpacing = 1.2f;

    public LayerMask unwalkableMask;

    public List<Node> path;

    public bool onlyDisplayPath;

    public int maxSize { get { return mapSize.x * mapSize.y; } }

    void Start() {
        GenerateMap();
    }

    //生成地图
    void GenerateMap() {
        Transform nodeParent = new GameObject("NodeParent").transform;
        nodes = new Node[mapSize.x, mapSize.y];

        for (int y = 0; y < mapSize.y; y++) {
            for (int x = 0; x < mapSize.x; x++) {
                Vector2 pos = GetNodePos(x, y);

                //判定旁边是否有不可通行图层物体
                bool walkable = !Physics2D.OverlapCircle(pos, nodeSpacing / 2, unwalkableMask);

                Node node = new Node(x, y, walkable);
                nodes[x, y] = node;
            }
        }
    }

    //从世界坐标获取节点
    public Node GetNodeFromWorldPos(Vector2 worldPos) {
        float percentX = (worldPos.x + mapSize.x / 2 * nodeSpacing) / mapSize.x / nodeSpacing;
        float percentY = (worldPos.y + mapSize.y / 2 * nodeSpacing) / mapSize.y / nodeSpacing;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((mapSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((mapSize.y - 1) * percentY);

        return nodes[x, y];
    }

    //获取节点位置
    Vector2 GetNodePos(int x, int y) {
        Vector2 startingPos = new Vector2(-((float)mapSize.x - 1) / 2, -((float)mapSize.y - 1) / 2) * nodeSpacing;
        return startingPos + new Vector2(x, y) * nodeSpacing;
    }

    //Gizmos
    private void OnDrawGizmos() {
        if (onlyDisplayPath) {
            if (path != null) {
                foreach (var node in path) {
                    Vector2 pos = GetNodePos(node.x, node.y);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(pos, Vector3.one);
                }
            }

            return;
        }

        for (int y = 0; y < mapSize.y; y++) {
            for (int x = 0; x < mapSize.x; x++) {
                Vector2 pos = GetNodePos(x, y);

                if (nodes != null)
                    Gizmos.color = nodes[x, y].walkable ? Color.green : Color.red;

                if (path != null && path.Contains(nodes[x, y])) {
                    Gizmos.color = Color.blue;
                }

                Gizmos.DrawWireCube(pos, Vector3.one);
            }
        }
    }

}