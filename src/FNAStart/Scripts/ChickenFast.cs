using FNAStart.Engine;

namespace FNAStart.Scripts
{
    public class ChickenFast : Chicken
    {
        public static Atlas AtlasChickenFast;

        public ChickenFast() : base()
        {
            animationRun.AddFrame(AtlasChickenFast);

            speedRun = 80.0f;
        }
    }
}
