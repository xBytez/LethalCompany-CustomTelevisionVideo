using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Video;

namespace CustomTelevisionVideo.Patches
{
	[HarmonyPatch(typeof(TVScript), "TurnTVOnOff")]
    internal class CustomTelevisionVideo_TurnTVOnOff
	{
		public static void Prefix(bool on, TVScript __instance)
		{
			__instance.video.clip = null;
			__instance.tvSFX.clip = null;
			__instance.video.url = "";
		}

		public static void Postfix(bool on, TVScript __instance)
		{
			__instance.tvOn = on;

			/* if ((int)__instance.video.source != 1 || __instance.video.url == "")
			{ */
			// }
			if (on)
			{
				Plugin.Log("Turning on TV");
				var randomMp4File = Plugin.GetRandomMp4File();
				if(randomMp4File == null) {
					Plugin.Log("No mp4 files found in video folder");
				} else {
					Plugin.Log("Picked video: " + randomMp4File);
					__instance.video.clip = null;
					__instance.tvSFX.clip = null;
					__instance.video.url = $"file://{randomMp4File}";
					__instance.video.source = (VideoSource)1;
					__instance.video.controlledAudioTrackCount = 1;
					__instance.video.audioOutputMode = (VideoAudioOutputMode)1;
					__instance.video.SetTargetAudioSource((ushort)0, __instance.tvSFX);
					__instance.video.Prepare();
					__instance.video.Stop();
					__instance.tvSFX.Stop();
				}
				SetTVScreenMaterial(__instance, b: true);
				__instance.video.Play();
				__instance.tvSFX.Play();
				__instance.tvSFX.PlayOneShot(__instance.switchTVOn);
				WalkieTalkie.TransmitOneShotAudio(__instance.tvSFX, __instance.switchTVOn, 1f);
			}
			else
			{
				Plugin.Log("Turning off TV");
				SetTVScreenMaterial(__instance, b: false);
				__instance.tvSFX.Stop();
				__instance.tvSFX.PlayOneShot(__instance.switchTVOff);
				__instance.video.Stop();
				WalkieTalkie.TransmitOneShotAudio(__instance.tvSFX, __instance.switchTVOff, 1f);
			}
			return;
		}
        public static void SetTVScreenMaterial(TVScript instance, bool b)
		{
			MethodInfo method = ((object)instance).GetType().GetMethod("SetTVScreenMaterial", BindingFlags.Instance | BindingFlags.NonPublic);
			method.Invoke(instance, new object[1] { b });
		}
    }
}