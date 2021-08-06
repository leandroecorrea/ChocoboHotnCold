using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Tile
{
    private IntRect tilePosition;
    public enum Proximity { Cold, Warm, Hot, VeryHot, Dig }    
    Proximity proximity;

    public Tile(IntRect tilePosition)
    {
        this.tilePosition = tilePosition;
    }
    public IntRect GetTilePosition()
    {
        return tilePosition;
    }
    public Proximity HotOrCold
    {
        get { return proximity; }
        set { proximity = value; }
    }    
}

