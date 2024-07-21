using UnityEngine;
using UnityModManagerNet;

namespace tostilities
{
	public class Settings : UnityModManager.ModSettings
	{
		public const string playername_replaceo = "<playername>";
		private const int SPACE = 15;
		
		public int PushForceMultiplier = 1;
		private string PushForceMultiplier_text;
		public bool DisablePushSpeedDistanceLimiter = false;

		public bool DisableDerailing = false;
		public bool DisableDamage = false;
		public bool EnableBunnyHopping = false;
		public bool ReportDamage = true;
		public bool ConsoleStampsRealTime = false;

		public bool EnableWelcomeMessage = true;
		public string WelcomeMessage = "<playername>, welcome to the server!";
		// public bool MergePaymentMessages = false;
		public bool CrewDinges = false;
		
		//logging stuff
		public bool LogToConsole = false;
		public bool EnableDebugLogs = false;

		public void Setup()
		{
			PushForceMultiplier_text = PushForceMultiplier.ToString("0.0");
		}
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			GUILayout.Label("These always work and only apply to you:");
			GUILayout.Space(SPACE);
			
			EnableBunnyHopping = GUILayout.Toggle(EnableBunnyHopping, "Enable bunny hopping (hold space to keep jumping)");
			ConsoleStampsRealTime = GUILayout.Toggle(ConsoleStampsRealTime, "Show real-life time instead of in-game time on messages in the console");
			CrewDinges = GUILayout.Toggle(CrewDinges, "Make it possible to add any stock you own to a crew");
			
			GUILayout.Space(SPACE);
			GUILayout.Label("These only work in multiplayer if you are the server host, and apply to ALL players:");
			GUILayout.Space(SPACE);
			
			DrawIntInput("Car push force multiplier. Higher number -> bigger YEET.", ref PushForceMultiplier_text, ref PushForceMultiplier);
			DisablePushSpeedDistanceLimiter = GUILayout.Toggle(DisablePushSpeedDistanceLimiter, "By default, the speed of pushed cars is limited to 3 mph and the distance to 5 m. This tweak removes those limitations");
			
			DisableDerailing = GUILayout.Toggle(DisableDerailing, "Disable derailing");
			DisableDamage = GUILayout.Toggle(DisableDamage, "Disable damage to rolling stock");
			ReportDamage = GUILayout.Toggle(ReportDamage, "Report damage to rolling stock and derailments in the console");
			
			//TODO
			// MergePaymentMessages = GUILayout.Toggle(MergePaymentMessages, "When many cars get delivered at once, show only 1 payment message in the console");
			EnableWelcomeMessage = GUILayout.Toggle(EnableWelcomeMessage, "Show a customizable message in the console when a new player joins");

			if (EnableWelcomeMessage)
			{
				GUILayout.Label($"Welcome message text. {playername_replaceo} will be replaced with the name of the player.");
				WelcomeMessage = GUILayout.TextArea(WelcomeMessage);
			}
			
			// logging stuff
			GUILayout.Space(SPACE);
			GUILayout.Label("Logging stuff: ");
			GUILayout.Space(SPACE);
			
			LogToConsole = GUILayout.Toggle(LogToConsole, "Log messages to the in-game console as well as Player.log");
			EnableDebugLogs = GUILayout.Toggle(EnableDebugLogs, "Enable debug messages");
		}

		private void DrawIntInput(string descriptionText, ref string fieldText, ref int number)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(descriptionText);
			fieldText = GUILayout.TextField(fieldText);
			GUILayout.EndHorizontal();
			
			if (int.TryParse(fieldText, out int parsed))
			{
				number = parsed;
			}
			else
			{
				GUILayout.Label($"not a valid number");
			}
		}

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			Save(this, modEntry);
		}
	}
}