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
    protected bool[,] blocks;
    protected Texture2D emptyCell;
    protected Color color;
    protected Point position;
    protected int currentBlock, nextBlock;
    

    /// <summary>
    /// Constructor
    /// </summary>
    public TetrisBlock()
    {
        grid = new TetrisGrid();
        blocks = new bool[4, 4];
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        color = new Color();
        position = new Point((grid.Width/2 - 1) * emptyCell.Width, 0);
        nextBlock = GameWorld.Random.Next(7);
        currentBlock = GameWorld.Random.Next(7);
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
    public bool AllowedPosition(int moveX = 0, int moveY = 0) //WERKT NOG NIET
    {        
        if ((position.X + moveX >= 0) && (position.X + moveX <= (grid.Width - 1) * emptyCell.Width) 
            && (position.Y + moveY <= (grid.Height - 1) * emptyCell.Height) )
            //&& (grid.GridArray[(position.X + moveX)/emptyCell.Width, (position.Y + moveY)/emptyCell.Height] == Color.Gray))
            return true;
        return false;
    }

    public void TetronimoToGrid()
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (blocks[i, j] == true)
                {
                    grid.GridArray[position.X / emptyCell.Width + i, position.Y / emptyCell.Height + j] = color;
                }
            }
        }
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Left))
        {
            if (AllowedPosition(-emptyCell.Width))
                position.X -= emptyCell.Width;
        }
        else if (inputHelper.KeyPressed(Keys.Right))
        {
            if (AllowedPosition(emptyCell.Width))
                position.X += emptyCell.Width;
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
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (blocks[i, j] == true)
                    spriteBatch.Draw(emptyCell, new Vector2(position.X + (emptyCell.Width * i), position.Y + (emptyCell.Height * j)), color);
            }
        }
    }
    public void DrawNextBlock(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (blocks[i, j] == true)
                    spriteBatch.Draw(emptyCell, new Vector2(350 + (emptyCell.Width * i), 4 * grid.Height + (emptyCell.Height * j)), color);
            }
        }
    }

    public void Reset()
    {
        position.X = (grid.Width / 2 - 1) * emptyCell.Width;
        position.Y = 0;
        currentBlock = nextBlock;
        nextBlock = GameWorld.Random.Next(7);
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
                if (x == 1)
                    blocks[x, y] = true;
                else
                    blocks[x, y] = false;
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 0)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 0)
            DrawNextBlock(gameTime, spriteBatch);
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
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 1)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 1)
            DrawNextBlock(gameTime, spriteBatch);
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
                if (y == 0 || y == 3 || x == 3 || (y == 2 && x == 0) || (y == 2 && x == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 2)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 2)
            DrawNextBlock(gameTime, spriteBatch);
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
                if (y == 0 || y == 3 || x == 3 || (y == 1 && x == 0) || (y == 2 && x == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 3)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 3)
            DrawNextBlock(gameTime, spriteBatch);
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
                if (y == 3 || x == 0 || x == 3 || (y == 0 && x == 2) || (y == 1 && x == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 4)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 4)
            DrawNextBlock(gameTime, spriteBatch);
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
                if (y == 0 || y == 3 || x == 3 || (y == 2 && x == 0) || (y == 1 && x == 2))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 5)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 5)
            DrawNextBlock(gameTime, spriteBatch);
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
                if (y == 3 || x == 0 || x == 3 || (y == 1 && x == 1) || (y == 2 && x == 1))
                    blocks[x, y] = false;
                else
                    blocks[x, y] = true;
            }
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 6)
            base.Draw(gameTime, spriteBatch);
        if (nextBlock == 6)
            DrawNextBlock(gameTime, spriteBatch);
    }
}

