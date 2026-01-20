using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SpriteFontPlus; //ONLY CONRAD NEEDS THIS CODE (MACOS THINGS)
using System.IO; //ONLY CONRAD NEEDS THIS CODE (MACOS THINGS)

namespace Lesson05KeyboardInput;

public class KeyboardInputGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private string _message = "In 2.5 weeks, days will be 1 hour longer.";
    private SpriteFont _font;

    private KeyboardState _kbPreviousState;
    public KeyboardInputGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _kbPreviousState = Keyboard.GetState();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //_arialFont = Content.Load<SpriteFont>("SystemArialFont");

        byte[] fontBytes = File.ReadAllBytes("Content/Tahoma.ttf");
        _font = TtfFontBaker.Bake(fontBytes, 30, 1024, 1024, new[] { CharacterRange.BasicLatin }).CreateSpriteFont(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kbCurrentState = Keyboard.GetState();

        _message = "";

        #region arrow keys
        if(kbCurrentState.IsKeyDown(Keys.Down))
        {
            _message += "Down ";
        }
        if(kbCurrentState.IsKeyDown(Keys.Up))
        {
            _message += "Up ";
        }
        if(kbCurrentState.IsKeyDown(Keys.Left))
        {
            _message += "Left ";
        }
        if(kbCurrentState.IsKeyDown(Keys.Right))
        {
            _message += "Right";
        }
        #endregion
        #region "key down" event
        if(_kbPreviousState.IsKeyUp(Keys.Space) && kbCurrentState.IsKeyDown(Keys.Space))
        {
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
            _message += "------------------------------------------------------\n";
        }
        #endregion
        #region key hold
        if(_kbPreviousState.IsKeyDown(Keys.Space) && kbCurrentState.IsKeyDown(Keys.Space))
        {
            _message += " Space";
        }
        #endregion
        #region key up
        if(_kbPreviousState.IsKeyDown(Keys.Space) && kbCurrentState.IsKeyUp(Keys.Space))
        {
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
            _message += "++++++++++++++++++++++++++++++++++++++++++++++++++++++\n";
        }
        #endregion
        
        _kbPreviousState = kbCurrentState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, _message, Vector2.Zero, Color.Black);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
