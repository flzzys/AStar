using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node[,] nodes;

    public Vector2Int mapSize = new Vector2Int(8, 8);

    float spacing = 1.2f;

    public LayerMask unwalkableMask;

    public List<Node> path;

	public bool onlyDisplayPath;

	public int maxSize { get { return mapSize.x * mapSize.y; } }

    private void OnDrawGizmos()
    {
        Vector2 startingPos = new Vector2(-mapSize.x / 2, -mapSize.y / 2) * spacing;

		if(onlyDisplayPath) {
			if (path != null) {
				foreach (var item in path) {
					Vector2 pos = new Vector2(item.x, item.y) * spacing + startingPos;
					Gizmos.color = Color.blue;
					Gizmos.DrawWireCube(pos, Vector3.one);
				}
			}

			return;
		}

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector2 pos = new Vector2(x, y) * spacing + startingPos;

                if(nodes != null)
                    Gizmos.color = nodes[x, y].walkable ? Color.green : Color.red;

                if(path != null && path.Contains(nodes[x, y]))
                {
                    Gizmos.color = Color.blue;
                }

                Gizmos.DrawWireCube(pos, Vector3.one);
            }
        }
    }

    void Start()
    {
        GenerateMap();
    }

    //生成地图
    void GenerateMap()
    {
        Transform nodeParent = new GameObject("NodeParent").transform;
        nodes = new Node[mapSize.x, mapSize.y];

        Vector2 startingPos = new Vector2(-mapSize.x / 2, -mapSize.y / 2) * spacing;
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector2 pos = new Vector2(x, y) * spacing + startingPos;

                //判定旁边是否有不可通行图层物体
                bool walkable = !Physics2D.OverlapCircle(pos, spacing / 2, unwalkableMask);

                Node node = new Node(x, y, walkable);
                nodes[x, y] = node;
            }
        }
    }

    //从世界坐标获取节点
    public Node GetNodeFromWorldPos(Vector2 worldPos)
    {
        float percentX = (worldPos.x + mapSize.x / 2 * spacing) / mapSize.x / spacing;
        float percentY = (worldPos.y + mapSize.y / 2 * spacing) / mapSize.y / spacing;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((mapSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((mapSize.y - 1) * percentY);

        return nodes[x, y];
    }

}