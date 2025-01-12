namespace BlazeCanvas.Components
{
    public class DivPinComponent : ScriptComponent
    {
        public override string JsClassName() => "divPinComponent";
        public static object GetArgs() => new
        {
        };

        public async Task SetPinId(string pinId)
        {
            await InvokeJSAsync1("setPinId", pinId);
        }
    }
}
