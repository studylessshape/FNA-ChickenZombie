using FNAStart.Engine;
using Microsoft.Xna.Framework.Audio;

namespace FNAStart.Scripts
{
    public class ChickenFast : Chicken
    {
        public static Atlas AtlasChickenFast;

        public ChickenFast(Atlas atlasExplosion, SoundEffect soundExplosion)
            : base(atlasExplosion, soundExplosion, AtlasChickenFast)
        {
            speedRun = 80.0f;
        }
    }
}
