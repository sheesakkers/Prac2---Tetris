using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

/// <summary>
/// A class for representing the game world.
/// </summary>
class GameWorld
{
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    public enum GameState
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
    /// The current game state.
    /// </summary>
    public GameState gameState;

    /// <summary>
    /// Level, score score and if the level should increase.
    /// </summary>
    public int level, score;
    public bool levelUp;

    /// <summary>
    /// Sound effects and song of the game.
    /// </summary>
    public SoundEffect rotate;
    public SoundEffect clearRow;
    public SoundEffect clear4;
    public SoundEffect levelUpSound;
    public SoundEffect gameOver;
    public Song mainTheme;

    /// <summary>
    /// The main font of the game.
    /// </summary>
    SpriteFont font;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;

    /// <summary>
    /// Current block and the next block.
    /// </summary>
    TetrisBlock currentBlock;
    TetrisBlock nextBlock;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public GameWorld()
    {
        random = new Random();

        gameState = GameState.Playing;

        level = 1;
        score = 0;
        levelUp = false;

        rotate = TetrisGame.ContentManager.Load<SoundEffect>("rotate");
        clearRow = TetrisGame.ContentManager.Load<SoundEffect>("clear");
        clear4 = TetrisGame.ContentManager.Load<SoundEffect>("clear4");
        levelUpSound = TetrisGame.ContentManager.Load<SoundEffect>("success");
        gameOver = TetrisGame.ContentManager.Load<SoundEffect>("gameover");
        mainTheme = TetrisGame.ContentManager.Load<Song>("maintheme");

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
        currentBlock = new TetrisBlock(this, grid);
        nextBlock = new TetrisBlock(this, grid);

        currentBlock = RandomBlock();
        nextBlock = RandomBlock();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        currentBlock.HandleInput(gameTime, inputHelper);

        // if the game is over the player can restart by pressing <ENTER>
        if (gameState == GameState.GameOver)
        {
            if (inputHelper.KeyPressed(Keys.Enter))
            {
                Reset();
                gameState = GameState.Playing;
            }
        }

        inputHelper.Update(gameTime);
    }
    /// <summary>
    /// Only update if the player is playing the game.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        if (gameState == GameState.Playing)
        {
            currentBlock.Update(gameTime);
            nextBlock.Update(gameTime);
        }
    }
    /// <summary>
    /// Draw everything on screen.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        if (gameState == GameState.Playing)
        {
            grid.Draw(spriteBatch);
            currentBlock.Draw(spriteBatch, currentBlock.position);
            nextBlock.Draw(spriteBatch, new Point(315, 4 * grid.Height));
            spriteBatch.DrawString(font, "Level: " + level, new Vector2(315, grid.Height / 2), Color.Blue);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(315, 1.5f * grid.Height), Color.Blue);
            spriteBatch.DrawString(font, "Next block: ", new Vector2(315, 2.5f * grid.Height), Color.Blue);
            if (levelUp)
                spriteBatch.DrawString(font, "Level Up!!", new Vector2(315, 11f * grid.Height), Color.LawnGreen);
        }
        else
        {
            grid.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Level: " + level, new Vector2(315, grid.Height / 2), Color.Blue);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(315, 1.5f * grid.Height), Color.Blue);
            spriteBatch.DrawString(font, "Next block: ", new Vector2(315, 2.5f * grid.Height), Color.Blue);
            spriteBatch.DrawString(font, "GAME OVER !!", new Vector2(315, 10f * grid.Height), Color.Red);
            spriteBatch.DrawString(font, "Druk op <ENTER> om opnieuw te spelen.", new Vector2(315, 11f * grid.Height), Color.Blue);
        }        
        spriteBatch.End();
    }
    /// <summary>
    /// If the current tetromino is placed on the grid, a new current tetromino will become the next tetromino.
    /// The new next tetromino will be randomly chosen.
    /// </summary>
    public void BlockDown()
    {
        currentBlock = nextBlock;
        nextBlock = RandomBlock();

        currentBlock.position.X = (grid.Width / 2 - 1) * grid.emptyCell.Width;
        currentBlock.position.Y = 0;
    }
    /// <summary>
    /// Reset the entire game.
    /// </summary>
    public void Reset()
    {
        grid.Clear();
        level = 1;
        score = 0;
        currentBlock.waitTime = 0.3;
        currentBlock = RandomBlock();
        nextBlock = RandomBlock();
        currentBlock.position.X = (grid.Width / 2 - 1) * grid.emptyCell.Width;
        currentBlock.position.Y = 0;
        levelUp = false;
    }
    /// <summary>
    /// Random selection of the tetrominoes.
    /// </summary>
    TetrisBlock RandomBlock()
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
