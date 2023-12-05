using HarmonyLib;

namespace CustomTelevisionVideo.Patches
{
    [HarmonyPatch(typeof(TVScript), "TVFinishedClip")]
    internal class CustomTelevisionVideo_TVFinishedClip
	{
        public static bool Prefix()
        {
            return false;
        }
    }
}