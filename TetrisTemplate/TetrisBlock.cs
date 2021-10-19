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
    public TetrisBlock()
    {
        grid = new TetrisGrid();
        blocks = new bool[4, 4];
        nextBlocks = new bool[4, 4];
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        game = new GameWorld();
        color = new Color();
        position = new Point((grid.Width/2 - 1) * emptyCell.Width, 0);
        currentBlock = 0;
        nextBlock = 2;
        //currentBlock = GameWorld.Random.Next(7);
        //nextBlock = GameWorld.Random.Next(7);
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
    }

    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 0)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 0)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
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
        
        nextBlocks = (bool[,])blocks.Clone();
    }
    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 1)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 1)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
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
    }
    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 2)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 2)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
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
    }
    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 3)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 3)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
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
    }
    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 4)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 4)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
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
    }
    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 5)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 5)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
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
    }
    private void FillArray(bool[,] array)
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
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentBlock == 6)
        {
            FillArray(blocks);
            base.DrawBlock(gameTime, spriteBatch);
        }
        if (nextBlock == 6)
        {
            FillArray(nextBlocks);
            DrawNextBlock(gameTime, spriteBatch);
        }
    }
}

