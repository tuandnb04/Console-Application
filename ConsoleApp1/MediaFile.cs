namespace ConsoleApp1;

public abstract class MediaFile(string fileName, string extension, double fileSizeMb)
{
    public string FileName { get; set; } = fileName;
    public string Extension { get; set; } = extension;
    public double FileSizeMb { get; set; } = fileSizeMb;

    public virtual void PrintInfo()
    {
        Console.Write($"Tên file: {FileName} | Định dạng: {Extension} | Dung lượng: {FileSizeMb} MB");
    }

    public abstract void ConvertFormat(string newExtension);
}