using System.Windows.Forms;
using UI.Forms.Auth;

namespace UI
{
    internal static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                    Application.Run(new FrmAdministrarUsuarios());
            }
        }
    }
}
