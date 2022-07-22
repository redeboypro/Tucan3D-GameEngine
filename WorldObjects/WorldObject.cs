using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using OpenGL_3D_GameEngine;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tucan3D_GameEngine.Core;
using Tucan3D_GameEngine.GameComponents;
using Tucan3D_GameEngine.Rendering;
using Tucan3D_GameEngine.WorldObjects.Common;

namespace Tucan3D_GameEngine.WorldObjects
{
    public class WorldObject : SceneExtension, INullable, IDisposable
    {
        private string name;
        private Vector3 position = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.One;
        private WorldObject parent;
        private List<IComponent> components = new List<IComponent>();
        private bool meshIsCollidable = false;
        private ModelData modelData;
        private TextureData textureData;
        private TextureData normalTextureData;
        private Triangle[] triangles;
        private List<WorldObject> children = new List<WorldObject>();

        public ModelData ModelData
        {
            get => modelData;
            set
            {
                modelData = value;

                triangles = new Triangle[modelData.Triangles.Length];
                RecalculateTriangles();
            }
        }

        private void RecalculateTriangles()
        {
            for (int i = 0; i < triangles.Length; i++)
            {
                Triangle triangle = modelData.Triangles[i];
                triangles[i] = new Triangle(
                    triangle.P1 * scale + position,
                    triangle.P2 * scale + position,
                    triangle.P3 * scale + position,
                    triangle.Normal);
            }
        }
        
        public TextureData BaseTextureData 
        {
            get => textureData;
            set => textureData = value;
        }
        
        public TextureData NormalTextureData 
        {
            get => normalTextureData;
            set => normalTextureData = value;
        }

        public WorldObject(string name)
        {
            this.name = name;
        }

        public bool HasCollidableMesh
        {
            get => meshIsCollidable;
            set => meshIsCollidable = value;
        }

        public string Name => name;

        public List<WorldObject> Children => children;

        public List<IComponent> GetComponents() => components;

        public T GetComponent<T>() where T : IComponent
        {
            foreach (var c in components)
            {
                if (c.GetType().IsAssignableFrom(typeof(T)))
                    return (T)c;
            }

            return null;
        }

        public void AddComponent<T>() where T : IComponent
        {
            var component = Activator.CreateInstance<T>();
            component.AssignWorldObject(this);
            components.Add(component);
        }

        public void AddComponent(IComponent component)
        {
            component.AssignWorldObject(this);
            components.Add(component);
        }
        
        public void OnLoad()
        {
            foreach (var c in components)
            {
                c.Start();
            }
        }

        public void OnUpdateFrame()
        {
            foreach (var c in components)
            {
                c.Update();
            }
        }
        
        public void OnRenderFrame(ShaderData shaderData)
        {
            MeshRenderer.Draw(shaderData, this);
        }

        public WorldObject Parent
        {
            get => parent;
            set
            {
                Parent?.Children.Remove(this);
                parent = value;
                parent?.Children.Add(this);
            }
        }

        public Matrix4 localTransformation =>  Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(position);
        
        public Matrix4 globalTransformation
        {
            get
            {
                if (Parent != null)
                    return localTransformation * Parent.globalTransformation;
                
                return localTransformation;
            }
        }

        public Vector3 localPosition
        {
            get => position;
            set
            {
                position = value;
                if(HasCollidableMesh)
                   RecalculateTriangles();
            }
        }
        
        public Quaternion localOrientation
        {
            get => rotation;
            set
            {
                rotation = value;
            }
        }
        
        public Vector3 localScale
        {
            get => scale;
            set
            {
                scale = value;
                if(HasCollidableMesh)
                   RecalculateTriangles();
            }
        }

        public Vector3 localEulerAngles
        {
            get => MathUtils.ToEulerAngles(rotation);
            set => rotation = Quaternion.FromEulerAngles(MathUtils.VectorAxesToRadians(value));
        }
        
        public Vector3 globalPosition => globalTransformation.ExtractTranslation();
        public Quaternion globalOrientation => globalTransformation.ExtractRotation();
        public Vector3 globalScale => globalTransformation.ExtractScale();
        public Vector3 globalEulerAngles => MathUtils.ToEulerAngles(globalTransformation.ExtractRotation());
        
        public Vector3 localForwardDirection => localOrientation * Vector3.UnitZ;
        public Vector3 localLeftDirection => localOrientation * Vector3.UnitX;
        public Vector3 localUpDirection => localOrientation * Vector3.UnitY;
        
        public Vector3 forwardDirection => globalOrientation * Vector3.UnitZ;
        public Vector3 leftDirection => globalOrientation * Vector3.UnitX;
        public Vector3 upDirection => globalOrientation * Vector3.UnitY;
        
        public bool IsNull { get; }

        public static void Destroy(WorldObject WObject)
        {
            foreach (var child in WObject.Children)
            {
                Destroy(child);
            }
            WObject.Children.Clear();
            
            Tree.Remove(WObject);
            WObject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}