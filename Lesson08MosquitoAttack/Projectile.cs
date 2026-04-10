using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public abstract class Projectile
{
    // the "private" access modifier also hides data or methods from children
    // so, whatever we want the children to see, we will designate as "protected"
    protected Vector2 _position, _direction;
    protected Point _dimensions;
    protected float _speed;

    protected Rectangle _gameBoundingBox;

    protected enum State { Flying, NotFlying}
    protected State _state = State.NotFlying;

    internal Rectangle BoundingBox
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _dimensions.X, _dimensions.Y);
    }

    internal bool Launchable
    {
        get => _state == State.NotFlying;
    }

    //"virtual" means "my children MAY override this method, but they don't have to"
    internal virtual void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _position = Vector2.Zero;
        _direction = Vector2.Zero;
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;
    }
    
    // "abstract" forces the child class to define a method with this signature
    // but does not define the method
    // also, if at least one method is "abstract", then the entire class must be labelled
    // as abstract 
    internal abstract void LoadContent(ContentManager content);
    internal abstract void Update(GameTime gameTime);
    internal abstract void Draw(SpriteBatch spriteBatch);
    
    internal void Launch(Vector2 position, Vector2 direction)
    {
        if(_state == State.NotFlying)
        {
            _position = position;
            _direction = direction;
            _state = State.Flying;
        }
    }

    internal virtual bool ProcessCollision(Rectangle boundingBox)
    {
        bool returnValue = false;
        if(_state == State.Flying && BoundingBox.Intersects(boundingBox))
        {
            returnValue = true;
            _state = State.NotFlying;
        }
        return returnValue;
    }
}