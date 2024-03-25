using System;
using Game;
using HarmonyLib;
using UI.Console;

namespace tostilities.Patches;

/// <summary>
/// show real-life time instead of in-game time
/// </summary>
[HarmonyPatch(typeof(ExpandedConsole))]
[HarmonyPatch(nameof(ExpandedConsole.Add))]
public class ExpandedConsole_Add_Patch
{
	private static void Prefix(ref UI.Console.Console.Entry entry)
	{
		if (!Main.MySettings.ConsoleStampsRealTime)
		{
			return;
		}

		entry.Timestamp = Console_Stuff.RealNow();
	}
}

[HarmonyPatch(typeof(CollapsedConsole))]
[HarmonyPatch(nameof(CollapsedConsole.Add))]
public class CollapsedConsole_Add_Patch
{
	private static void Prefix(ref UI.Console.Console.Entry entry)
	{
		if (!Main.MySettings.ConsoleStampsRealTime)
		{
			return;
		}

		entry.Timestamp = Console_Stuff.RealNow();
	}
}

public static class Console_Stuff
{
	public static GameDateTime RealNow()
	{
		var now = DateTime.Now;
		return new GameDateTime(0, now.Hour + now.Minute / 60f);
	}
}