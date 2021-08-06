using SFML.System;
using System;



class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        FrameRate.InitFrameRiteSystem();
        while (game.UpdateWindow())
        {
            game.UpdateGame();
            CollisionObserver.GetInstance().CheckCollisions();
            game.DrawGame();
            FrameRate.OnFrameEnd();
        }
    }
}

