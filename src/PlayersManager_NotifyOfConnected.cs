using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Messages;
using Game.State;
using HarmonyLib;
using Network;
using UnityEngine;

namespace tostilities.Patches;

/// <summary>
/// Show a customizable message in the console when a player joins your server
/// </summary>
[HarmonyPatch(typeof(PlayersManager), nameof(PlayersManager.NotifyOfConnected))]
public class NotifyOfConnected
{
	private static void Postfix(PlayersManager __instance, Dictionary<PlayerId, Snapshot.Player> connectedPlayers)
	{
		if (!ModEntryPoint.MySettings!.EnableWelcomeMessage || !StateManager.IsHost || !connectedPlayers.Any())
		{
			return;
		}

		var playerNames = string.Join(", ", connectedPlayers.Select(p => Hyperlink.To(new EntityReference(p.Key))));
		var message = ModEntryPoint.MySettings.WelcomeMessage.Replace(Settings.playername_replaceo, playerNames);
		var coroutine = DelayedSend(message);
		__instance.StartCoroutine(coroutine);
	}
	
	private static IEnumerator DelayedSend(string message)
	{
		// wait until the player has loaded in so they actually see the message
		yield return new WaitForSeconds(5);
		Multiplayer.Broadcast(message);
	}
}