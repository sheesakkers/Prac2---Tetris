using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    enum GameState
    {
        Playing,
        GameOver
    }

    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return random; } }
    static Random random;

    /// <summary>
    /// The main font of the game.
    /// </summary>
    SpriteFont font;

    /// <summary>
    /// The current game state.
    /// </summary>
    GameState gameState;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;

    /// <summary>
    /// Blocks.
    /// </summary>
    TetrisBlock blocks;
    TetrisBlock nextBlocks;
    IShaped iShape;
    OShaped oShape;
    TShaped tShape;
    SShaped sShape;
    LShaped lShape;
    ZShaped zShape;
    JShaped jShape;

    int level = 1, score = 0;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public int Score
    {
        get { return score; }
        set { score = value; }
    }


    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
        blocks = new TetrisBlock(this);
        nextBlocks = new TetrisBlock(this);
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
    {
        blocks.Update(gameTime, inputHelper);
        nextBlocks.Update(gameTime, inputHelper);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch);
        blocks.DrawBlock(gameTime, spriteBatch);
        nextBlocks.DrawNextBlock(gameTime, spriteBatch);
        spriteBatch.DrawString(font, "Level: " + level, new Vector2(315, grid.Height/2), Color.Blue);
        spriteBatch.DrawString(font, "Score: " + score, new Vector2(315, 1.5f * grid.Height), Color.Blue);
        spriteBatch.DrawString(font, "Next block: ", new Vector2(315, 2.5f * grid.Height), Color.Blue);
        spriteBatch.End();
    }

    public void Reset()
    {
        level = 1;
        score = 0;
    }

    private TetrisBlock RandomBlock()
    {
        int random = Random.Next(0, 7);
        if (random == 0)
            return new IShaped(this);
        else if (random == 1)
            return new OShaped(this);
        else if (random == 2)
            return new TShaped(this);
        else if (random == 3)
            return new SShaped(this);
        else if (random == 4)
            return new LShaped(this);
        else if (random == 5)
            return new ZShaped(this);
        return new JShaped(this);
    }
}
