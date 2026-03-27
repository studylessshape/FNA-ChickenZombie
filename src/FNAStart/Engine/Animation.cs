using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FNAStart.Engine
{
    public class Animation
    {
        private struct Frame
        {
            public Rectangle RectSrc;
            public Texture2D? Texture = null;

            public Frame()
            {

            }

            public Frame(Texture2D? texture, Rectangle rectSrc)
            {
                this.Texture = texture;
                this.RectSrc = rectSrc;
            }
        }

        private readonly Timer timer;
        private readonly List<Frame> frameList = [];
        private int indexFrame = 0;

        public Vector2 Position { private get; set; }
        public float Angle { private get; set; }
        public Vector2 Center { private get; set; } = Vector2.Zero;
        public bool IsLoop { private get; set; } = true;
        public event Action? OnFinished;

        public Animation()
        {
            timer = new Timer
            {
                OneShot = false
            };
            timer.OnTimeout += Timer_OnTimeout;
        }

        private void Timer_OnTimeout()
        {
            indexFrame++;
            if (indexFrame == frameList.Count)
            {
                indexFrame = IsLoop ? 0 : frameList.Count - 1;
                if (!IsLoop)
                {
                    OnFinished?.Invoke();
                }
            }
        }

        public void Reset()
        {
            timer.Restart();
            indexFrame = 0;
        }

        public void SetInterval(float interval)
        {
            timer.WaitTime = interval;
        }

        public void AddFrame(Texture2D texture, int numHeight)
        {
            int width = texture.Width;
            int height = texture.Height;

            int widthFrame = width / numHeight;

            for (int i = 0; i < numHeight; i++)
            {
                Rectangle srcRect = new Rectangle()
                {
                    X = i * widthFrame,
                    Y = 0,
                    Width = widthFrame,
                    Height = height
                };

                frameList.Add(new(texture, srcRect));
            }
        }

        public void AddFrame(Atlas atlas)
        {
            for (int i = 0; i < atlas.Count; i++)
            {
                var texture = atlas.GetTexture(i);
                var srcRect = new Rectangle()
                {
                    X = 0,
                    Y = 0,
                    Width = texture?.Width ?? 0,
                    Height = texture?.Height ?? 0
                };

                frameList.Add(new(texture, srcRect));
            }
        }

        public void OnUpdate(float delta)
        {
            timer.Update(delta);
        }

        public void OnRender(Camera camera)
        {
            var frame = frameList[indexFrame];
            var posCamera = camera.Position;

            var dstRect = new Rectangle()
            {
                X = (int)(Position.X - frame.RectSrc.Width / 2),
                Y = (int)(Position.Y - frame.RectSrc.Height / 2),
                Width = frame.RectSrc.Width,
                Height = frame.RectSrc.Height
            };

            camera.RenderTexture(frame.Texture, frame.RectSrc, dstRect, Angle, Center);
        }
    }
}
