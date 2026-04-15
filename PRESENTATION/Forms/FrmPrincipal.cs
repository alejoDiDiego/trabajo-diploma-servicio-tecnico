using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Interfaces;
using PRESENTATION.Forms.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CROSSCUTTING.Configuration;

namespace PRESENTATION.Forms
{
    public partial class FrmPrincipal : Form
    {
        public static List<UsuarioDTO> usuariosList = new List<UsuarioDTO>();

        private int childFormNumber = 0;

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();
                if (sesion.ObtenerUsuarioActual() != null)
                {
                    MessageBox.Show($"Ya hay un usuario autenticado: {sesion.ObtenerUsuarioActual().Username}. Por favor, cierre sesión antes de iniciar una nueva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FrmLogin loginForm = new FrmLogin();
                loginForm.MdiParent = this;
                loginForm.Show();

            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void verCuentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();

                if (sesion.ObtenerUsuarioActual() == null)
                {
                    MessageBox.Show("No hay un usuario autenticado. Por favor, inicie sesión primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FrmCuenta cuentaForm = new FrmCuenta();
                cuentaForm.MdiParent = this;
                cuentaForm.Show();
            } 
            catch (Exception ex) 
            {
                MessageBox.Show($"Error al mostrar la cuenta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();
                sesion.Logout();
                MessageBox.Show("¡Sesión cerrada exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
