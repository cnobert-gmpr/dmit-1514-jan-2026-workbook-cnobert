using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson06TicTacToe01;

public class TicTacToe : Game
{
    private const int _WindowWidth = 170, _WindowHeight = 170;
    private const float _GameBoardLineWidth = 10;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _backgroundImage, _xImage, _oImage;

    public enum GameState
    {
        Initialize,
        WaitForPlayerMove,
        MakePlayerMove,
        EvaluatePlayerMove,
        GameOver
    }
    private GameState _currentGameState = GameState.Initialize;

    public enum GameSpaceState { X, O, Empty}
    private GameSpaceState _nextTokenToBePlayed = GameSpaceState.X;

    private GameSpaceState[,] _gameBoard =
    {
        { GameSpaceState.O, GameSpaceState.Empty, GameSpaceState.Empty },
        { GameSpaceState.Empty, GameSpaceState.X, GameSpaceState.X },
        { GameSpaceState.Empty, GameSpaceState.Empty, GameSpaceState.Empty },
    };

    private MouseState _currentMouseState, _previousMouseState;

    public TicTacToe()
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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundImage = Content.Load<Texture2D>("TicTacToeBoard");
        _xImage = Content.Load<Texture2D>("X");
        _oImage = Content.Load<Texture2D>("O");

    }

    protected override void Update(GameTime gameTime)
    {
        _currentMouseState = Mouse.GetState();
        // code that occurs in ANY state is put outside of the switch statement
        int x = _currentMouseState.X; //84
        int y = _currentMouseState.Y; // 26
        switch(_currentGameState)
        {
            case GameState.Initialize:
                _nextTokenToBePlayed = GameSpaceState.X;
                for(int row = 0; row < _gameBoard.GetLength(0); row++)
                {
                    for(int col = 0; col < _gameBoard.GetLength(1); col++)
                    {
                        _gameBoard[row, col] = GameSpaceState.Empty;
                    }
                }

                _currentGameState = GameState.WaitForPlayerMove;
                break;
            case GameState.WaitForPlayerMove:
                if( _previousMouseState.LeftButton == ButtonState.Pressed
                    && _currentMouseState.LeftButton == ButtonState.Released)
                {
                    
                    if(x > 0 && x < _WindowWidth && y > 0 && y < _WindowHeight)
                    {
                        _currentGameState = GameState.MakePlayerMove;
                    }
                }
                break;
            case GameState.MakePlayerMove:
                //TODO: add the line width to the equation below
                int theRow = y / _xImage.Height; // 26 / 50 = 0, remainder 26
                int column = x / _xImage.Width; // 84 / 50 = 1, remainder 34

                // TODO: check that the space being clicked on is empty
                // if so, move to MakePlayerMove
                _gameBoard[theRow, column] = _nextTokenToBePlayed;

                _currentGameState = GameState.EvaluatePlayerMove;
                break;
            case GameState.EvaluatePlayerMove:
                //TODO: was there a winner? If so, move to GameOver state.
                // otherwise change next token to be played and go back to the WaitForPlayerMove state
                if(_nextTokenToBePlayed == GameSpaceState.X)
                {
                    _nextTokenToBePlayed = GameSpaceState.O;
                }
                else
                {
                    _nextTokenToBePlayed = GameSpaceState.X;
                }
                _currentGameState = GameState.WaitForPlayerMove;
                break;
            case GameState.GameOver:
                break;
        }

        _previousMouseState = _currentMouseState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        #region 2D array practice
        // int[,] grid = new int[3, 3];
        // grid[0, 0] = 3;
        // grid[1, 0] = 18;
        // grid[2, 0] = 10342;

        int[,] grid =
        {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };    

        Console.Clear();
        //in a 2D array, the first index is the rows, the second is the columns
        //so, to output the number "1", we use row index 1 and column index 0
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for(int col = 0; col < grid.GetLength(1); col++)
            {
                // Console.Write(grid[row, col]);
            }
            //Console.Write("\n");  // Console.WriteLine();
        }

        //Exercise 01: print the array out in reverse order
        //Exercise 02: transpose the array (print it out with rows and columns swapped)
        #endregion

        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundImage, Vector2.Zero, Color.White);
        DrawCurrentGameBoard();
        switch(_currentGameState)
        {
            case GameState.Initialize:
                break;
            case GameState.WaitForPlayerMove:
                #region game token that follows mouse
                Vector2 adjustedMousePosition = new Vector2(
                    _currentMouseState.Position.X - (_xImage.Width / 2),
                    _currentMouseState.Position.Y - (_xImage.Height / 2)
                );

                if(_nextTokenToBePlayed == GameSpaceState.X)
                {
                    _spriteBatch.Draw(_xImage, adjustedMousePosition, Color.White);
                }
                else if (_nextTokenToBePlayed == GameSpaceState.O)
                {
                    _spriteBatch.Draw(_oImage, adjustedMousePosition, Color.White);
                }
                #endregion
                break;
            case GameState.MakePlayerMove:
                break;
            case GameState.EvaluatePlayerMove:
                break;
            case GameState.GameOver:
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    private void DrawCurrentGameBoard()
    {
        //Exercise 01: make it draw the "O" tokens as well
        //Exercise 02: make it draw the tokens, centred on the game spaces by
        //  taking into account the line widths (10 pixels each)
        //  we have created private const float _GameBoardLineWidth = 10; for this.
        for(int row = 0; row < _gameBoard.GetLength(0); row++)
        {
            for(int col = 0; col < _gameBoard.GetLength(1); col++)
            {
                if(_gameBoard[row, col] == GameSpaceState.X)
                {
                    float xPosition = col * _xImage.Width;
                    float yPosition = row * _xImage.Height;
                    Vector2 drawPosition = new Vector2(xPosition, yPosition);
                    _spriteBatch.Draw(_xImage, drawPosition, Color.White);
                }
            }
        }
    }
}
