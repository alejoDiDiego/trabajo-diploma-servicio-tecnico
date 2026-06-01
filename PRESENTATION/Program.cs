using System.Windows.Forms;
using UI.Forms;

namespace UI
{
    internal static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FrmPrincipal());
        }
    }
}
