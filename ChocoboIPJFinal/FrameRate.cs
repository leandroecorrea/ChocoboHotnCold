using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

public static class FrameRate
{
    private static Clock clock;
    private static Time previousTime;
    private static Time currentTime;
    private static float fps;
    public static readonly uint FRAMERATE_LIMIT = 60;
    private static float deltaTime;

    public static void InitFrameRiteSystem()
    {
        clock = new Clock();
        Time previousTime = clock.ElapsedTime;
    }

    public static void OnFrameEnd()
    {
        currentTime = clock.ElapsedTime;
        deltaTime = currentTime.AsSeconds() - previousTime.AsSeconds();
        float fps = 1.0f / (currentTime.AsSeconds() - previousTime.AsSeconds());
        previousTime = currentTime;
    }
    public static float GetCurrentFPS()
    {
        return fps;
    }
    public static float GetDeltaTime()
    {
        return deltaTime;
    }
    

}

