namespace ConsoleApp1
{
    internal static class Program
    {
        private static readonly ProcessingQueue Queue = new ProcessingQueue(10);

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("=== HỆ THỐNG QUẢN LÝ FILE MEDIA ===");
                Console.WriteLine("   1.  Thêm file vào hàng đợi");
                Console.WriteLine("   2.  Xem danh sách hàng đợi");
                Console.WriteLine("   3.  Bắt đầu xử lý (Nén & Chuyển đổi)");
                Console.WriteLine("   4.  Thoát chương trình");
                Console.Write("Chọn chức năng (1-4): ");

                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        Console.Write("\nNhập tên file: ");
                        var name = Console.ReadLine()?.Trim() ?? string.Empty;
                        if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine("Tên file không hợp lệ!\n");
                            break;
                        }

                        Console.Write("Nhập định dạng (.mp4, .avi, .mp3, .wav): ");
                        var ext = Console.ReadLine()?.Trim() ?? string.Empty;
                        if (!ProcessingQueue.CheckValidExtension(ext))
                        {
                            Console.WriteLine("Định dạng không được hỗ trợ!\n");
                            break;
                        }

                        Console.Write("Nhập dung lượng (MB): ");
                        if (!double.TryParse(Console.ReadLine(), out double size) || size <= 0)
                        {
                            Console.WriteLine("Dung lượng không hợp lệ!\n");
                            break;
                        }

                        ext = ext.ToLower();
                        if (!ext.StartsWith(".")) ext = "." + ext;

                        if (ext is ".mp4" or ".avi")
                        {
                            Console.Write("Nhập độ phân giải (240p, 360p, 480p, 720p, 1080p, 1440p, 4K): ");
                            var res = Console.ReadLine()?.Trim() ?? string.Empty;
                            var isValid = false;
                            foreach (var r in VideoFile.Resolutions)
                            {
                                if (!r.Equals(res, StringComparison.OrdinalIgnoreCase)) continue;
                                res = r;
                                isValid = true;
                                break;
                            }
                            if (!isValid)
                            {
                                Console.WriteLine("Độ phân giải không hỗ trợ!\n");
                                break;
                            }

                            var video = new VideoFile(name, ext, size, res);
                            if (Queue.AddFile(video))
                            {
                                Console.WriteLine("Thêm file video vào hàng đợi thành công!\n");
                            }
                        }
                        else
                        {
                            Console.Write("Nhập bitrate (64kbps, 96kbps, 128kbps, 192kbps, 256kbps, 320kbps): ");
                            var bitrate = Console.ReadLine()?.Trim() ?? string.Empty;
                            var isValid = false;
                            foreach (var b in AudioFile.Bitrates)
                            {
                                if (!b.Equals(bitrate, StringComparison.OrdinalIgnoreCase)) continue;
                                bitrate = b;
                                isValid = true;
                                break;
                            }
                            if (!isValid)
                            {
                                Console.WriteLine("Bitrate không hỗ trợ!\n");
                                break;
                            }

                            var audio = new AudioFile(name, ext, size, bitrate);
                            if (Queue.AddFile(audio))
                            {
                                Console.WriteLine("Thêm file audio vào hàng đợi thành công!");
                            }
                        }
                        break;

                    case "2":
                        Console.WriteLine();
                        var activeFiles = Queue.GetFiles();
                        ProcessingQueue.ShowFiles(activeFiles, activeFiles.Length);
                        Console.WriteLine();
                        break;

                    case "3":
                        var files = Queue.GetFiles();
                        if (files.Length == 0)
                        {
                            Console.WriteLine("Hàng đợi trống! Không có gì để xử lý.");
                            break;
                        }

                        Console.WriteLine("\nBắt đầu xử lý hàng đợi (Nén & Chuyển đổi)...");
                        for (var i = 0; i < files.Length; i++)
                        {
                            Console.WriteLine($"--- Đang xử lý file {i + 1}/{files.Length}: {files[i].FileName} ---");
                            files[i].PrintInfo();
                            Console.WriteLine();

                            if (files[i] is ICompressible compressible)
                            {
                                compressible.Compress();
                            }

                            var currentExt = files[i].Extension.ToLower();

                            var targetExt = files[i] switch
                            {
                                VideoFile => currentExt == ".mp4" ? ".avi" : ".mp4",
                                AudioFile => currentExt == ".mp3" ? ".wav" : ".mp3",
                                _ => currentExt
                            };

                            files[i].ConvertFormat(targetExt);

                            var newSize = ProcessingQueue.EstimateSize(files[i].FileSizeMb);
                            Console.WriteLine($"-> Dung lượng cũ: {files[i].FileSizeMb} MB | Dung lượng mới dự kiến: {newSize} MB");
                        }
                        Console.WriteLine("Đã xử lý xong tất cả các file trong hàng đợi!");
                        Queue.Clear();
                        break;

                    case "4":
                        Console.WriteLine("Cảm ơn bạn đã sử dụng chương trình. Tạm biệt!");
                        return;

                    default:
                        Console.WriteLine("Cảnh báo: Lựa chọn không hợp lệ! Vui lòng chỉ nhập số từ 1 đến 4.");
                        break;
                }
            }
        }
    }
}