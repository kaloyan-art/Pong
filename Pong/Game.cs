using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballSpeedVector;
        float ballSpeed;
        double remainderX;
        double remainderY;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
           
            ballPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            ballSpeed = 200f; 

          
            ballSpeedVector = new Vector2(1f, -1f);
            ballSpeedVector.Normalize(); 

            remainderX = 0;
            remainderY = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball"); 
        }

        protected override void Update(GameTime gameTime)
        {
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

          
            float moveX = ballSpeedVector.X * ballSpeed * elapsed;
            float moveY = ballSpeedVector.Y * ballSpeed * elapsed;

           
            remainderX += moveX;
            remainderY += moveY;

           
            int deltaX = (int)remainderX;
            int deltaY = (int)remainderY;

            ballPosition.X += deltaX;
            ballPosition.Y += deltaY;

           
            remainderX -= deltaX;
            remainderY -= deltaY;

          
            int screenWidth = _graphics.PreferredBackBufferWidth;
            int screenHeight = _graphics.PreferredBackBufferHeight;
            float halfBallWidth = ballTexture.Width / 2f;
            float halfBallHeight = ballTexture.Height / 2f;

           
            if (ballPosition.X - halfBallWidth <= 0 || ballPosition.X + halfBallWidth >= screenWidth)
            {
                ballSpeedVector.X *= -1;
                ballPosition.X = MathHelper.Clamp(ballPosition.X, halfBallWidth, screenWidth - halfBallWidth); 
            }

          
            if (ballPosition.Y - halfBallHeight <= 0 || ballPosition.Y + halfBallHeight >= screenHeight)
            {
                ballSpeedVector.Y *= -1;
                ballPosition.Y = MathHelper.Clamp(ballPosition.Y, halfBallHeight, screenHeight - halfBallHeight); 
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
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
