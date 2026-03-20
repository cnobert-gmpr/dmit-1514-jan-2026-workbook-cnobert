using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;

namespace Lesson08MosquitoAttack;

public class MosquitoAttack : Game
{
    private const int _WindowWidth = 550, _WindowHeight = 400;
    private const int _NumMosquitoes = 10;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _kbCurrentState, _kbPreviousState;

    private Texture2D _background;
    private SpriteFont _font;
    private string _message;

    private enum GameState { Playing, Paused, Over}
    private GameState _gameState = GameState.Playing;

    Cannon _cannon;

    Mosquito[] _mosquitoes;

    private Rectangle GameBoundingBox => new Rectangle(0, 0, _WindowWidth, _WindowHeight);

    public MosquitoAttack()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();

        _cannon = new Cannon();
        _cannon.Initialize(new Vector2(50, 325), 150, GameBoundingBox);

        _mosquitoes = new Mosquito[_NumMosquitoes];
        for(int c = 0; c < _NumMosquitoes; c++)
        {
            _mosquitoes[c] = new Mosquito();
        }

        Random random = new Random();
        foreach(Mosquito m in _mosquitoes)
        {
            int xDirection = random.Next(1, 3) == 2 ? -1 : 1;
            int xPosition = random.Next(1, _WindowWidth - 50);
            int yPosition = random.Next(1, 151);
            int speed = random.Next(150, 251);

            m.Initialize(new Vector2(xPosition, yPosition), speed, 
                new Vector2(xDirection, 0), GameBoundingBox);
        }

        base.Initialize();
    }
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");

        _cannon.LoadContent(Content);
        foreach(Mosquito m in _mosquitoes)
        {
            m.LoadContent(Content);
        }
        //_font = Content.Load<SpriteFont>("SystemArialFont");
        #region students, don't look here
        //MacOS ONLY
         byte[] fontBytes = File.ReadAllBytes("Content/Tahoma.ttf");
        _font = TtfFontBaker.Bake(fontBytes, 30, 1024, 1024, new[] { CharacterRange.BasicLatin }).CreateSpriteFont(GraphicsDevice);
        #endregion

    }
    protected override void Update(GameTime gameTime)
    {
        _kbCurrentState = Keyboard.GetState();

        switch(_gameState)
        {
            case GameState.Playing:
                if(_kbCurrentState.IsKeyDown(Keys.A))
                    _cannon.Direction = new Vector2(-1, 0);
                else if(_kbCurrentState.IsKeyDown(Keys.D))
                    _cannon.Direction = new Vector2(1, 0);
                else
                    _cannon.Direction = new Vector2(0, 0);
                
                _cannon.Update(gameTime);
                
                foreach(Mosquito m in _mosquitoes)
                {
                    m.Update(gameTime);
                    if(m.Alive && _cannon.ProcessCollision(m.BoundingBox))
                    {
                        m.Die();
                    }
                }

                if(Pressed(Keys.P))
                {
                    _gameState = GameState.Paused;
                    _message = "Game paused, press P to start playing.";
                }

                if(Pressed(Keys.Space))
                {
                    _cannon.Shoot();
                }

                break;
            case GameState.Paused:
                if(Pressed(Keys.P))
                    _gameState = GameState.Playing;
                break;
            case GameState.Over:
                break;
        }

        base.Update(gameTime);

        _kbPreviousState = _kbCurrentState;
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        switch(_gameState)
        {
            case GameState.Playing:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
                _cannon.Draw(_spriteBatch);
                foreach(Mosquito m in _mosquitoes)
                    m.Draw(_spriteBatch);
                break;
            case GameState.Paused:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.Silver);
                _cannon.Draw(_spriteBatch);
                foreach(Mosquito m in _mosquitoes)
                    m.Draw(_spriteBatch);
                _spriteBatch.DrawString(_font, _message, new Vector2(10, 135), Color.White);
                break;
            case GameState.Over:
                break;
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private bool Pressed(Keys key)
    {
        // it's a new key press if it is down now, but it was up 1/60 of a second ago
        return _kbCurrentState.IsKeyDown(key) && _kbPreviousState.IsKeyUp(key);
    }
}
