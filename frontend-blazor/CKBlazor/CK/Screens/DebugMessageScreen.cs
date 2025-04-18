namespace CKBlazor.CK.Screens
{
    public class DebugMessageScreen : IScreen
    {
        public ScreenType ScreenType => ScreenType.DebugMessage;
        public string Message { get; }

        public DebugMessageScreen(string message) 
        {
            Message = message;
        }

        public void Dispose()
        {
        }
    }
}
