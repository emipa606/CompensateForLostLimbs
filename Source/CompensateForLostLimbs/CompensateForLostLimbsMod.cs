using System;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompensateForLostLimbs;

[StaticConstructorOnStartup]
internal class CompensateForLostLimbsMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static CompensateForLostLimbsMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private CompensateForLostLimbsSettings settings;

    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public CompensateForLostLimbsMod(ModContentPack content) : base(content)
    {
        Instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    public CompensateForLostLimbsSettings Settings
    {
        get
        {
            settings ??= GetSettings<CompensateForLostLimbsSettings>();

            return settings;
        }
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Compensate For Lost Limbs";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();

        listingStandard.Label("CFLL.maxefficency.label".Translate(Math.Round(Settings.MaxEfficiency * 100)), -1f,
            "CFLL.maxefficency.description".Translate());
        Settings.MaxEfficiency = listingStandard.Slider(Settings.MaxEfficiency, 0, 1f);
        listingStandard.Label(
            "CFLL.recoverytime.label".Translate(Math.Round(Settings.RecoveryTime / GenDate.TicksPerDay)), -1f,
            "CFLL.recoverytime.description".Translate());
        Settings.RecoveryTime = listingStandard.Slider(Settings.RecoveryTime, 1, 2 * GenDate.TicksPerYear);

        if (currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            listingStandard.Label("CFLL.version.label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}