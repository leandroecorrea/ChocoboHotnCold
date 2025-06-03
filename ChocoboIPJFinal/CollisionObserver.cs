using System;
using System.Collections.Generic;
using System.Text;


class CollisionObserver
{
    private static CollisionObserver instance;
    public static CollisionObserver GetInstance()
    {
        if(instance == null)
        {
            instance = new CollisionObserver();
        }
        return instance;
    }
    private List<IColisionable> collisionables;
    private List<KeyValuePair<IColisionable, IColisionable>> collisionRegister;

    private CollisionObserver()
    {
        collisionables = new List<IColisionable>();
        collisionRegister = new List<KeyValuePair<IColisionable, IColisionable>>();
    }

    public void AddToCollisionObserver(IColisionable collisionable)
    {
        collisionables.Add(collisionable);
    }

    public void RemoveFromCollisionObserver(IColisionable collisionable)
    {
        if (collisionables.Contains(collisionable))
        {
            collisionables.Remove(collisionable);
        }
    }

    public void CheckCollisions()
    {
        for (int i = 0; i < collisionables.Count; i++)// 1[0] - 2[1] - 3[2]
        {
            for (int j = 0; j < collisionables.Count; j++)// 1[0] - 2[1] - 3[2]
            {
                if (i != j)
                {
                    KeyValuePair<IColisionable, IColisionable> register = new KeyValuePair<IColisionable, IColisionable>(collisionables[i], collisionables[j]);

                    if (collisionables[i].GetBounds().Intersects(collisionables[j].GetBounds()))
                    {
                        if (!collisionRegister.Contains(register))
                        {
                            collisionRegister.Add(register);                            
                            
                        }
                        collisionables[i].OnCollision(collisionables[j]);
                    }
                    else
                    {
                        if (collisionRegister.Contains(register))
                        {                            
                            collisionRegister.Remove(register);
                        }
                    }
                }
            }
        }
    }
}



