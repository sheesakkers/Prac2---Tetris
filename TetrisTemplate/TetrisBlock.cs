using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Base class of the blocks
/// </summary>
class TetrisBlock
{
    /// <summary>
    /// Position of the top-left corner of the 4x4 array that describes the tetrominoes.
    /// </summary>
    public Point position;

    /// <summary>
    /// The time to wait before the tetrominoes fall down to the next row.
    /// </summary>
    public double waitTime;

    /// <summary>
    /// 4x4 array that describes the tretronimos using true and false.
    /// </summary>
    protected bool[,] blocks;

    /// <summary>
    /// Specific colors linked to the 7 tetrominoes.
    /// </summary>
    protected Color color;

    /// <summary>
    /// Passed time between every time the Update method is used.
    /// </summary>
    double velocity;

    /// <summary>
    /// Main grid of the game.
    /// </summary>
    TetrisGrid grid;

    /// <summary>
    /// A single square sprite in the grid.
    /// </summary>
    Texture2D emptyCell;

    /// <summary>
    /// Main game.
    /// </summary>
    GameWorld game;

    /// <summary>
    /// Constructor
    /// </summary>
    public TetrisBlock(GameWorld gameWorld, TetrisGrid tetrisGrid)
    {
        grid = tetrisGrid;
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        game = gameWorld;

        position = new Point((grid.Width / 2 - 1) * emptyCell.Width, -emptyCell.Height);
        waitTime = 0.3;
        blocks = new bool[4, 4];
        color = new Color();
        velocity = 1;
    }

    /// <summary>
    /// Rotates the 4x4 array that describes the tetromino 90 degrees clockwise.
    /// </summary>
    void ClockWise()
    {
        bool[] rotate = new bool[16];
        int counter = 0;
        for (int x = 0; x <= 3; x++)
        {
            for (int y = 0; y <= 3; y++)
            {
                rotate[counter] = blocks[x, y];
                counter++;
            }
        }
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x <= 3; x++)
            {
                counter--;
                blocks[x, y] = rotate[counter];
            }
        }
    }

    /// <summary>
    /// Rotates the 4x4 array that describes the tetromino 90 degrees counterclockwise.
    /// </summary>
    void CounterClockWise()
    {
        bool[] rotate = new bool[16];
        int counter = 0;
        for (int y = 0; y <= 3; y++)
        {
            for (int x = 0; x <= 3; x++)
            {
                rotate[counter] = blocks[x, y];
                counter++;
            }
        }
        for (int x = 3; x >= 0; x--)
        {
            for (int y = 0; y <= 3; y++)
            {
                counter--;
                blocks[x, y] = rotate[counter];
            }
        }
    }

    /// <summary>
    /// Checks if the tetromino is allowed to move to desired position.
    /// It can not go beyond the borders of the grid or overlap with another tetromino.
    /// Returns true or false.
    /// </summary>
    bool AllowedPosition()
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y])
                {
                    if ((position.X + x * emptyCell.Width < 0) || (position.Y + y * emptyCell.Height < 0)
                        || (position.X + emptyCell.Width + x * emptyCell.Width > grid.Width * emptyCell.Width)
                        || (position.Y + emptyCell.Height + y * emptyCell.Height > grid.Height * emptyCell.Height)
                        || (grid.GridArray[position.X / emptyCell.Width + x, position.Y / emptyCell.Height + y] != Color.White))
                        return false;
                }
            }
        }    
        return true;
    }

    /// <summary>
    /// If the tetromino has reached the bottom and cannot be moved anymore, its position will be documented in the main grid.
    /// </summary>
    void TetrominoToGrid()
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y])
                    grid.GridArray[position.X / emptyCell.Width + x, position.Y / emptyCell.Height + y] = color;
            }
        }
    }

    /// <summary>
    /// Checks if the current row is full.
    /// Returns true or false.
    /// </summary>
    bool IsRowFull(int y)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            if (grid.GridArray[x, y] == Color.White)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if there are any full rows using the IsRowFull-method. 
    /// The amount of full rows is saved in a variable.
    /// Every full row will increase the score with 10 points.
    /// The player can level-up by scoring 100 points per level and the blocks will start moving down faster.
    /// </summary>
    void ClearRows()
    {
        int counter = 0;
        for (int y = grid.Height - 1; y > 0; y--)
        {
            if (IsRowFull(y))
            {
                for (int j = y; j > 0; j--)
                {
                    for (int i = 0; i < grid.Width; i++)
                    {
                        grid.GridArray[i, j] = grid.GridArray[i, j - 1];
                    }
                }
                y++;
                counter++;
            }
        }

        if (counter > 0)
        {
            game.score += 10 * counter;
            if (game.score >= game.level * 100)
            {
                game.level += 1;
                game.levelUp = true;
                waitTime *= 3;
                game.levelUpSound.Play();
            }
            else
            {
                if (counter == 4)
                    game.clear4.Play();
                else
                    game.clearRow.Play();
            }
        }
    }

    /// <summary>
    /// The tetromino will move automatically down.
    /// </summary>
    void MovingDown()
    {
        if (velocity <= waitTime)
        {
            position.Y += emptyCell.Height;
            if (!AllowedPosition())
            {
                position.Y -= emptyCell.Height;
                Reset();
            }
            velocity = 1;
        }
    }

    /// <summary>
    /// If the left-key is pressed, the tetromino will move to the left if possible.
    /// If the right-key is pressed, the tetromino will move to the right if possible.
    /// If the D is pressed, the tetromino will rotate clockwise if possible.
    /// If the A is pressed, the tetromino will rotate counterclockwise if possible.
    /// If the space-bar is pressed, the tetromino will move down as far as possible and the position will be saved in the main grid.
    /// </summary>
    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Left))
        {
            position.X -= emptyCell.Width;
            if (!AllowedPosition())
                position.X += emptyCell.Width;
        }
        else if (inputHelper.KeyPressed(Keys.Right))
        {
            position.X += emptyCell.Width;
            if (!AllowedPosition())
                position.X -= emptyCell.Width;
        }
        else if (inputHelper.KeyPressed(Keys.D))
        {
            ClockWise();
            if (!AllowedPosition())
                CounterClockWise();
            else
                game.rotate.Play();
        }
        else if (inputHelper.KeyPressed(Keys.A))
        {
            CounterClockWise();
            if (!AllowedPosition())
                ClockWise();
            else
                game.rotate.Play();
        }
        else if (inputHelper.KeyPressed(Keys.Space))
        {
            position.Y += emptyCell.Height;
            while (AllowedPosition())
                position.Y += emptyCell.Height;
            position.Y -= emptyCell.Height;
            Reset();
        }
    }

    /// <summary>
    /// Updates the movement of the tetromino.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        velocity -= gameTime.ElapsedGameTime.TotalSeconds;
        MovingDown();
    }

    /// <summary>
    /// Draws the tetrominoes on screen.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, Point pos)
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y])
                    spriteBatch.Draw(emptyCell, new Vector2(pos.X + (emptyCell.Width * x), pos.Y + (emptyCell.Height * y)), color);
            }
        }
    }

    /// <summary>
    /// Resets the entire game if the the game is over.
    /// Otherwise, it will save the current tetromino the the main grid, clear rows and shows a new tetromino at the top of the grid.
    /// </summary>
    void Reset()
    {
        game.levelUp = false;
        if (!AllowedPosition())
        {
            game.gameState = GameWorld.GameState.GameOver;
            game.gameOver.Play();
        }
        else
        {
            TetrominoToGrid();
            ClearRows();
            game.BlockDown();
        }
    }
}
/// <summary>
/// Characteristics of the I-shape
/// </summary>
class IShaped : TetrisBlock
{
    public IShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.Aqua;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 1)
                    array[x, y] = true;
                else
                    array[x, y] = false;
            }
        }
    }
}
/// <summary>
/// Characteristics of the O-shape
/// </summary>
class OShaped : TetrisBlock
{
    public OShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.Yellow;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 0 || y == 0 || x == 3 || y == 3)
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the T-shape
/// </summary>
class TShaped : TetrisBlock
{
    public TShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.Purple;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 0 || y == 3 || x == 3 || (y == 2 && x == 0) || (y == 2 && x == 2))
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the S-shape
/// </summary>
class SShaped : TetrisBlock
{
    public SShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.Green;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 0 || y == 3 || x == 3 || (y == 1 && x == 0) || (y == 2 && x == 2))
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the L-shape
/// </summary>
class LShaped : TetrisBlock
{
    public LShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.Orange;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 3 || x == 0 || x == 3 || (y == 0 && x == 2) || (y == 1 && x == 2))
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the Z-shape
/// </summary>
class ZShaped : TetrisBlock
{
    public ZShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.Red;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 0 || y == 3 || x == 3 || (y == 2 && x == 0) || (y == 1 && x == 2))
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the J-shape
/// </summary>
class JShaped : TetrisBlock
{
    public JShaped(GameWorld gameWorld, TetrisGrid tetrisGrid) : base(gameWorld, tetrisGrid)
    {
        color = Color.DarkBlue;
        FillArray(blocks);
    }
    /// <summary>
    /// Fills in the tetromino in the array using true and false.
    /// </summary>
    void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 3 || x == 0 || x == 3 || (y == 1 && x == 2) || (y == 2 && x == 2))
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}

