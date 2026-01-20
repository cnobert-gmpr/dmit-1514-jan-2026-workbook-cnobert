using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson04Animation;

public class AnimationGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SimpleAnimation _walkingAnimation;

    public AnimationGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D spriteSheet = Content.Load<Texture2D>("Walking");
        _walkingAnimation = new SimpleAnimation(spriteSheet, 81, 144, 8, 8);
    }

    protected override void Update(GameTime gameTime)
    {
        _walkingAnimation.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _walkingAnimation.Draw(_spriteBatch, Vector2.Zero, SpriteEffects.None);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
