using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using NAudio.Wave;


namespace SilentSound
{
    internal class Program
    {

        static int SelectDevice()
        {
            var devicenumber=0;

            // 出力デバイス一覧を表示して、該当の文字列のデバイスを見つける。
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var capabilities = WaveOut.GetCapabilities(i);
                Console.WriteLine($"{i}: {capabilities.ProductName}");
                if (capabilities.ProductName.Contains("Realtek(R) Audio"))
                {
                    devicenumber = i;
                }
                
            }

            return devicenumber;
        }

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
            Console.WriteLine("play the file:" + validFiles[0]);

            // 結果の出力
            // Console.WriteLine("見つかった通常ファイル:");

            try
            {
                using (var audioFile = new AudioFileReader(validFiles[0]) )
                using (var outputDevice = new WaveOutEvent())
                {
                    // デバイス番号を指定（ここでは0を明示）
                    outputDevice.DeviceNumber = SelectDevice();

                    // 再生用に初期化
                    outputDevice.Init(audioFile);

                    // 再生
                    outputDevice.Play();

                    // 再生が終了するまで待機
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(200);
                    }

                    Console.WriteLine("再生が完了しました。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }


        }
    }
}

