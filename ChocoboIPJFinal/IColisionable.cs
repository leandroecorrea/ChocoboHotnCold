using SFML.Graphics;


public interface IColisionable
{
    public FloatRect GetBounds();
    public void OnCollision(IColisionable other);    
}

