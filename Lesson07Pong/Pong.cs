using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07Pong;

public class Pong : Game
{
    private const int _WindowWidth = 750, _WindowHeight = 450;
    private const int _PlayAreaEdgeLineWidth = 12;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _backgroundTexture, _paddleTexture;

    private Ball _ball;

    private Paddle _paddleRight;

    private Vector2 _leftPaddlePosition, _leftPaddleDirection, _leftPaddleDimensions;
    private float _leftPaddleSpeed;

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

        _ball = new Ball();
        _ball.Initialize(new Vector2(150, 195), 60, new Point(21, 21), new Vector2(-1, -1), PlayAreaBoundingBox);

        _paddleRight = new Paddle();
        _paddleRight.Initialize(new Vector2(690, 198), 240, new Point(8, 124), PlayAreaBoundingBox);

        _leftPaddlePosition = new Vector2(54, 198);
        _leftPaddleSpeed = 240;
        _leftPaddleDimensions = new Vector2(8, 124);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Court");

        _ball.LoadContent(Content);
        
        _paddleRight.LoadContent(Content);

        _paddleTexture = Content.Load<Texture2D>("Paddle");
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _ball.Update(gameTime);

        
        KeyboardState kbState = Keyboard.GetState();
        #region right paddle
        if(kbState.IsKeyDown(Keys.Up))
        {
            //tell the paddle that its direction is upwards
            _paddleRight.Direction = new Vector2(0, -1);
        }
        else if(kbState.IsKeyDown(Keys.Down))
        {
            _paddleRight.Direction = new Vector2(0, 1);
        }
        else
        {
            _paddleRight.Direction = Vector2.Zero;
        }
        _paddleRight.Update(gameTime);
        
        #endregion

        #region left paddle
         if(kbState.IsKeyDown(Keys.W))
        {
            _leftPaddleDirection = new Vector2(0, -1);
        }
        else if(kbState.IsKeyDown(Keys.S))
        {
            _leftPaddleDirection = new Vector2(0, 1);
        }
        else
        {
            _leftPaddleDirection = Vector2.Zero;
        }
        _leftPaddlePosition += _leftPaddleDirection * _leftPaddleSpeed * dt;

        if(_leftPaddlePosition.Y <= PlayAreaBoundingBox.Top)
        {
            _leftPaddlePosition.Y = PlayAreaBoundingBox.Top;
        }
        else if((_leftPaddlePosition.Y + _leftPaddleDimensions.Y) >= PlayAreaBoundingBox.Bottom)
        {
            _leftPaddlePosition.Y = PlayAreaBoundingBox.Bottom - _leftPaddleDimensions.Y;
        }
        #endregion

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);

        _paddleRight.Draw(_spriteBatch);

        Rectangle leftPaddleRectangle =
            new Rectangle((int)_leftPaddlePosition.X, (int)_leftPaddlePosition.Y, (int)_leftPaddleDimensions.X, (int)_leftPaddleDimensions.Y);
        _spriteBatch.Draw(_paddleTexture, leftPaddleRectangle, Color.Fuchsia);

        _ball.Draw(_spriteBatch);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}
