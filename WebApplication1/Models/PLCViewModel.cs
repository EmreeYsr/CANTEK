namespace WebApplication1.Models
{
    public class PLCViewModel
    {
        public string Durum { get; set; } = "";

        // MW5 ve MW6'ten okunan veri
        public string MW5Data { get; set; } // MW5'ten okunan veri
        public string MW6Data { get; set; } // MW6'ten okunan veri
    }
}
