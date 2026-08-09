using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace ResumableJobProgress
{
	internal static class OtherModCompatibility
	{
		private static bool cacheIsSmartDecon;
		private static bool hasCachedIsSmartDecon = false;
		private static Type medievalOverhaulMendingDriverType;
		private static bool hasCachedMedievalOverhaulMendingDriverType;

		public static bool IsSmartDecon()
		{
			if (!hasCachedIsSmartDecon)
			{
				cacheIsSmartDecon = ModLister.HasActiveModWithName("Smarter Deconstruction and Mining")
					|| ModLister.HasActiveModWithName("Smarter Deconstruction and Mining (Continued)");

				if (!cacheIsSmartDecon)
				{
					foreach (var mod in ModsConfig.ActiveModsInLoadOrder)
					{
						if (mod?.Name?.IndexOf("Smarter Deconstruction", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							cacheIsSmartDecon = true;
							break;
						}
					}
				}

				hasCachedIsSmartDecon = true;
			}
			return cacheIsSmartDecon;
		}

		public static bool IsMedievalOverhaulMendingDriver(JobDriver driver)
		{
			if (driver is null)
			{
				return false;
			}

			if (!hasCachedMedievalOverhaulMendingDriverType)
			{
				medievalOverhaulMendingDriverType = AccessTools.TypeByName("MedievalOverhaul.JobDriver_DoMending");
				hasCachedMedievalOverhaulMendingDriverType = true;
			}

			return medievalOverhaulMendingDriverType?.IsInstanceOfType(driver) == true;
		}
	}
}
