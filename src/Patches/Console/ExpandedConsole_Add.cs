using HarmonyLib;
using UI.Console;

namespace tostilities.Patches.Console;

/// <summary>
/// show real-life time instead of in-game time
/// </summary>
[HarmonyPatch(typeof(ExpandedConsole), nameof(ExpandedConsole.Add))]
public class ExpandedConsole_Add
{
	private static void Prefix(ref UI.Console.Console.Entry entry)
	{
		if (!ModEntryPoint.MySettings!.ConsoleStampsRealTime)
		{
			return;
		}

		entry.Timestamp = ConsoleHelper.RealNow();
	}
}