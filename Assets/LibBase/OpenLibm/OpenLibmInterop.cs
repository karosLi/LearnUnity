using System;
using System.Runtime.InteropServices;
using System.Security; // Needed for SuppressUnmanagedCodeSecurity (optional performance gain)

namespace LibBase.OpenLibm
{
    [SuppressUnmanagedCodeSecurity] // 可选，可以提高 P/Invoke 调用速度，但需确保原生代码是安全的
    public static class OpenLibmInterop
    {
        // 根据你的库文件名进行调整
#if UNITY_IOS && !UNITY_EDITOR
    private const string LibName = "__Internal"; // 对于 iOS 静态库，使用 __Internal
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        private const string LibName = "openlibm"; // 指向 openlibm.dll (省略 .dll)
#else
    private const string LibName = "openlibm"; // 其他平台可能需要不同的名称或处理
#endif

        // --- P/Invoke Declarations ---
        // 注意：函数名需要与 C 库中导出的名称完全一致
        // 如果 C 库编译时有名称修饰（mangling），可能需要使用 EntryPoint

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sin")] // 假设 C 库导出名为 sin
        public static extern double sin(double x);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "cos")]
        public static extern double cos(double x);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "tan")]
        public static extern double tan(double x);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "atan2")]
        public static extern double atan2(double y, double x);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqrt")]
        public static extern double sqrt(double x);

        // ... 添加其他你需要的 OpenLibm 函数声明 ...

        // --- Convenience Wrappers (可选，但推荐) ---
        // 可以提供与 System.Math 类似的静态方法，内部调用 P/Invoke
        public static double Sin(double x) => sin(x);
        public static double Cos(double x) => cos(x);
        public static double Tan(double x) => tan(x);
        public static double Atan2(double y, double x) => atan2(y, x);
        public static double Sqrt(double x) => sqrt(x);
    }
}