using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class Login : MonoBehaviour
    {
        private void Start()
        {
            TalkSystem.GetInstance().LoadTalkData();
        }
    }
}
