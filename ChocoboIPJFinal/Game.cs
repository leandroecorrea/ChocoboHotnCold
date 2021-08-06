using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

public class Game
{
    private static Vector2f windowSize;
    private RenderWindow window;    
    GamePlay gamePlay;

    public Game()
    {
        VideoMode videoMode = new VideoMode();
        videoMode.Height = 600;
        videoMode.Width = 1067;

        window = new RenderWindow(videoMode, "Chocobo Dig!");
        window.SetKeyRepeatEnabled(false);
        window.Closed += CloseWindow;
        window.SetFramerateLimit(FrameRate.FRAMERATE_LIMIT);

        gamePlay = new GamePlay(window);

    }
     public void DrawGame()
    {
        gamePlay.Draw(window);
        window.Display();
    }
    private void CloseWindow(object sender, EventArgs a)
    {
        window.Close();        
    }
    public bool UpdateWindow()
    {
        window.DispatchEvents();
        window.Clear(Color.Black);
        return window.IsOpen;
    }
    public void UpdateGame()
    {
        gamePlay.Update();
        windowSize = window.GetView().Size;
    }
    public static Vector2f GetWindowSize()
    {
        return windowSize;
    }
}

