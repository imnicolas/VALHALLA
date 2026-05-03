using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
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
            var roleService = new RoleService();
            return roleService.GetRoleNameById(idRol);
        }

        private void LoadRoleBasedMenu()
        {
            // We stack buttons from Top to Bottom, so we bring them to front in reverse order or use DockStyle.Top.
            // Logout button is common and stays at the bottom
            Button btnLogout = CreateMenuButton("Cerrar Sesión");
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.Click += (s, e) => this.Close();
            sidebarPanel.Controls.Add(btnLogout);

            string nombreRol = GetRoleName(_currentUser.IdRol);

            if (nombreRol == "Desarrollador")
            {
                AddMenuButton("Restaurar Entorno", Menu_RestaurarEntorno_Click);
                AddMenuButton("Mis Secretos", Menu_MisSecretos_Click);
            }
            else if (nombreRol == "Administrador IT")
            {
                AddMenuButton("Inventario de Activos", Menu_Inventario_Click);
                AddMenuButton("Asignar Hardware", Menu_AsignarHardware_Click);
            }
            else if (nombreRol == "Lider Tecnico")
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
            while (contentPanel.Controls.Count > 1) contentPanel.Controls.RemoveAt(1);

            Panel pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };

            Label lblTitle = new Label
            {
                Text = "Restaurar Entorno de Desarrollo",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30),
                ForeColor = Color.FromArgb(41, 53, 65)
            };

            Label lblDesc = new Label
            {
                Text = "Este proceso validará su hardware (Seguridad Zero Trust), leerá su perfil asignado\ny ensamblará un script automatizado con sus herramientas y secretos.",
                Font = new Font("Segoe UI", 11F),
                AutoSize = true,
                Location = new Point(30, 80),
                ForeColor = Color.DimGray
            };

            Button btnRestaurar = new Button
            {
                Text = "⚙️ GENERAR INSTALADOR Y RESTAURAR",
                Location = new Point(30, 150),
                Width = 350,
                Height = 50,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRestaurar.FlatAppearance.BorderSize = 0;

            btnRestaurar.Click += (s, args) =>
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PowerShell Script|*.ps1";
                    sfd.Title = "Guardar Script de Restauración";
                    sfd.FileName = "instalar_entorno.ps1";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var entornoService = new EntornoService();
                            entornoService.GenerarScriptRestauracion(_currentUser.Id, sfd.FileName);
                            
                            MessageBox.Show("¡Script generado con éxito!\nSe ha guardado en la ruta especificada.\nHaga clic derecho en el archivo y seleccione 'Ejecutar con PowerShell'.", "Entorno Restaurado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error de Seguridad / Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblDesc);
            pnl.Controls.Add(btnRestaurar);

            contentPanel.Controls.Add(pnl);
            pnl.BringToFront();
        }

        private void Menu_MisSecretos_Click(object sender, EventArgs e)
        {
            while (contentPanel.Controls.Count > 1) contentPanel.Controls.RemoveAt(1);

            Panel pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };

            Label lblTitle = new Label
            {
                Text = "Mis Secretos Personales",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30),
                ForeColor = Color.FromArgb(41, 53, 65)
            };

            Label lblTipo = new Label { Text = "Tipo de Documento:", Location = new Point(30, 90), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            ComboBox cmbTipo = new ComboBox { Location = new Point(30, 115), Width = 300, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipo.Items.AddRange(new string[] { ".npmrc", "settings.xml", "aws_credentials", ".env" });

            Label lblContenido = new Label { Text = "Contenido del Archivo (Plano):", Location = new Point(30, 160), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtContenido = new TextBox 
            { 
                Location = new Point(30, 185), 
                Width = 500, 
                Height = 200, 
                Multiline = true, 
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 10F)
            };

            Button btnGuardar = new Button
            {
                Text = "ENCRIPTAR Y GUARDAR SECRETO",
                Location = new Point(30, 400),
                Width = 250,
                Height = 40,
                BackColor = Color.FromArgb(40, 167, 69), // Green
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            btnGuardar.Click += (s, args) =>
            {
                if (cmbTipo.SelectedItem == null || string.IsNullOrWhiteSpace(txtContenido.Text))
                {
                    MessageBox.Show("Por favor, seleccione el tipo de documento e ingrese el contenido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var secretoService = new SecretoService();
                    secretoService.GuardarSecreto(_currentUser.Id, cmbTipo.SelectedItem.ToString(), txtContenido.Text);
                    
                    MessageBox.Show("Secreto encriptado y guardado en la bóveda con éxito.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtContenido.Clear();
                    cmbTipo.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblTipo);
            pnl.Controls.Add(cmbTipo);
            pnl.Controls.Add(lblContenido);
            pnl.Controls.Add(txtContenido);
            pnl.Controls.Add(btnGuardar);

            contentPanel.Controls.Add(pnl);
            pnl.BringToFront();
        }

        private void Menu_Inventario_Click(object sender, EventArgs e)
        {
            // Clear content panel
            while (contentPanel.Controls.Count > 1)
            {
                contentPanel.Controls.RemoveAt(1);
            }

            Panel pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };

            Label lblTitle = new Label
            {
                Text = "Inventario de Activos",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30),
                ForeColor = Color.FromArgb(41, 53, 65)
            };

            Label lblMac = new Label { Text = "MAC Address:", Location = new Point(30, 90), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtMac = new TextBox { Location = new Point(30, 115), Width = 300, Font = new Font("Segoe UI", 10F) };

            Label lblSerie = new Label { Text = "Nro de Serie:", Location = new Point(30, 160), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtSerie = new TextBox { Location = new Point(30, 185), Width = 300, Font = new Font("Segoe UI", 10F) };

            Button btnRegistrar = new Button
            {
                Text = "REGISTRAR EQUIPO",
                Location = new Point(30, 240),
                Width = 200,
                Height = 40,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegistrar.FlatAppearance.BorderSize = 0;

            btnRegistrar.Click += (s, args) =>
            {
                try
                {
                    var hardwareService = new HardwareService();
                    hardwareService.RegistrarEquipo(txtMac.Text, txtSerie.Text);
                    MessageBox.Show("Equipo registrado correctamente en el inventario.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMac.Clear();
                    txtSerie.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblMac);
            pnl.Controls.Add(txtMac);
            pnl.Controls.Add(lblSerie);
            pnl.Controls.Add(txtSerie);
            pnl.Controls.Add(btnRegistrar);

            contentPanel.Controls.Add(pnl);
            pnl.BringToFront();
        }

        private void Menu_AsignarHardware_Click(object sender, EventArgs e)
        {
            // Clear content panel
            while (contentPanel.Controls.Count > 1)
            {
                contentPanel.Controls.RemoveAt(1);
            }

            Panel pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };

            Label lblTitle = new Label
            {
                Text = "Asignar Hardware a Empleado",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30),
                ForeColor = Color.FromArgb(41, 53, 65)
            };

            Label lblLegajo = new Label { Text = "Seleccionar Desarrollador:", Location = new Point(30, 90), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            ComboBox cmbDesarrolladores = new ComboBox { Location = new Point(30, 115), Width = 350, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblMac = new Label { Text = "Seleccionar Equipo (MAC):", Location = new Point(30, 160), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            ComboBox cmbEquipos = new ComboBox { Location = new Point(30, 185), Width = 350, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };

            var hardwareService = new HardwareService();

            try
            {
                var devs = hardwareService.ObtenerDesarrolladores();
                foreach (var d in devs)
                {
                    cmbDesarrolladores.Items.Add(new { Text = $"{d.Legajo} - {d.Email}", Value = d.Legajo });
                }
                cmbDesarrolladores.DisplayMember = "Text";
                cmbDesarrolladores.ValueMember = "Value";

                var equipos = hardwareService.ObtenerEquiposDisponibles();
                foreach (var eq in equipos)
                {
                    cmbEquipos.Items.Add(new { Text = $"{eq.MacAddress} - {eq.NroSerie}", Value = eq.MacAddress });
                }
                cmbEquipos.DisplayMember = "Text";
                cmbEquipos.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar listas: " + ex.Message);
            }

            Button btnAsignar = new Button
            {
                Text = "ASIGNAR EQUIPO",
                Location = new Point(30, 240),
                Width = 200,
                Height = 40,
                BackColor = Color.FromArgb(0, 122, 204), // Bright blue
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAsignar.FlatAppearance.BorderSize = 0;

            btnAsignar.Click += (s, args) =>
            {
                if (cmbDesarrolladores.SelectedItem == null || cmbEquipos.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un Desarrollador y un Equipo de las listas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    dynamic selectedDev = cmbDesarrolladores.SelectedItem;
                    dynamic selectedEq = cmbEquipos.SelectedItem;
                    
                    hardwareService.AsignarHardware(selectedDev.Value, selectedEq.Value);
                    
                    MessageBox.Show("Equipo asignado exitosamente al desarrollador.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Remove the assigned equipment from combo box so it can't be selected again
                    cmbEquipos.Items.Remove(selectedEq);
                    cmbDesarrolladores.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Validación (Reglas de Negocio)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblLegajo);
            pnl.Controls.Add(cmbDesarrolladores);
            pnl.Controls.Add(lblMac);
            pnl.Controls.Add(cmbEquipos);
            pnl.Controls.Add(btnAsignar);

            contentPanel.Controls.Add(pnl);
            pnl.BringToFront();
        }

        private void Menu_DefinirStack_Click(object sender, EventArgs e)
        {
            // Clear content panel
            while (contentPanel.Controls.Count > 1)
            {
                contentPanel.Controls.RemoveAt(1);
            }

            Panel pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };

            Label lblTitle = new Label
            {
                Text = "Definir Stack Tecnológico",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30),
                ForeColor = Color.FromArgb(41, 53, 65)
            };

            Label lblNombre = new Label { Text = "Nombre del Perfil:", Location = new Point(30, 90), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtNombre = new TextBox { Location = new Point(30, 115), Width = 300, Font = new Font("Segoe UI", 10F) };

            Label lblHerr = new Label { Text = "Seleccione las Herramientas:", Location = new Point(30, 160), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            
            CheckedListBox chkHerramientas = new CheckedListBox
            {
                Location = new Point(30, 185),
                Width = 400,
                Height = 200,
                Font = new Font("Segoe UI", 10F),
                CheckOnClick = true
            };

            // Load tools from DB
            var stackService = new StackService();
            try
            {
                var herramientasDisponibles = stackService.ObtenerHerramientasDisponibles();
                foreach (var h in herramientasDisponibles)
                {
                    // Display Member / Value Member style logic: we can just add the object and override ToString() in Herramienta
                    // Or add string, but we need the IDs later. We will add the object and set DisplayMember.
                    chkHerramientas.Items.Add(h);
                }
                chkHerramientas.DisplayMember = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar herramientas: " + ex.Message);
            }

            Button btnGuardar = new Button
            {
                Text = "GUARDAR PERFIL",
                Location = new Point(30, 400),
                Width = 200,
                Height = 40,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            btnGuardar.Click += (s, args) =>
            {
                try
                {
                    var seleccionadas = chkHerramientas.CheckedItems.Cast<Herramienta>().ToList();
                    stackService.CrearPerfilStack(txtNombre.Text, seleccionadas);
                    
                    MessageBox.Show("¡Perfil Stack creado y asociado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNombre.Clear();
                    for (int i = 0; i < chkHerramientas.Items.Count; i++)
                        chkHerramientas.SetItemChecked(i, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblNombre);
            pnl.Controls.Add(txtNombre);
            pnl.Controls.Add(lblHerr);
            pnl.Controls.Add(chkHerramientas);
            pnl.Controls.Add(btnGuardar);

            contentPanel.Controls.Add(pnl);
            pnl.BringToFront();
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
