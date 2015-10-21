using System;
using System.IO;
using System.Threading.Tasks;

namespace ZSharp.Framework.Serializations
{
    public interface ISerializer
    {
        SerializationFormat Format { get; }

        /// <summary>
        /// 按指定類型，返回序列化結果
        /// </summary>
        /// <typeparam name="T">T的類型限定為：string或者byte[]</typeparam>
        /// <param name="item">需要序列化的對象</param>
        /// <returns>序列化結果</returns>
        T Serialize<T>(object item);

        Task<T> SerializeAsync<T>(object item);

        /// <summary>
        /// 將給定的經過序列化的對象反序列化成指定類型，serializedObject僅支持string或者byte[]類型
        /// </summary>
        /// <typeparam name="T">返回結果指定類型</typeparam>
        /// <param name="serializedObject">經過序列化的數據</param>
        /// <returns>指定類型對象</returns>
        T Deserialize<T>(object serializedObject);

        Task<T> DeserializeAsync<T>(object serializedObject);

        /// <summary>
        /// 將給定的經過序列化的對象反序列化成指定類型，serializedObject僅支持string或者byte[]類型
        /// </summary>
        /// <param name="serializedObject">經過序列化的數據</param>
        /// <param name="type">需要返回的數據類型</param>
        /// <returns>object對象</returns>
        object Deserialize(object serializedObject, Type type);

        Task<object> DeserializeAsync(object serializedObject, Type type);
    }
}
