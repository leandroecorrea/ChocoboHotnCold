using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;


public abstract class GameObjectBase
{
    protected Texture texture;
    protected Sprite sprite;
    protected Vector2f currentPosition;
	bool toDelete;

    public GameObjectBase(string texturePath, Vector2f startPosition)
    {
        texture = new Texture(texturePath);
        sprite = new Sprite(texture);
        currentPosition = startPosition;
        sprite.Position = currentPosition;
		toDelete = false;
    }
	public GameObjectBase(string texturePath)
	{
		texture = new Texture(texturePath);
		sprite = new Sprite(texture);
		sprite.Position = new Vector2f(0.0f, 0.0f);
	}
	public virtual void Update()
	{
		sprite.Position = currentPosition;
	}

	public virtual void Draw(RenderWindow window)
	{
		window.Draw(sprite);
	}

	public Vector2f GetPosition()
	{
		return currentPosition;
	}
	public virtual void DisposeNow()
	{
		sprite.Dispose();
		texture.Dispose();
		toDelete = true;
	}
}

