using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Renci.SshNet;

namespace TrySSH
{
    public class Runner
    {
        public void Run(string hostName, string userName, string password, string cmd, Action<SshCommand> callback)
        {
            SshCommand result;
            int exitStatus = 0;
            using (SshClient ssh = new SshClient(hostName, userName, password))
            {
                ssh.Connect();
                result = ssh.RunCommand(cmd);
                ssh.Disconnect();
            }
            callback(result);
        }
    }
}
