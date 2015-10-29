using System;
using System.Threading.Tasks;
using ZSharp.Framework.Logging;
using System.Text;

namespace ZSharp.Framework.Serializations
{
    public abstract class BaseSerializer : ISerializer 
    {
        private readonly ILogger logger;

        public BaseSerializer()
        {
            logger = LogManager.GetLogger(this.GetType());
            this.IsDefaultString = false;
        }

        /// <summary>
        /// 序列化結果是否默認為string類型
        /// </summary>
        public bool IsDefaultString { get; set; }
        
        public SerializationFormat Format
        {
            get
            {
                return GetSerializationFormat();
            }
        }

        internal abstract SerializationFormat GetSerializationFormat();
        internal abstract object DoSerialize<T>(object item);
        internal abstract T DoDeserialize<T>(object serializedObject);
        internal abstract object DoDeserialize(object serializedObject, Type type);

        internal byte[] GetEncodingBytes(object serializedObject)
        {
            byte[] buffer;
            if (serializedObject.GetType() == typeof(string))
            {
                buffer = GetEncodingBytes(serializedObject.ToString());
            }
            else
            {
                buffer = serializedObject as byte[];
            }
            return buffer;
        }

        internal byte[] GetEncodingBytes(string str)
        {
            if (this.IsDefaultString)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            else
            {
                return Convert.FromBase64String(str);
            }
        }

        internal string GetEncodingString(byte[] bytes)
        {
            if (this.IsDefaultString)
            {
                return Encoding.UTF8.GetString(bytes);
            }
            else
            {
                return Convert.ToBase64String(bytes);
            }
        }

        internal string GetEncodingString(object serializedObject)
        {
            string text;
            if (serializedObject.GetType() == typeof(string))
            {
                text = serializedObject.ToString();
            }
            else
            {
                text = GetEncodingString(serializedObject as byte[]);
            }
            return text;
        }

        #region Implement Interfaces

        public T Serialize<T>(object item)
        {           
            var typeName = typeof(T);
            if (typeName != typeof(string) && typeName != typeof(byte[]))
            {
                throw new ArgumentException("僅支持返回string或者byte[]類型的序列化結果！");
            }

            T result = default(T);
            try
            {
                result = (T)DoSerialize<T>(item);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public Task<T> SerializeAsync<T>(object item)
        {
            return Task.FromResult(this.Serialize<T>(item));
        }

        public T Deserialize<T>(object serializedObject)
        {
            CheckSerializedObjectType(serializedObject);
            T result = default(T);
            try
            {
                result = DoDeserialize<T>(serializedObject);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }      

        public Task<T> DeserializeAsync<T>(object serializedObject)
        {
            return Task.FromResult<T>(Deserialize<T>(serializedObject));
        }

        public object Deserialize(object serializedObject, Type type)
        {
            CheckSerializedObjectType(serializedObject);
            object result = null;
            try
            {
                result = DoDeserialize(serializedObject, type);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        public Task<object> DeserializeAsync(object serializedObject, Type type)
        {
            return Task.FromResult(Deserialize(serializedObject, type));
        }

        #endregion
                
        private void CheckSerializedObjectType(object serializedObject)
        {
            var type = serializedObject.GetType();
            if (type != typeof(string) && type != typeof(byte[]))
            {
                throw new ArgumentException("serializedObject類型僅支持string或者byte[]類型！");
            }
        }

        private void HandleException(Exception ex)
        {
            var msg = this.GetType().Name + " Deserialize Error.";
            logger.Error(msg, ex);
            throw ex;
        }       
    }
}
