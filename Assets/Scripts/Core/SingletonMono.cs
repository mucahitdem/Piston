using UnityEngine;

namespace Core
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;
        
        public static T Instance
        {
            get
            {
                if (!Application.isPlaying) 
                    s_instance = FindObjectOfType<T>();
                return s_instance;
            }
            set => s_instance = value;
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
            }

            OnAwake();
        }


        protected abstract void OnAwake();
    }
}