
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// �ṩ�ͻ�������ʶ���ص�����������
    /// </summary>
    public sealed class PoolListener<T> where T : PoolableBase, new()
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public PoolListener() { }

        /// <summary>
        /// �������������ȡ����ʵ��
        /// </summary>
        /// <returns></returns>
        public T Acquire()
        {
            if (ObjectDispatch.Available)
            {
                return ObjectDispatch.Acquire<T>();
            }
            else
            {
                return null;
            }
        }
    }
}
