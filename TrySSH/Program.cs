using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySSH
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner();
            string results = runner.Run("192.168.10.19", "jjesus", "passw0rd", "ls");
            Console.WriteLine(results);
            Console.WriteLine("Hit ENTER to continue");
            Console.ReadLine();
        }
    }
}
