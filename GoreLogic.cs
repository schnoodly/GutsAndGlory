using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace GutsAndGlory
{
    public class GoreLogic : MissionLogic
    {
        public static GoreLogic Instance => GutsAndGlorySubModule.GoreLogic;
        
        public static void BreakOffBodyPartType(Agent victim, Agent killer, float overkill, AttackCollisionData collision, Vec3 blowDir, Vec3 swingDir) // So guy is dead, do we break bodypart off, and in what style?
        {
            sbyte boneHit = collision.CollisionBoneIndex;           // what bone got hit
            Vec3 globalHitPos = collision.CollisionGlobalPosition;  // where in the world it was hit
            Ray ray = new Ray(globalHitPos, swingDir);
            InformationManager.DisplayMessage(new InformationMessage("Mesh: " + ray.EndPoint));
            // unfortunately you can't just "pop off" a mesh
            // We have to spawn a (dead) agent in the same place
            // In order to do this, we need to pass AgentBuildData from the guy we're killing.
            // So first we make AgentBuildData
            /*
            AgentBuildData agentBuildData = new AgentBuildData(victim.Character);
            agentBuildData.TroopOrigin(victim.Origin);
            agentBuildData.Team(victim.Team);               // We must specify the team they're on to spawn
            agentBuildData.Banner(victim.Origin.Banner);    // also their banner for visual reason if they're on our team
            agentBuildData.InitialFrame(new MatrixFrame     // Specify where in world the new agent is
            {
                origin = victim.Position + victim.LookDirection * -0.75f,
                rotation = victim.Frame.rotation
            });
            Agent victimSlicedLimb = Mission.Current.SpawnAgent(agentBuildData, false, 0); // Now we make the agent from that data
            victimSlicedLimb.AgentVisuals.GetEntity().ActivateRagdoll();
            // Make sure he's dead
            List<string> agentToKill = new List<string>();
            agentToKill.Add(victimSlicedLimb.Name);
            Mission.KillAgent(agentToKill);

            // Now we set up the bones and skeletons we want to slice
            Skeleton victimBones = victim.AgentVisuals.GetSkeleton();               // Skeleton for the main body
            Skeleton slicedLimbBones = victimSlicedLimb.AgentVisuals.GetSkeleton(); // Skeleton for the limb that will be liberated from the body
            byte childBoneTotal = victimBones.GetBoneChildCount((byte)boneHit);     // number of bones that are the children of the bone that got hit
            ArrayList limbToBeSliced = new ArrayList();                             // where we will store the bones that make up the sliced limb
            byte totalVictimBones = slicedLimbBones.GetBoneCount();                 // total bones for the "copy" we made above, to be used later

            int compTotal = victimBones.GetComponentCount(GameEntity.ComponentType.MetaMesh);
            InformationManager.DisplayMessage(new InformationMessage("total components: " + compTotal));
            int compCount = 0;
            IEnumerable<Mesh> meshes = victimBones.GetAllMeshes();
            foreach (Mesh m in meshes)
            {
                InformationManager.DisplayMessage(new InformationMessage("Mesh: " + m.Name));
            }
            while (compCount < compTotal)
            {
                GameEntityComponent compIndex = victimBones.GetComponentAtIndex(GameEntity.ComponentType.MetaMesh, compCount);
                compIndex.GetEntity().GetBodyShape();
                compCount++;
            }
            /*
            // This is where we remove the limb from the main mesh
            for (sbyte childBoneCount = 0; childBoneCount < (sbyte)childBoneTotal; childBoneCount += 1)
            {
                byte boneToClear = victimBones.GetBoneChildAtIndex((byte)boneHit, (byte)childBoneCount); // all these casts are getting a bit ridiculous. why do these all use different structs?
                limbToBeSliced.Add(boneToClear);                                                         // add this to the ArrayList we made above
                victimBones.ClearMeshesAtBone(boneToClear);                                              // clear the meshes at this bone
                InformationManager.DisplayMessage(new InformationMessage("clearing limb meshes at: " + boneToClear.ToString()));
            }
            

            // This is where we create the standalone limb
            // We do this via ClearMeshesAtBone(), deleting all body meshes but the one that needs to pop off
            for (sbyte b = 0; b < (sbyte)totalVictimBones; b += 1)
            {
                if (!limbToBeSliced.Contains((byte)b))
                {
                    slicedLimbBones.ClearMeshesAtBone(b);
                    //InformationManager.DisplayMessage(new InformationMessage("clearing body meshes at: " + b.ToString()));
                }
            }
            

            // Get the Entitial Frame, which I guess is a frame that entities can be attached to.
            // Serves to pinpoint where a bone is literally just for the blood burst particle, because apparently you can't access collision.CollisionGlobalPosition
            MatrixFrame boneEntitialFrameWithIndex = victim.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex((byte)boneHit);
            Vec3 globalPos = victim.AgentVisuals.GetGlobalFrame().TransformToParent(boneEntitialFrameWithIndex.origin);
            victim.CreateBloodBurstAtLimb(boneHit, ref globalPos, 0.5f + MBRandom.RandomFloat * 0.5f);
            
            // and now for limb
            boneEntitialFrameWithIndex = victimSlicedLimb.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex((byte)boneHit);
            Vec3 limbGlobalPos = victimSlicedLimb.AgentVisuals.GetGlobalFrame().TransformToParent(boneEntitialFrameWithIndex.origin);
            victimSlicedLimb.CreateBloodBurstAtLimb(boneHit, ref limbGlobalPos, 0.5f + MBRandom.RandomFloat * 0.5f);
            */
        }
    }
}
