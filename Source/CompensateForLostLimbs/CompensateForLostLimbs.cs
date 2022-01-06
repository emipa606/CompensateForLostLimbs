using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace CompensateForLostLimbs;

[StaticConstructorOnStartup]
public static class CompensateForLostLimbs
{
    private static Dictionary<string, float> cachedMissingLimbs;
    private static int lastQuery;

    static CompensateForLostLimbs()
    {
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