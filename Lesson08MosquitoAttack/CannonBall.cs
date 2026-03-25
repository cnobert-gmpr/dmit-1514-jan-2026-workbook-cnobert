
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public class CannonBall
{
    private Texture2D _texture;
    private Vector2 _position;
    private Vector2 _direction;
    private float _speed;

    private Rectangle _gameBoundingBox;

    private enum State { Flying, NotFlying}
    private State _state = State.NotFlying;

    private Explosion _explosion;
    private Trail _trail;

    internal Rectangle BoundingBox
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
    }
    internal bool Launchable
    {
        get => _state == State.NotFlying;
    }

    internal void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _position = new Vector2(50, 300);
        _direction = new Vector2(0, -1);
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;

        _explosion = new Explosion();
        _trail = new Trail();
        _trail.Initialize();
    }
    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("CannonBall");
        _explosion.LoadContent(content);
        _trail.LoadContent(content);
    }
    internal void Update(GameTime gameTime)
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
    internal void Draw(SpriteBatch spriteBatch)
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

    internal void Launch(Vector2 position, Vector2 direction)
    {
        if(_state == State.NotFlying)
        {
            _position = position;
            _direction = direction;
            _state = State.Flying;
        }
    }

    internal bool ProcessCollision(Rectangle boundingBox)
    {
        bool returnValue = false;
        if(_state == State.Flying && BoundingBox.Intersects(boundingBox))
        {
            // this cannonBall has just hit the bounding box 
            // that was passed down
            returnValue = true;
            _state = State.NotFlying;
            _trail.Clear();

            Vector2 explosionCentre = BoundingBox.Center.ToVector2();; //_position + new Vector2(_texture.Width / 2, _texture.Height / 2);
            _explosion.Start(explosionCentre);
        }
        return returnValue;
    }
}