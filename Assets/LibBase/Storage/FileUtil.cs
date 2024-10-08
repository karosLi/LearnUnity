using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace LibBase
{
    public class FileUtil
    {
        public static string GetStreamingAssetsFilePath(string pathName)
        {
            try
            {
#if UNITY_EDITOR
                var filepath = Application.dataPath + "/StreamingAssets/" + pathName;
#elif UNITY_IOS
    var filepath = Application.dataPath + "/Raw/" + pathName;
#elif UNITY_ANDROID
    var filepath = "jar:file://" + Application.dataPath + "!/assets/" + pathName;
#endif
                return System.IO.Path.Combine(Application.streamingAssetsPath, pathName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return "";
        }

        public static Stream ReadFileStreamFromStreaming(string fileName)
        {
            try
            {
                Stream fs;
#if UNITY_EDITOR
                string filepath = Application.dataPath + "/StreamingAssets/" + fileName;
                fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

#elif UNITY_ANDROID
        string filepath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
        WWW www = new WWW(filepath);
        while (!www.isDone) {
        }
        fs = CovertBytesToSteam(www.bytes);
#else
        string filepath = Application.dataPath + "/Raw/" + fileName;
        Log.i("------->FileUtil ReadFileStream filepath=" + filepath);

        FileInfo info = new FileInfo(filepath);
        if (!info.Exists) return null;

        fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
#endif
                return fs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static byte[] ReadFileBytesFromSteaming(string fileName)
        {
            try
            {
                byte[] bytes;
#if UNITY_EDITOR
                string filepath = Application.dataPath + "/StreamingAssets/" + fileName;
                bytes = ReadStreamBytes(filepath);

#elif UNITY_ANDROID
        string filepath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
        WWW www = new WWW(filepath);
        while (!www.isDone) {
        }
        bytes = www.bytes;
#else
        string filepath = Application.dataPath + "/Raw/" + fileName;
        Log.i("------->FileUtil ReadFileStream filepath=" + filepath);

        FileInfo info = new FileInfo(filepath);
        if (!info.Exists) return null;

        bytes = ReadStreamBytes(filepath);

#endif
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static byte[] ReadStreamBytes(string pathName)
        {
            try
            {
                var fileStream = new FileInfo(pathName).OpenRead();
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                fileStream.Dispose();
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static bool IsExit(string pathName)
        {
            FileInfo fileInfo = new FileInfo(pathName);
            return fileInfo.Exists && fileInfo.Length > 0;
        }

        public static string CovertBytesToString(byte[] bytes)
        {
            try
            {
                if (bytes == null) return "";
                return System.Text.Encoding.Default.GetString(bytes);
            }
            catch (Exception e)
            {
                Debug.Log("------>CovertBytesToString " + e);
                return "";
            }
        }

        public static byte[] CovertStringToBytes(string content)
        {
            return System.Text.Encoding.Default.GetBytes(content);
        }

        public static Stream CovertBytesToSteam(byte[] data)
        {
            return new MemoryStream(data);
        }

        public static byte[] CovertSteamToBytes(Stream stream)
        {
            try
            {
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static string ReadFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (!file.Exists) return null;

                FileStream fileStream = file.OpenRead();

                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                fileStream.Dispose();
                string result = System.Text.Encoding.Default.GetString(bytes);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static byte[] ReadFilesBytes(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (!file.Exists) return null;

                FileStream fileStream = file.OpenRead();
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                fileStream.Dispose();
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static void DeleteFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (!file.Exists) return;

                file.Delete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public static void CheckFolder(string path)
        {
            try
            {
                string folder = Path.GetDirectoryName(path);
                //Debug.Log("------>FileUtil CheckFolder path="+path+" folder="+folder);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CreateFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CopyFile(string srcPath, string destPath)
        {
            SaveFile(destPath, ReadFilesBytes(srcPath), null);
        }


        public static bool SaveFile(string path, Stream stream, Action<float> progress)
        {
            try
            {
                CheckFolder(path);
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    long fileLength = 0;
                    long totalLength = stream.Length;
                    byte[] buffer = new byte[1024];
                    int length = stream.Read(buffer, 0, buffer.Length);
                    while (length > 0)
                    {
                        fs.Write(buffer, 0, length);
                        fileLength += length;
                        if (progress != null) progress((float)fileLength / totalLength);
                        length = stream.Read(buffer, 0, buffer.Length);
                    }
                    fs.Close();
                    stream.Close();
                    stream.Dispose();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool SaveFile(string path, byte[] data, Action<float> progress)
        {
            return SaveFile(path, new MemoryStream(data), progress);
        }
        //
        // private static IEnumerator Download(string url, string path, PCallback loadCallback)
        // {
        //     WWW www = new WWW(url);
        //     while (!www.isDone)
        //     {
        //         if (loadCallback != null) loadCallback.OnProgress(www.progress * 0.8f);
        //         yield return null;
        //     }
        //
        //     if (string.IsNullOrEmpty(www.error))
        //     {
        //         bool save = SaveFile(path, www.bytes, f =>
        //         {
        //             if (loadCallback != null) loadCallback.OnProgress(0.8f + f * 0.2f);
        //         });
        //         if (save)
        //         {
        //             if (loadCallback != null) loadCallback.OnSuccess();
        //         }
        //         else
        //         {
        //             if (loadCallback != null) loadCallback.OnFail();
        //         }
        //     }
        //     else
        //     {
        //         if (loadCallback != null) loadCallback.OnFail();
        //     }
        // }
        //
        // public static void DownloadFile(string url, string path, PCallback loadCallback)
        // {
        //     Loom.Coroutine(Download(url, path, loadCallback));
        // }
    }
}