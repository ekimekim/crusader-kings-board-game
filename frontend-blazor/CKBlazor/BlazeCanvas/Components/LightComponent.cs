namespace BlazeCanvas.Components
{
    public class LightComponent : Component
    {
        public override string JsClassName() => "light";
        public static object GetLightArgs() => new
        {
            type = "directional",
            color = new
            {
                _type = "pc.Color",
                _args = new object[] { 1, 1, 1 },
            },
            intensity = 1
        };
    }
}
