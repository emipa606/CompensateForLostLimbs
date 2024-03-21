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
    public static CompensateForLostLimbsMod instance;

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
        instance = this;
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
            if (settings == null)
            {
                settings = GetSettings<CompensateForLostLimbsSettings>();
            }

            return settings;
        }
        set => settings = value;
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();

        listing_Standard.Label("CFLL.maxefficency.label".Translate(Math.Round(Settings.MaxEfficency * 100)), -1f,
            "CFLL.maxefficency.description".Translate());
        Settings.MaxEfficency = listing_Standard.Slider(Settings.MaxEfficency, 0, 1f);
        listing_Standard.Label(
            "CFLL.recoverytime.label".Translate(Math.Round(Settings.RecoveryTime / GenDate.TicksPerDay)), -1f,
            "CFLL.recoverytime.description".Translate());
        Settings.RecoveryTime = listing_Standard.Slider(Settings.RecoveryTime, 1, 2 * GenDate.TicksPerYear);

        if (currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            listing_Standard.Label("CFLL.version.label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Settings.Write();
    }
}