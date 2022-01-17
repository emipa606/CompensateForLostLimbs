using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace CompensateForLostLimbs;

[HarmonyPatch(typeof(PawnCapacityUtility), "CalculatePartEfficiency", typeof(HediffSet), typeof(BodyPartRecord),
    typeof(bool), typeof(List<PawnCapacityUtility.CapacityImpactor>))]
public class PawnCapacityUtility_CalculatePartEfficiency
{
    public static void Postfix(HediffSet diffSet, BodyPartRecord part, ref float __result)
    {
        if (__result != 0)
        {
            return;
        }

        if (Scribe.mode != LoadSaveMode.Inactive)
        {
            return;
        }

        var partHash = $"{diffSet.pawn.GetHashCode()}|{part.GetHashCode()}";
        if (CompensateForLostLimbs.CachedMissingLimbs.ContainsKey(partHash))
        {
            __result = CompensateForLostLimbs.CachedMissingLimbs[partHash];

            return;
        }

        var lostLimb = diffSet.hediffs.Where(hediff =>
        {
            if (hediff is not Hediff_MissingPart)
            {
                return false;
            }

            if (part == hediff.Part)
            {
                return true;
            }

            var parent = part.parent;
            while (parent is { IsCorePart: false })
            {
                if (parent == hediff.Part)
                {
                    return true;
                }

                parent = parent.parent;
            }

            return false;
        });

        if (!lostLimb.Any())
        {
            CompensateForLostLimbs.CachedMissingLimbs[partHash] = 0;
            return;
        }

        CompensateForLostLimbs.CachedMissingLimbs[partHash] =
            CompensateForLostLimbs.GetEfficencyFromHediffAge(lostLimb.First().ageTicks);

        __result = CompensateForLostLimbs.CachedMissingLimbs[partHash];
    }
}