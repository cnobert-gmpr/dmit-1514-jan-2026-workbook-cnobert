using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson06TicTacToe01;

public class TicTacToe : Game
{
    private const int _WindowWidth = 170, _WindowHeight = 170;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _backgroundImage, _xImage, _oImage;

    public enum GameSpaceState
    {
        X,
        O
    }
    private GameSpaceState _nextTokenToBePlayed = GameSpaceState.X;

    private MouseState _currentMouseState, _previousMouseState;

    public TicTacToe()
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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundImage = Content.Load<Texture2D>("TicTacToeBoard");
        _xImage = Content.Load<Texture2D>("X");
        _oImage = Content.Load<Texture2D>("O");

    }

    protected override void Update(GameTime gameTime)
    {
        _currentMouseState = Mouse.GetState();

        //detect a mouse button up (mouse button release) event
        if( _previousMouseState.LeftButton == ButtonState.Pressed
            && _currentMouseState.LeftButton == ButtonState.Released)
        {
            //when this "if" statement is entered, change the next token to be played
            if(_nextTokenToBePlayed == GameSpaceState.X)
            {
                _nextTokenToBePlayed = GameSpaceState.O;
            }
            else
            {
                _nextTokenToBePlayed = GameSpaceState.X;
            }
        }

        _previousMouseState = _currentMouseState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_backgroundImage, Vector2.Zero, Color.White);

        Vector2 adjustedMousePosition = new Vector2(
            _currentMouseState.Position.X - (_xImage.Width / 2),
            _currentMouseState.Position.Y - (_xImage.Height / 2)
        );

        if(_nextTokenToBePlayed == GameSpaceState.X)
        {
            _spriteBatch.Draw(_xImage, adjustedMousePosition, Color.White);
        }
        else if (_nextTokenToBePlayed == GameSpaceState.O)
        {
            _spriteBatch.Draw(_oImage, adjustedMousePosition, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
