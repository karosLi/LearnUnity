using UnityEngine;

namespace LibBase.Singleton {
    public class MainComponentManager {
        private static MainComponentManager instance;

        private static void CreateInstance() {
            if (instance == null) {
                instance = new MainComponentManager();
                GameObject go = GameObject.Find("Main");
                if (go == null) {
                    go = new GameObject("Main");
                    instance.main = go;
                    Object.DontDestroyOnLoad(go);
                }
            }
        }

        private GameObject main;

        private static MainComponentManager SharedInstance {
            get {
                if (instance == null) {
                    CreateInstance();
                }

                return instance;
            }
        }

        public static T AddMainComponent<T>() where T : Component {
            T t = SharedInstance.main.GetComponent<T>();
            if (t != null) {
                return t;
            }

            return SharedInstance.main.AddComponent<T>();
        }

        public static T GetMainComponent<T>() where T : Component
        {
            return SharedInstance.main.GetComponent<T>();
        }

        public static void RemoveMainComponent<T>() where T : Component
        {
            T t = SharedInstance.main.GetComponent<T>();
            if (t != null)
            {
                Object.Destroy(t);
            }
        }
    }
}