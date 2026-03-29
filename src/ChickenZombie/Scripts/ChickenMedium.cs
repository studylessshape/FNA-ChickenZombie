using FNAStart.Engine;

namespace FNAStart.Scripts
{
    public class ChickenMedium : Chicken
    {
        public static Atlas AtlasChickenMedium;

        public ChickenMedium()
        {
            animationRun.AddFrame(AtlasChickenMedium);

            speedRun = 50.0f;
        }
    }
}
