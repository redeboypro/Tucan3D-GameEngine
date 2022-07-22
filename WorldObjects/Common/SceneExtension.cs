using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OpenTK;
using Tucan3D_GameEngine.GameComponents;
using Tucan3D_GameEngine.Utils;

namespace Tucan3D_GameEngine.WorldObjects.Common
{
    public class SceneExtension
    {
        private const string DEFAULT_GAME_OBJECT_PREFIX = "GameObject";
        
        private static List<WorldObject> sceneTree = new List<WorldObject>();
        public static List<WorldObject> Tree => sceneTree;
        
        private static string GetInstanceDefaultName(string stName = DEFAULT_GAME_OBJECT_PREFIX)
        {
            var nameToApply = stName;
            if (Find(nameToApply) != null)
            {
                if (!NameFormatExist(nameToApply))
                {
                    nameToApply += CloseString(1.ToString());
                    nameToApply = GetInstanceDefaultName(nameToApply);
                }
                else
                {
                    var valueStr = StringUtils.GetBetweenStrings(nameToApply, "(", ")");
                    nameToApply = nameToApply.Replace(CloseString(valueStr),
                        CloseString((int.Parse(valueStr) + 1).ToString()));
                    nameToApply = GetInstanceDefaultName(nameToApply);
                }
            }

            return nameToApply;
        }

        private static string CloseString(string s) => $"({s})";

        private static bool NameFormatExist(string name)
        {
            if (name.Contains("(") &&
                name.Contains(")"))
                return StringUtils.GetBetweenStrings
                        (name, "(", ")")
                    .Any(char.IsNumber);

            return false;
        }

        public static WorldObject Instantiate(Vector3 position, Vector3 eulerAngles, Vector3 scale,
            string name = DEFAULT_GAME_OBJECT_PREFIX, IComponent[] behaviours = null)
        {
            var entity = new WorldObject(GetInstanceDefaultName(name));

            entity.localPosition = position;
            entity.localEulerAngles = eulerAngles;
            entity.localScale = scale;

            if (behaviours != null)
                foreach (var c in behaviours)
                    entity.AddComponent(c);
            
            sceneTree.Add(entity);
            return entity;
        }

        public static WorldObject Instantiate() =>
            Instantiate(Vector3.Zero, Vector3.Zero, Vector3.One, DEFAULT_GAME_OBJECT_PREFIX);

        public static WorldObject Find(string name)
        {
            foreach (var WO in sceneTree)
            {
                if (WO.Name == name) return WO;
            }

            return null;
        }
        
        public static WorldObject Find<T>() where T : IComponent
        {
            foreach (var WO in sceneTree)
            {
                if (WO.GetComponent<T>() != null) return WO;
            }

            return null;
        }
        
        public static List<WorldObject> FindMultiple<T>() where T : IComponent
        {
            List<WorldObject> tmp = new List<WorldObject>();
            foreach (var WO in sceneTree)
            {
                if (WO.GetComponent<T>() != null) tmp.Add(WO);
            }

            return tmp;
        }

        public static void OnLoad()
        {
            foreach (var WO in sceneTree)
            {
                WO.OnLoad();
            }
        }
        
        public static void OnUpdateFrame()
        {
            foreach (var WO in sceneTree)
            {
                WO.OnUpdateFrame();
            }
        }
        
        public static void OnRenderFrame(ShaderData shaderData)
        {
            foreach (var WO in sceneTree)
            {
                WO.OnRenderFrame(shaderData);
            }
        }
    }
}