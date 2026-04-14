using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public class Explosion
{
    private const float _Duration = 0.25f, _MinRadius = 4f, _MaxRadius = 30f;
    private Texture2D _pixel;
    private Vector2 _center;
    private float _timer;
    private bool _active;

    internal bool Active
    {
        get => _active;
    }
    internal Vector2 Center
    {
        get => _center;
    }
    internal float Radius
    {
        get
        {
            float radius = 0f;

            if(_active)
            {
                float progress = _timer / _Duration;
                radius = MathHelper.Lerp(_MinRadius, _MaxRadius, progress);
            }

            return radius;
        }
    }
    internal void LoadContent(ContentManager content)
    {
        IGraphicsDeviceService graphicsDeviceService = 
            (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
        GraphicsDevice graphicsDevice = graphicsDeviceService.GraphicsDevice;

        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _center = Vector2.Zero;
        _timer = 0f;
        _active = false;
    }
    internal void Start(Vector2 center)
    {
        _center = center;
        _timer = 0f;
        _active = true;
    }
    internal void Update(GameTime gameTime)
    {
        if(_active)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timer >= _Duration)
            {
                _timer = 0f;
                _active = false;
            }
        }
    }
    internal void Draw(SpriteBatch spriteBatch)
    {
        if(_active)
        {
            float progress = _timer / _Duration;
            float radius = MathHelper.Lerp(_MinRadius, _MaxRadius, progress);
            float alpha = 1f - progress;

            Rectangle explosionRectangle = new Rectangle
            (
                (int)(_center.X - radius),
                (int)(_center.Y - radius),
                (int)(radius * 2f),
                (int)(radius * 2f)
            );

            spriteBatch.Draw(_pixel, explosionRectangle, Color.OrangeRed * alpha);
        }
    }
}