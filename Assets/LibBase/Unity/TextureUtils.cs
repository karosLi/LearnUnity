// using LibBase.CDebug;
using UnityEngine;

namespace LibBase.Unity {
    public class TextureUtil {
        private static Texture emptyTexture;

        public static Texture getEmptyTexture() {
            if (emptyTexture != null) {
                return emptyTexture;
            }

            emptyTexture = new Texture2D(0, 0);
            return emptyTexture;
        }

        public static Texture getTexture(byte[] data, int width, int height) {
            //Log.i("TextureUtil", string.Format("getTexture data:{0}, width:{1}, height:{2}", (data == null ? 0 : data.Length), width, height));
            var texture = new Texture2D(width, height);
            texture.LoadImage(data);
            texture.Apply();
            return texture;
        }


        public static Texture getTexture(byte[] data) {
            return getTexture(data, 2, 2);
        }
    }
}