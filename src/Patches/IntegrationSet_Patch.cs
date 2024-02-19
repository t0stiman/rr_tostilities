using HarmonyLib;
using Model;
using Model.Physics;
using UnityEngine;

namespace tostilities.Patches;

/// <summary>
/// Super-human strength
/// </summary>
[HarmonyPatch(typeof(IntegrationSet))]
[HarmonyPatch(nameof(IntegrationSet.AddVelocityToCar))]
public class IntegrationSet_AddVelocityToCar_Patch
{
	private static bool Prefix(IntegrationSet __instance, Car car, float velocity, float maxVelocity)
	{
		if (Main.MySettings.DisablePushSpeedDistanceLimiter)
		{
			maxVelocity = 9999;
		}
		
		velocity *= Main.MySettings.PushForceMultiplier;
		
		
		float nextDistance = velocity * Time.fixedDeltaTime;
		var indexOfCar = __instance.ValidIndexOfCar(car);
		var element = __instance._elements[indexOfCar];
		
		float nextPosition = element.position + (car.FrontIsA ? nextDistance : -nextDistance);
		float newVelocity = Mathf.Abs(element.position - element.oldPosition) / Time.fixedDeltaTime;
		
		var somethingVelocity = Mathf.Abs(nextPosition - element.oldPosition) / Time.fixedDeltaTime;

		if (!Main.MySettings.DisablePushSpeedDistanceLimiter)
		{
			if (newVelocity < maxVelocity && somethingVelocity > maxVelocity ||
			    newVelocity > maxVelocity && somethingVelocity > newVelocity)
			{
				return false;
			}
		}

		__instance._elements[indexOfCar].position = nextPosition;

		return false;
	}
}