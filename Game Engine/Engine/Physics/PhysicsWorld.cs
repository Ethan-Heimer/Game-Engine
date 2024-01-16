using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics
{
    
    public class PhysicsWorld
    {
        List<RigidBody> rigidBodies = new List<RigidBody>();

        public static void AddRigidBody(RigidBody body) => SceneManager.currentScene?.PhysicsWorld.rigidBodies?.Add(body);
        public static void RemoveRigidBody(RigidBody body) => SceneManager.currentScene?.PhysicsWorld.rigidBodies?.Remove(body);

        public static RigidBody[] GetRigidBodies() => SceneManager.currentScene.PhysicsWorld.rigidBodies.ToArray(); 
    }
}
