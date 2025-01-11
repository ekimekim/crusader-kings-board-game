namespace BlazeCanvas.Components
{
    public class CameraComponent : Component
    {
        public override string JsClassName() => "camera";
        public static object GetArgs(float clearColorR = 0.1f, float clearColorG = 0.1f, float clearColorB = 0.1f) => new
        {
            clearColor = PCTypes.NewColor(clearColorR, 0.1f, 0.1f),
        };
    }
}
