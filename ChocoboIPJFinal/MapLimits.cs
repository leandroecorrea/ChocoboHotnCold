using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
//main left 200 top 475 width 760 height 125
//top square left 0 top 0 height 150 width 943
//right top square left 943 top 0 height 209 width 150
//left bottom square top 209 height 275 width 805


class InvisibleWall : IColisionable
{
    private Vector2f position;
    private Vector2f size;
    private List<InvisibleWall> invisibleWalls;
    private FloatRect boxPosition;

    public InvisibleWall()
    {        
        invisibleWalls = new List<InvisibleWall>();
        addBoxesToObserver();
    }

    public InvisibleWall(float left, float top, float width, float height)
    {
        boxPosition = new FloatRect(left, top, width, height);
    }

    private void addBoxesToObserver()
    {        
        //topbox-vertical
        invisibleWalls.Add(new InvisibleWall(0f, 0f, 750f, 400f));
        //topbbox2-vertical
        invisibleWalls.Add(new InvisibleWall(751f, 0f, 50f, 375f));
        //topbox3-vertical
        invisibleWalls.Add(new InvisibleWall(802f, 0f, 25f, 350f));
        //topright1 square box
        invisibleWalls.Add(new InvisibleWall(828f, 0f, 120f, 160f));
        //topright2 square box
        invisibleWalls.Add(new InvisibleWall(950f, 0f, 100f, 200f));
        //right limit
        invisibleWalls.Add(new InvisibleWall(1051f, 0f, 10f, 600f));
        //left limit
        invisibleWalls.Add(new InvisibleWall(0f, 0f, -10f, 600f));
        invisibleWalls.Add(new InvisibleWall(0f, 401f, 50f, 50f));
        //bottom limit
        invisibleWalls.Add(new InvisibleWall(0f, 600f, 1049f, 10f));        ;
        foreach (InvisibleWall collisionBox in invisibleWalls)
        {
            CollisionObserver.GetInstance().AddToCollisionObserver(collisionBox);
            Console.WriteLine("added collisionbox");
        }
    }

    public FloatRect GetBounds()
    {
        return boxPosition;
    }

    public void OnCollision(IColisionable other)
    {
    }

    
}
