using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStar : MonoBehaviour {
    public Grid grid;

    /// <summary>
    /// 寻路，返回路径或者空
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <param name="onComplete"></param>
    public void FindPath(Node startNode, Node endNode, Action<List<Node>> onComplete) {
        StartCoroutine(FindPathCor(startNode, endNode, onComplete));
    }
    IEnumerator FindPathCor(Node startNode, Node endNode, Action<List<Node>> onComplete) {
        //开始计时
        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        //开集和闭集
        Heap<Node> openList = new Heap<Node>(grid.maxSize);
        HashSet<Node> closeList = new HashSet<Node>();

        //添加初始节点
        openList.Add(startNode);

        //遍历开集节点
        while (openList.Count > 0) {
            //取出消耗最低的节点
            Node node = openList.RemoveFirst();
            closeList.Add(node);

            //如果就是终点
            if (node == endNode) {
                //生成路径然后返回
                List<Node> path = new List<Node>();
                while (node != startNode) {
                    path.Add(node);

                    node = node.parentNode;
                }

                path.Add(startNode);

                path.Reverse();

                //停止计时
                timer.Stop();
                Debug.Log(timer.ElapsedMilliseconds.ToString() + "ms");

                onComplete?.Invoke(path);

                yield break;
            }

            //获取周围一圈的节点，计算g消耗，加入开集
            foreach (var item in GetNeighbourNodes(node)) {
                //在闭集中或者不可通行，跳过
                if (closeList.Contains(item) || !item.walkable)
                    continue;

                //如果这条路径到该节点路程更短，或者该点不在开集中
                float newCost = node.g + EstimateNodeDistance(node, item);
                if (newCost < item.g || !openList.Contains(item)) {
                    item.g = newCost;
                    item.h = EstimateNodeDistance(item, endNode);
                    item.parentNode = node;

                    if (openList.Contains(item) == false)
                        openList.Add(item);
                }
            }
        }

        onComplete?.Invoke(null);
    }

    //估算节点间距离
    float EstimateNodeDistance(Node node1, Node node2) {
        float distanceX = Mathf.Abs(node1.x - node2.x);
        float distanceY = Mathf.Abs(node1.y - node2.y);

        //如果水平距离更远，先算出斜线距离
        if (distanceX > distanceY) {
            return 1.414f * distanceY + (distanceX - distanceY);
        }

        return 1.414f * distanceX + (distanceY - distanceX);
    }

    //获取周围一圈的节点
    List<Node> GetNeighbourNodes(Node node) {
        List<Node> nodeList = new List<Node>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0)
                    continue;

                int x = node.x + i;
                int y = node.y + j;
                if (x >= 0 && x < grid.mapSize.x && y >= 0 && y < grid.mapSize.y) {
                    nodeList.Add(grid.GetNode(x, y));
                }
            }
        }

        return nodeList;
    }
}