using LitJson;

namespace LibBase.Utils
{
    public static class JsonUtils
    {
        public static string GetString(this JsonData jsonData, string key, string defaultValue = "")
        {
            return jsonData.ContainsKey(key) ? (string)jsonData[key] : defaultValue;
        }

        public static int GetInt(this JsonData jsonData, string key, int defaultValue = 0)
        {
            return jsonData.ContainsKey(key) ? (int)jsonData[key] : defaultValue;
        }

        public static long GetLong(this JsonData jsonData, string key, long defaultValue = 0)
        {
            return jsonData.ContainsKey(key) ? (long)jsonData[key] : defaultValue;
        }

        public static bool GetBool(this JsonData jsonData, string key, bool defaultValue = false)
        {
            return jsonData.ContainsKey(key) ? (bool)jsonData[key] : defaultValue;
        }
    }
}