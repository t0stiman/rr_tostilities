using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Messages;
using Game.Notices;
using Game.State;
using HarmonyLib;
using Network;
using Serilog;

namespace tostilities.Patches;

[HarmonyPatch(typeof(PlayersManager))]
[HarmonyPatch(nameof(PlayersManager.NotifyOfConnected))]
public class PlayersManager_NotifyOfConnected_Patch
{
	private static bool Prefix(ref PlayersManager __instance, Dictionary<PlayerId, Snapshot.Player> connectedPlayers)
	{
		if (!Main.MySettings.EnableWelcomeMessage && !Main.MySettings.ConsoleStampsRealTime)
		{
			return Constants.EXECUTE_ORIGINAL;
		}
		
		// ============= unchanged: =============
		
		if (!connectedPlayers.Any()){
			return Constants.SKIP_ORIGINAL;
		}
		
		Log.Information("Connected: {players}", connectedPlayers);
		var playerNames = string.Join(", ", connectedPlayers.OrderBy(p => p.Value.Name).Select(p => Hyperlink.To(new EntityReference(p.Key))));
		var plural = connectedPlayers.Count != 1;
		
		if (__instance._hasNotifiedOfPlayers || StateManager.IsHost)
		{
			// ============= changed: =============
			
			// 24h format instead of AM PM
			var timeString = Main.MySettings.ConsoleStampsRealTime ? "" : DateTime.Now.ToString("HH:mm")+" ";
			Console.Log($"{timeString}{playerNames} has connected.");
			
			if (Main.MySettings.EnableWelcomeMessage && StateManager.IsHost)
			{
				var message = Main.MySettings.WelcomeMessage.Replace(Settings.playername_replaceo, playerNames);
				Multiplayer.Broadcast(message);
			}
			
			// ============= unchanged: =============
		}
		else
		{
			Console.Log(playerNames + " " + (plural ? "are" : "is") + " connected.");
		}

		__instance._hasNotifiedOfPlayers = true;
		foreach (var connectedPlayer in connectedPlayers)
		{
			var playerId = connectedPlayer.Key;
			NoticeManager.Shared.PostEphemeralLocal(new EntityReference(playerId), "conn", "Connected");
		}

		return Constants.SKIP_ORIGINAL;
	}
}