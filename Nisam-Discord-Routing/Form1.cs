using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Diagnostics;

namespace Nisam_Discord_Routing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void FlowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FlowLayoutPanel2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FlowLayoutPanel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("درحال ست کردن روتینگ", "نیسم تیم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                // Ensure the application is running with administrative privileges
                if (!IsRunAsAdmin())
                {
                    // Restart the application with administrative privileges
                    MessageBox.Show("برنامه باید بصورت ادمین اجرا شود!", "نیسم تیم", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RestartAsAdmin();
                }
                else
                {
                    ModifyHostsFile();
                    MessageBox.Show("تنظیمات روتینگ انجام شد", "نیسم تیم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ModifyHostsFile()
        {
            string hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
            string newEntry = "162.159.134.234 gateway.discord.gg";

            var lines = File.ReadAllLines(hostsPath).ToList();

            if (lines.Any(line => line.Contains(newEntry)))
            {
                // Remove existing entry
                lines = lines.Where(line => !line.Contains(newEntry)).ToList();
                MessageBox.Show("تنظیمات روتینگ دیسکورد شما به حالت اولیه بازگشت!", "نیسم تیم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Add new entry
                lines.Add(newEntry);
                MessageBox.Show("روتینگ دیسکورد با موفقیت انجام شد", "نیسم تیم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            File.WriteAllLines(hostsPath, lines);
        }

        private bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RestartAsAdmin()
        {
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo startInfo = new ProcessStartInfo(exeName)
            {
                Verb = "runas"
            };
            Process.Start(startInfo);
            Application.Exit();
        }
    }
}
