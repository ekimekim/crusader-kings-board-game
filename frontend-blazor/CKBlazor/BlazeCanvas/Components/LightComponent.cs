namespace BlazeCanvas.Components
{
    public class LightComponent : Component
    {
        public override string JsClassName() => "light";
        public static object GetLightArgs() => new
        {
            type = "directional",
            color = PCTypes.NewColor(1, 1, 1),
            intensity = 1
        };
    }
}
