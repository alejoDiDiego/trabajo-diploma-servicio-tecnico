using System.Windows.Forms;
using PRESENTATION.Forms.Auth;

namespace PRESENTATION
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
