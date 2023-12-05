using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using System;
using HarmonyLib;

namespace CustomTelevisionVideo
{
	public static class ConfigSettings
	{
		public static ConfigEntry<string> videoPath;

		public static void BindConfigSettings()
		{
			Plugin.Log("BindingConfigs");
			videoPath = ((BaseUnityPlugin)Plugin.instance).Config.Bind<string>("CustomTelevisionVideo", "CustomVideoPath", "xBytez-CustomTelevisionVideo/videos/", "Absolute or local video folder path. Use forward slashes in your path. Local paths are local to the BepInEx/plugins folder, and should not begin with a slash.");
		}
	}
    [BepInPlugin(GUID, NAME, VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private Harmony _harmony;
		public static Plugin instance;
		public static string filePath;
        private const string GUID = "xBytez.CustomTelevisionVideo";
        private const string NAME = "CustomTelevisionVideo";
        private const string VERSION = "1.1.4";
		public const bool DEV = false;
		public int seed = 420;
		public Random videoRandomizer;
		private void Awake()
		{
			instance = this;
			ConfigSettings.BindConfigSettings();
			filePath = ConfigSettings.videoPath.Value;
			if (filePath.Length <= 0 || Enumerable.Contains(filePath, ' '))
			{
				filePath = ((ConfigEntryBase)ConfigSettings.videoPath).DefaultValue.ToString();
			}
			filePath = filePath.Replace('/', Path.DirectorySeparatorChar);
			if (!Path.IsPathRooted(filePath))
			{
				filePath = Path.Combine(Paths.PluginPath, filePath.TrimStart(new char[1] { '/' }));
			}
			videoRandomizer = new Random(seed);

			Log("Using path from config: " + filePath);
			_harmony = new Harmony("CustomTelevisionVideo");
			_harmony.PatchAll();
			Logger.LogInfo((object)"CustomTelevisionVideo (xBytez fork) mod loaded");
		}
        public static void Log(string message)
		{
			instance.Logger.LogInfo((object)message);
		}

		public static string GetRandomMp4File()
		{
			var mp4Files = Directory.GetFiles(filePath, "*.mp4");
			if (mp4Files.Length == 0)
			{
				return null; // or handle the absence of files as needed
			}
			Array.Sort(mp4Files, (first, second) => first.CompareTo(second));
			
			int randomIndex = instance.videoRandomizer.Next(mp4Files.Length); // Get a random index
			return mp4Files[randomIndex]; // Return the file at the random index
		}
	}
}