using HarmonyLib;

namespace rr_utilities.Patches;

/// <summary>
/// Super-human strength
/// </summary>
[HarmonyPatch(typeof(TrainController))]
[HarmonyPatch(nameof(TrainController.HandleManualMoveCar))]
public class TrainController_HandleManualMoveCar_Patch
{
	private static void Prefix(ref float amount)
	{
		amount *= Main.MySettings.PushForceMultiplier;
	}
}