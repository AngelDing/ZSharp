
namespace BuilderPattern.Equipment.Implementation3
{
    class BuilderClient3
    {
        public static void Test()
        {
            EQPBuilder builder = new EQPBuilder();
            Equipment eqp = LCDFactory.CreateEQP(builder, EquipmentType.InputEQP);
            eqp.Run();

            builder = new EQPBuilder();
            eqp = LCDFactory.CreateEQP(builder, EquipmentType.IOPutEQP);
            eqp.Run();
        }
    }
}
