namespace BlazeCanvas.Components
{
    public class ModelComponent : Component
    {
        public override string JsClassName() => "model";
        public static object GetBoxArgs() => new { type = "box" };
        public static object GetAssetArgs(Asset asset) => new
        {
            type = "asset",
            asset = JSTypes.NewJSAssetRef(asset),
        };
    }
}
