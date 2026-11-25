using System;
using HarmonyLib;

namespace tostilities.Patches.Car;

/// <summary>
/// Disable damage, report damage
/// </summary>
[HarmonyPatch(typeof(Model.Car), nameof(Model.Car.ApplyConditionDelta))]
public class ApplyConditionDelta
{
	// delta > 0 means repairing, < 0 means damaging
	private static bool Prefix(float delta, Model.Car __instance)
	{
		if (delta >= 0.0) // repair
		{
			return Constants.EXECUTE_ORIGINAL;
		}

		if (ModEntryPoint.MySettings!.DisableDamage)
		{
			return Constants.SKIP_ORIGINAL;
		}

		if (ModEntryPoint.MySettings.ReportDamage)
		{
			DamageRegistrar.Instance.AddDamageEntry(__instance.id, __instance.DisplayName, DateTime.Now, Math.Abs(delta));
		}
		
		return Constants.EXECUTE_ORIGINAL;
	}
}

