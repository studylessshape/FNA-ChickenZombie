using FNAStart.Engine;
using Microsoft.Xna.Framework.Audio;

namespace FNAStart.Scripts
{
    public class ChickenMedium : Chicken
    {
        public static Atlas AtlasChickenMedium;

        public ChickenMedium(Atlas atlasExplosion, SoundEffect soundExplosion)
            : base(atlasExplosion, soundExplosion, AtlasChickenMedium)
        {
            speedRun = 50.0f;
        }
    }
}
