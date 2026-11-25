using HarmonyLib;

namespace tostilities.Patches;

/// <summary>
/// Make it possible to add any rolling stock you own to a crew
/// </summary>
[HarmonyPatch(typeof(Model.Car), nameof(Model.Car.SupportsTrainCrew), MethodType.Getter)]
public class SupportsTrainCrew
{
	private static bool Prefix(ref bool __result)
	{
		if (!ModEntryPoint.MySettings!.CrewDinges)
		{
			return Constants.EXECUTE_ORIGINAL;
		}
		
		__result = true;
		return Constants.SKIP_ORIGINAL;
	}
}
