namespace BlazeCanvas.Components
{
    public class ModelComponent : Component
    {
        public override string JsClassName() => "model";
        public static object GetBoxArgs() => new { type = "box" };
    }
}
