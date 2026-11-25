using HarmonyLib;
using UI.Console;

namespace tostilities.Patches.Console;

/// <summary>
/// show real-life time instead of in-game time
/// </summary>
[HarmonyPatch(typeof(CollapsedConsole), nameof(CollapsedConsole.Add))]
public class CollapsedConsole_Add
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