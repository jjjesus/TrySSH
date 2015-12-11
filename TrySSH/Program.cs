using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Renci.SshNet;
using System.IO;

namespace TrySSH
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputDir = "d:\\tmp";

            Runner runner = new Runner() {
                HostName = "192.168.10.6", 
                UserName = "root",
                Password = "root" };
            
            List<string> cmds = BuildCommands();

            foreach (var cmd in cmds)
            {
                runner.Run(cmd,
                    (result) =>
                    {
                        if (result.ExitStatus != 0)
                        {
                            // Report error here
                        }
                        else
                        {
                            string logFileName = createLogFilename(outputDir);

                            string preamble = createPreamble(cmd);
                            Console.WriteLine(preamble + result.Result);
                            File.AppendAllText(logFileName, preamble + result.Result);
                        }
                    });
            }

            Console.WriteLine("Hit ENTER to continue");
            Console.ReadLine();
        }

        private static string createPreamble(string cmd)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=====");
            sb.AppendLine("\t" + cmd);
            sb.AppendLine("=====");
            return sb.ToString();
        }

        private static List<string> BuildCommands()
        {
            return new List<string>()
            {
                //"ipmitool -I lan -H 192.168.10.7 -A none sdr elist mcloc",
                // "ipmitool -I lan -H 192.168.10.7 -A none sel elist",

                "ipmitool -I serial -D /dev/ttyS1:115200 sdr elist mcloc",
                "ipmitool -I serial -D /dev/ttyS1:115200 -m 0x82 -t 0x20 sel list",
                "ipmitool -I serial -D /dev/ttyS1:115200 mc info",
                "ipmitool -I serial -D /dev/ttyS1:115200 sdr",
                "ipmitool -I serial -D /dev/ttyS1:115200 fru",
                "ipmitool -I serial -D /dev/ttyS1:115200 sensor",
            };
        }

        private static string createLogFilename(string outputDir)
        {
            StringBuilder sb = new StringBuilder(outputDir + Path.DirectorySeparatorChar);
            sb.Append("ipmi.log".InsertTimeStamp());
            return sb.ToString();
        }
    }
}
