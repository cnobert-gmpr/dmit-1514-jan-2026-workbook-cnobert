using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public class Cannon
{
    private const int _NumProjectiles = 5;
    private SimpleAnimation _animationAlive, _animationDying;
    private Vector2 _position, _direction;
    private Point _dimensions;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private enum State { Alive, Dying, Dead }
    private State _state;

    private Projectile[] _projectiles;

    internal Vector2 Direction
    {
        set
        {
            value.Y = 0;
            _direction = value;
            _animationAlive.Reverse = _direction.X < 0;
        }
    }

    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_animationAlive.FrameDimensions.X,
                (int)_animationAlive.FrameDimensions.Y
            );
        }
    }
    
    internal bool Alive { get => _state == State.Alive; }

    internal void Initialize(Vector2 position, float speed, Rectangle gameBoundingBox)
    {
        _position = position;
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;
        _state = State.Alive;
        _projectiles = new Projectile[_NumProjectiles];

        _projectiles[0] = new CannonBall();
        _projectiles[1] = new FireBall();
        _projectiles[2] = new FireBall();
        _projectiles[3] = new CannonBall();
        _projectiles[4] = new CannonBall();
        foreach(Projectile p in _projectiles)
        {
            p.Initialize(50, _gameBoundingBox);
        }
    }
    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Cannon");
        _dimensions = new Point(texture.Width / 4, texture.Height);
        _animationAlive = new SimpleAnimation(texture, _dimensions.X, _dimensions.Y, 4, 2f);

        texture = content.Load<Texture2D>("Poof");
        _animationDying =
            new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4);

        foreach(Projectile p in _projectiles)
            p.LoadContent(content);
    }
    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        switch(_state)
        {
            case State.Alive:
                _position += _speed * _direction * dt;

                if(_direction != Vector2.Zero)
                    _animationAlive.Update(gameTime);
                break;
            case State.Dying:
                _animationDying.Update(gameTime);
                if(_animationDying.DonePlayingOnce)
                    _state = State.Dead;
                break;
            case State.Dead:
                break;
        }

        foreach(Projectile p in _projectiles)
            p.Update(gameTime);
    }
    internal void Draw(SpriteBatch spriteBatch)
    {
        switch(_state)
        {
            case State.Alive:
                    _animationAlive?.Draw(spriteBatch, _position, SpriteEffects.None);
                break;
            case State.Dying:
                    _animationDying?.Draw(spriteBatch, _position, SpriteEffects.None);
                break;
            case State.Dead:
                break;
        }
        foreach(Projectile p in _projectiles)
            p.Draw(spriteBatch);
    }

    internal void Shoot()
    {
        foreach(Projectile p in _projectiles)
        {
            if(p.Launchable)
            {
                Vector2 projectilePosition =  new Vector2(BoundingBox.Center.X - p.BoundingBox.Width / 2, 
                                                            _position.Y - p.BoundingBox.Height);
                p.Launch(projectilePosition, new Vector2(0, -1));
                return;
            }
        }
    }

    internal void Die()
    {
        _state = State.Dying;
        _animationDying.Looping = false;
    }

    internal bool ProcessCollision(Rectangle boundingBox)
    {
        foreach(Projectile p in _projectiles)
        {
            if(p.ProcessCollision(boundingBox))
            {
                //this one of my cannonBalls just hit the bounding box
                return true;
            }
        }
        return false;
    }
}