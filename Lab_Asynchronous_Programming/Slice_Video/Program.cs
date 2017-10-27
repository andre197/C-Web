namespace Slice_Video
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            string videoPath = Console.ReadLine();
            string destination = Console.ReadLine();
            int parts = int.Parse(Console.ReadLine());

            SliceAsync(videoPath, destination, parts);

            Console.WriteLine("Anything else?");

            while (true)
            {
                string something = Console.ReadLine();

                if (something == "end")
                {
                    break;
                }
            }
        }

        private static void SliceAsync(string sourceFile, string destinationPath, int parts)
        {
            Task.Run(() =>
            {
                Slice(sourceFile, destinationPath, parts);
            });

            Console.WriteLine("Slice completed");
        }

        private static void Slice(string sourceFile, string destinationPath, int parts)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (var file = new FileStream(sourceFile, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(sourceFile);

                long partLenght = (file.Length / parts) + 1;
                long currentByte = 0;

                for (int i = 1; i <= parts; i++)
                {
                    string filePath = $"{destinationPath}/video-part{i}{fileInfo.Extension}";

                    using (var destination = new FileStream(filePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[partLenght];

                        while (currentByte <= partLenght * i)
                        {
                            int readBytesCount = file.Read(buffer, 0, buffer.Length);

                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }
                    }
                }
            }
        }
    }
}
