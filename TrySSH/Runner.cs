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
        public string HostName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public void Run(string cmd, Action<SshCommand> callback)
        {
            SshCommand result;
            using (SshClient ssh = new SshClient(HostName, UserName, Password))
            {
                ssh.Connect();
                result = ssh.RunCommand(cmd);
                ssh.Disconnect();
            }
            callback(result);
        }
    }
}
