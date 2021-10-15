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
    IShaped iShape;
    OShaped oShape;
    TShaped tShape;
    SShaped sShape;
    LShaped lShape;
    ZShaped zShape;
    JShaped jShape;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
        blocks = new TetrisBlock();
        iShape = new IShaped();
        oShape = new OShaped();
        tShape = new TShaped();
        sShape = new SShaped();
        lShape = new LShaped();
        zShape = new ZShaped();
        jShape = new JShaped();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime, InputHelper inputHelper)
    {
        blocks.Update(gameTime, inputHelper);
        iShape.Update(gameTime, inputHelper);
        oShape.Update(gameTime, inputHelper);
        tShape.Update(gameTime, inputHelper);
        sShape.Update(gameTime, inputHelper);
        lShape.Update(gameTime, inputHelper);
        zShape.Update(gameTime, inputHelper);
        jShape.Update(gameTime, inputHelper);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch);
        blocks.Draw(gameTime, spriteBatch);
        iShape.Draw(gameTime, spriteBatch);
        oShape.Draw(gameTime, spriteBatch);
        tShape.Draw(gameTime, spriteBatch);
        sShape.Draw(gameTime, spriteBatch);
        lShape.Draw(gameTime, spriteBatch);
        zShape.Draw(gameTime, spriteBatch);
        jShape.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(font, "Level: ", new Vector2(350, grid.Height/2), Color.Blue);
        spriteBatch.DrawString(font, "Score: ", new Vector2(350, 1.5f * grid.Height), Color.Blue);
        spriteBatch.DrawString(font, "Next block: ", new Vector2(350, 2.5f * grid.Height), Color.Blue);
        spriteBatch.End();
    }

    public void Reset()
    {
    }

}
