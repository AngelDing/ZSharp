
namespace BuilderPattern.Equipment
{
    public static class BuilderClient1
    {
        public static void Test()
        {
            Machine machine = new Machine("InputOutputMachine");
            Port inputPort = new InputPort();
            Port outputPort = new OutputPort();
            Equipment eqp = new Equipment();
            eqp.Machine = machine;
            eqp.Name = machine.Name;
            eqp.AddPort(inputPort);
            eqp.AddPort(outputPort);
            eqp.Run();
        }
    }
}
