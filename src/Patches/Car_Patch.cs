using HarmonyLib;
using Model;

namespace rr_utilities.Patches;

/// <summary>
/// Disable derailments
/// </summary>
[HarmonyPatch(typeof(Car))]
[HarmonyPatch(nameof(Car.ApplyDerailmentDelta))]
public class Car_ApplyDerailmentDelta_Patch
{
	// delta > 0 means derailment, < 0 means rerailment
	private static bool Prefix(ref float delta)
	{
		if (!Main.MySettings.DisableDerailing || delta <= 0.0)
		{
			return true; //execute original function
		}
		
		return false; //skip original function
	}
}

/// <summary>
/// Disable damage
/// </summary>
[HarmonyPatch(typeof(Car))]
[HarmonyPatch(nameof(Car.ApplyConditionDelta))]
public class Car_ApplyConditionDelta_Patch
{
	// delta > 0 means repairing, < 0 means damaging
	private static bool Prefix(ref float delta)
	{
		if (!Main.MySettings.DisableDamage || delta >= 0.0)
		{
			return true; //execute original function
		}
		
		return false; //skip original function
	}
}