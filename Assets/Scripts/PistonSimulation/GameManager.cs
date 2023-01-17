using Core;
using UnityEngine;

namespace PistonSimulation
{
    public class GameManager : SingletonMono<GameManager>
    {
        public Camera Cam => cam;
        
        [SerializeField]
        private Camera cam;

        protected override void OnAwake()
        {
            
        }
    }
}