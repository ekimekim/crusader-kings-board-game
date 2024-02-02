#pragma warning disable IDE1006 // Naming Styles
namespace CKBlazor.CK.Data
{
    public class Character
    {
        public required int gender { get; set; }
        public required int number { get; set; }
        public required string name { get; set; }
        public Character? spouse { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles
