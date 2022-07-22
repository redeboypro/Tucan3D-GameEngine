using System.Data.SqlTypes;
using OpenGL_3D_GameEngine;
using Tucan3D_GameEngine.WorldObjects;
using Tucan3D_GameEngine.WorldObjects.Common;

namespace Tucan3D_GameEngine.GameComponents
{
    public abstract class IComponent : SceneExtension, INullable
    {
        public bool IsNull { get; }
        
        private WorldObject _worldObject;
        public WorldObject worldObject => worldObject;
        
        public void AssignWorldObject(WorldObject worldObject) => this._worldObject = worldObject;

        public virtual void Start(){}
        public virtual void Update(){}
    }
}