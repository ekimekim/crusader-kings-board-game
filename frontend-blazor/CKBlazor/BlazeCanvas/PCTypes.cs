
namespace BlazeCanvas
{
    public static class PCTypes
    {
        public static object NewColor(float r, float g, float b) => JSTypes.NewJSType("pc.Color", [r, g, b]);
    }
}
