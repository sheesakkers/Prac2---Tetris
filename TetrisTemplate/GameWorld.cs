using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    TetrisBlock currentBlock;
    TetrisBlock nextBlock;
    IShaped iShape;
    OShaped oShape;
    TShaped tShape;
    SShaped sShape;
    LShaped lShape;
    ZShaped zShape;
    JShaped jShape;

    int level = 1, score = 0, speed;

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
        currentBlock = new TetrisBlock(this, grid);
        nextBlock = new TetrisBlock(this, grid);
        currentBlock = RandomBlock();
        nextBlock = RandomBlock();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
    {
        if (gameState == GameState.Playing)
        {
            currentBlock.Update(gameTime, inputHelper);
            nextBlock.Update(gameTime, inputHelper);
        }
        else
        {
            if (inputHelper.KeyPressed(Keys.Enter))
                gameState = GameState.Playing;
        }   
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        if (gameState == GameState.Playing)
        {
            grid.Draw(gameTime, spriteBatch);
            currentBlock.Draw(gameTime, spriteBatch, currentBlock.Position);
            nextBlock.Draw(gameTime, spriteBatch, new Point(315, 4 * grid.Height));
            spriteBatch.DrawString(font, "Level: " + level, new Vector2(315, grid.Height / 2), Color.Blue);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(315, 1.5f * grid.Height), Color.Blue);
            spriteBatch.DrawString(font, "Next block: ", new Vector2(315, 2.5f * grid.Height), Color.Blue);
        }
        else
        {
            grid.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, "Level: " + level, new Vector2(315, grid.Height / 2), Color.Blue);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(315, 1.5f * grid.Height), Color.Blue);
            spriteBatch.DrawString(font, "Next block: ", new Vector2(315, 2.5f * grid.Height), Color.Blue);
            spriteBatch.DrawString(font, "GAME OVER !!", new Vector2(315, 10f * grid.Height), Color.Red);
            spriteBatch.DrawString(font, "Druk op <ENTER> om opnieuw te spelen.", new Vector2(315, 11f * grid.Height), Color.Blue);
        }        
        spriteBatch.End();
    }

    public void BlockDown()
    {
        currentBlock = nextBlock;
        nextBlock = RandomBlock();
    }

    public void Reset()
    {
        grid.Clear();
        level = 1;
        score = 0;
        currentBlock.speed = 1;
        currentBlock = RandomBlock();
        nextBlock = RandomBlock();
    }

    public TetrisBlock RandomBlock()
    {
        int random = Random.Next(0, 7);
        if (random == 0)
            return new IShaped(this, grid);
        else if (random == 1)
            return new OShaped(this, grid);
        else if (random == 2)
            return new TShaped(this, grid);
        else if (random == 3)
            return new SShaped(this, grid);
        else if (random == 4)
            return new LShaped(this, grid);
        else if (random == 5)
            return new ZShaped(this, grid);
        return new JShaped(this, grid);
    }
}
