
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public class CannonBall : Projectile
{
    private Texture2D _texture;

    private Explosion _explosion;
    private Trail _trail;

    //"override" means "I'm hiding the parent method"
    internal override void Initialize(float speed, Rectangle gameBoundingBox)
    {
        base.Initialize(speed, gameBoundingBox);

        _dimensions = new Point(4, 4);
        _explosion = new Explosion();
        _trail = new Trail();
        _trail.Initialize();
    }
    
    // a method that is labelled as "abstract" in the parent must be explicitly overridden
    internal override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("CannonBall");
        _explosion.LoadContent(content);
        _trail.LoadContent(content);
    }
    internal override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch(_state)
        {
            case State.Flying:
                _position += _direction * _speed * dt;

                _trail.Update(gameTime, _position);

                if(!BoundingBox.Intersects(_gameBoundingBox))
                {
                    _state = State.NotFlying;
                    _trail.Clear();
                }
                break;
            case State.NotFlying:
                break;
        }
        _explosion.Update(gameTime);
    }
    internal override void Draw(SpriteBatch spriteBatch)
    {
        switch(_state)
        {
            case State.Flying:
                spriteBatch.Draw(_texture, _position, Color.White);
                _trail.Draw(spriteBatch);
                break;
            case State.NotFlying:
                break;
        }
        _explosion.Draw(spriteBatch);
    }

    internal override bool ProcessCollision(Rectangle boundingBox)
    {
        if(base.ProcessCollision(boundingBox))
        {
            _trail.Clear();
            Vector2 explosionCentre = BoundingBox.Center.ToVector2();; //_position + new Vector2(_texture.Width / 2, _texture.Height / 2);
            _explosion.Start(explosionCentre);
            return true;
        }
        return false;
    }
}