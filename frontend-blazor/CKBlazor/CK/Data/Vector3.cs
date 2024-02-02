#pragma warning disable IDE1006 // Naming Styles
namespace CKBlazor.CK.Data
{
    public class Vector3
    {
        public required float x { get; set; }
        public required float y { get; set; }
        public required float z { get; set; }
        
        public string ToAFramePosition()
        {
            return $"{x} {y} {z}";
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles