using System;
using Game;

namespace tostilities.Patches.Console;

public static class ConsoleHelper
{
	public static GameDateTime RealNow()
	{
		var now = DateTime.Now;
		return new GameDateTime(0, now.Hour + now.Minute / 60f);
	}
}