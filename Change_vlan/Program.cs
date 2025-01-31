using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Change_vlan
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            if (!IsRunningAsAdministrator())
            {
                Console.WriteLine("L'application n'est PAS lancée en tant qu'administrateur.");
                MessageBox.Show("L'application n'est PAS lancée en tant qu'administrateur.");
                Application.Exit();
                return;
            }

            Application.Run(new VlanChangerApp());

            bool IsRunningAsAdministrator()
            {
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }

            }

        }
    }
}
