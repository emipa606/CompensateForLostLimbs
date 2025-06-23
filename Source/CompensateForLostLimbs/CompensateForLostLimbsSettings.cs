using Verse;

namespace CompensateForLostLimbs;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class CompensateForLostLimbsSettings : ModSettings
{
    public float MaxEfficiency = 0.25f;
    public float RecoveryTime = 3600000;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref MaxEfficiency, "MaxEfficency", 0.25f);
        Scribe_Values.Look(ref RecoveryTime, "RecoveryTime", 3600000);
    }
}