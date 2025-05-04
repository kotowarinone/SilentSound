using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace SilentSound
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 通常ファイルとして存在するパスを格納するリスト
            List<string> validFiles = new List<string>();

            foreach (string path in args)
            {
                try
                {
                    // ファイルが存在し、かつディレクトリではないことを確認
                    if (File.Exists(path) && !Directory.Exists(path))
                    {
                        validFiles.Add(path);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"エラー（{path}）: {ex.Message}");
                }
            }

            // 結果の出力
            // Console.WriteLine("見つかった通常ファイル:");
            // foreach (var file in validFiles)
            // {
            // Console.WriteLine(" - " + file);
            // }

            Console.WriteLine("play the file:" + validFiles[0]);
            SoundPlayer player = new SoundPlayer(validFiles[0]);
            player.Load();
            try {
                for (; ; )
                {
                    player.PlaySync();   // 同期再生
                    Thread.Sleep(60000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
                return;
            }
        }
    }
}

