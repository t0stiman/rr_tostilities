using HarmonyLib;

namespace tostilities.Patches.GameInput;

/// <summary>
/// Bunny hop
/// </summary>
[HarmonyPatch(typeof(UI.GameInput), nameof(UI.GameInput.JumpDown), MethodType.Getter)]
public class JumpDown
{
	// delta > 0 means derailment, < 0 means rerailment
	private static bool Prefix(UI.GameInput __instance, ref bool __result)
	{
		if (!ModEntryPoint.MySettings!.EnableBunnyHopping)
		{
			return Constants.EXECUTE_ORIGINAL;
		}

		__result = __instance._jumpAction.IsPressed();
		
		return Constants.SKIP_ORIGINAL;
	}
}