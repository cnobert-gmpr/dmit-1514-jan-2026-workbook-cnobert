
using System.Collections.Generic;
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

    private List<Vector2> _trailPositions;
    private float _trailTimer;
    private const float _TrailSpawnInterval = 0.03f;
    private const int _MaxTrailPositions = 12;

    private enum State { Flying, NotFlying}
    private State _state = State.NotFlying;

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

        _trailPositions = new List<Vector2>();
        _trailTimer = 0;
    }
    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("CannonBall");
    }
    internal void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch(_state)
        {
            case State.Flying:
                _position += _direction * _speed * dt;

                _trailTimer += dt;
                if(_trailTimer >= _TrailSpawnInterval)
                {
                    _trailTimer = 0;
                    _trailPositions.Insert(0, _position);
                    if(_trailPositions.Count > _MaxTrailPositions)
                    {
                        _trailPositions.RemoveAt(_trailPositions.Count - 1);
                    }
                }

                if(!BoundingBox.Intersects(_gameBoundingBox))
                {
                    _state = State.NotFlying;
                    _trailPositions.Clear();
                }
                break;
            case State.NotFlying:
                break;
        }
    }
    internal void Draw(SpriteBatch spriteBatch)
    {
        switch(_state)
        {
            case State.Flying:
                spriteBatch.Draw(_texture, _position, Color.White);
                DrawTrail(spriteBatch);
                break;
            case State.NotFlying:
                break;
        }
    }

    private void DrawTrail(SpriteBatch spriteBatch)
    {
        for(int i = 0; i < _trailPositions.Count; i++)
        {
            // gets closer and closer to the number 1 as i increases
            float alpha = 1f - ((float)(i + 1) / (_trailPositions.Count + 1));
            // gets smaller as i increase
            float scale = 1f - (i * 0.1f);

            if(scale < 0.2f)
            {
                scale = 0.2f;
            }
            Vector2 drawPosition = _trailPositions[i];
            Vector2 origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Vector2 centeredPosition = drawPosition + new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            spriteBatch.Draw
                (_texture, centeredPosition, null, 
                Color.Gray * (alpha * 0.5f), 0f, origin, scale, SpriteEffects.None, 0f);
        }
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
        }
        return returnValue;
    }
}