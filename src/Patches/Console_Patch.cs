using System;
using HarmonyLib;

namespace rr_utilities.Patches;

[HarmonyPatch(typeof(UI.Console.Console))]
[HarmonyPatch(nameof(UI.Console.Console.AddLine))]
public class Console_AddLine_Patch
{
	private static void Prefix(ref string text)
	{
		if (!Main.MySettings.ConsoleTimeStamps)
		{
			return;
		}

		text = $"{DateTime.Now.ToString("HH:mm")} | " + text;
		//todo PlayersManager.NotifyOfNewPlayers already has a timestamp
	}
}