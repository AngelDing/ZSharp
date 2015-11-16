
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// �ػ���������
    /// <remarks>
    /// ʵ��Ӧ�������ڶ���Ĵ����������ܷǳ����ӣ���˰��շֹ��ѡ����д��������������������
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectBuilder<T> where T : class, IPoolable, new()
    {
        /// <summary>
        /// ����ָ������ʵ��
        /// </summary>
        /// <returns></returns>
        public T BuildUp()
        {
            return new T();
        }
    }
}