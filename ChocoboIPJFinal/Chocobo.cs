using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.IO;
using SFML.Audio;

public class Chocobo : GameObjectBase, IColisionable
{

    private float speedX;
    private float speedY;
    private int peckDepth;
    private float peckingSpeed;
    private float fireRate;
    private int xPoints;
    private int level;
    private List<IntRect> peckTextureAreas = new List<IntRect>();
    private Texture peckTexture;
    private List<IntRect> movementTextureAreas = new List<IntRect>();
    Vector2f verticalAdjust = new Vector2f(0.025f, 0.025f);
    private float animationTime;
    private int peckRightFrame;
    private int peckLeftFrame;
    private int moveRightFrame;
    private int moveLeftFrame;
    private float acceleration;
    private float inertia;
    private Message kwehText;
    public bool dig;
    private enum Status { IdleLeft, IdleRight, MoveLeft, MoveRight, PeckLeft, PeckRight }
    private Status animation;
    private SoundBuffer peckBuffer;
    private SoundBuffer digUpBuffer;
    private Sound peckSound;
    private Sound digUpSound;

    public Chocobo() : base("Sprites" + Path.DirectorySeparatorChar + "movement_sprites.png", new Vector2f(300f, 400f))
    {
        AddIntRects();
        sprite.Scale = new Vector2f(3.5f, 3.5f);
        speedX = 380.0f;
        speedY = 160.0f;
        peckDepth = 7;
        animationTime = FrameRate.GetDeltaTime();
        peckRightFrame = 0;
        peckLeftFrame = 5;
        moveRightFrame = 0;
        moveLeftFrame = 5;
        animation = Status.IdleRight;
        sprite.TextureRect = peckTextureAreas[0];
        peckTexture = new Texture("Sprites" + Path.DirectorySeparatorChar + "peck_sprites.png");
        peckingSpeed = 0.5f;
        fireRate = 0;
        CollisionObserver.GetInstance().AddToCollisionObserver(this);
        acceleration = 0;
        dig = false;
        peckBuffer = new SoundBuffer("Sounds" + Path.DirectorySeparatorChar + "peck.wav");
        peckSound = new Sound(peckBuffer);
        peckSound.Volume = 65;
        digUpBuffer = new SoundBuffer("Sounds" + Path.DirectorySeparatorChar + "itemfound.ogg");
        digUpSound = new Sound(digUpBuffer);
        xPoints = 0;
    }
    public int Update(Vector2f treasurePosition, int treasureDepth)
    {
        if (dig)
        {
            treasureDepth = Dig(treasureDepth);
        }
        else
        {
            Movement(treasurePosition);
        }
        StatusChecker();
        animationTime += FrameRate.GetDeltaTime();
        RestoreFire();
        base.Update();
        if (kwehText != null)
        {
            kwehText.UpdateFloatingMessage();
        }
        return treasureDepth;
    }
    public override void Update()
    {
        KeyInput();
        StatusChecker();
        animationTime += FrameRate.GetDeltaTime();
        base.Update();
    }
    public void AddIntRects()
    {
        movementTextureAreas.Add(new IntRect(0, 0, 30, 30));
        movementTextureAreas.Add(new IntRect(30, 0, 30, 30));
        movementTextureAreas.Add(new IntRect(60, 0, 30, 30));
        movementTextureAreas.Add(new IntRect(90, 0, 30, 30));
        movementTextureAreas.Add(new IntRect(0, 31, 30, 30));
        movementTextureAreas.Add(new IntRect(30, 31, 30, 30));
        movementTextureAreas.Add(new IntRect(60, 31, 30, 30));
        movementTextureAreas.Add(new IntRect(90, 31, 30, 30));
        peckTextureAreas.Add(new IntRect(0, 0, 30, 30));
        peckTextureAreas.Add(new IntRect(30, 0, 30, 30));
        peckTextureAreas.Add(new IntRect(60, 0, 30, 30));
        peckTextureAreas.Add(new IntRect(90, 0, 30, 30));
        peckTextureAreas.Add(new IntRect(120, 0, 30, 30));
        peckTextureAreas.Add(new IntRect(0, 31, 30, 30));
        peckTextureAreas.Add(new IntRect(30, 31, 30, 30));
        peckTextureAreas.Add(new IntRect(60, 31, 30, 30));
        peckTextureAreas.Add(new IntRect(90, 31, 30, 30));
        peckTextureAreas.Add(new IntRect(120, 31, 30, 30));
    }
    public override void Draw(RenderWindow window)
    {
        base.Draw(window);
        if (kwehText != null)
        {
            kwehText.DrawText(window);
        }
    }
    public void Movement(Vector2f treasurePosition)
    {
        Peck(treasurePosition);
        if (animation != Status.PeckLeft && animation != Status.PeckRight)
        {
            KeyInput();
        }
    }

    public void KeyInput()
    {
        sprite.Texture = texture;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            if (animation == Status.MoveLeft)
            {
                acceleration = 0f;
                animation = Status.MoveRight;
                Console.WriteLine(acceleration);
            }
            else
            {
                Accelerate();
                animation = Status.MoveRight;
                currentPosition.X += Move();
            }
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            if (animation == Status.MoveRight)
            {
                acceleration = 0f;
                animation = Status.MoveLeft;
                Console.WriteLine(acceleration);
            }
            else
            {
                Accelerate();
                animation = Status.MoveLeft;
                currentPosition.X -= Move();
            }
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            MoveDown();
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            MoveUp();
        }
        if (!Keyboard.IsKeyPressed(Keyboard.Key.A) && !Keyboard.IsKeyPressed(Keyboard.Key.W) && !Keyboard.IsKeyPressed(Keyboard.Key.D) && !Keyboard.IsKeyPressed(Keyboard.Key.S) && !Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            if (animation == Status.MoveLeft)
            {
                animation = Status.IdleLeft;
            }
            if (animation == Status.MoveRight)
            {
                animation = Status.IdleRight;
            }
        }
    }
    public float Move()
    {
        return (speedX * acceleration * FrameRate.GetDeltaTime());
    }
    public bool Deaccelerate()
    {
        inertia = acceleration;
        if (inertia > 0f)
        {
            acceleration -= FrameRate.GetDeltaTime() * 1.5f;
            return true;
        }
        else
        {
            inertia = 0f;
            return false;
        }
    }
    public void Accelerate()
    {
        if (acceleration < 1.0f)
        {
            acceleration += FrameRate.GetDeltaTime() * 1f;
        }
        else
        {
            acceleration = 1f;
        }
        inertia = acceleration;
    }
    public int XPoints
    {
        get { return xPoints; }
        set { xPoints = value; }
    }
    public void StatusChecker()
    {
        switch (animation)
        {
            case Status.IdleLeft:
                sprite.TextureRect = peckTextureAreas[5];
                Idle();
                break;
            case Status.IdleRight:
                sprite.TextureRect = peckTextureAreas[0];
                Idle();
                break;
            case Status.MoveLeft:
                LeftMovementAnimation();
                break;
            case Status.MoveRight:
                RightMovementAnimation();
                break;
            case Status.PeckLeft:
                LeftPeckAnimation();
                break;
            case Status.PeckRight:
                RightPeckAnimation();
                break;
        }
    }
    public void MoveUp()
    {
        SetVerticalAnimationOnMove();
        currentPosition.Y -= speedY * FrameRate.GetDeltaTime();
        if (sprite.Position.Y < 350f)
        {
            sprite.Scale -= verticalAdjust;
            currentPosition.Y += 25f * FrameRate.GetDeltaTime();
        }
    }
    public void MoveDown()
    {
        SetVerticalAnimationOnMove();
        currentPosition.Y += speedY * FrameRate.GetDeltaTime();
        if (sprite.Position.Y < 350f)
        {
            sprite.Scale += verticalAdjust;
            currentPosition.Y -= 25f * FrameRate.GetDeltaTime();
        }
    }
    private void Idle()
    {
        acceleration = 0f;
    }
    private void SetVerticalAnimationOnMove()
    {
        if (animation == Status.IdleLeft || animation == Status.MoveLeft)
        {
            animation = Status.MoveLeft;
        }
        if (animation == Status.IdleRight || animation == Status.MoveRight)
        {
            animation = Status.MoveRight;
        }
    }
    private void Peck(Vector2f treasurePosition)
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left) && fireRate == 0)
        {
            //acceleration = 0;
            //inertia = 0;
            if (animation == Status.IdleLeft || animation == Status.MoveLeft)
            {
                animation = Status.PeckLeft;

            }
            else
            {
                animation = Status.PeckRight;
            }
            CheckDistance(sprite.Position, treasurePosition);
            fireRate = peckingSpeed;
            peckSound.Play();
        }
    }

    private int Dig(int treasureDepth)
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left) && fireRate == 0)
        {
            if (animation == Status.IdleLeft || animation == Status.MoveLeft)
            {
                animation = Status.PeckLeft;

            }
            else
            {
                animation = Status.PeckRight;
            }
            peckSound.Play();
            fireRate = peckingSpeed;
            treasureDepth -= peckDepth;
            if (treasureDepth < 0)
            {
                treasureDepth = 0;
                digUpSound.Play();
            }
            CreateText(Convert.ToString(treasureDepth), 24);
        }
        return treasureDepth;
    }

    public void RemoveText()
    {
        kwehText = null;
    }

    private void CheckDistance(Vector2f peckPosition, Vector2f treasurePosition)
    {
        if (Math.Abs(peckPosition.X - treasurePosition.X) < 25 && Math.Abs(peckPosition.Y - treasurePosition.Y) < 25)
        {
            dig = true;
            CreateText("KWEEEHHHH!!!!?!???", 24);
        }
        else if (Math.Abs(peckPosition.X - treasurePosition.X) < 50 && Math.Abs(peckPosition.Y - treasurePosition.Y) < 50)
        {
            CreateText("kweeehhh!!!", 20);
            dig = false;
        }
        else if (Math.Abs(peckPosition.X - treasurePosition.X) < 100 && Math.Abs(peckPosition.Y - treasurePosition.Y) < 100)
        {
            CreateText("kweeh!!?", 16);
            dig = false;
        }
        else
        {
            CreateText("kweh", 12);
            dig = false;
        }
    }

    private void CreateText(string hotOrColdMessage, int characterSize)
    {
        if (kwehText == null)
        {
            kwehText = new Message(hotOrColdMessage, sprite.Position);
            kwehText.SetTextSize(characterSize);
            kwehText.FloatingMessage();
        }
    }
    private void RestoreFire()
    {
        if (fireRate > 0)
        {
            fireRate -= FrameRate.GetDeltaTime();
        }
        else
        {
            kwehText = null;
            fireRate = 0;
        }
    }
    #region Animation methods
    private void RightMovementAnimation()
    {
        if (animationTime > 0.1f)
        {
            if (moveRightFrame < 3)
            {
                moveRightFrame++;
            }
            else
            {
                moveRightFrame = 0;
            }
            animationTime = 0;
        }
        sprite.TextureRect = peckTextureAreas[moveRightFrame];
    }
    private void LeftMovementAnimation()
    {
        if (animationTime > 0.1f)
        {
            if (moveLeftFrame > 4 && moveLeftFrame < 8)
            {
                moveLeftFrame++;
            }
            else
            {
                moveLeftFrame = 5;
            }
            animationTime = 0;
        }
        sprite.TextureRect = peckTextureAreas[moveLeftFrame];
    }
    private void RightPeckAnimation()
    {
        if (animationTime > 0.05f)
        {
            if (peckRightFrame < 4)
            {
                peckRightFrame++;
            }
            else if (peckRightFrame > 4)
            {
                peckRightFrame = 0;
            }
            animationTime = 0;
        }
        sprite.Texture = peckTexture;
        sprite.TextureRect = peckTextureAreas[peckRightFrame];
        if (peckRightFrame == 4)
        {
            animation = Status.IdleRight;
            peckRightFrame = 0;
        }
    }
    private void LeftPeckAnimation()
    {
        if (animationTime > 0.05f)
        {
            if (peckLeftFrame > 4 && peckLeftFrame < 9)
            {
                peckLeftFrame++;
            }
            animationTime = 0;
        }

        sprite.Texture = peckTexture;
        sprite.TextureRect = peckTextureAreas[peckLeftFrame];
        if (peckLeftFrame == 9)
        {
            animation = Status.IdleLeft;
            peckLeftFrame = 5;
        }
    }
    #endregion    

    public FloatRect GetBounds()
    {
        return sprite.GetGlobalBounds();
    }

    public void OnCollision(IColisionable other)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            currentPosition.X -= Move();
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {

            currentPosition.X += Move();
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            MoveUp();
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            MoveDown();
        }
    }
}

