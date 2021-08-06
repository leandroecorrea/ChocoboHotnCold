using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.IO;

class Map : GameObjectBase
{
    private Sprite sprite;
    private List<IntRect> tilesPosition;
    public Treasure treasure;
    private string fileName;
    
    public Map(string fileName) : base("Sprites" + Path.DirectorySeparatorChar + fileName)
    {
        this.fileName = fileName;
        sprite = base.sprite;
        tilesPosition = new List<IntRect>();
        tilesPosition.Add(new IntRect(782, 190, 160, 30));
        tilesPosition.Add(new IntRect(840, 220, 100, 70));
        tilesPosition.Add(new IntRect(820, 291, 150, 100));
        tilesPosition.Add(new IntRect(105, 467, 850, 50));
        TreasureGenerator();     
    }
    public void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }

    public void TreasureGenerator()
    {
        Random random = new Random();

        int treasureBoxTile = randomBox(random.Next(0, 100));
        Vector2f treasurePosition = new Vector2f(random.Next(tilesPosition[treasureBoxTile].Left, (tilesPosition[treasureBoxTile].Left + tilesPosition[treasureBoxTile].Width)),
                                    random.Next(tilesPosition[treasureBoxTile].Top, (tilesPosition[treasureBoxTile].Top + tilesPosition[treasureBoxTile].Height)));

        if(treasure == null)
        {
            treasure = new Treasure(30, "item", treasurePosition);
        }
        else
        {
            treasure.Depth = 30;
            treasure.Item = "item";
            treasure.Position = treasurePosition;
        }       
    }

    private int randomBox(int chances)
    {
        if (chances < 50)
        {
           return 3;
        }
        else
        {
            if (chances < 75)
            {
                return 2;
            }
            else
            {
                if (chances < 95)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }
    }      
}

