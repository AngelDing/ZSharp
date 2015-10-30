using ZSharp.Framework.Logging;

namespace Framework.ConsoleTest
{
    public class LoggerTest
    {
        public static void Test()
        {
            var logger = LogManager.GetLogger(typeof(LoggerTest));
            logger.Error("{\"ConferenceId\":\"af3dbf05-68f2-44f4-8a0b-014f6ac32349\",\"OrderId\":\"e9019c64-f70b-44ba-b7ed-014fa83dca99\",\"Id\":\"827b2e6d-580e-43c8-af9a-25759b7f6f1c\",\"Seats\":[{\"SeatType\":\"2d44579c-43a7-4f95-823c-014f3c5cbcce\",\"Quantity\":2},{\"SeatType\":\"78b2f864-7a04-4b7d-8ee8-014f3c5cbe6d\",\"Quantity\":0},{\"SeatType\":\"2d554d0e-bba2-4c97-8d8e-014f3c5cfe31\",\"Quantity\":0},{\"SeatType\":\"ae82e1aa-9870-4059-ac6b-014f3c5d381d\",\"Quantity\":0}]}");
            logger.Info("info");
            logger.Debug("debug");
            logger.Trace("trace");
            logger.Warn("warn");
            logger.Error("error");
            logger.Fatal("fatal");
        }
    }
}
