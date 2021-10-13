using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/// <summary>
/// Base class of the blocks
/// </summary>
class TetrisBlock
{
    TetrisGrid grid;
    protected bool[,] blocks;
    protected Texture2D tetromino;
    protected Color color;
    protected Point position;
    protected int randomBlock;

    /// <summary>
    /// Constructor
    /// </summary>
    public TetrisBlock()
    {
        grid = new TetrisGrid();
        blocks = new bool[4, 4];
        tetromino = TetrisGame.ContentManager.Load<Texture2D>("block");
        color = new Color();
        position = new Point((grid.Width/2)*tetromino.Width, 0);
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
        for (int y = 0; y <= 3; y++)
        {
            for (int x = 3; x >= 0; x--)
            {
                blocks[x, y] = rotate[counter];
                counter--;
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
                blocks[x, y] = rotate[counter];
                counter--;
            }
        }
    }
    /// <summary>
    /// Checks if the block is allowed to move to desired position
    /// </summary>
    /// <returns></returns>
    public bool AllowedPosition()
    {
        if (position.X > 0 || position.X < (grid.Width - 1) * tetromino.Width)
            return true;
        return false;
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Left))
        {
            if (AllowedPosition())
                position.X -= tetromino.Width;
        }
        else if (inputHelper.KeyPressed(Keys.Right))
        {
            if (AllowedPosition())
                position.X += tetromino.Width;
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

        randomBlock = GameWorld.Random.Next(7);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (blocks[i, j] == true)
                    spriteBatch.Draw(tetromino, new Vector2(position.X + (tetromino.Width * i), position.Y + (tetromino.Height * j)), color);
            }
        }
    }
}
/// <summary>
/// Characteristics of the I-shape
/// </summary>
class IShaped : TetrisBlock
{
    public IShaped()
    {
        color = Color.Aqua;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (y == 1)
                    blocks[x, y] = true;
                else
                    blocks[x, y] = false;
            }
        }
    }

    
}
/// <summary>
/// Characteristics of the O-shape
/// </summary>
class OShaped : TetrisBlock
{
    public OShaped()
    {
        color = Color.Yellow;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 0 || y == 0 || x == 3 || y == 3)
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the T-shape
/// </summary>
class TShaped : TetrisBlock
{
    public TShaped()
    {
        color = Color.Purple;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 0 || x == 3 || y == 3 || (x == 2 && y == 0) || (x == 2 && y == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the S-shape
/// </summary>
class SShaped : TetrisBlock
{
    public SShaped()
    {
        color = Color.Green;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 0 || x == 3 || y == 3 || (x == 1 && y == 0) || (x == 2 && y == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the L-shape
/// </summary>
class LShaped : TetrisBlock
{
    public LShaped()
    {
        color = Color.Orange;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 3 || y == 0 || y == 3 || (x == 0 && y == 2) || (x == 1 && y == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the Z-shape
/// </summary>
class ZShaped : TetrisBlock
{
    public ZShaped()
    {
        color = Color.Red;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 0 || x == 3 || y == 3 || (x == 2 && y == 0) || (x == 1 && y == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
}
/// <summary>
/// Characteristics of the J-shape
/// </summary>
class JShaped : TetrisBlock
{
    public JShaped()
    {
        color = Color.DarkBlue;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (x == 3 || y == 2 || y == 3 || (x == 1 && y == 0) || (x == 2 && y == 0))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
}

