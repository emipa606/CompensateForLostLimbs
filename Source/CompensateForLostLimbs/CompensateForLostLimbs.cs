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
    public static List<BodyPartDef> bodyPartsToIgnoreForBlindsight;

    static CompensateForLostLimbs()
    {
        if (ModLister.IdeologyInstalled)
        {
            bodyPartsToIgnoreForBlindsight = DefDatabase<BodyPartDef>.AllDefsListForReading
                .Where(def => def.tags.Contains(BodyPartTagDefOf.SightSource)).ToList();
        }
        else
        {
            bodyPartsToIgnoreForBlindsight = new List<BodyPartDef>();
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

    public static float GetEfficencyFromHediffAge(int hediffAgeTicks)
    {
        if (hediffAgeTicks >= CompensateForLostLimbsMod.instance.Settings.RecoveryTime)
        {
            return CompensateForLostLimbsMod.instance.Settings.MaxEfficency;
        }

        return CompensateForLostLimbsMod.instance.Settings.MaxEfficency *
               (hediffAgeTicks / CompensateForLostLimbsMod.instance.Settings.RecoveryTime);
    }
}