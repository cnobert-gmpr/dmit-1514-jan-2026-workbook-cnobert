using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson03_Images;

public class SpritesGame : Game
{
    private const int _WindowWidth = 640, _WindowHeight = 320;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _shipSprite, _backgroundSprite;

    public SpritesGame()
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
        _shipSprite = Content.Load<Texture2D>("Beetle");
       
        _backgroundSprite = Content.Load<Texture2D>("Station");
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        //_shipSprite.Bounds.Height

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_shipSprite, Vector2.Zero, null, Color.White);
        _spriteBatch.Draw(_backgroundSprite, Vector2.Zero, null, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
