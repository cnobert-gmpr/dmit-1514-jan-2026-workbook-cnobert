using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07Pong;

public class Pong : Game
{
    private const int _WindowWidth = 250 * _Scale, _WindowHeight = 150 * _Scale;
    private const int _PlayAreaEdgeLineWidth = 4 * _Scale;
    private const int _Scale = 4;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _backgroundTexture;

    private Ball[] _balls;

    private Paddle _paddleRight, _paddleLeft;

    private Rectangle PlayAreaBoundingBox
    {
        get => new Rectangle(0, _PlayAreaEdgeLineWidth, _WindowWidth, _WindowHeight - 2 * _PlayAreaEdgeLineWidth);
    }

    public Pong()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();

        _balls = new Ball[2];
        for(int c = 0; c < _balls.Length; c++)
        {
            _balls[c] = new Ball();
        }
        _balls[0].Initialize(new Vector2(50 * _Scale, 65 * _Scale), 20 * _Scale, new Point(7 * _Scale, 7 * _Scale), new Vector2(-1, -1), PlayAreaBoundingBox);
        _balls[1].Initialize(new Vector2(60 * _Scale, 120 * _Scale), 100 * _Scale, new Point(7 * _Scale, 7 * _Scale), new Vector2(1, -1), PlayAreaBoundingBox);
        

        _paddleRight = new Paddle();
        _paddleRight.Initialize(new Vector2(215 * _Scale, 75 * _Scale), 240, new Point(3 * _Scale, 31 * _Scale), PlayAreaBoundingBox, GetRandomColor());

        _paddleLeft = new Paddle();
        _paddleLeft.Initialize(new Vector2(35 * _Scale, 198 * _Scale), 240, new Point(3 * _Scale, 31 * _Scale), PlayAreaBoundingBox, GetRandomColor());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundTexture = Content.Load<Texture2D>("Court");

        foreach(Ball ball in _balls)
            ball.LoadContent(Content);       
        
        _paddleRight.LoadContent(Content);
        _paddleLeft.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        #region input
        KeyboardState kbState = Keyboard.GetState();
        if(kbState.IsKeyDown(Keys.Up))
            _paddleRight.Direction = new Vector2(0, -1);
        else if(kbState.IsKeyDown(Keys.Down))
            _paddleRight.Direction = new Vector2(0, 1);
        else
            _paddleRight.Direction = Vector2.Zero;
        if(kbState.IsKeyDown(Keys.W))
            _paddleLeft.Direction = new Vector2(0, -1);
        else if(kbState.IsKeyDown(Keys.S))
            _paddleLeft.Direction = new Vector2(0, 1);
        else
            _paddleLeft.Direction = Vector2.Zero;
        #endregion
        
        foreach(Ball ball in _balls)
            ball.Update(gameTime);
        
        _paddleRight.Update(gameTime);
        _paddleLeft.Update(gameTime);

        // Paddle hit colour change [3 pts] - When the ball collides with a paddle, 
        // the paddle changes to a different colour for 500 milliseconds, 
        // then returns to its original colour.
        foreach(Ball ball in _balls)
        {
            ball.ProcessCollision(_paddleRight.BoundingBox);
            ball.ProcessCollision(_paddleLeft.BoundingBox);
        }
        // if(_ball.ProcessCollision(_paddleRight.BoundingBox))
        // {
        //     // Flash() sets a timer for 0.5 seconds, and during that 
        //     // time the paddle draws in a different colour
        //     //_paddleRight.Flash(); 
        // }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);

        foreach(Ball ball in _balls)
            ball.Draw(_spriteBatch);
        
        _paddleRight.Draw(_spriteBatch);
        _paddleLeft.Draw(_spriteBatch);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
    
    #region AI-Generated methods to produce Random colours
    private Random _random = new Random();
    private Color GetRandomColor()
    {
        float hue = (float)_random.NextDouble() * 360f;   // 0–360
        float saturation = 0.8f + (float)_random.NextDouble() * 0.2f; // 0.8–1.0
        float value = 0.8f + (float)_random.NextDouble() * 0.2f;      // 0.8–1.0

        return HSVToColor(hue, saturation, value);
    }
    private Color HSVToColor(float h, float s, float v)
    {
        float c = v * s;
        float x = c * (1 - MathF.Abs((h / 60f % 2) - 1));
        float m = v - c;

        float r = 0, g = 0, b = 0;

        if (h < 60)
        {
            r = c; g = x; b = 0;
        }
        else if (h < 120)
        {
            r = x; g = c; b = 0;
        }
        else if (h < 180)
        {
            r = 0; g = c; b = x;
        }
        else if (h < 240)
        {
            r = 0; g = x; b = c;
        }
        else if (h < 300)
        {
            r = x; g = 0; b = c;
        }
        else
        {
            r = c; g = 0; b = x;
        }

        return new Color(r + m, g + m, b + m);
    }
    #endregion
}
