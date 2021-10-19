using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/// <summary>
/// Base class of the blocks
/// </summary>
class TetrisBlock
{
    protected TetrisGrid grid;
    protected bool[,] blocks, nextBlocks;
    protected Texture2D emptyCell;
    protected GameWorld game;
    protected Color color;
    protected Point position;
    protected int currentBlock, nextBlock;    

    /// <summary>
    /// Constructor
    /// </summary>
    public TetrisBlock(GameWorld gameWorld)
    {
        grid = new TetrisGrid();
        blocks = new bool[4, 4];
        nextBlocks = new bool[4, 4];
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        game = gameWorld;
        color = new Color();
        position = new Point((grid.Width/2 - 1) * emptyCell.Width, 0);
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
    public bool AllowedPosition() //WERKT NOG NIET
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y])
                {
                    if ((position.X + x * emptyCell.Width < 0) || (position.X + emptyCell.Width + x * emptyCell.Width > grid.Width * emptyCell.Width)
                        || (position.Y + emptyCell.Height + x * emptyCell.Height > grid.Height * emptyCell.Height))
                        //|| (grid.GridArray[position.X / emptyCell.Width + x, position.Y / emptyCell.Height + y] != Color.Gray))
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
    /// </summary>
    public void FullRow()
    {
        
        for (int y = grid.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                
                game.Score += 10;
                if (game.Score % 100 == 0)
                    game.Level += 1;
            }
        }
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
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
        }
        else if (inputHelper.KeyPressed(Keys.A))
        {
            CounterClockWise();
            if (!AllowedPosition())
                ClockWise();
        }
        else if (inputHelper.KeyPressed(Keys.Space))
        {
            position.Y = (grid.Height - blocks.GetLength(1)) * emptyCell.Height;
            while (!AllowedPosition())
                position.Y -= emptyCell.Height;
            TetronimoToGrid();
            Reset();
        }
    }

    public void DrawBlock(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y])
                    spriteBatch.Draw(emptyCell, new Vector2(position.X + (emptyCell.Width * x), position.Y + (emptyCell.Height * y)), color);
            }
        }
    }
    public void DrawNextBlock(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int x = 0; x < nextBlocks.GetLength(0); x++)
        {
            for (int y = 0; y < nextBlocks.GetLength(1); y++)
            {
                if (nextBlocks[x, y])
                    spriteBatch.Draw(emptyCell, new Vector2(315 + (emptyCell.Width * x), 4 * grid.Height + (emptyCell.Height * y)), color);
            }
        }
    }

    public void Reset()
    {
        TetronimoToGrid();
        FullRow();
        position.X = (grid.Width / 2 - 1) * emptyCell.Width;
        position.Y = 0;
    }
}
/// <summary>
/// Characteristics of the I-shape
/// </summary>
class IShaped : TetrisBlock
{
    public IShaped(GameWorld gameWorld) : base(gameWorld)
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
    public OShaped(GameWorld gameWorld) : base(gameWorld)
    {
        color = Color.Yellow;
        
        nextBlocks = (bool[,])blocks.Clone();
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
    public TShaped(GameWorld gameWorld) : base(gameWorld)
    {
        color = Color.Purple;
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
    public SShaped(GameWorld gameWorld) : base(gameWorld)
    {
        color = Color.Green;
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
    public LShaped(GameWorld gameWorld) : base(gameWorld)
    {
        color = Color.Orange;
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
    public ZShaped(GameWorld gameWorld) : base(gameWorld)
    {
        color = Color.Red;        
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
    public JShaped(GameWorld gameWorld) : base(gameWorld)
    {
        color = Color.DarkBlue;        
    }
    public void FillArray(bool[,] array)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 3 || x == 0 || x == 3 || (y == 1 && x == 1) || (y == 2 && x == 1))
                    array[x, y] = false;
                else
                    array[x, y] = true;
            }
        }
    }
}

