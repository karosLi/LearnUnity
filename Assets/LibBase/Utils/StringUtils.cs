using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace LibBase.Utils {
    public class StringUtils {
        private static StringBuilder ms_temp = new StringBuilder();

        public static string CovertToUTF8(string content) {
            if (string.IsNullOrEmpty(content)) return content;
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(content));
        }

        public static string CovertToUTF8(byte[] data) {
            if (data == null) return "";
            return Encoding.UTF8.GetString(data);
        }

        public static void CopyToClipboard(string content) {
            GUIUtility.systemCopyBuffer = content;
        }

        public static bool Equals(string str1, string str2) {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2)) {
                return string.IsNullOrEmpty(str1) == string.IsNullOrEmpty(str2);
            }

            return str1.Equals(str2);
        }

        public static string ToString(Array target) {
            ms_temp.Length = 0;
            ms_temp.Append("[");
            if (target.Length > 0) {
                ms_temp.Append(target.GetValue(0));
            }

            for (int i = 1; i < target.Length; i++) {
                ms_temp.Append(",");
                ms_temp.Append(target.GetValue(i));
            }

            ms_temp.Append("]");
            return ms_temp.ToString();
        }

        public static string ToString(IList target) {
            ms_temp.Length = 0;
            ms_temp.Append("[");
            if (target.Count > 0) {
                ms_temp.Append(target[0]);
            }

            for (int i = 1; i < target.Count; i++) {
                ms_temp.Append(",");
                ms_temp.Append(target[i]);
            }

            ms_temp.Append("]");
            return ms_temp.ToString();
        }

        private const string BSP = " ";
        private const string NBSP = "\u00A0"; 
        public static string NonBreakingSpaceString(string txt)
        {
            return txt.Replace(BSP, NBSP);
        }
    }
}