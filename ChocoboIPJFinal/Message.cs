using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SFML.Graphics;
using SFML.System;

class Message
{
    public Text textMessage;
    private Vector2f currentPosition;
    private Random random;
    private bool floatingMessage;

    public Message(string text, Vector2f position)
    {
        Font alexandria = new Font("Fonts" + Path.DirectorySeparatorChar + "Alexandria.ttf");
        textMessage = new Text(text, alexandria);
        textMessage.FillColor = Color.White;
        textMessage.OutlineColor = Color.Black;
        textMessage.OutlineThickness = 2f;
        textMessage.CharacterSize = 12;
        currentPosition = position;
        textMessage.Position = currentPosition;
    }

    public void DrawText(RenderWindow window)
    {
        window.Draw(textMessage);
    }

    public void UpdateFloatingMessage()
    {
        currentPosition.X -= 20.0f * FrameRate.GetDeltaTime();
        currentPosition.Y -= 40.0f * FrameRate.GetDeltaTime();
        textMessage.Position = currentPosition;
        if(floatingMessage)
        {
            if (textMessage.Rotation == 0f)
            {
                textMessage.Rotation += 0.5f;
            }
            else
            {
                textMessage.Rotation = textMessage.Rotation * 1.1f;
            }
        }
        
    }

    public void UpdateMessage(string updatedText)
    {
        textMessage.DisplayedString = updatedText;
    }

    public void FloatingMessage()
    {
        floatingMessage = true;
        random = new Random();
        textMessage.Rotation = (float)random.Next(-2, 1);       
    }
    public void SetTextSize(int characterSize)
    {
        textMessage.CharacterSize = (uint)characterSize;
    }
}

