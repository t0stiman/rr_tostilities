using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Messages;
using Game.State;
using HarmonyLib;
using Network;
using Network.Client;
using Serilog;

namespace tostilities.Patches;

[HarmonyPatch(typeof(PlayersManager))]
[HarmonyPatch(nameof(PlayersManager.NotifyOfNewPlayers))]
public class PlayersManager_NotifyOfNewPlayers_Patch
{
	private static bool Prefix(ref PlayersManager __instance, Dictionary<string, Snapshot.Player> players)
	{
		if (!Main.MySettings.EnableWelcomeMessage && !Main.MySettings.ConsoleTimeStamps)
		{
			return true; //execute original function
		}
		
		// ============= unchanged =============
		
		HashSet<PlayerId> playerIdSet = new HashSet<PlayerId>(__instance._remotePlayers.Keys);
		HashSet<Snapshot.Player> playerSet = new HashSet<Snapshot.Player>();
		foreach (KeyValuePair<string, Snapshot.Player> player1 in players)
		{
			PlayerId key = new PlayerId(player1.Key);
			playerIdSet.Remove(key);
			if (!__instance._remotePlayers.ContainsKey(key) && key != PlayersManager.PlayerId)
			{
				Snapshot.Player player2 = player1.Value;
				playerSet.Add(player2);
			}
		}
		if (playerSet.Any())
		{
			Log.Information("Connected: {players}", playerSet);
			string playerNames = string.Join(", ", playerSet.OrderBy(p => p.Name).Select(p => p.Name));
			bool flag = playerSet.Count != 1;
			if (__instance._hasNotifiedOfPlayers)
			{
				// ============= changed =============

				var timeString = Main.MySettings.ConsoleTimeStamps ? "" : DateTime.Now.ToString("HH:mm")+" ";
				Console.Log($"{timeString}{playerNames} has connected.");

				if (Main.MySettings.EnableWelcomeMessage)
				{
					var message = Main.MySettings.WelcomeMessage.Replace(Settings.playername_replaceo, playerNames);
					Multiplayer.Broadcast(message);
				}
				
				// ============= unchanged =============
			}
			else
			{
				Console.Log(playerNames + " " + (flag ? "are" : "is") + " connected.");
			}

			__instance._hasNotifiedOfPlayers = true;
		}
		foreach (PlayerId playerId in playerIdSet)
		{
			RemotePlayer remotePlayer;
			if (!__instance._remotePlayers.TryGetValue(playerId, out remotePlayer))
			{
				Log.Error("Couldn't find name of disconnected player {playerId}", playerId);
			}
			else
			{
				Log.Information("{name} {playerId} has disconnected", remotePlayer.playerName, remotePlayer.playerId);
				
				// ============= changed =============
				
				var timeString = Main.MySettings.ConsoleTimeStamps ? "" : DateTime.Now.ToString("HH:mm")+" ";
				Console.Log($"{timeString}{remotePlayer.playerName} has disconnected.");
				
				// ============= unchanged =============
			}
		}
		
		return false; //skip original function
	}
}

/*

[HarmonyPatch(typeof(PlayersManager))]
[HarmonyPatch(nameof(PlayersManager.NotifyOfNewPlayers))]
public class PlayersManager_NotifyOfNewPlayers_Patch
{
	private static void Prefix(out List<PlayerId> __state, PlayersManager __instance)
	{
		__state = __instance._remotePlayers.Keys.ToList();

		foreach (var aaa in __instance._remotePlayers)
		{
			Console.Log("old:"+aaa.Value.Name);
		}
	}

	private static void Postfix(List<PlayerId> __state, PlayersManager __instance)
	{
		// _hasNotifiedOfPlayers is false at server startup
		if (!Main.MySettings.EnableWelcomeMessage || !__instance._hasNotifiedOfPlayers)
		{
			return;
		}

		foreach (var aaa in __instance._remotePlayers)
		{
			Console.Log("new:"+aaa.Value.Name);
		}

		if (__instance._remotePlayers.Keys.ToList() != __state)
		{
			Console.Log("player list changed!");
		}

		List<string> newPlayerNames = new();

		foreach (var aPlayerNow in __instance._remotePlayers)
		{
			Console.Log(aPlayerNow.Value.Name);
			if (!__state.Contains(aPlayerNow.Key))
			{
				Console.Log("new");
				//new player
				newPlayerNames.Add(aPlayerNow.Value.Name);
			}
			else
			{
				Console.Log("newn't");
			}
		}

		foreach (var playerName in newPlayerNames)
		{
			var message = Main.MySettings.WelcomeMessage.Replace(Settings.playername_replaceo, playerName);
			Multiplayer.Broadcast(message);
		}
	}
}

*/