using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

public class Treasure
{
    private int depth;
    private string item;
    private Vector2f position;

    public Treasure (int depth, string item, Vector2f position)
    {
        this.depth = depth;
        this.item = item;
        this.position = position;
    }

    public int Depth 
    {
        get { return depth; }
        set { depth = value; }
    }

    public Vector2f Position
    {
        get { return position; }
        set { position = value; }
    }
    public string Item
    {
        get { return item; }
        set { item = value; }
    }

    public bool DugUpTreasure(int peckDepth) => (depth -= peckDepth) <= 0 ? true : false;
}

