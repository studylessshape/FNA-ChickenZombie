using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FNAStart
{
    internal class MainGame : Game
    {
        KeyboardState _keyboardPrev;
        public MainGame()
        {
            new GraphicsDeviceManager(this);
            _keyboardPrev = new KeyboardState();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardCur = Keyboard.GetState();

            if (_keyboardPrev.IsKeyUp(Keys.Space) && keyboardCur.IsKeyDown(Keys.Space))
            {
                Console.WriteLine("Space bar was pressed!");
            }

            _keyboardPrev = keyboardCur;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
