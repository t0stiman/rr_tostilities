using HarmonyLib;

namespace tostilities.Patches.GameInput;

[HarmonyPatch(typeof(UI.GameInput), nameof(UI.GameInput.Update))]
public class Update
{
	private static void Prefix()
	{
		DamageRegistrar.Instance.Update();
	}
}