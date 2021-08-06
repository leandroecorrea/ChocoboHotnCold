using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.IO;

public class Moguri : GameObjectBase
{
    public Moguri() : base("Sprites" + Path.DirectorySeparatorChar + "mog.png", new Vector2f(20f, 375f))
    {
        sprite.Scale = new Vector2f(3.5f, 3.5f);
    }
}

