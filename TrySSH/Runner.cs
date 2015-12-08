using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySSH
{
    public class Runner
    {
        public string Run(string hostName, string userName, string password, string cmd)
        {
            string results = string.Empty;
            using (SshClient ssh = new SshClient(hostName, userName, password))
            {
                ssh.Connect();
                var result = ssh.RunCommand(cmd);
                if (result.ExitStatus == 0) results = result.Result;
                ssh.Disconnect();
            }
            return results;
        }
    }
}
