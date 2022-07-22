namespace Tucan3D_GameEngine.Core
{
    public enum AssetType { Image, Model }
    public class AssetFile
    {
        private string fileName;
        private Asset asset;

        public string GetFileName() => fileName;
        public Asset GetAsset() => asset;

        private AssetType type;
        public AssetType Type => type;

        public AssetFile(string fileName, Asset asset, AssetType type)
        {
            this.fileName = fileName;
            this.asset = asset;
            this.asset.AssignFile(this);
            this.type = type;
        }
    }
    
    public abstract class Asset
    {
        private AssetFile assetFile;
        public AssetFile GetAssetFile() => assetFile;

        public void AssignFile(AssetFile assetFile)
        {
            this.assetFile = assetFile;
        }
    }

    public class AssetFormat
    {
        private string pattern;
        private AssetType type;

        public string Pattern => pattern;
        public AssetType Type => type;
        
        public AssetFormat(string pattern, AssetType type)
        {
            this.pattern = pattern;
            this.type = type;
        }
    }
}