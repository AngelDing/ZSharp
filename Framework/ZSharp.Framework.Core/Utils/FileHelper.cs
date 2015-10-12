using System;
using System.IO;

namespace ZSharp.Framework.Utils
{
    /// <summary>
    /// 文件操作扩展方法类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 读取文件到字节数组
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static byte[] ReadFile(this string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            byte[] buffer = new byte[fs.Length];
            try
            {
                fs.Read(buffer, 0, buffer.Length);
                fs.Seek(0, SeekOrigin.Begin);
                return buffer;
            }
            catch
            {
                return buffer;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 将字节数组写到指定文件
        /// </summary>
        /// <param name="readByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool WriteFile(this byte[] readByte, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            try
            {
                fs.Write(readByte, 0, readByte.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return true;
        }
    }
}
