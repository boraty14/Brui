using UnityEngine;

namespace DefaultNamespace
{
    public class DummyA : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}