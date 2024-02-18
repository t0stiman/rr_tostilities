using HarmonyLib;
using Model.Physics;

namespace tostilities.Patches;

/// <summary>
/// Super-human strength
/// </summary>
[HarmonyPatch(typeof(IntegrationSet))]
[HarmonyPatch(nameof(IntegrationSet.AddVelocityToCar))]
public class IntegrationSet_AddVelocityToCar_Patch
{
	private static void Prefix(ref float amount)
	{
		amount *= Main.MySettings.PushForceMultiplier;
	}
}