namespace BlazeCanvas.Components
{
    public class LightComponent : BasicComponent
    {
        public override string JsClassName() => "light";
        public static object GetLightArgs(float intensity = 1) => new
        {
            type = "directional",
            color = PCTypes.NewColor(1, 1, 1),
            intensity = intensity
        };
    }
}
