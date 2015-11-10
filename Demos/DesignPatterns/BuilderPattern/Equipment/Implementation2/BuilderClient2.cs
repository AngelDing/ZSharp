
namespace BuilderPattern.Equipment
{
    class BuilderClient2
    {
        public static void Test()
        {
            EQPBuilder builder = new InputEQPBuilder();
            Equipment eqp = LCDFactory.CreateEQP(builder, "InputMachine");
            eqp.Run();

            builder = new IOPutEQPBuilder();
            eqp = LCDFactory.CreateEQP(builder, "InputOutputMachine");
            eqp.Run();
        }
    }
}
