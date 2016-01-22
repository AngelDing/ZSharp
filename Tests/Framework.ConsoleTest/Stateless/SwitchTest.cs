using System;
using ZSharp.Framework.Stateless;

namespace Framework.ConsoleTest
{
    public class SwitchTest
    {
        public static void MainTest()
        {
            try
            {
                string on = "On", off = "Off";
                var space = ' ';

                var onOffSwitch = new StateMachine<string, char>(off);

                onOffSwitch.Configure(off).Permit(space, on);
                onOffSwitch.Configure(on).Permit(space, off);

                Console.WriteLine("Press <space> to toggle the switch. Any other key will raise an error.");

                while (true)
                {
                    Console.WriteLine("Switch is in state: " + onOffSwitch.State);
                    var pressed = Console.ReadKey(true).KeyChar;
                    onOffSwitch.Fire(pressed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
