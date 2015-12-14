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

                // Create an SDR cache
                // 
                "ipmitool -I serial -D /dev/ttyS1:115200 sdr dump log.sdr",

                // The command below can be used to build map of sensors to
                // boards and oids.  The Entity ID field maps to a board.
                // The ID number is unique and can be manually mapped
                // to a sensor name.
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr sdr elist",

                // The -c gives CSV output, but, does not include ID numbers
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr sdr type temperature",
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr sdr type current",
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr sdr type voltage",

                /**
                The -c gives CSV output, but, does not include ID numbers
                But, if using the Entity ID in the query, then, -c is OK
                Entity Numbers are from Appendix H of the SFM6102 manual,
                which also lists the IPMB addresses:

                Slot    | Module FRU Number | Entity ID | IPMB Address
                1       |   5               | 193.97    | 130 0x82
                2       |   6               | 193.98    | 132 0x84
                3       |   7               | 193.99    | 134 0x86
                4       |   8               | 193.100   | 136 0x88
                5       |   9               | 193.101   | 138 0x8A
                6       |   10              | 193.102   | 140 0x8C
                **/

                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr -c sdr entity 193.100",
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr sdr elist mcloc",
                "ipmitool -I serial -D /dev/ttyS1:115200 -m 0x82 -t 0x20 -S log.sdr sel elist",
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr mc info",
                "ipmitool -I serial -D /dev/ttyS1:115200 -S log.sdr fru print",
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
