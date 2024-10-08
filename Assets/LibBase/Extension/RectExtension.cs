using UnityEngine;

namespace LibBase.Extension {
    public static class RectExtension {

        //不支持反向的
        public static bool Contains(this Rect src, Rect dst) {
            float sx0 = src.x;
            float sy0 = src.y;
            float sx1 = sx0 + src.width;
            float sy1 = sy0 + src.height;

            float dx0 = dst.x;
            float dy0 = dst.y;
            float dx1 = dx0 + dst.width;
            float dy1 = dy0 + dst.height;

            return sx0 <= dx0 && sx1 >= dx1 && sy0 <= dy0 && sy1 >= dy1;
        }
    }
}