using HarmonyLib;
using Model.Physics;
using UnityEngine;

namespace tostilities.Patches;

/// <summary>
/// Super-human strength
/// </summary>
[HarmonyPatch(typeof(IntegrationSet), nameof(IntegrationSet.AddVelocityToCar))]
public class IntegrationSet_AddVelocityToCar
{
	private static bool Prefix(IntegrationSet __instance, Model.Car car, float velocity, float maxVelocity)
	{
		if (ModEntryPoint.MySettings!.DisablePushSpeedDistanceLimiter)
		{
			maxVelocity = 9999;
		}
		
		velocity *= ModEntryPoint.MySettings.PushForceMultiplier;
		
		float nextDistance = velocity * Time.fixedDeltaTime;
		var indexOfCar = __instance.ValidIndexOfCar(car);
		var element = __instance._elements[indexOfCar];
		
		float nextPosition = element.position + (car.FrontIsA ? nextDistance : -nextDistance);
		float newVelocity = Mathf.Abs(element.position - element.oldPosition) / Time.fixedDeltaTime;
		
		var somethingVelocity = Mathf.Abs(nextPosition - element.oldPosition) / Time.fixedDeltaTime;

		if (!ModEntryPoint.MySettings.DisablePushSpeedDistanceLimiter)
		{
			if (newVelocity < maxVelocity && somethingVelocity > maxVelocity ||
			    newVelocity > maxVelocity && somethingVelocity > newVelocity)
			{
				return Constants.SKIP_ORIGINAL;
			}
		}

		__instance._elements[indexOfCar].position = nextPosition;

		return Constants.SKIP_ORIGINAL;
	}
}