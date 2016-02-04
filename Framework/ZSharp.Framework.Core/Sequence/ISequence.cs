namespace ZSharp.Framework.Sequence
{
    /// <summary>
    /// 多实例和线程安全的序列化生成器:
    /// http://www.cnblogs.com/shanyou/p/5180594.html
    /// </summary>
    public interface ISequence
    {
        long StartAt { get;  }

        int Increment { get;  }

        long MaxValue { get;  }

        long MinValue { get;  }

        bool Cycle { get;  }

        long CurrentValue { get; set; }
    }
}