
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// ����ʱ�ػ��������������Ϣ
    /// </summary>
    public interface IPoolableConfiguration
    {
        /// <summary>
        /// ���ĳػ�����
        /// </summary>
        int Max { get;}

        /// <summary>
        /// �����һ�Ρ�����/�ͷš��ĳ�ʱʱ��
        /// </summary>
        int Timeout { get;}
    }
}
