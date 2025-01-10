using Microsoft.JSInterop;

namespace BlazeCanvas
{
    public class Asset
    {
        public Session Session { get; private set; }
        public string Id { get; private set; }

        public enum AssetType
        {
            animation,
            audio,
            binary,
            bundle,
            container, // gltf
            cubemap,
            css,
            font,
            json,
            html,
            material,
            model,
            script,
            shader,
            sprite,
            template,
            text,
            texture,
            textureatlas,
        }

        private Asset()
        {
            // Please use LoadAsset()
        }

        public static async Task<Asset> LoadAsset(Session session, AssetType assetType, string url)
        {
            var nextId = session.IdGenerator.NextId();

            var error = await session.Js.InvokeAsync<string>("window.BC.loadAssetAsync", session.Id, nextId, assetType.ToString(), new { url });

            if (error != null)
            {
                throw new Exception("Failed to load: " + error);
            }

            var asset = new Asset();
            asset.Id = nextId;
            asset.Session = session;
            return asset;
        }
    }
}
