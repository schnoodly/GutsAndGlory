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
        public static GoreLogic Instance => GutsAndGlorySubModule.GoreLogic;

        /*
        public override void OnRegisterBlow(Agent attacker, Agent victim, GameEntity realHitEntity, Blow b, ref AttackCollisionData collisionData)
        {
            InformationManager.DisplayMessage(new InformationMessage("Character hit!"));
            if (victim.Character != null && attacker != null && attacker.Character != null)
            {
                int damage = b.InflictedDamage;
                bool isFatal = victim.Health - damage < 1f;
                if (isFatal)
                {
                    string msg = "";
                    //BreakOffBodyPartType(victim, attacker, damage, b, collisionData);
                    foreach (Mesh mesh in victim.AgentVisuals.GetSkeleton().GetAllMeshes())
                    {
                        msg += mesh.Name.ToLower() + " ";
                    }
                    GutsAndGlorySubModule.DisplayMessage("BodyParts: " + msg, 16711680U);
                }
            }
            else
            {
                GutsAndGlorySubModule.DisplayMessage("one or more values null");
            }
        }
        */
        
        public static void BreakOffBodyPartType(Agent victim, Agent killer, float overkill, AttackCollisionData collision, Vec3 blowDir, Vec3 swingDir) // So guy is dead, do we break bodypart off, and in what style?
        {
            string msg = "";
            foreach (Mesh mesh in victim.AgentVisuals.GetSkeleton().GetAllMeshes())
            {
                msg += mesh.Name.ToLower() + ", ";
            }
            GutsAndGlorySubModule.DisplayMessage("BodyParts: " + msg, new Color(0f, 1f, 0f, 1f));
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
                // List<string> bodyPart = GenerateMeshNameListFromPartHit();
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
