using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CompensateForLostLimbs;

[StaticConstructorOnStartup]
public static class CompensateForLostLimbs
{
    private static Dictionary<string, float> cachedMissingLimbs;
    private static int lastQuery;
    public static readonly List<BodyPartDef> BodyPartsToIgnoreForBlindsight;

    static CompensateForLostLimbs()
    {
        if (ModLister.IdeologyInstalled)
        {
            BodyPartsToIgnoreForBlindsight = DefDatabase<BodyPartDef>.AllDefsListForReading
                .Where(def => def.tags.Contains(BodyPartTagDefOf.SightSource)).ToList();
        }
        else
        {
            BodyPartsToIgnoreForBlindsight = [];
        }

        var harmony = new Harmony("Mlie.CompensateForLostLimbs");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    public static Dictionary<string, float> CachedMissingLimbs
    {
        get
        {
            if (cachedMissingLimbs == null || GenTicks.TicksAbs < 0)
            {
                cachedMissingLimbs = new Dictionary<string, float>();
            }

            if (lastQuery + GenTicks.TickRareInterval > GenTicks.TicksAbs)
            {
                lastQuery = GenTicks.TicksAbs;
                return cachedMissingLimbs;
            }

            lastQuery = GenTicks.TicksAbs;
            cachedMissingLimbs = new Dictionary<string, float>();
            return cachedMissingLimbs;
        }
    }

    public static float GetEfficiencyFromHediffAge(int hediffAgeTicks)
    {
        if (hediffAgeTicks >= CompensateForLostLimbsMod.Instance.Settings.RecoveryTime)
        {
            return CompensateForLostLimbsMod.Instance.Settings.MaxEfficiency;
        }

        return CompensateForLostLimbsMod.Instance.Settings.MaxEfficiency *
               (hediffAgeTicks / CompensateForLostLimbsMod.Instance.Settings.RecoveryTime);
    }
}