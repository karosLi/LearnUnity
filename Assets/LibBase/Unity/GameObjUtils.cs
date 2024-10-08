using UnityEngine;

namespace LibBase.Unity {
    public class GameObjUtils {
        public static void SetActive(GameObject go, bool state) {
            if (go == null) return;

            if (go.activeSelf != state) {
                go.SetActive(state);
            }
        }

        public static T EnsureComponent<T>(GameObject target) where T : Component {
            T comp = target.GetComponent<T>();
            if (comp == null) {
                return target.AddComponent<T>();
            }

            return comp;
        }
    }
}