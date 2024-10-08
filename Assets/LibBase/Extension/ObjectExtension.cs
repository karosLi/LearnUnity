using System;

namespace LibBase.Extension {
    public static class ObjectExtension {
        public static void ThrowIfNull(this object o, string message) {
            if (o == null) throw new NullReferenceException(message);
        }
    }
}