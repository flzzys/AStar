using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    //寻路，返回路径或者空
    public List<Node> FindPath(Node startNode, Node endNode)
    {
        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        Heap<Node> openList = new Heap<Node>(grid.maxSize);
        HashSet<Node> closeList = new HashSet<Node>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
			//取出消耗最低的节点
			Node node = openList.RemoveFirst();
            closeList.Add(node);

            //如果就是终点
            if(node == endNode)
            {
                //生成路径然后返回
                List<Node> path = new List<Node>();
                while (node.parentNode != null)
                {
                    path.Add(node);

                    node = node.parentNode;
                }

                path.Reverse();

				timer.Stop();
                Debug.Log(timer.ElapsedMilliseconds.ToString() + "ms");

                return path;
            }

            //获取周围一圈的节点，计算g消耗，加入开集
            foreach (var item in GetNeighbourNodes(node))
            {
                if (closeList.Contains(item) || !item.walkable)
                    continue;

                //如果这条路径到该节点路程更短，或者该点不在开集中
                float newCost = node.g + EstimateNodeDistance(node, item);
                if (newCost < item.g || !openList.Contains(item))
                {
                    item.g = newCost;
                    item.h = EstimateNodeDistance(item, endNode);
                    item.parentNode = node;

                    if (openList.Contains(item) == false)
                        openList.Add(item);
                }
            }
        }

        return null;
    }

    //估算节点间距离
    float EstimateNodeDistance(Node node1, Node node2)
    {
        float distanceX = Mathf.Abs(node1.x - node2.x);
        float distanceY = Mathf.Abs(node1.y - node2.y);

        //如果水平距离更远，先算出斜线距离
        if(distanceX > distanceY)
        {
            return 1.414f * distanceY + (distanceX - distanceY);
        }

        return 1.414f * distanceX + (distanceY - distanceX);
    }

    //获取周围一圈的节点
    List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> nodeList = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int x = node.x + i;
                int y = node.y + j;
                if(x >= 0 && x < grid.mapSize.x && y >= 0 && y < grid.mapSize.y)
                {
                    nodeList.Add(grid.nodes[x, y]);
                }
            }
        }

        return nodeList;
    }
}