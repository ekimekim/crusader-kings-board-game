#pragma warning disable IDE1006 // Naming Styles
namespace CKBlazor.CK.Data
{
    public class Player
    {
        public required User user { get; set; }
        public required string color { get; set; }
        public required bool hasCrusaded { get; set; }
        public required Card[] hand { get; set; }
        public required Card[] tech { get; set; }
        public required string[] traits { get; set; }
        public required Character ruler { get; set; }
        public required Character[] siblings { get; set; }
        public required Character[] children { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles