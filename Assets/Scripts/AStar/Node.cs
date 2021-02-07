﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public float g, h;
    public float f { get { return g + h; } }

    public Node parentNode;

    public int x, y;

    public bool walkable = true;

    public Node(int x, int y, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
    }

	int heapIndex;
	public int HeapIndex {
		get { return heapIndex; }
		set { heapIndex = value; }
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = f.CompareTo(nodeToCompare.f);
		if(compare == 0) {
			compare = h.CompareTo(nodeToCompare.h);
		}

		return -compare;
	}
}