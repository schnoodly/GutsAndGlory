using System;
using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;

namespace GutsAndGlory
{
    [HarmonyPatch(typeof(Mission), "MeleeHitCallback")]
    internal static class MeleeHitCallbackPatch
    {
        private static void Postfix(Agent victim, Agent killer, Vec3 blowDir, Vec3 swingDir, ref AttackCollisionData collisionData)
        {
            GutsAndGlorySubModule.DisplayMessage("Agent " + victim.Name + " was hit!", new Color(0f, 1f, 0f, 1f));
            bool flag = collisionData.CollisionResult == CombatCollisionResult.Parried || collisionData.CollisionResult == CombatCollisionResult.Blocked || collisionData.CollisionResult == CombatCollisionResult.ChamberBlocked;
            if (!flag && victim != null && victim.IsHuman && collisionData.CollisionBoneIndex != -1)
            {
                int damage = collisionData.InflictedDamage;
                float overkillAmount = victim.Health - damage;
                bool isFatal = overkillAmount < 1f;
                bool damageBreaksThreshold = overkillAmount < ModOptions.Instance.damageThreshold * -1;
                if (isFatal && damageBreaksThreshold)
                {
                    GoreLogic.BreakOffBodyPartType(victim, killer, overkillAmount, collisionData, blowDir, swingDir);
                }
            }
        }
    }
}
