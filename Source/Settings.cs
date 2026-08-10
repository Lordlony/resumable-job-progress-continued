using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ResumableJobProgress
{
	public class Settings : ModSettings
	{
		static readonly IEnumerable<string> listedResumingJobs = new List<string>
		{
			"DesignatorDeconstruct",
			"DesignatorUninstall",
			"DesignatorHarvest",
		};

		static readonly Dictionary<string, string> resumingJobFallbackLabels = new Dictionary<string, string>
		{
			{ "DesignatorDeconstruct", "Deconstruct" },
			{ "DesignatorUninstall", "Uninstall" },
			{ "DesignatorHarvest", "Harvest" },
		};

		static bool strictIngredient = false;
		static Dictionary<string, bool> disabledResumingJobs = new Dictionary<string, bool>();

		public static bool IsStrictIngredient => strictIngredient;
		public static bool IsDisabledDeconstruct => IsDisabledJob("DesignatorDeconstruct");
		public static bool IsDisabledUninstall => IsDisabledJob("DesignatorUninstall");
		public static bool IsDisabledHarvest => IsDisabledJob("DesignatorHarvest");

		override public void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref strictIngredient, "strictIngredient");
			Scribe_Collections.Look(ref disabledResumingJobs, "disabledResumingJobs", LookMode.Value, LookMode.Value);
			// Old or incomplete settings files can deserialize this dictionary as null.
			disabledResumingJobs ??= new Dictionary<string, bool>();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				// 古いバージョンの設定がある場合は、引き継ぐ
				// English: Carry over settings when an older version's keys are present.
				var loopResumingJobs = new Dictionary<string, bool>(disabledResumingJobs);
				foreach (var tempResumingJob in loopResumingJobs)
				{
					var oldJobItemKey = tempResumingJob.Key.Replace("Designator", "");
					if (disabledResumingJobs.ContainsKey(oldJobItemKey))
					{
						disabledResumingJobs.SetOrAdd(tempResumingJob.Key, disabledResumingJobs[oldJobItemKey]);
						disabledResumingJobs.Remove(oldJobItemKey);
					}
				}
			}
		}

		public static void DoSettingsWindowContents(Rect inRect)
		{
			var listing = new Listing_Standard();

			listing.Begin(new Rect(inRect.x, inRect.y, inRect.width * 0.6f, inRect.height));
			listing.CheckboxLabeled(
				Utility.TranslateWithFallback("ResumableJobProgress.StrictIngredient.label", "Require a matching ingredient to resume cooking"),
				ref strictIngredient,
				Utility.TranslateWithFallback("ResumableJobProgress.StrictIngredient.desc", "A cooking job can resume only if at least one ingredient has the same type as an ingredient used before interruption."));
			listing.Gap();
			listing.Label(Utility.TranslateWithFallback("ResumableJobProgress.DisablesResuming", "Disable progress resuming for:"));
			listing.GapLine(6f);
			foreach (var listedResumingJob in listedResumingJobs)
			{
				if (!disabledResumingJobs.TryGetValue(listedResumingJob, out bool isResuming))
				{
					isResuming = false;
				};
				// Vanilla designator labels also receive an English fallback in partial language packs.
				listing.CheckboxLabeled(Utility.TranslateWithFallback(listedResumingJob, resumingJobFallbackLabels[listedResumingJob]), ref isResuming);
				disabledResumingJobs[listedResumingJob] = isResuming;
			}
			listing.End();
		}

		static bool IsDisabledJob(string key)
		{
			if (disabledResumingJobs.TryGetValue(key, out bool value))
			{
				return value;
			}
			return false;
		}
	}
}
