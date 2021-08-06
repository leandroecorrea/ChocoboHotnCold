using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



public abstract class Item : GameObjectBase
{
    protected string tag;    
    private int xPoints;
    private bool toRemove;

    public Item(string tag, int xpPoints, string fileName) : base("Sprites" + Path.DirectorySeparatorChar + fileName)
    {        
        sprite.Scale = new Vector2f(5f, 5f);
        sprite.Position = new Vector2f(100f, 200f);
        this.tag = tag;        
        this.xPoints = xpPoints;
    }        
    public void ItemScaleNPosition(Vector2f size, Vector2f position)
    {
        sprite.Scale = size;
        currentPosition = position;        
    }      
    public int GetXPoints()
    {
        return xPoints;
    }
    public bool ToRemove
    {
        get { return toRemove; }
        set { toRemove = value; }
    }
}

public class PhoenixDown : Item
{
    public PhoenixDown() : base ("PhoenixDown", 500, "phoenixdown.png")
    {

    }
}
public class Rod : Item
{
    public Rod() : base ("Rod", 650, "rod.png")
    {

    }
}

public class Dagger : Item
{
    public Dagger() : base ("Rod", 750, "dagger.png")
    {

    }
}

