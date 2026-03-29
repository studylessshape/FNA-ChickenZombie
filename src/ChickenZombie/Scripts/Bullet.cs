using FNAStart.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FNAStart.Scripts
{
    public class Bullet
    {
        private readonly Texture2D bulletTexture;

        private float angle = 0;
        private Vector2 velocity;
        private bool isValid = true;
        private float speed = 800f;

        public Vector2 Position { get; set; }

        public Bullet(Texture2D bulletTexture, float angle)
        {
            this.bulletTexture = bulletTexture;
            this.angle = angle;

            float radinas = MathHelper.ToRadians(angle);
            velocity.X = MathF.Cos(radinas) * speed;
            velocity.Y = MathF.Sin(radinas) * speed;
        }

        public void Update(float delta)
        {
            Position += velocity * delta;

            if (Position.X <= 0 || Position.X >= 1280 || Position.Y <= 0 || Position.Y >= 720)
            {
                isValid = false;
            }
        }

        public void Render(Camera camera)
        {
            Rectangle rectBullet = new Rectangle()
            {
                X = (int)(Position.X - 4),
                Y = (int)(Position.Y - 2),
                Width = 8,
                Height = 4
            };
            camera.RenderTexture(bulletTexture, null, rectBullet, angle, Vector2.Zero);
        }

        public void Hit()
        {
            isValid = false;
        }

        public bool CanRemove()
        {
            return !isValid;
        }
    }
}
