using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.Command
{


    public class ConsolePrint
    {
        static ConcurrentQueue<string> ConsoPrintQueue = new ConcurrentQueue<string>();
        string retValue;
        public void ConsoPrint(string TxT)
        {
            ConsoPrintQueue.Enqueue(TxT);

            foreach (string queue in ConsoPrintQueue)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}][Info] ");
                Console.ResetColor();
                Console.WriteLine(queue.ToString());
                ConsoPrintQueue.TryDequeue(out retValue);
            }
            //return Task.CompletedTask;
        }

        //public void StartConPt()
        //{

        //}
    }
}
