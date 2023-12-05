using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace CustomTelevisionVideo.Patches
{
    internal static class RandomizerHelper
    {
        public static void ResetRandomizer(string reason, StartOfRound __instance) {
            int seed = Plugin.instance.seed;
            if (__instance.randomMapSeed != 0)
            {
                seed = __instance.randomMapSeed;
                Plugin.instance.seed = seed;
            } else {
                seed = 420;
                Plugin.instance.seed = seed;
            }
            Plugin.Log("Randomizer reset because of " + reason + ". Seed: " + seed.ToString() + ".");
            Plugin.instance.videoRandomizer = new Random(seed);
        }
    }

    [HarmonyPatch(typeof(StartOfRound), "Start")]
    internal class StartOfLobbySeedReset
    {
        static void Postfix(StartOfRound __instance)
        {   
            RandomizerHelper.ResetRandomizer("new lobby", __instance);
        }
    }

    [HarmonyPatch(typeof(StartOfRound), "StartGame")]
    internal class StartOfRoundSeedReset
    {
        static void Postfix(StartOfRound __instance)
        {   
            RandomizerHelper.ResetRandomizer("new round", __instance);
        }
    }
}