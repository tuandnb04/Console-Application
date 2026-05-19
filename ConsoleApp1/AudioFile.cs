namespace ConsoleApp1;

public class AudioFile(string fileName, string extension, double fileSizeMb, string bitrate)
    : MediaFile(fileName, extension, fileSizeMb), ICompressible
{
    public static readonly string[] Bitrates = ["64kbps", "96kbps", "128kbps", "192kbps", "256kbps", "320kbps"];
    private string Bitrate { get; set; } = bitrate;

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.Write($" | Bitrate: {Bitrate}");
    }

    public void Compress()
    {
        Console.WriteLine("Đang nén audio...");
    }

    public override void ConvertFormat(string newExtension)
    {
        Console.WriteLine("Đang mix lại luồng âm thanh...");
    }
}