using UnityEngine;
namespace Assets.Src
{
    public interface IInputListener
    {
        void OnInputTriggered(string name);
    }
}