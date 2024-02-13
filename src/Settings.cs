using UI;
using UnityEngine;
using UnityModManagerNet;

namespace rr_utilities
{
	public class Settings : UnityModManager.ModSettings
	{
		private const int SPACE = 15;
		
		public float PushForceMultiplier = 10;
		private string PushForceMultiplier_text;

		public bool DisableDerailing = false;
		public bool DisableDamage = false;
		public bool EnableBunnyHopping = false;
		public bool ReportDamage = false;
		public bool ConsoleTimeStamps = false;
		
		public void Setup()
		{
			PushForceMultiplier_text = PushForceMultiplier.ToString("0.0");
		}
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			GUILayout.Label("These always work and only apply to you:");
			GUILayout.Space(SPACE);
			
			EnableBunnyHopping = GUILayout.Toggle(EnableBunnyHopping, "Hold space to keep jumping");
			ConsoleTimeStamps = GUILayout.Toggle(ConsoleTimeStamps, "Show a timestamp on every message in the console");
			
			GUILayout.Space(SPACE);
			GUILayout.Label("These only work in multiplayer if you are the server host, and apply to ALL players:");
			GUILayout.Space(SPACE);
			
			DrawFloatInput("Car push force multiplier. Higher number -> bigger YEET.", 
				ref PushForceMultiplier_text, ref PushForceMultiplier);
			
			DisableDerailing = GUILayout.Toggle(DisableDerailing, "Disable derailing");
			DisableDamage = GUILayout.Toggle(DisableDamage, "Disable damage to rolling stock");
			ReportDamage = GUILayout.Toggle(ReportDamage, "Report damage to rolling stock and derailments in the console");
		}

		private void DrawFloatInput(string descriptionText, ref string fieldText, ref float number)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(descriptionText);
			fieldText = GUILayout.TextField(fieldText);
			GUILayout.EndHorizontal();
			
			if (float.TryParse(fieldText, out float parsed))
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