using System;
using System.Drawing;
using System.Windows.Forms;
using ENTITY;

namespace template_api
{
    public partial class DashboardForm : Form
    {
        private User _currentUser;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Label lblWelcome;

        public DashboardForm(User user)
        {
            _currentUser = user;
            InitializeComponent();
            SetupDashboard();
            LoadRoleBasedMenu();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 600);
            this.Name = "DashboardForm";
            this.Text = "VALHALLA - Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        private void SetupDashboard()
        {
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(240, 240, 240); // Light gray background

            // Sidebar setup
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(41, 53, 65) // Dark blueish gray
            };

            // Logo area in sidebar
            Panel logoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(31, 43, 55)
            };

            Label lblLogo = new Label
            {
                Text = "VALHALLA",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            logoPanel.Controls.Add(lblLogo);
            sidebarPanel.Controls.Add(logoPanel);

            // Content Panel setup
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Top Header in Content Panel
            Panel headerContentPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            // A simple bottom border for the header
            Panel headerBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.LightGray };
            headerContentPanel.Controls.Add(headerBorder);

            string roleName = GetRoleName(_currentUser.IdRol);
            lblWelcome = new Label
            {
                Text = $"Bienvenido, {_currentUser.Email} | Rol: {roleName}",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(20, 18),
                ForeColor = Color.FromArgb(64, 64, 64)
            };
            headerContentPanel.Controls.Add(lblWelcome);

            contentPanel.Controls.Add(headerContentPanel);

            // Add main panels to form
            this.Controls.Add(contentPanel);
            this.Controls.Add(sidebarPanel); // Added after contentPanel so it docks correctly
        }

        private string GetRoleName(int idRol)
        {
            switch (idRol)
            {
                case 1: return "Desarrollador";
                case 2: return "Administrador IT";
                case 3: return "Líder Técnico";
                default: return "Desconocido";
            }
        }

        private void LoadRoleBasedMenu()
        {
            // We stack buttons from Top to Bottom, so we bring them to front in reverse order or use DockStyle.Top.
            // When using DockStyle.Top, the last added control appears at the bottom.
            // To maintain order, we can add them to a FlowLayoutPanel, or just standard Panel with Dock=Top.
            
            // Logout button is common and stays at the bottom
            Button btnLogout = CreateMenuButton("Cerrar Sesión");
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.Click += (s, e) => this.Close();
            sidebarPanel.Controls.Add(btnLogout);

            // 1 = Desarrollador
            if (_currentUser.IdRol == 1)
            {
                AddMenuButton("Restaurar Entorno", Menu_RestaurarEntorno_Click);
                AddMenuButton("Mis Secretos", Menu_MisSecretos_Click);
            }
            // 2 = Administrador IT
            else if (_currentUser.IdRol == 2)
            {
                AddMenuButton("Inventario de Activos", Menu_Inventario_Click);
                AddMenuButton("Asignar Hardware", Menu_AsignarHardware_Click);
            }
            // 3 = Lider Tecnico
            else if (_currentUser.IdRol == 3)
            {
                AddMenuButton("Definir Stack Tecnológico", Menu_DefinirStack_Click);
            }
        }

        private void AddMenuButton(string text, EventHandler clickHandler)
        {
            Button btn = CreateMenuButton(text);
            btn.Dock = DockStyle.Top;
            btn.Click += clickHandler;
            
            // Adding to Sidebar. Because of Dock.Top, we use BringToFront so it stays below the Logo panel
            sidebarPanel.Controls.Add(btn);
            btn.BringToFront();
        }

        private Button CreateMenuButton(string text)
        {
            Button btn = new Button
            {
                Text = "  " + text,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.LightGray,
                BackColor = Color.FromArgb(41, 53, 65),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 43, 55);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 122, 204);
            return btn;
        }

        // --- Click Event Handlers for Menu ---

        private void Menu_RestaurarEntorno_Click(object sender, EventArgs e)
        {
            ShowContent("Restaurar Entorno", "Aquí se implementará el Caso de Uso: CU-01 Restaurar Entorno de Desarrollo.");
        }

        private void Menu_MisSecretos_Click(object sender, EventArgs e)
        {
            ShowContent("Gestión de Secretos", "Aquí se implementará el Caso de Uso: CU-04 Gestionar Secretos Personales.");
        }

        private void Menu_Inventario_Click(object sender, EventArgs e)
        {
            ShowContent("Inventario de Activos", "Módulo de gestión del inventario físico de hardware.");
        }

        private void Menu_AsignarHardware_Click(object sender, EventArgs e)
        {
            ShowContent("Asignar Hardware a Empleado", "Aquí se implementará el Caso de Uso: CU-03 Asignar Hardware a Empleado.");
        }

        private void Menu_DefinirStack_Click(object sender, EventArgs e)
        {
            ShowContent("Definir Stack Tecnológico", "Aquí se implementará el Caso de Uso: CU-02 Definir Stack Tecnológico.");
        }

        private void ShowContent(string title, string description)
        {
            // Clear previous user controls (except header which is at index 0 or 1, let's just clear specific panel)
            // A better way is to have a specific mainContentPanel inside contentPanel.
            
            // For now, we'll just clear and recreate
            while (contentPanel.Controls.Count > 1)
            {
                contentPanel.Controls.RemoveAt(1);
            }

            Panel pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };
            
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30),
                ForeColor = Color.FromArgb(41, 53, 65)
            };

            Label lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 12F),
                AutoSize = true,
                Location = new Point(30, 80),
                ForeColor = Color.Gray
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblDesc);

            contentPanel.Controls.Add(pnl);
            pnl.BringToFront();
        }
    }
}
