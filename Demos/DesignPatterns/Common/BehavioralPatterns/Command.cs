using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.BehavioralPatterns
{
    /// <summary>
    /// 抽象命令对象
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 提供给调用者的统一操作方法
        /// </summary>
        void Execute();

        Task ExecuteAsync();
    }

    public class BaseCommand<T> : ICommand where T : class, IInput
    {
        private Action<T> action;
        protected T ParamObject { get; set; }

        public BaseCommand(Action<T> action)
             : this(action, null)
        {
        }

        public BaseCommand(Action<T> action, T obj)
        {
            this.action = action;
            var result = CheckParamObject();
            if (result == false)
            {
                throw new ArgumentException("result.Message");
            }
            this.ParamObject = obj;
        }

        protected virtual bool CheckParamObject()
        {
            return true;
        }

        public virtual void Execute()
        {
            action.Invoke(ParamObject);
        }

        public Task ExecuteAsync()
        {
            return Task.Factory.StartNew(Execute);
        }
    }

    public class Receiver
    {
        public void Action(Input obj)
        {
            Console.WriteLine(obj.Name);
        }
    }

    /// <summary>
    /// 要执行的命令，如果带参数，则统一采用一个参数对象，此对象需要继承此接口
    /// </summary>
    public class Input : IInput
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// 保存Command的队列
    /// </summary>
    public class CommandQueue : Queue<ICommand>
    {
    }

    /// <summary>
    /// 调用者
    /// </summary>
    public class Invoker
    {
        /// <summary>
        /// 管理相关命令对象
        /// </summary>
        private CommandQueue queue;

        public Invoker()
        {
        }

        public Invoker(ICommand command)
        {
            AddCommand(command);
        }

        public Invoker(CommandQueue queue)
        {
            this.queue = queue;
        }

        public void AddCommand(ICommand command)
        {
            StoreCommand(command);
            queue.Enqueue(command);
        }


        /// <summary>
        /// 按照队列方式执行排队的命令。相对而言，这时候Invoker
        /// 具有执行的主动性，此处可以嵌入很多其他控制逻辑
        /// </summary>
        public void Run()
        {
            while (queue.Count > 0)
            {
                var command = queue.Dequeue();
                command.Execute();
                //command.ExecuteAsync();
            }
        }

        private void StoreCommand(ICommand command)
        {
            //Store to db
        }
    }


    public class ConcreteCommand : BaseCommand<Input>
    {
        public ConcreteCommand(Action<Input> action, Input input)
            : base(action, input)
        {
        }

        protected override bool CheckParamObject()
        {
            //Do somthing
            return true;
        }

        public override void Execute()
        {
            PreExecute();
            base.Execute();
            Ececuted();
        }

        private void Ececuted()
        {
            throw new NotImplementedException();
        }

        private void PreExecute()
        {
            throw new NotImplementedException();
        }
    }

    public class CommandClient
    {
        public void Test()
        {
            Receiver receiver = new Receiver();
            var input = new Input { Name = "Hello World" };
            var cmd = new ConcreteCommand(receiver.Action, input);
            Invoker invoker = new Invoker(cmd);
            invoker.Run();
        }
    }
}
