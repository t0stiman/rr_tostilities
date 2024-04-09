using System.Collections;
using System.Collections.Generic;
using System.Text;
using Game.State;
using HarmonyLib;
using Model.OpsNew;
using Network;
using Serilog;
using UnityEngine;

namespace tostilities.Patches;

[HarmonyPatch(typeof(IndustryContext))]
[HarmonyPatch(nameof(IndustryContext.PayWaybill))]
public class IndustryContext_PayWaybill_Patch
{
	private static List<WaybillPayment> paymentBuffer = new();
	private static bool buffering = false;
	
	private static bool Prefix(ref IndustryContext __instance , IOpsCar car, Waybill waybill)
	{
		if (!Main.MySettings.MergePaymentMessages)
		{
			return Constants.EXECUTE_ORIGINAL;
		}

		var now = __instance.Now;
		var paymentOnArrival = waybill.PaymentOnArrival;
		var damageFine = Mathf.FloorToInt(paymentOnArrival * (1f - Mathf.Lerp(0.25f, 1f, car.Condition)));
		var timelyDeliveryBonus = 0;
		if (__instance._industry.HasActiveContract(now))
		{
			timelyDeliveryBonus = __instance._industry.Contract.Value.TimelyDeliveryBonus(Mathf.FloorToInt(now.DaysSince(waybill.Created)),
				paymentOnArrival);
		}

		var payment = new WaybillPayment(__instance._industry, paymentOnArrival, timelyDeliveryBonus, damageFine);

		var total = payment.TotalPayment;
		if (total == 0)
		{
			return Constants.SKIP_ORIGINAL;
		}

		__instance._industry.ApplyToBalance(total, Ledger.Category.Freight, count: 1, quiet: true);
		paymentBuffer.Add(payment);

		if (!buffering)
		{
			buffering = true;
			__instance._trainController.StartCoroutine(NotifyOfPayment());
		}

		return Constants.SKIP_ORIGINAL;
	}
	
	private class WaybillPayment
	{
		public Industry industry;
		public int paymentOnArrival;
		public int timelyDeliveryBonus;
		public int damageFine;

		public WaybillPayment(Industry industry, int paymentOnArrival, int timelyDeliveryBonus, int damageFine)
		{
			this.industry = industry;
			this.paymentOnArrival = paymentOnArrival;
			this.timelyDeliveryBonus = timelyDeliveryBonus;
			this.damageFine = damageFine;
		}

		public int TotalPayment => paymentOnArrival + timelyDeliveryBonus - damageFine;
	}

	private static IEnumerator NotifyOfPayment()
	{
		yield return new WaitForSeconds(1);
		
		int paymentOnArrival = 0;
		int timelyDeliveryBonus = 0;
		int damageFine = 0;
		int totalPayment = 0;

		foreach (var payment in paymentBuffer)
		{
			paymentOnArrival += payment.paymentOnArrival;
			timelyDeliveryBonus += payment.timelyDeliveryBonus;
			damageFine += payment.damageFine;
			totalPayment += payment.TotalPayment;
		}
		
		var stringBuilder = new StringBuilder();
		var carCars = paymentBuffer.Count == 1 ? "car" : "cars";
		stringBuilder.Append(
			$"Payment for delivery of {paymentBuffer.Count} {carCars} to {Hyperlink.To(paymentBuffer[0].industry)}: {paymentOnArrival:C0}");
		if (timelyDeliveryBonus > 0)
			stringBuilder.Append($" + {timelyDeliveryBonus:C0} timely");
		if (damageFine > 0)
			stringBuilder.Append($" - {damageFine:C0} damage");
		if (totalPayment != paymentOnArrival)
			stringBuilder.Append($" = {totalPayment:C0}");
		
		var str = stringBuilder.ToString();
		Log.Debug(str);
		Multiplayer.Broadcast(str);

		paymentBuffer = new();
		buffering = false;
	}
}