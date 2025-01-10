namespace BlazeCanvas
{
    public static class JSTypes
    {
        public static object NewJSAssetRef(Asset asset) => new { _asset = asset.Id };
        public static object NewJSMethod(string method, object[] args) => new { _method = method, _args = args };
        public static object NewJSType(string type, object[] args) => new { _type = type, _args = args };
    }
}
