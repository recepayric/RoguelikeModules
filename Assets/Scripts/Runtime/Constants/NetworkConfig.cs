using UnityEngine;

namespace Runtime.Constants
{
    public class NetworkConfig
    {
        public static GameObject GameRoot;
        public static bool IsMultiplayer;

        public static int roomIDLength = 6;
        public static string activeRoomName = "";
    }
}