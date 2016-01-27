using Demos.CQRS.Common;
using ZSharp.Framework.Domain;
using ZSharp.Framework.Infrastructure;

namespace Demos.CQRS.Publisher
{
    class Program
    {
        /// <summary>
        /// 採用Command同直接調用Service有何區別？
        /// 如果不涉及共享資源的訪問，僅僅涉及CRUD等操作，則可以直接訪問Service；
        /// 如果涉及共享資源，或者長時間操作，則可以採用異步的Command，從而提高用戶友好性。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var commandBus = ServiceLocator.GetInstance<ICommandBus>();
            var customerService = ServiceLocator.GetInstance<ICustomerService>();

            var creatCommand = new CreateCustomerCommand
            {
                FirstName = "Jacky",
                LastName = "Zhou",
                Email = "a@b.com"
            };
            //異步
            commandBus.Send(creatCommand, "Customer");

            var cInfo = creatCommand.Map<CustomerInfo>();
            //同步 
            customerService.CreateCustomer(cInfo);
        }
    }
}
