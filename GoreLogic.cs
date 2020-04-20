using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace GutsAndGlory
{
    public class GoreLogic : MissionLogic
    {
        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, int weaponKind, int currentWeaponUsageIndex)
        {
            if (affectedAgent.Character != null && affectorAgent != null && affectorAgent.Character != null && affectedAgent.State == AgentState.Active)
            {
                bool isFatal = affectedAgent.Health - (float)damage < 1f;
                bool isTeamKill = affectedAgent.Team.Side == affectorAgent.Team.Side;
                affectorAgent.Origin.OnScoreHit(affectedAgent.Character, damage, isFatal, isTeamKill, weaponKind, currentWeaponUsageIndex);
                if (isFatal)
                {
                    BreakOffBodyPartType(affectedAgent, damage, weaponKind);
                }
            }
        }

        private void BreakOffBodyPartType(Agent victim, int damage, int weaponKind) // So guy is dead, do we break bodypart off, and in what style?
        {
            // unfortunately you can't just "pop off" a mesh
            // We have to spawn a (dead) agent in the same place
            // In order to do this, we need to pass AgentBuildData from the guy we're killing.
            // So first we make AgentBuildData
            AgentBuildData agentBuildData = new AgentBuildData(victim.Character);
            agentBuildData.Team(victim.Team); // I believe we must specify the team they're on
            agentBuildData.InitialFrame(new MatrixFrame // Specify where in world the new agent is
            {
                origin = victim.Position + victim.LookDirection * -0.75f,
                rotation = victim.Frame.rotation
            });

            Agent victimPoppedMesh = Mission.Current.SpawnAgent(agentBuildData, false, 0); // Now we make the agent from that data
            victimPoppedMesh.State = AgentState.Killed; // Make sure he's dead

            // This is where we create the standalone limb
            // We do this via ClearMesh(), deleting all body meshes but the one that needs to pop off
            foreach (Mesh mesh in victimPoppedMesh.AgentVisuals.GetSkeleton().GetAllMeshes())
            {
                List<string> bodyPart = GenerateMeshNameListFromPartHit();
                bool flag2 = !mesh.Name.ToLower().Contains("head") && !mesh.Name.ToLower().Contains("hair") && !mesh.Name.ToLower().Contains("beard") && !mesh.Name.ToLower().Contains("eyebrow") && !mesh.Name.ToLower().Contains("helmet") && !mesh.Name.ToLower().Contains("_cap_");
                if (flag2)
                {
                    mesh.SetVisibilityMask((VisibilityMaskFlags)4293918720U);
                    mesh.ClearMesh();
                }
            }
            victimPoppedMesh.AgentVisuals.GetEntity().ActivateRagdoll();

            // Now we delete the mesh of the limb / bodypart we popped off from the original Agent (victim) 
            foreach (Mesh mesh2 in victim.AgentVisuals.GetSkeleton().GetAllMeshes())
            {
                bool flag3 = mesh2.Name.ToLower().Contains("head") || mesh2.Name.ToLower().Contains("hair") || mesh2.Name.ToLower().Contains("beard") || mesh2.Name.ToLower().Contains("eyebrow") || mesh2.Name.ToLower().Contains("helmet") || mesh2.Name.ToLower().Contains("_cap_");
                if (flag3)
                {
                    mesh2.SetVisibilityMask((VisibilityMaskFlags)4293918720U);
                    mesh2.ClearMesh();
                }
            }

            // Learning
            MatrixFrame boneEntitialFrameWithIndex = victim.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex((byte)victim.BoneMappingArray[HumanBone.Head]);
            Vec3 vec = victim.AgentVisuals.GetGlobalFrame().TransformToParent(boneEntitialFrameWithIndex.origin);
            victim.CreateBloodBurstAtLimb(13, ref vec, 0.5f + MBRandom.RandomFloat * 0.5f);
            boneEntitialFrameWithIndex = victimPoppedMesh.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex((byte)victimPoppedMesh.BoneMappingArray[HumanBone.Head]);
            vec = victimPoppedMesh.AgentVisuals.GetGlobalFrame().TransformToParent(boneEntitialFrameWithIndex.origin);
            victimPoppedMesh.CreateBloodBurstAtLimb(13, ref vec, 0.5f + MBRandom.RandomFloat * 0.5f);
        }

        public List<string> GenerateMeshNameListFromPartHit()
        {
            List<string> list = new List<string>();
            return list;
        }
    }
}
