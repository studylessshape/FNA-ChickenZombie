using FNAStart.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FNAStart.Scripts
{
    public abstract class Chicken
    {
        private readonly SoundEffect soundExplosion;

        protected float speedRun = 10.0f;
        protected Animation animationRun = new();

        private Vector2 position;
        private Animation animationExplosion = new();
        private Animation animationCurrent = null!;

        private bool isValid = true;

        public bool IsAlive { get; private set; } = true;
        public Vector2 Position => position;

        protected Chicken(Atlas atlasExplosion, SoundEffect soundExplosion, Atlas atlasRun)
        {
            this.soundExplosion = soundExplosion;

            animationRun.AddFrame(atlasRun);
            animationRun.IsLoop = true;
            animationRun.SetInterval(0.1f);

            animationExplosion.IsLoop = false;
            animationExplosion.SetInterval(0.08f);
            animationExplosion.AddFrame(atlasExplosion);
            animationExplosion.OnFinished += AnimationExplosion_OnFinished;

            position.X = 40f + Random.Shared.Next(1200);
            position.Y = -50f;
        }

        private void AnimationExplosion_OnFinished()
        {
            isValid = false;
        }

        public void Update(float delta)
        {
            if (IsAlive)
            {
                position.Y += speedRun * delta;
            }

            animationCurrent = IsAlive ? animationRun : animationExplosion;
            animationRun.Position = position;
            animationCurrent.Update(delta);
        }

        public void Hurt()
        {
            IsAlive = false;
            soundExplosion.Play();
        }

        public void MakeInvalid()
        {
            isValid = false;
        }

        public bool CanRemove()
        {
            return !isValid;
        }
    }
}
