using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07Pong;

public class Paddle
{
    private Point _dimensions;
    private Vector2 _position, _direction;
    private float _speed;
    private Texture2D _texture;
    private Rectangle _playAreaBoundingBox;

    //"write only" property
    internal Vector2 Direction
    {
        set
        {
            value.X = 0;
            _direction = value;
        }
    }

    internal void Initialize(Vector2 position, float speed, Point dimensions, Rectangle playAreaBoundingBox)
    {
        _position = position;
        _speed = speed;
        _dimensions = dimensions;
        _playAreaBoundingBox = playAreaBoundingBox;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("Paddle");
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _position += _direction * _speed * dt;

        if(_position.Y <= _playAreaBoundingBox.Top)
        {
            _position.Y = _playAreaBoundingBox.Top;
        }
        else if(_position.Y   + _dimensions.Y >= _playAreaBoundingBox.Bottom)
        {
            _position.Y = _playAreaBoundingBox.Bottom - _dimensions.Y;
        }
    }

    internal void Draw(SpriteBatch _spriteBatch)
    {
        Rectangle paddleRectangle = new Rectangle(_position.ToPoint(), _dimensions);
        _spriteBatch.Draw(_texture, paddleRectangle, Color.LightYellow);
    }
}