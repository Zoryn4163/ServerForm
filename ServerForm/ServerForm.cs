using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerForm
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        
        ProcessStartInfo psi = new ProcessStartInfo();
        Process server;
        StreamWriter inputWriter;
        StreamReader outputReader;
        //StreamReader errorReader;

        Thread ServerThread;
        char[] outBuffer = new char[1024];
        char[] errorBuffer = new char[1024];
        char[] baseBuffer = new char[1024];
        ListEventable<String> PrevCommands;
        int PrevNum = 0;
        int MaxToRemember = 1024;

        String EXEDir = Path.GetDirectoryName((System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:///", ""));
        String ConfigLoc;
        Byte[] BaseSettings;
        String[] Settings;

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            ConfigLoc = EXEDir + @"\ServerForm.cfg";

            BaseSettings = Encoding.ASCII.GetBytes("<FileName: C:\\File.exe>\r\n<Arguments: -fake=true>\r\n<WorkingDirectory: C:\\Windows\\>\r\n<NoWindow(True/False): True>\r\n<WindowTitle: Server Window>\r\n<MaximumCommandsRemembered: 1024>\r\n[Replace the above lines with what is stated (the colons provide an example). This line and all after will be ignored!]");

            if (!File.Exists(ConfigLoc))
            {
                FileStream fs = File.Create(ConfigLoc);
                fs.Write(BaseSettings, 0, BaseSettings.Length);
                fs.Flush();
                fs.Close();
                MessageBox.Show("Config file generated. Please fill it with the information requested.", "Closing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Process.Start(ConfigLoc);
                Environment.Exit(2);
            }
            Settings = File.ReadAllLines(ConfigLoc);

            txtbxConsole.Multiline = false;
            PrevCommands = new ListEventable<String>();
            PrevCommands.OnAdd += PrevCommands_OnAdd;
            MaxToRemember = Int32.Parse(Settings[5]);

            this.Text = Settings[4];
            notifyTray.Text = Settings[4];
            notifyTray.Visible = true;
            //notifyTray.ShowBalloonTip(0);
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add(new MenuItem("Hide"));
            cm.MenuItems[0].Click += ServerForm_ShowHideClick;
            cm.MenuItems.Add(new MenuItem("Open Program Dir"));
            cm.MenuItems[1].Click += ServerForm_OpenDirClick;
            cm.MenuItems.Add(new MenuItem("Open Process Dir"));
            cm.MenuItems[2].Click += ServerForm_OpenPrcClick;
            cm.MenuItems.Add(new MenuItem("Exit"));
            cm.MenuItems[3].Click += ServerForm_ExitClick;
            notifyTray.ContextMenu = cm;

            ServerThread = new Thread(new ThreadStart(ServerWork));
            ServerThread.Start();
            txtbxConsole.Select();
        }

        void PrevCommands_OnAdd(object sender, EventArgs e)
        {
            if (PrevCommands.Count > MaxToRemember)
                PrevCommands.RemoveRange(0, PrevCommands.Count - MaxToRemember);
        }

        void ServerForm_ExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        void ServerForm_ShowHideClick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
                notifyTray.ContextMenu.MenuItems[0].Text = "Show";
            }
            else
            {
                this.Show();
                notifyTray.ContextMenu.MenuItems[0].Text = "Hide";
            }
        }

        void ServerForm_OpenDirClick(object sender, EventArgs e)
        {
            Process.Start(EXEDir);
        }
        void ServerForm_OpenPrcClick(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(psi.FileName));
        }

        void ServerWork()
        {
            try
            {
                //psi.FileName = @"C:\Program Files\Java\jdk1.7.0_71\bin\java";
                psi.FileName = Settings[0];
                string name = Path.GetFileNameWithoutExtension(psi.FileName);
                Process[] Similar = Process.GetProcessesByName(name);
                if (Similar.Count() != 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Process p in Similar)
                        sb.AppendLine(String.Format("[{0}]{1} - {2}: {3}", p.StartTime.ToLocalTime(), p.ProcessName, p.Id, p.StartInfo.Arguments));
                    DialogResult dr = MessageBox.Show(String.Format("Found {0} similar processes by the name of '{1}'.\r\nKill them?\r\n{2}", Similar.Count(), name, sb.ToString()), "Similar Processes Running", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (dr == System.Windows.Forms.DialogResult.Yes)
                        for (int i = 0; i < Similar.Count(); i++)
                            Similar[i].Kill();
                }

                //psi.Arguments = @"-server -Xms1024m -Xmx3072m -XX:PermSize=256m -XX:+UseParNewGC -XX:+UseConcMarkSweepGC -jar forge-1.7.10-10.13.3.1388-1.7.10-universal.jar nogui";
                psi.Arguments = Settings[1];
                //psi.WorkingDirectory = @"E:\Users\Zoryn\AppData\Roaming\.technic\#Servers\Forge";
                psi.WorkingDirectory = Settings[2];
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                psi.ErrorDialog = false;
                psi.RedirectStandardError = true;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                //psi.CreateNoWindow = true;
                psi.CreateNoWindow = Boolean.Parse(Settings[3]);

                server = Process.Start(psi);
                server.Refresh();

                inputWriter = server.StandardInput;
                outputReader = server.StandardOutput;
                //errorReader = server.StandardError;

                inputWriter.AutoFlush = true;

                while (true)
                {
                    if (server.HasExited)
                        Environment.Exit(-1);

                    try
                    {
                        outBuffer = new char[1024];
                        outputReader.Read(outBuffer, 0, outBuffer.Length);
                        if (outBuffer != baseBuffer)
                            txtbxLog.AppendText(new String(outBuffer));
                    }
                    catch { }

                    txtbxLog.SelectionStart = txtbxLog.Text.Length;
                    txtbxLog.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                this.Enabled = false;
                MessageBox.Show("This is most-likely due to a config issue.\r\n\r\n" + ex.ToString(), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Kill();
            server.WaitForExit();
            ServerThread.Abort();
        }

        private void txtbxConsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                char[] command = txtbxConsole.Text.ToCharArray();
                if (PrevCommands.Count() > 0)
                {
                    if ((!String.IsNullOrEmpty(new String(command)) && !String.IsNullOrWhiteSpace(new String(command))) && PrevCommands.Last() != new String(command))
                        PrevCommands.Add(new String(command));
                }
                else
                    PrevCommands.Add(new String(command));
                PrevNum = PrevCommands.Count;
                txtbxConsole.Clear();
                inputWriter.WriteLine(command);
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (PrevCommands.Count() > 0)
                {
                    GetPrevNum(0);
                    txtbxConsole.Text = PrevCommands[PrevNum];
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (PrevCommands.Count() > 0)
                {
                    GetPrevNum(1);
                    if (PrevNum == -4)
                    {
                        txtbxConsole.Clear();
                        PrevNum = PrevCommands.Count;
                    }
                    else
                        txtbxConsole.Text = PrevCommands[PrevNum];
                }
            }
        }

        void GetPrevNum(int pm)
        {
            if (pm == 0)
                PrevNum = Clamp(PrevNum - 1, 0, PrevCommands.Count - 1);
            else if (pm == 1)
                if (PrevNum >= PrevCommands.Count - 1)
                    PrevNum = -4;
                else
                PrevNum = Clamp(PrevNum + 1, 0, PrevCommands.Count - 1);
            else
                return;
        }

        private void txtbxConsole_Enter(object sender, EventArgs e)
        {
            txtbxConsole.ReadOnly = false;
        }

        private void txtbxConsole_Leave(object sender, EventArgs e)
        {
            txtbxConsole.ReadOnly = true;
        }

        Int32 Clamp(Int32 val, Int32 min, Int32 max)
        {
            if (val < min)
                val = min;
            else if (val > max)
                val = max;
            return val;
        }
    }
}