using FNAStart.Engine;
using Microsoft.Xna.Framework.Audio;

namespace FNAStart.Scripts
{
    public class ChickenSlow : Chicken
    {
        public static Atlas AtlasChickenSlow;

        public ChickenSlow(Atlas atlasExplosion, SoundEffect soundExplosion)
            : base(atlasExplosion, soundExplosion, AtlasChickenSlow)
        {
            speedRun = 50.0f;
        }
    }
}
