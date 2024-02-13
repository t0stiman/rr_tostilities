using System;
using System.Collections.Generic;
using Network;

namespace tostilities;

public class DamageRegistrar
{
	private DamageRegistrar(){}

	private static DamageRegistrar instance;
	public static DamageRegistrar Instance
	{
		get
		{
			if (instance is null)
			{
				instance = new DamageRegistrar();
			}

			return instance;
		}
	}
	
	// key: car ID
	private List<DamageEntry> damageEntries = new();
	
	public void AddDamageEntry(string id, string displayName, DateTime timestamp, float delta)
	{
		foreach (var e in damageEntries)
		{
			if (e.CarID == id)
			{
				e.When = timestamp;
				e.Damage += delta;
				return;
			}
		}
		
		damageEntries.Add(new DamageEntry(id, displayName, timestamp, delta));
	}

	public void Update()
	{
		foreach (var e in damageEntries)
		{
			if ((DateTime.Now - e.When).TotalSeconds >= 2.0)
			{
				var damagePercent = (int)Math.Ceiling(e.Damage * 100);
				Multiplayer.Broadcast($"<noparse>{e.CarDisplayName}</noparse> received <b>{damagePercent}%</b> damage!");
				damageEntries.Remove(e);
			}
		}
	}
	
	private class DamageEntry
	{
		public string CarID;
		public string CarDisplayName;
		public DateTime When;
		public float Damage; // 1 = 100% damage

		public DamageEntry(string carID, string carDisplayName, DateTime when, float damage)
		{
			CarID = carID;
			CarDisplayName = carDisplayName;
			When = when;
			Damage = damage;
		}
	}
}

