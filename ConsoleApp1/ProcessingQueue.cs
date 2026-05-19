namespace ConsoleApp1;

public class ProcessingQueue(int capacity)
{
    private readonly MediaFile[] _files = new MediaFile[capacity];
    private int _count;
    private static readonly string[] SupportedExtensions = [".mp4", ".avi", ".mp3", ".wav"];

    public static bool CheckValidExtension(string ext)
    {
        if (string.IsNullOrEmpty(ext)) return false;

        var formattedExt = ext.ToLower().Trim();
        if (!formattedExt.StartsWith("."))
        {
            formattedExt = "." + formattedExt;
        }

        return SupportedExtensions.Any(supported => supported == formattedExt);
    }

    public static double EstimateSize(double currentSize)
    {
        var rand = new Random();
        var reductionPercent = rand.Next(10, 41) / 100.0;
        var estimated = currentSize * (1 - reductionPercent);
        return Math.Round(estimated, 2);
    }

    public bool AddFile(MediaFile file)
    {
        if (_count >= capacity)
        {
            Console.WriteLine("Hàng đợi đã đầy!");
            return false;
        }

        _files[_count] = file;
        _count++;
        return true;
    }

    public bool RemoveFile(int index)
    {
        if (index < 0 || index >= _count)
        {
            Console.WriteLine("Vị trí không hợp lệ!");
            return false;
        }

        for (var i = index; i < _count - 1; i++)
        {
            _files[i] = _files[i + 1];
        }
        _files[_count - 1] = null!;
        _count--;
        return true;
    }

    public void ShowFiles()
    {
        if (_count == 0)
        {
            Console.WriteLine("Hàng đợi trống!");
            return;
        }

        Console.WriteLine("Danh sách file trong hàng đợi:");
        for (var i = 0; i < _count; i++)
        {
            Console.Write($"{i + 1}. ");
            _files[i].PrintInfo();
            Console.WriteLine();
        }
    }

    public static void ShowFiles(MediaFile[] files, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Hàng đợi trống!");
            return;
        }

        Console.WriteLine("Danh sách file trong hàng đợi:");
        for (var i = 0; i < count; i++)
        {
            Console.Write($"{i + 1}. ");
            files[i].PrintInfo();
            Console.WriteLine();
        }
    }

    public MediaFile[] GetFiles()
    {
        var activeFiles = new MediaFile[_count];
        Array.Copy(_files, activeFiles, _count);
        return activeFiles;
    }

    public void Clear()
    {
        Array.Clear(_files, 0, capacity);
        _count = 0;
    }
}