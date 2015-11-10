
namespace BuilderPattern.Equipment.Implementation4
{
    class BuilderClient4
    {
        public static void Test()
        {
            var builder = new EquipmentBuilder();
            Equipment eqp = LCDDirector.CreateEQP(builder, EquipmentType.InputEQP);
            eqp.Run();

            builder = new EquipmentBuilder();
            eqp = LCDDirector.CreateEQP(builder, EquipmentType.IOPutEQP);
            eqp.Run();
        }
    }
}
