using System;
using System.Linq;
using Game.Messages;
using Game.State;
using HarmonyLib;
using Model.Database;
using UI.Menu;

namespace tostilities.Patches;

/// <summary>
/// Makes it possible to join MP games even if the host doesn't have this mod installed.
/// </summary>
[HarmonyPatch(typeof(GlobalGameManager), nameof(GlobalGameManager.ReconcileModsForSnapshot))]
public class ReconcileModsForSnapshot
{
	private static void Prefix(ref Snapshot.ModConfig snapshotConfig)
	{
		if (
			GlobalGameManager.IgnoreMods 
		  || StateManager.IsHost 
		  || snapshotConfig.Entries.Any(entry => entry.Identifier == ModEntryPoint.Context!.ModIdentifier)
		)
		{
			return;
		}

		Snapshot.ModVersionEntry thisModEntry;
		try
		{
			thisModEntry = AssetPackManager.Shared.GetLoadedModConfig().Entries
				.Single(entry => entry.Identifier == ModEntryPoint.Context!.ModIdentifier);
		}
		catch (InvalidOperationException)
		{
			ModEntryPoint.Error($"{nameof(GlobalGameManager.ReconcileModsForSnapshot)}: couldn't find mod entry");
			return;
		}
		
		snapshotConfig.Entries.Add(thisModEntry);
	}
}
