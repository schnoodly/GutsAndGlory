using System;
using TaleWorlds.Core;
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

        private void BreakOffBodyPartType(Agent victim, int damage, int weaponKind) // So guy is dead, do we break bodypart off, and in what way?
        {
            victim.State = agentState;
            if (flag)
            {
                this.KillAgent(agent);
            }
            agent.AgentVisuals.SetVoiceDefinitionIndex(-1, 0f);
            AgentBuildData agentBuildData = new AgentBuildData(agent.Character);
            agentBuildData.NoHorses(true);
            agentBuildData.NoWeapons(true);
            agentBuildData.NoArmor(false);
            agentBuildData.Team(Mission.Current.PlayerEnemyTeam);
            agentBuildData.TroopOrigin(agent.Origin);
            agentBuildData.InitialFrame(new MatrixFrame
            {
                origin = agent.Position + agent.LookDirection * -0.75f,
                rotation = agent.Frame.rotation
            });
            Agent agent2 = Mission.Current.SpawnAgent(agentBuildData, false, 0);
            List<string> list = new List<string>();
            list.Add("[" + agent.Name + "]");
            foreach (Mesh mesh in agent2.AgentVisuals.GetSkeleton().GetAllMeshes())
            {
                bool flag2 = !mesh.Name.ToLower().Contains("head") && !mesh.Name.ToLower().Contains("hair") && !mesh.Name.ToLower().Contains("beard") && !mesh.Name.ToLower().Contains("eyebrow") && !mesh.Name.ToLower().Contains("helmet") && !mesh.Name.ToLower().Contains("_cap_");
                if (flag2)
                {
                    mesh.SetVisibilityMask((VisibilityMaskFlags)4293918720U);
                    mesh.ClearMesh();
                }
                list.Add(mesh.Name);
            }
            File.WriteAllLines("meshes temp test.txt", list);
            agent2.AgentVisuals.GetEntity().ActivateRagdoll();
            agent2.AgentVisuals.SetVoiceDefinitionIndex(-1, 0f);
            this.KillAgent(agent2);
            foreach (Mesh mesh2 in agent.AgentVisuals.GetSkeleton().GetAllMeshes())
            {
                bool flag3 = mesh2.Name.ToLower().Contains("head") || mesh2.Name.ToLower().Contains("hair") || mesh2.Name.ToLower().Contains("beard") || mesh2.Name.ToLower().Contains("eyebrow") || mesh2.Name.ToLower().Contains("helmet") || mesh2.Name.ToLower().Contains("_cap_");
                if (flag3)
                {
                    mesh2.SetVisibilityMask((VisibilityMaskFlags)4293918720U);
                    mesh2.ClearMesh();
                }
            }
            MatrixFrame boneEntitialFrameWithIndex = agent.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex((byte)agent.BoneMappingArray[HumanBone.Head]);
            Vec3 vec = agent.AgentVisuals.GetGlobalFrame().TransformToParent(boneEntitialFrameWithIndex.origin);
            agent.CreateBloodBurstAtLimb(13, ref vec, 0.5f + MBRandom.RandomFloat * 0.5f);
            boneEntitialFrameWithIndex = agent2.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex((byte)agent2.BoneMappingArray[HumanBone.Head]);
            vec = agent2.AgentVisuals.GetGlobalFrame().TransformToParent(boneEntitialFrameWithIndex.origin);
            agent2.CreateBloodBurstAtLimb(13, ref vec, 0.5f + MBRandom.RandomFloat * 0.5f);
        }
    }
}
