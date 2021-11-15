using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.Command
{


    public class ConsolePrint
    {
        public Task ConsoPrint(string TxT)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[Info] ");
            Console.ResetColor();
            Console.WriteLine(TxT);
            return Task.CompletedTask;
        }

        //public void StartConPt()
        //{

        //}
    }
}
