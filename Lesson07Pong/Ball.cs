using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07Pong;

public class Ball
{
    // Point is like Vector2, but X and Y are integers instead of floats
    private Point _dimensions;
    private Vector2 _position, _direction;
    private float _speed;
    private Texture2D _texture;
    private Rectangle _playAreaBoundingBox;


    internal void Initialize(Vector2 position, float speed, Point dimensions, Vector2 direction, Rectangle playAreaBoundingBox)
    {
        _position = position;
        _speed = speed;
        _dimensions = dimensions;
        _direction = direction;
        _playAreaBoundingBox = playAreaBoundingBox;
    }
    
    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("Ball");
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

         _position += _direction * _speed * dt;

        if(_position.X <= _playAreaBoundingBox.Left
            || _position.X + _dimensions.X >= _playAreaBoundingBox.Right)
        {
            _direction.X *= -1;
        }

        if(_position.Y <= _playAreaBoundingBox.Top
            || _position.Y + _dimensions.Y >= _playAreaBoundingBox.Bottom)
        {
            _direction.Y *= -1;
        }
    }
    internal void Draw(SpriteBatch _spriteBatch)
    {
        Rectangle ballRectangle = new Rectangle(_position.ToPoint(), _dimensions);
        _spriteBatch.Draw(_texture, ballRectangle, Color.LightYellow);
    }
}