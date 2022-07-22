using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenGL_3D_GameEngine;
using Tucan3D_GameEngine.Rendering;

namespace Tucan3D_GameEngine.Core
{
    public static class AssetManager
    {
        private static AssetFormat[] assetFormats = { 
            new AssetFormat("*.obj", AssetType.Model),
            new AssetFormat("*.gltf", AssetType.Model),
            new AssetFormat("*.dae", AssetType.Model),
            new AssetFormat("*.fbx", AssetType.Model),
            new AssetFormat("*.png", AssetType.Image),
            new AssetFormat("*.jpg", AssetType.Image)
        };
        
        private static List<AssetFile> assetFiles = new List<AssetFile>();
        public static List<AssetFile> AssetFiles => assetFiles;

        private static TextureData debugTextureData;
        public static TextureData DebugTexture => debugTextureData;

        public static Asset FindAsset(string fileName)
        {
            foreach (var assetFile in assetFiles)
            {
                if (assetFile.GetFileName() == fileName)
                    return assetFile.GetAsset();
            }

            return null;
        }

        public static void InitializeAssets(string directory)
        {
            debugTextureData = new TextureData();
            
            foreach (var format in assetFormats)
            {
                var assets = Directory.GetFiles(directory, format.Pattern);

                foreach (var assetFile in assets)
                {
                    var shortFileName = assetFile.Split('\\').Last();
                        
                    switch (format.Type)
                    {
                        case AssetType.Model :
                            assetFiles.Add(new AssetFile(shortFileName, Importer.LoadFromFile(assetFile), format.Type));
                            break;
                        case AssetType.Image :
                            assetFiles.Add(new AssetFile(shortFileName, new TextureData(assetFile), format.Type));
                            break;
                    }
                }
            }
        }
    }
}