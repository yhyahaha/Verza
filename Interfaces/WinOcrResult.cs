namespace Interfaces
{
    public struct WinOcrResult
    {
        public string Words { get; set; }
        public double RectLeft { get; set; }
        public double RectTop { get; set; }
        public double RectWidth { get; set; }
        public double RectHeight { get; set; }
    }
}
