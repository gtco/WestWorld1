using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WestWorld1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Miner m = new Miner(1);

            for (int i = 0; i < 150; i++)
            {
                m.Update();
            }

            Console.ReadKey();
        }
    }
}
