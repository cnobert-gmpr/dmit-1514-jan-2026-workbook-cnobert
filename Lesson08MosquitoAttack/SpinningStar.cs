using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public class SpinningStar : Projectile
{
    private const int _StarSize = 30;
    private const float _RotationSpeed = 5f, _ArmThicknessRatio = 0.125f, _MainArmRatio = 0.4375f, _DiagonalArmRatio = 0.375f;

    private Texture2D _pixel;
    private float _rotation;

    internal override void Initialize(float speed, Rectangle gameBoundingBox)
    {
        base.Initialize(speed, gameBoundingBox);

        _dimensions = new Point(_StarSize, _StarSize);
        _rotation = 0f;
    }

    internal override void LoadContent(ContentManager content)
    {
        _pixel = CreatePixel(content, Color.Gold);
    }

    internal override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch(_state)
        {
            case State.Flying:
                _position += _direction * _speed * dt;
                _rotation += _RotationSpeed * dt;

                if(!BoundingBox.Intersects(_gameBoundingBox))
                {
                    _state = State.NotFlying;
                    _rotation = 0f;
                }
                break;
            case State.NotFlying:
                break;
        }
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        switch(_state)
        {
            case State.Flying:
                Vector2 center = _position + new Vector2(_dimensions.X / 2f, _dimensions.Y / 2f);
                float thickness = _StarSize * _ArmThicknessRatio;
                float mainArmLength = _StarSize * _MainArmRatio;
                float diagonalArmLength = _StarSize * _DiagonalArmRatio;

                DrawArm(spriteBatch, center, _rotation, mainArmLength, thickness);
                DrawArm(spriteBatch, center, _rotation + MathHelper.PiOver2, mainArmLength, thickness);
                DrawArm(spriteBatch, center, _rotation + MathHelper.PiOver4, diagonalArmLength, thickness);
                DrawArm(spriteBatch, center, _rotation + MathHelper.PiOver4 + MathHelper.PiOver2, diagonalArmLength, thickness);
                break;
            case State.NotFlying:
                break;
        }
    }

    internal void Launch(Vector2 position, Vector2 direction, float startingRotation)
    {
        _rotation = startingRotation;
        base.Launch(position, direction);
    }

    private Texture2D CreatePixel(ContentManager content, Color colour)
    {
        IGraphicsDeviceService graphicsDeviceService =
            (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
        GraphicsDevice graphicsDevice = graphicsDeviceService.GraphicsDevice;
        Texture2D pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData(new[] { colour });
        return pixel;
    }

    private void DrawArm(SpriteBatch spriteBatch, Vector2 center, float rotation, float length, float thickness)
    {
       spriteBatch.Draw(_pixel, center, null, Color.White, rotation, new Vector2(0.5f, 0.5f), new Vector2(length, thickness), SpriteEffects.None, 0f);
    }
}