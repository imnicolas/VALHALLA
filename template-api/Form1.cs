using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace template_api
{
    public partial class Form1 : Form
    {
        private AuthService _authService;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        public Form1()
        {
            InitializeComponent();
            _authService = new AuthService();
            SetupLoginForm();
        }

        private void SetupLoginForm()
        {
            // Main Form Settings
            this.Text = "VALHALLA - Control de Accesos";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            // Header Panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(41, 53, 65) // Dark blueish gray
            };

            lblTitle = new Label
            {
                Text = "VALHALLA",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(125, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Sistema de Gestión de Activos IT",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(110, 60)
            };

            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSubtitle);

            // Content Controls
            int contentStartX = 75;
            int currentY = 150;

            Label lblUser = new Label { Text = "Legajo:", Left = contentStartX, Top = currentY, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            currentY += 25;
            txtUsername = new TextBox { Left = contentStartX, Top = currentY, Width = 280, Font = new Font("Segoe UI", 12F) };

            currentY += 40;

            Label lblPass = new Label { Text = "Contraseña:", Left = contentStartX, Top = currentY, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            currentY += 25;
            txtPassword = new TextBox { Left = contentStartX, Top = currentY, Width = 280, PasswordChar = '•', Font = new Font("Segoe UI", 12F) };

            currentY += 60;

            btnLogin = new Button
            {
                Text = "INGRESAR",
                Left = contentStartX,
                Top = currentY,
                Width = 280,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204), // Bright Blue
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Handle Enter key for login
            this.AcceptButton = btnLogin;

            // Add Controls to Form
            this.Controls.Add(panelHeader);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = _authService.Authenticate(txtUsername.Text, txtPassword.Text);
                if (user != null)
                {
                    DashboardForm dashboard = new DashboardForm(user);
                    this.Hide();
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
