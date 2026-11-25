using HarmonyLib;
using Network;

namespace tostilities.Patches.Car;

/// <summary>
/// Disable derailments, report derailments
/// </summary>
[HarmonyPatch(typeof(Model.Car), nameof(Model.Car.ApplyDerailmentDelta))]
public class ApplyDerailmentDelta
{
	// delta > 0 means derailment, < 0 means rerailment
	private static bool Prefix(float delta, Model.Car __instance)
	{
		if (delta <= 0.0) // re-rail
		{
			return Constants.EXECUTE_ORIGINAL;
		}

		if (ModEntryPoint.MySettings!.DisableDerailing)
		{
			return Constants.SKIP_ORIGINAL;
		}

		if (ModEntryPoint.MySettings.ReportDamage && !__instance.IsDerailed)
		{
			Multiplayer.Broadcast($"{__instance.DisplayName} derailed!");
		}

		return Constants.EXECUTE_ORIGINAL;
	}
}