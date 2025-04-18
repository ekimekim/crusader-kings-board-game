namespace CKBlazor.CK.Screens
{
    public interface IScreen : IDisposable
    {
        public ScreenType ScreenType { get; }
    }
}
