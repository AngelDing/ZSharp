
namespace BuilderPattern.Equipment.Implementation4
{
    class BuilderClient4
    {
        public static void Test()
        {
            var builder = new EquipmentBuilder(EquipmentType.InputEQP);
            Equipment eqp = LCDDirector.CreateEQP(builder);
            eqp.Run();

            builder = new EquipmentBuilder(EquipmentType.IOPutEQP);
            eqp = LCDDirector.CreateEQP(builder);
            eqp.Run();
        }
    }
}
