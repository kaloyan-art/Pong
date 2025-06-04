using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        bool isGameOver;

        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballSpeedVector;
        float ballSpeed;

        Vector2 pl1BatPosition;
        Vector2 pl2BatPosition;
        Texture2D paddleTexture;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                       _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;
            ballSpeedVector = new Vector2(1, -1);

            pl1BatPosition = new Vector2(30, _graphics.PreferredBackBufferHeight / 2);
            pl2BatPosition = new Vector2(_graphics.PreferredBackBufferWidth - 30, _graphics.PreferredBackBufferHeight / 2);

            isGameOver = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");

           
            paddleTexture = new Texture2D(GraphicsDevice, 1, 1);
            paddleTexture.SetData(new[] { Color.White });
        }

        private void checkBallCollision()
        {
            Rectangle ballRect = new Rectangle((int)(ballPosition.X - ballTexture.Width / 2),
                                               (int)(ballPosition.Y - ballTexture.Height / 2),
                                               ballTexture.Width,
                                               ballTexture.Height);

            Rectangle pl1BatRect = new Rectangle((int)pl1BatPosition.X - 10, (int)pl1BatPosition.Y - 40, 20, 80);
            Rectangle pl2BatRect = new Rectangle((int)pl2BatPosition.X - 10, (int)pl2BatPosition.Y - 40, 20, 80);


            if (ballPosition.X < 0 || ballPosition.X > _graphics.PreferredBackBufferWidth)
            {
                isGameOver = true;
                return;
            }

            if (ballPosition.Y < ballTexture.Height / 2 || ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                ballSpeedVector.Y = -ballSpeedVector.Y;
            }


            if (ballRect.Intersects(pl1BatRect) || ballRect.Intersects(pl2BatRect))
            {
                ballSpeedVector.X = -ballSpeedVector.X;
            }
        }

        private void updateBallPosition(float updatedBallSpeed)
        {
            float ratio = this.ballSpeedVector.X / this.ballSpeedVector.Y;
            float deltaY = updatedBallSpeed / (float)Math.Sqrt(1 + ratio * ratio);
            float deltaX = Math.Abs(ratio * deltaY);

            if (this.ballSpeedVector.X > 0)
                this.ballPosition.X += deltaX;
            else
                this.ballPosition.X -= deltaX;

            if (this.ballSpeedVector.Y > 0)
                this.ballPosition.Y += deltaY;
            else
                this.ballPosition.Y -= deltaY;
        }

        private void updateBatsPositions()
        {
            var kstate = Keyboard.GetState();
            float paddleSpeed = 200f;
            float delta = paddleSpeed * (float)this.TargetElapsedTime.TotalSeconds;


            if (kstate.IsKeyDown(Keys.W))
                pl1BatPosition.Y -= delta;
            if (kstate.IsKeyDown(Keys.S))
                pl1BatPosition.Y += delta;


            if (kstate.IsKeyDown(Keys.Up))
                pl2BatPosition.Y -= delta;
            if (kstate.IsKeyDown(Keys.Down))
                pl2BatPosition.Y += delta;

            pl1BatPosition.Y = Math.Clamp(pl1BatPosition.Y, 40, _graphics.PreferredBackBufferHeight - 40);
            pl2BatPosition.Y = Math.Clamp(pl2BatPosition.Y, 40, _graphics.PreferredBackBufferHeight - 40);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isGameOver)
            {
                checkBallCollision();
                float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                updateBallPosition(updatedBallSpeed);
                updateBatsPositions();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(
     ballTexture,
     ballPosition,
     null,
     Color.White,
     0f,
     new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
     Vector2.One,
     SpriteEffects.None,
     0f
 );

            _spriteBatch.Draw(paddleTexture, new Rectangle((int)pl1BatPosition.X - 10, (int)pl1BatPosition.Y - 40, 20, 80), Color.White);
            _spriteBatch.Draw(paddleTexture, new Rectangle((int)pl2BatPosition.X - 10, (int)pl2BatPosition.Y - 40, 20, 80), Color.Black);

            if (isGameOver)
            {
                var font = Content.Load<SpriteFont>("DefaultFont");
                string gameOverText = "GAME OVER";
                Vector2 size = font.MeasureString(gameOverText);
                _spriteBatch.DrawString(font, gameOverText,
                    new Vector2((_graphics.PreferredBackBufferWidth - size.X) / 2,
                                (_graphics.PreferredBackBufferHeight - size.Y) / 2),
                    Color.Yellow);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
