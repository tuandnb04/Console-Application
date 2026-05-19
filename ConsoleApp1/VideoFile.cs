namespace ConsoleApp1;

public class VideoFile(string fileName, string extension, double fileSizeMb, string resolution)
    : MediaFile(fileName, extension, fileSizeMb), ICompressible
{
    public static readonly string[] Resolutions = ["240p", "360p", "480p", "720p", "1080p", "1440p", "4K"];
    private string Resolution { get; set; } = resolution;

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.Write($" | Độ phân giải: {Resolution}");
    }

    public void Compress()
    {
        Console.WriteLine("Đang nén video...");
    }

    public override void ConvertFormat(string newExtension)
    {
        Console.WriteLine("Đang re-render video...");
    }
}