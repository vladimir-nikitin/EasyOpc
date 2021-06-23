using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EasyOpc.Common.Helpers
{
    public class BufferFile : IDisposable
    {
        private CancellationTokenSource CancellationTokenSource { get; }

        private Queue<string> Buffer { get; }

        public string Path { get; }

        public BufferFile(string path)
        {
            Path = path;
            Buffer = new Queue<string>();

            CancellationTokenSource = new CancellationTokenSource();
            var token = CancellationTokenSource.Token;

            Task.Run(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    await Task.Delay(4000);

                    Write();
                }

                Write();
            });
        }

        public void WriteLine(string line)
        {
            lock (Buffer)
            {
                Buffer.Enqueue(line);
            }
        }

        public void Dispose()
        {
            try
            {
                CancellationTokenSource?.Cancel();
            }
            catch { }
        }

        private void Write()
        {
            if (Buffer.Count > 0)
            {
                lock (Buffer)
                {
                    if (Buffer.Count > 0)
                    {
                        try
                        {
                            File.AppendAllLines(Path, Buffer);
                            Buffer.Clear();
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
