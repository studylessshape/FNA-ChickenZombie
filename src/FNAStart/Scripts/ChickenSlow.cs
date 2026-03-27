using FNAStart.Engine;
using Microsoft.Xna.Framework.Audio;

namespace FNAStart.Scripts
{
    public class ChickenSlow : Chicken
    {
        public static Atlas AtlasChickenSlow;

        public ChickenSlow()
        {
            animationRun.AddFrame(AtlasChickenSlow);

            speedRun = 30.0f;
        }
    }
}
