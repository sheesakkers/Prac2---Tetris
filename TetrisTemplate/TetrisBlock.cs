using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/// <summary>
/// Base class of the blocks
/// </summary>
class TetrisBlock
{
    protected TetrisGrid grid;
    protected bool[,] blocks;
    protected Texture2D emptyCell;
    protected GameWorld game;
    protected Color color;
    protected Point position;

    public Point Position
    {
        get { return position; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public TetrisBlock(GameWorld gameWorld, TetrisGrid tetrisGrid)
    {
        grid = tetrisGrid;
        blocks = new bool[4, 4];
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        game = gameWorld;
        color = new Color();
        position = new Point((grid.Width/2 - 1) * emptyCell.Width, -emptyCell.Height);
    }

    /// <summary>
    /// If D is pressed, the tetris-block will rotate 90 degrees clockwise.
    /// </summary>
    private void ClockWise()
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
    /// If A is pressed, the tetris-block will rotate 90 degrees counterclockwise.
    /// </summary>
    private void CounterClockWise()
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
    /// Checks if the block is allowed to move to desired position
    /// </summary>
    /// <returns></returns>
    public bool AllowedPosition()
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
                        || (grid.GridArray[position.X / emptyCell.Width + x, position.Y / emptyCell.Height + y] != Color.Gray))
                        return false;
                }
            }
        }    
        return true;
    }

    public void TetronimoToGrid()
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
    /// Checks if there are any full rows. 
    /// The score is dependent on the amount of lines that is cleared.
    /// The player can level-up by scoring higher and higher per level.
    /// </summary>
    public void FullRow()
    {
        int counter = 0;
        Color[,] arrayCopy = new Color[grid.Width, grid.Height];
        for (int y = grid.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (grid.GridArray[x, y] == Color.Gray)
                    break;
                else if (x == grid.Width - 1)
                {
                    int shift = 1;
                    for (int j = 0; j < grid.Height; j++)
                    {
                        for (int i = 0; i < grid.Width; i++)
                        {
                            if (j == 0)
                                arrayCopy[i, 0] = Color.Gray;
                            else if (j == y + shift)
                            {
                                arrayCopy[i, j] = grid.GridArray[i, j];
                                if (x == grid.Width - 1)
                                    shift--;
                            }
                            else
                                arrayCopy[i, j] = grid.GridArray[i, j - shift];
                        }
                    }
                    shift++;
                    counter++;
                }
            }
        }
        if (counter > 0)
        {
            game.Score += 10 * counter;
            grid.GridArray = arrayCopy;
        }
        if ((game.Score > 0) && (game.Score % (250 * 0.7 * game.Level) == 0))
            game.Level += 1;
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
    {
        if (!AllowedPosition())
            position.Y += emptyCell.Height; //DRAAIEN FIXEN
        else if (inputHelper.KeyPressed(Keys.Left))
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
        }
        else if (inputHelper.KeyPressed(Keys.A))
        {
            CounterClockWise();
            if (!AllowedPosition())
                ClockWise();
        }
        else if (inputHelper.KeyPressed(Keys.Space))
        {
            position.Y += emptyCell.Height;
            while (AllowedPosition())
                position.Y += emptyCell.Height;
            position.Y -= emptyCell.Height;
            Reset();
        }
        inputHelper.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point pos)
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

    public void Reset()
    {
        TetronimoToGrid();
        FullRow();
        position.X = (grid.Width / 2 - 1) * emptyCell.Width;
        position.Y = -emptyCell.Height;
        game.BlockDown();
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

    public void FillArray(bool[,] array)
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
    public void FillArray(bool[,] array)
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
    public void FillArray(bool[,] array)
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
    public void FillArray(bool[,] array)
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
    public void FillArray(bool[,] array)
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
    public void FillArray(bool[,] array)
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
    public void FillArray(bool[,] array)
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

