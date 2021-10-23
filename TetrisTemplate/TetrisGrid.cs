using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    public Texture2D emptyCell;

    /// Indicates which grid positions are occupied by a block.
    Color[,] gridArr;

    /// The number of grid elements in the x-direction.
    public int Width { get { return 10; } }
   
    /// The number of grid elements in the y-direction.
    public int Height { get { return 20; } }

    public Color[,] GridArray 
    { 
        get { return gridArr; } 
        set { gridArr = value; }
    }

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        gridArr = new Color[10, 20];
        Clear();
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        for(int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
                spriteBatch.Draw(emptyCell, new Vector2(x*emptyCell.Width, y*emptyCell.Height), gridArr[x, y]);
        }
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
                gridArr[x, y] = Color.White;
        }
    }
}

