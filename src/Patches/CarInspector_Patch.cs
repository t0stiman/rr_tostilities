using HarmonyLib;
using Model.Definition;
using tostilities.Extensions;
using UI.Builder;
using UI.CarInspector;

namespace tostilities.Patches;

[HarmonyPatch(typeof(CarInspector))]
[HarmonyPatch(nameof(CarInspector.PopulateOperationsPanel))]
public class CarInspector_PopulateOperationsPanel_Patch
{
	private static bool Prefix(CarInspector __instance, UIPanelBuilder builder)
	{
		if (!Main.MySettings.CrewDinges)
		{
			return Constants.EXECUTE_ORIGINAL;
		}
		
		builder.VScrollView(builder2 =>
		{
			builder2.FieldLabelWidth = 100f;
			
			// ==================================

			if (builder2.AddTrainCrewDropdown_2(__instance._car))
			{
				builder2.Spacer(4f);
			}

			// ==================================
			
			if (!__instance._car.Archetype.IsFreight())
				return;
			var waybill = __instance.Waybill;
			if (waybill.HasValue)
				__instance.PopulateWaybillPanel(builder2, waybill.Value);
			builder2.AddExpandingVerticalSpacer();
			if (!__instance.CanSetWaybill)
				return;
			__instance.PopulateSetWaybillPanel(builder2);
		});
		
		return Constants.SKIP_ORIGINAL;
	}
}
