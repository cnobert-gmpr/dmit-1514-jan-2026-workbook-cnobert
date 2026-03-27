using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08MosquitoAttack;

public class Trail
{
    private List<Vector2> _trailPositions;
    private float _trailTimer;
    private const float _TrailSpawnInterval = 0.1f;
    private const int _MaxTrailPositions = 12;

    private Texture2D _texture;

    internal void Initialize()
    {
        _trailPositions = new List<Vector2>();
        _trailTimer = 0;
    }
    internal void Update(GameTime gameTime, Vector2 position)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _trailTimer += dt;
        if(_trailTimer >= _TrailSpawnInterval)
        {
            _trailTimer = 0;
            _trailPositions.Insert(0, position);
            if(_trailPositions.Count > _MaxTrailPositions)
            {
                _trailPositions.RemoveAt(_trailPositions.Count - 1);
            }
        }

    }
    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("CannonBall"); //this would be a Trail-specific asset
    }
    internal void Draw(SpriteBatch spriteBatch)
    {
        for(int c = 0; c < _trailPositions.Count; c++)
        {
            // gets closer and closer to the number 0 as i increases
            float alpha = 1f - ((float)(c + 1) / (_trailPositions.Count + 1));
            // gets smaller as i increase
            float scale = 1f - (c * 0.1f);

            if(scale < 0.2f)
            {
                scale = 0.2f;
            }
            Vector2 drawPosition = _trailPositions[c];
            Vector2 origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Vector2 centeredPosition = drawPosition + new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            spriteBatch.Draw
                (_texture, centeredPosition, null, 
                Color.Gray * (alpha * 0.5f), 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
    internal void Clear()
    {
        _trailPositions.Clear();
    }

}