namespace BlazeCanvas.Components
{
    public class CameraComponent : BasicComponent
    {
        public override string JsClassName() => "camera";
        public static object GetArgs(float clearColorR = 0.1f, float clearColorG = 0.1f, float clearColorB = 0.1f) => new
        {
            clearColor = PCTypes.NewColor(clearColorR, clearColorG, clearColorB),
        };
    }
}
