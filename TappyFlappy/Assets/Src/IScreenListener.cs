using UnityEngine;

namespace Assets.Src
{
    public interface IScreenListener
    {
        void OnScreenResolutionChanged(int width, int height);
    }
}
