using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Renci.SshNet;

namespace TrySSH
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner();
            string cmd = BuildCommand();
            runner.Run("192.168.10.6", "root", "root", cmd,
                (result) =>
                {
                    if (result.ExitStatus != 0)
                    {
                        // Report error here
                    }
                    else
                    {
                        Console.WriteLine(result.Result); 
                    }
                });

            Console.WriteLine("Hit ENTER to continue");
            Console.ReadLine();
        }

        static string BuildCommand()
        {
            return "ipmitool -I serial -D /dev/ttyS1:115200 sdr";
        }
    }
}
