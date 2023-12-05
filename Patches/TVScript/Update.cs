using HarmonyLib;

namespace CustomTelevisionVideo.Patches
{
    [HarmonyPatch(typeof(TVScript), "Update")]
    internal class CustomTelevisionVideo_Update
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}