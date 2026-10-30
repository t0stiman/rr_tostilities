using System;
using System.Reflection;
using HarmonyLib;
using Railroader.Modding;

namespace tostilities;

public sealed class ModEntryPoint : IModEntryPoint
{
	private Harmony? harmony;
	public static IModContext? Context { get; private set; } 
	public static Settings? MySettings { get; private set; }

	public void OnModLoad(IModContext context_)
	{
		Context = context_;
		
		MySettings = new Settings();
		
		try
		{
			harmony = new Harmony(context_.ModIdentifier);
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
		catch (Exception ex)
		{
			context_.Log.Error($"Failed to load {context_.ModIdentifier}: {ex}");
			harmony?.UnpatchAll(context_.ModIdentifier);
			throw;
		}

		Context.Log.Info($"Loaded from {Context.ModDirectory}"); 
	}

	public void OnModUnload()
	{
		harmony?.UnpatchAll(Context!.ModIdentifier);
		Context?.Log.Info("Unloaded mod");
	}

	// Logger Commands
	public static void Debug(string message)
	{
		if (!MySettings!.EnableDebugLogs)
		{
			return;
		}

		if (MySettings.LogToConsole)
		{
			Console.Log($"{Context!.ModIdentifier}[DEBUG]{message}");
		}

		Context!.Log.Debug(message);
	}

	public static void Info(string message)
	{
		if (MySettings!.LogToConsole)
		{
			Console.Log($"{Context!.ModIdentifier}[INFO]{message}");
		}

		Context!.Log.Info(message);
	}

	public static void Warning(string message)
	{
		if (MySettings!.LogToConsole)
		{
			Console.Log($"{Context!.ModIdentifier}[WARNING]{message}");
		}

		Context!.Log.Warning(message);
	}

	public static void Error(string message)
	{
		if (MySettings!.LogToConsole)
		{
			Console.Log($"{Context!.ModIdentifier}[ERROR]{message}");
		}

		Context!.Log.Error(message);
	}
}