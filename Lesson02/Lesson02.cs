using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SpriteFontPlus; //ONLY CONRAD NEEDS THIS CODE (MACOS THINGS)
using System.IO; //ONLY CONRAD NEEDS THIS CODE (MACOS THINGS)

namespace Lesson02;

public class Lesson02 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private const int _WindowWidth = 800;
    private const int _WindowHeight = 400;

    private SpriteFont _font;
    private string _message = "Hello World";
    private Vector2 _position;
    private float _xVelocity = 360, _yVelocity = 360;
    public Lesson02() 
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

        _position = new Vector2(10, 190);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //_arialFont = Content.Load<SpriteFont>("SystemArialFont");

        byte[] fontBytes = File.ReadAllBytes("Content/Tahoma.ttf");
        _font = TtfFontBaker.Bake(
                    fontBytes,
                    30,
                    1024,
                    1024,
                    new[] { CharacterRange.BasicLatin })
                .CreateSpriteFont(GraphicsDevice);
    }

    //gets called 60 times per second
    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 stringDimensions = _font.MeasureString(_message);

        //multiply the speed by the amount of time that has elapsed since the
        //last call to update
        _position.X += _xVelocity * dt;
        if(_position.X < 0 || (_position.X + stringDimensions.X) > _WindowWidth)
        {
            _xVelocity *= -1;
        }
        _position.Y += _yVelocity * dt;
        if(_position.Y < 0 || (_position.Y + stringDimensions.Y) > _WindowHeight)
        {
            _yVelocity *= -1;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        //represents the output device
        _spriteBatch.Begin();

        _spriteBatch.DrawString(_font, _message, _position, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
