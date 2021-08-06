using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class GamePlay
{
    private Chocobo chocobo;
    private Moguri moguri;
    private Map map;
    private Map topLayerSprite;
    private InvisibleWall invisibleWall;
    private Message dugUpMessage;
    private Message gameCountdown;
    private Message startGameCountdown;
    private Message totalScore;
    private int scoreDisplayed;
    private int roundFinalScore;    
    private float startCountdown;
    private float itemFoundMessageTime;
    private List<Item> itemList;
    private bool play;
    private float remainingTime;
    private Message beginGameMessage;
    private Message endGameMessage;
    private Window getWindow;
    private SoundBuffer menuBuffer;
    private SoundBuffer gameBuffer;
    private Sound menuMusic;
    private Sound gameMusic;
    private bool newGameSet;
    [Flags]
    public enum MusicStatus { None = 0, FadeMenu = 1, FadeGame = 2, StopMenu = 4, StopGame = 8 }
    private MusicStatus musicStatus;
    float gameMusicFadeTime;
    float menuMusicFadeTime;
    float searchBalance;

    public GamePlay(Window gameWindow)
    {
        getWindow = gameWindow;
        chocobo = new Chocobo();
        moguri = new Moguri();
        map = new Map("bg_00.png");
        topLayerSprite = new Map("bg_01.png");
        invisibleWall = new InvisibleWall();
        itemFoundMessageTime = 0f;
        play = false;
        startCountdown = 0f;
        menuBuffer = new SoundBuffer("Sounds" + Path.DirectorySeparatorChar + "menu.ogg");
        menuMusic = new Sound(menuBuffer);
        menuMusic.Volume = 15f;
        gameBuffer = new SoundBuffer("Sounds" + Path.DirectorySeparatorChar + "game.ogg");
        gameMusic = new Sound(gameBuffer);
        gameMusic.Volume = 15f;
        newGameSet = false;
        beginGameMessage = new Message("Start", new Vector2f(400, 300));
        beginGameMessage.SetTextSize(46);
        musicStatus = new MusicStatus();
        gameMusicFadeTime = 0f;
        menuMusicFadeTime = 0f;
        searchBalance = 5f;        
        scoreDisplayed = 0;
        roundFinalScore = scoreDisplayed;
        totalScore = new Message("0", new Vector2f(800, 50));
        totalScore.SetTextSize(22);
    }

    public void Update()
    {
        CheckMusic();
        if (play)
        {
            if (gameCountdown == null && itemList == null)
            {
                StopMusic(MusicStatus.FadeMenu);
                //menuMusic.Stop();
                chocobo.Update();
                StartCountdown();
            }
            else
            {
                map.treasure.Depth = chocobo.Update(map.treasure.Position, map.treasure.Depth);
                if (gameCountdown != null)
                {
                    gameCountdown.textMessage.DisplayedString = SecondsRemaining();
                }
                if (map.treasure.Depth <= 0)
                {
                    DugUpMessage();
                }
                if (remainingTime <= 0)
                {
                    SearchBalance();
                    //play = false;
                    //chocobo.RemoveText();
                    //newGameSet = false;
                    //beginGameMessage = new Message("Start", new Vector2f(400, 300));
                    //beginGameMessage.SetTextSize(46);
                    //StopMusic(MusicStatus.FadeGame);
                    //gameMusic.Stop();
                }
            }
        }
        else
        {
            if (!newGameSet)
            {
                map.treasure.Depth = 30;
                gameCountdown = null;
                itemList = null;
                dugUpMessage = null;
                chocobo.dig = false;
                //StopMusic(MusicStatus.PlayMenu);                
                menuMusic.Play();
                menuMusic.Volume = 15f;
                newGameSet = true;
                Console.WriteLine("new game set");
            }
            StartGame();
            chocobo.Update();
        }
        moguri.Update();
    }

    private void InitializeGame()
    {
        gameCountdown = new Message(SecondsRemaining(), new Vector2f(500, 50));
        gameCountdown.SetTextSize(32);
        remainingTime = 45.5f;
        itemList = new List<Item>();
    }
    private void GenerateItem()
    {
        Random random = new Random();
        int itemChances = random.Next(0, 100);
        if (itemChances < 50)
            itemList.Add(new PhoenixDown());
        else if (itemChances < 75)
            itemList.Add(new Rod());
        else
            itemList.Add(new Dagger());
    }

    private void DugUpMessage()
    {
        if (dugUpMessage == null)
        {
            dugUpMessage = new Message("Item found!", new Vector2f(500, 300));
            dugUpMessage.SetTextSize(60);
            GenerateItem();
        }
        else
        {
            itemFoundMessageTime += FrameRate.GetDeltaTime();
            if (itemFoundMessageTime > 1f)
            {
                itemFoundMessageTime = 0f;
                dugUpMessage = null;
                chocobo.dig = false;
                itemList[itemList.Count - 1].ItemScaleNPosition(new Vector2f(1f, 1f), new Vector2f(0f + (50f * (float)itemList.Count - 1), 30f));
                map.TreasureGenerator();
            }
            itemList[itemList.Count - 1].Update();
        }
    }
    private void StartCountdown()
    {

        if (startCountdown < 5f)
        {
            if (startGameCountdown == null)
            {
                startGameCountdown = new Message("Ready!", new Vector2f(500, 300));
                startGameCountdown.SetTextSize(36);
                musicStatus &= ~MusicStatus.StopGame;
                gameMusic.Play();
                gameMusic.Volume = 15f;
            }
            startCountdown += FrameRate.GetDeltaTime();
        }
        else if (startCountdown < 7f)
        {
            startGameCountdown.UpdateMessage("GO!");
            startCountdown += FrameRate.GetDeltaTime();
        }
        else
        {
            startGameCountdown = null;
            beginGameMessage = null;
            InitializeGame();
            startCountdown = 0;
        }
    }
    private void SearchBalance()
    {
        searchBalance -= FrameRate.GetDeltaTime();
        if (searchBalance > 0f)
        {
            if (itemList.Count == 0)
            {
                if (endGameMessage == null)
                {
                    gameCountdown = null;
                    chocobo.RemoveText();
                    endGameMessage = new Message("No items found...", new Vector2f(400, 300));
                    endGameMessage.SetTextSize(24);
                }
            }
            else
            {
                if (endGameMessage == null)
                {
                    gameCountdown = null;
                    chocobo.RemoveText();
                    endGameMessage = new Message("Items found!", new Vector2f(400, 300));
                    endGameMessage.SetTextSize(24);
                    foreach(Item item in itemList)
                    {
                        roundFinalScore += item.GetXPoints();
                    }
                    roundFinalScore += scoreDisplayed;
                }
                else if (searchBalance < 2f && itemList.Count > 0)
                {
                    if (!itemList[itemList.Count - 1].ToRemove)
                    {
                        itemList[itemList.Count - 1].ItemScaleNPosition(new Vector2f(3f, 3f), new Vector2f(500f, 250f));
                        chocobo.XPoints += itemList[itemList.Count - 1].GetXPoints();
                        searchBalance += 3f;
                        itemList[itemList.Count - 1].ToRemove = true;
                    }
                    else
                    {
                        itemList[itemList.Count - 1].DisposeNow();
                        itemList.RemoveAt(itemList.Count - 1);
                    }
                }
                else if (itemList.Count > 0)
                {
                    totalScore.textMessage.DisplayedString = Convert.ToString(scoreDisplayed < roundFinalScore ? scoreDisplayed += 10 : scoreDisplayed);
                }
            }
        }
        else
        {
            endGameMessage = null;
            play = false;
            newGameSet = false;
            beginGameMessage = new Message("Start", new Vector2f(400, 300));
            beginGameMessage.SetTextSize(46);
            StopMusic(MusicStatus.FadeGame);
        }
    }
    private string SecondsRemaining()
    {
        return Convert.ToString((int)(remainingTime -= FrameRate.GetDeltaTime()));
    }

    public void Draw(RenderWindow window)
    {
        map.Draw(window);
        chocobo.Draw(window);
        moguri.Draw(window);
        totalScore.DrawText(window);
        if (startGameCountdown != null)
        {
            startGameCountdown.DrawText(window);
        }
        if (gameCountdown != null)
        {
            gameCountdown.DrawText(window);
        }
        if (dugUpMessage != null)
        {
            dugUpMessage.DrawText(window);
        }
        if (itemList != null)
        {
            foreach (Item item in itemList)
            {
                item.Draw(window);
            }
        }
        if (endGameMessage != null)
        {
            endGameMessage.DrawText(window);
        }
        if (!play)
        {
            beginGameMessage.DrawText(window);
        }
        topLayerSprite.Draw(window);
    }

    private void StartGame()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left) && (beginGameMessage.textMessage.GetGlobalBounds().Contains(Mouse.GetPosition(getWindow).X, Mouse.GetPosition(getWindow).Y)))
        {
            play = true;
        }
    }

    public void StopMusic(MusicStatus musicStatus)
    {
        if (!this.musicStatus.HasFlag(musicStatus))
        {
            this.musicStatus |= musicStatus;
            if (musicStatus.HasFlag(MusicStatus.FadeMenu) && !musicStatus.HasFlag(MusicStatus.StopMenu))
            {
                menuMusicFadeTime = 5f;
            }
            if (musicStatus.HasFlag(MusicStatus.FadeGame) && !musicStatus.HasFlag(MusicStatus.StopGame))
            {
                gameMusicFadeTime = 3f;
            }
        }
    }

    public void CheckMusic()
    {
        if (musicStatus.HasFlag(MusicStatus.FadeMenu))
        {
            if (menuMusicFadeTime < 3f || menuMusic.Volume < 1f)
            {
                menuMusic.Stop();
                musicStatus &= ~MusicStatus.FadeMenu;
            }
            else
            {
                menuMusicFadeTime -= FrameRate.GetDeltaTime();
                menuMusic.Volume -= 0.2f;
            }
        }

        if (musicStatus.HasFlag(MusicStatus.FadeGame) || gameMusic.Volume < 1f)
        {
            if (gameMusicFadeTime < 0.5f)
            {
                gameMusic.Stop();
                musicStatus &= ~MusicStatus.FadeGame;
            }
            else
            {
                gameMusicFadeTime -= FrameRate.GetDeltaTime();
                gameMusic.Volume -= 0.2f;
            }
        }
    }
}

