using UnityModManagerNet;

namespace XXLModCV
{
    public class Logger
    {
        public static void Log(string message)
        {
            UnityModManager.Logger.Log(message);
        }
    }
}
