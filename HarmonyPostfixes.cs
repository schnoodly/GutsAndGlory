using System;
using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using TaleWorlds.Core;

namespace GutsAndGlory
{
    [HarmonyPatch(typeof(Mission))]
    [HarmonyPatch("MeleeHitCallback")]
    internal static class MeleeHitCallbackPatch
    {
        private static void Postfix(ref AttackCollisionData collisionData, Agent victim, Agent attacker, Vec3 blowDir, Vec3 swingDir)
        {
            bool flag = collisionData.CollisionResult == CombatCollisionResult.Parried || collisionData.CollisionResult == CombatCollisionResult.Blocked || collisionData.CollisionResult == CombatCollisionResult.ChamberBlocked;
            if (!flag && victim != null && victim.IsHuman && collisionData.CollisionBoneIndex != -1)
            {
                int damage = collisionData.InflictedDamage;
                float overkillAmount = victim.Health - damage;
                bool isFatal = overkillAmount < 1f;
                bool damageBreaksThreshold = overkillAmount < ModOptions.Instance.damageThreshold * -1;
                if (isFatal && damageBreaksThreshold)
                {
                    GoreLogic.BreakOffBodyPartType(victim, attacker, overkillAmount, collisionData, blowDir, swingDir);
                    InformationManager.DisplayMessage(new InformationMessage("Cutting off a limb"));
                }
            }
        }
    }
}
