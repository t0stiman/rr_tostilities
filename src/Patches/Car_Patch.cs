using System;
using HarmonyLib;
using Model;
using Network;

namespace tostilities.Patches;

/// <summary>
/// Disable derailments, report derailments
/// </summary>
[HarmonyPatch(typeof(Car))]
[HarmonyPatch(nameof(Car.ApplyDerailmentDelta))]
public class Car_ApplyDerailmentDelta_Patch
{
	// delta > 0 means derailment, < 0 means rerailment
	private static bool Prefix(float delta, Car __instance)
	{
		if (delta <= 0.0) // re-rail
		{
			return true; //execute original function
		}

		if (Main.MySettings.DisableDerailing)
		{
			return false; //skip original function
		}

		if (Main.MySettings.ReportDamage && !__instance.IsDerailed)
		{
			Multiplayer.Broadcast($"{__instance.DisplayName} derailed!");
		}

		return true;
	}
}

/// <summary>
/// Disable damage, report damage
/// </summary>
[HarmonyPatch(typeof(Car))]
[HarmonyPatch(nameof(Car.ApplyConditionDelta))]
public class Car_ApplyConditionDelta_Patch
{
	// delta > 0 means repairing, < 0 means damaging
	private static bool Prefix(float delta, Car __instance)
	{
		if (delta >= 0.0) // repair
		{
			return true; //execute original function
		}

		if (Main.MySettings.DisableDamage)
		{
			return false; //skip original function
		}

		if (Main.MySettings.ReportDamage)
		{
			DamageRegistrar.Instance.AddDamageEntry(__instance.id, __instance.DisplayName, DateTime.Now, Math.Abs(delta));
		}
		
		return true;
	}
}

