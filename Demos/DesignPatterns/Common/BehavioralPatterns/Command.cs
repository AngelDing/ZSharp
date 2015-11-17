using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public interface IInput
    {
    }

    public class BaseCommand<T> : ICommand where T : IInput
    {
        private Action<T> action;
        private T obj;

        public BaseCommand(Action<T> action, T obj)
        {
            this.action = action;
            this.obj = obj;
        }

        public void Execute()
        {
            action.Invoke(obj);
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

    public class Input : IInput
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// 调用者
    /// </summary>
    public class Invoker
    {
        /// <summary>
        /// 管理相关命令对象
        /// </summary>
        private IList<ICommand> commands = new List<ICommand>();

        public void AddCommand(ICommand command)
        {
            commands.Add(command);
            StoreCommand(command);
        }

        /// <summary>
        /// 经过调用者组织后，供客户程序操作命令对象的方法
        /// </summary>
        public void Run()
        {
            foreach (ICommand command in commands)
            {
                command.Execute();
            }
        }

        private void StoreCommand(ICommand command)
        {
            //Store to db
        }
    }

    public class CommandClient
    {
        public void Test()
        {
            Receiver receiver = new Receiver();
            var input = new Input { Name = "Hello World" };
            var cmd = new BaseCommand<Input>( receiver.Action, input);
            Invoker invoker = new Invoker();
            invoker.AddCommand(cmd);
            invoker.Run();
        }
    }
}
