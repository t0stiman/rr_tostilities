using UI;
using UnityEngine;
using UnityModManagerNet;

namespace rr_utilities
{
	public class Settings : UnityModManager.ModSettings
	{
		public float PushForceMultiplier = 10;
		private string PushForceMultiplier_text;

		public bool DisableDerailing = false;
		public bool DisableDamage = false;
		
		public void Setup()
		{
			PushForceMultiplier_text = PushForceMultiplier.ToString("0.0");
		}
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			GUILayout.Label("Note: these only work in multiplayer if you are the server host.");
			GUILayout.Space(20f);
			
			GUILayout.Label("Car push force multiplier. Higher number -> bigger YEET.");
			DrawFloatInput(ref PushForceMultiplier_text, ref PushForceMultiplier);
			
			DisableDerailing = GUILayout.Toggle(DisableDerailing, "Disable derailing");
			DisableDamage = GUILayout.Toggle(DisableDamage, "Disable damage to rolling stock");
		}

		private void DrawFloatInput(ref string text, ref float number)
		{
			text = GUILayout.TextField(text);
			if (float.TryParse(text, out float parsed))
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