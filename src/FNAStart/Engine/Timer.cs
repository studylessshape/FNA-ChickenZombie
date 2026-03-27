using System;

namespace FNAStart.Engine
{
    public class Timer
    {
        private float passTime;
        private bool paused;
        private bool shotted;
        
        public float WaitTime { private get; set; }
        public bool OneShot { private get; set; }

        public event Action? OnTimeout;

        public void Restart()
        {
            passTime = 0;
            shotted = false;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }

        public void Update(float delta)
        {
            if (paused) return;

            passTime += delta;
            if (passTime >= WaitTime)
            {
                bool canShot = (!OneShot || (OneShot && !shotted));
                shotted = true;
                if (canShot)
                {
                    OnTimeout?.Invoke();
                }
                passTime -= WaitTime;
            }
        }
    }
}
