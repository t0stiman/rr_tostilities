﻿using HarmonyLib;
using UI;

namespace rr_utilities.Patches;

/// <summary>
/// Bunny hop patch
/// </summary>
[HarmonyPatch(typeof(GameInput))]
[HarmonyPatch(nameof(GameInput.JumpDown), MethodType.Getter)]
public class GameInput_JumpDown_Patch
{
	// delta > 0 means derailment, < 0 means rerailment
	private static bool Prefix(GameInput __instance, ref bool __result)
	{
		if (!Main.MySettings.EnableBunnyHopping)
		{
			return true; //execute original function
		}

		__result = __instance._jumpAction.IsPressed();
		
		return false; //skip original function
	}
}

[HarmonyPatch(typeof(GameInput))]
[HarmonyPatch(nameof(GameInput.Update))]
public class GameInput_Update_Patch
{
	private static void Prefix()
	{
		DamageRegistrar.Instance.Update();
	}
}