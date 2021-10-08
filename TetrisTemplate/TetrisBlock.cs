using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Base class of the blocks
/// </summary>
class TetrisBlock
{
    // Every block can be defined by a 2D array
    protected bool[,] blocks;
    static Random random;
    Texture2D tetromino;
    protected Color color;

    /// <summary>
    /// Constructor
    /// </summary>
    public TetrisBlock()
    {
        blocks = new bool[4, 4];
        random = new Random();
        tetromino = TetrisGame.ContentManager.Load<Texture2D>("block");
        color = new Color();
    }

    /// <summary>
    /// If A is pressed, the tetris-block will rotate 90 degrees clockwise.
    /// </summary>
    public void ClockWise()
    {
        bool[] rotate = new bool[16];
        int counter = 0;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                rotate[counter] = blocks[x, y];
                counter++;
            }
        }
        for (int y = 0; y < 4; y++)
        {
            for (int x = 3; x >= 0; x--)
            {
                blocks[x, y] = rotate[counter];
                counter--;
            }
        }
    }
    /// <summary>
    /// If D is pressed, the tetris-block will rotate 90 degrees counterclockwise.
    /// </summary>
    public void CounterClockWise()
    {
        bool[] rotate = new bool[16];
        int counter = 0;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                rotate[counter] = blocks[x, y];
                counter++;
            }
        }
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                blocks[x, y] = rotate[counter];
                counter--;
            }
        }
    }
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        random.Next(7);
        spriteBatch.Draw(tetromino, Vector2.Zero, color);
    }
}
/// <summary>
/// Characteristics of the I-shape
/// </summary>
class IShaped : TetrisBlock
{
    color = Color.
    public IShaped()
    {
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

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch()
    }

}
/// <summary>
/// Characteristics of the O-shape
/// </summary>
class OShaped : TetrisBlock
{
    public OShaped()
    {
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

