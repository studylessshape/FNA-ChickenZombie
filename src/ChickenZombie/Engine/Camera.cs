using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FNAStart.Engine
{
    public class Camera
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Timer timerShake;

        private Vector2 position;
        private bool isShaking;
        private float shakingStrength = 0;

        public Vector2 Position => position;

        public Camera(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            position = new Vector2(0, 0);
            timerShake = new Timer
            {
                OneShot = true
            };
            timerShake.OnTimeout += TimerShake_OnTimeout;
        }

        private void TimerShake_OnTimeout()
        {
            isShaking = false;
            Reset();
        }

        public void Reset()
        {
            position.X = 0;
            position.Y = 0;
        }

        public void Update(float delta)
        {
            timerShake.Update(delta);

            if (isShaking)
            {
                position.X = (-50 + Random.Shared.Next(100)) / 50.0f * shakingStrength;
                position.Y = (-50 + Random.Shared.Next(100)) / 50.0f * shakingStrength;
            }
        }

        public void Shake(float strength, float duration)
        {
            isShaking = true;
            shakingStrength = strength;

            timerShake.WaitTime = duration;
            timerShake.Restart();
        }

        public void BeginRender()
        {
            spriteBatch.Begin();
        }

        public void RenderTexture(Texture2D? texture, Rectangle? srcRect, Rectangle dstRect, float angleDegrees, Vector2 center)
        {
            var screenDst = new Rectangle(
                dstRect.X - (int)position.X,
                dstRect.Y - (int)position.Y,
                dstRect.Width,
                dstRect.Height
            );

            spriteBatch.Draw(
                texture,
                screenDst,
                srcRect,
                Color.White,
                MathHelper.ToRadians(angleDegrees),
                center,
                SpriteEffects.None,
                0f
            );
        }

        public void EndRender()
        {
            spriteBatch.End();
        }
    }
}
