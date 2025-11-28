namespace Change_vlan
{
    partial class VlanChangerApp
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VlanChangerApp));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vLANToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL6ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL12ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL13ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL14ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL15ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL17ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL18ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL19ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vL20ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parametreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.carteRéseauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.network1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.network2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.network3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.network4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.network5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listeDesVLANToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPAUTOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.désactiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BTN_ADD_VLAN = new System.Windows.Forms.Button();
            this.BTN_SUPP_VLAN = new System.Windows.Forms.Button();
            this.BTN_MODIF_VLAN = new System.Windows.Forms.Button();
            this.txtB_ID_VLAN = new System.Windows.Forms.TextBox();
            this.txtB_Name_VLAN = new System.Windows.Forms.TextBox();
            this.lb_ID_VLAN = new System.Windows.Forms.Label();
            this.lb_Name_VLAN = new System.Windows.Forms.Label();
            this.listView_Vlan_Liste = new System.Windows.Forms.ListView();
            this.Column_ID_VLAN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Name_VLAN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_IP_Adresse = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lb_ip_adresse = new System.Windows.Forms.Label();
            this.txtB_IP_Adresse = new System.Windows.Forms.TextBox();
            this.txtB_CIDR = new System.Windows.Forms.TextBox();
            this.lb_cidr = new System.Windows.Forms.Label();
            this.lb_ip_mode = new System.Windows.Forms.Label();
            this.cB_ip_static = new System.Windows.Forms.CheckBox();
            this.cB_ip_dhcp = new System.Windows.Forms.CheckBox();
            this.vlanToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nToolStripMenuItem,
            this.vLANToolStripMenuItem,
            this.parametreToolStripMenuItem,
            this.quitterToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // nToolStripMenuItem
            // 
            resources.ApplyResources(this.nToolStripMenuItem, "nToolStripMenuItem");
            this.nToolStripMenuItem.Name = "nToolStripMenuItem";
            // 
            // vLANToolStripMenuItem
            // 
            this.vLANToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vL1ToolStripMenuItem,
            this.vL2ToolStripMenuItem,
            this.vL3ToolStripMenuItem,
            this.vL4ToolStripMenuItem,
            this.vL5ToolStripMenuItem,
            this.vL6ToolStripMenuItem,
            this.vL7ToolStripMenuItem,
            this.vL8ToolStripMenuItem,
            this.vL9ToolStripMenuItem,
            this.vL10ToolStripMenuItem,
            this.vL11ToolStripMenuItem,
            this.vL12ToolStripMenuItem,
            this.vL13ToolStripMenuItem,
            this.vL14ToolStripMenuItem,
            this.vL15ToolStripMenuItem,
            this.vL16ToolStripMenuItem,
            this.vL17ToolStripMenuItem,
            this.vL18ToolStripMenuItem,
            this.vL19ToolStripMenuItem,
            this.vL20ToolStripMenuItem});
            this.vLANToolStripMenuItem.Name = "vLANToolStripMenuItem";
            resources.ApplyResources(this.vLANToolStripMenuItem, "vLANToolStripMenuItem");
            // 
            // vL1ToolStripMenuItem
            // 
            this.vL1ToolStripMenuItem.CheckOnClick = true;
            this.vL1ToolStripMenuItem.Name = "vL1ToolStripMenuItem";
            resources.ApplyResources(this.vL1ToolStripMenuItem, "vL1ToolStripMenuItem");
            // 
            // vL2ToolStripMenuItem
            // 
            this.vL2ToolStripMenuItem.Name = "vL2ToolStripMenuItem";
            resources.ApplyResources(this.vL2ToolStripMenuItem, "vL2ToolStripMenuItem");
            // 
            // vL3ToolStripMenuItem
            // 
            this.vL3ToolStripMenuItem.Name = "vL3ToolStripMenuItem";
            resources.ApplyResources(this.vL3ToolStripMenuItem, "vL3ToolStripMenuItem");
            // 
            // vL4ToolStripMenuItem
            // 
            this.vL4ToolStripMenuItem.Name = "vL4ToolStripMenuItem";
            resources.ApplyResources(this.vL4ToolStripMenuItem, "vL4ToolStripMenuItem");
            // 
            // vL5ToolStripMenuItem
            // 
            this.vL5ToolStripMenuItem.Name = "vL5ToolStripMenuItem";
            resources.ApplyResources(this.vL5ToolStripMenuItem, "vL5ToolStripMenuItem");
            // 
            // vL6ToolStripMenuItem
            // 
            this.vL6ToolStripMenuItem.Name = "vL6ToolStripMenuItem";
            resources.ApplyResources(this.vL6ToolStripMenuItem, "vL6ToolStripMenuItem");
            // 
            // vL7ToolStripMenuItem
            // 
            this.vL7ToolStripMenuItem.Name = "vL7ToolStripMenuItem";
            resources.ApplyResources(this.vL7ToolStripMenuItem, "vL7ToolStripMenuItem");
            // 
            // vL8ToolStripMenuItem
            // 
            this.vL8ToolStripMenuItem.Name = "vL8ToolStripMenuItem";
            resources.ApplyResources(this.vL8ToolStripMenuItem, "vL8ToolStripMenuItem");
            // 
            // vL9ToolStripMenuItem
            // 
            this.vL9ToolStripMenuItem.Name = "vL9ToolStripMenuItem";
            resources.ApplyResources(this.vL9ToolStripMenuItem, "vL9ToolStripMenuItem");
            // 
            // vL10ToolStripMenuItem
            // 
            this.vL10ToolStripMenuItem.Name = "vL10ToolStripMenuItem";
            resources.ApplyResources(this.vL10ToolStripMenuItem, "vL10ToolStripMenuItem");
            // 
            // vL11ToolStripMenuItem
            // 
            this.vL11ToolStripMenuItem.Name = "vL11ToolStripMenuItem";
            resources.ApplyResources(this.vL11ToolStripMenuItem, "vL11ToolStripMenuItem");
            // 
            // vL12ToolStripMenuItem
            // 
            this.vL12ToolStripMenuItem.Name = "vL12ToolStripMenuItem";
            resources.ApplyResources(this.vL12ToolStripMenuItem, "vL12ToolStripMenuItem");
            // 
            // vL13ToolStripMenuItem
            // 
            this.vL13ToolStripMenuItem.Name = "vL13ToolStripMenuItem";
            resources.ApplyResources(this.vL13ToolStripMenuItem, "vL13ToolStripMenuItem");
            // 
            // vL14ToolStripMenuItem
            // 
            this.vL14ToolStripMenuItem.Name = "vL14ToolStripMenuItem";
            resources.ApplyResources(this.vL14ToolStripMenuItem, "vL14ToolStripMenuItem");
            // 
            // vL15ToolStripMenuItem
            // 
            this.vL15ToolStripMenuItem.Name = "vL15ToolStripMenuItem";
            resources.ApplyResources(this.vL15ToolStripMenuItem, "vL15ToolStripMenuItem");
            // 
            // vL16ToolStripMenuItem
            // 
            this.vL16ToolStripMenuItem.Name = "vL16ToolStripMenuItem";
            resources.ApplyResources(this.vL16ToolStripMenuItem, "vL16ToolStripMenuItem");
            // 
            // vL17ToolStripMenuItem
            // 
            this.vL17ToolStripMenuItem.Name = "vL17ToolStripMenuItem";
            resources.ApplyResources(this.vL17ToolStripMenuItem, "vL17ToolStripMenuItem");
            // 
            // vL18ToolStripMenuItem
            // 
            this.vL18ToolStripMenuItem.Name = "vL18ToolStripMenuItem";
            resources.ApplyResources(this.vL18ToolStripMenuItem, "vL18ToolStripMenuItem");
            // 
            // vL19ToolStripMenuItem
            // 
            this.vL19ToolStripMenuItem.Name = "vL19ToolStripMenuItem";
            resources.ApplyResources(this.vL19ToolStripMenuItem, "vL19ToolStripMenuItem");
            // 
            // vL20ToolStripMenuItem
            // 
            this.vL20ToolStripMenuItem.Name = "vL20ToolStripMenuItem";
            resources.ApplyResources(this.vL20ToolStripMenuItem, "vL20ToolStripMenuItem");
            // 
            // parametreToolStripMenuItem
            // 
            this.parametreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.carteRéseauToolStripMenuItem,
            this.listeDesVLANToolStripMenuItem,
            this.iPAUTOToolStripMenuItem});
            this.parametreToolStripMenuItem.Name = "parametreToolStripMenuItem";
            resources.ApplyResources(this.parametreToolStripMenuItem, "parametreToolStripMenuItem");
            // 
            // carteRéseauToolStripMenuItem
            // 
            this.carteRéseauToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.network1ToolStripMenuItem,
            this.network2ToolStripMenuItem,
            this.network3ToolStripMenuItem,
            this.network4ToolStripMenuItem,
            this.network5ToolStripMenuItem});
            this.carteRéseauToolStripMenuItem.Name = "carteRéseauToolStripMenuItem";
            resources.ApplyResources(this.carteRéseauToolStripMenuItem, "carteRéseauToolStripMenuItem");
            // 
            // network1ToolStripMenuItem
            // 
            this.network1ToolStripMenuItem.Name = "network1ToolStripMenuItem";
            resources.ApplyResources(this.network1ToolStripMenuItem, "network1ToolStripMenuItem");
            // 
            // network2ToolStripMenuItem
            // 
            this.network2ToolStripMenuItem.Name = "network2ToolStripMenuItem";
            resources.ApplyResources(this.network2ToolStripMenuItem, "network2ToolStripMenuItem");
            // 
            // network3ToolStripMenuItem
            // 
            this.network3ToolStripMenuItem.Name = "network3ToolStripMenuItem";
            resources.ApplyResources(this.network3ToolStripMenuItem, "network3ToolStripMenuItem");
            // 
            // network4ToolStripMenuItem
            // 
            this.network4ToolStripMenuItem.Name = "network4ToolStripMenuItem";
            resources.ApplyResources(this.network4ToolStripMenuItem, "network4ToolStripMenuItem");
            // 
            // network5ToolStripMenuItem
            // 
            this.network5ToolStripMenuItem.Name = "network5ToolStripMenuItem";
            resources.ApplyResources(this.network5ToolStripMenuItem, "network5ToolStripMenuItem");
            // 
            // listeDesVLANToolStripMenuItem
            // 
            this.listeDesVLANToolStripMenuItem.Name = "listeDesVLANToolStripMenuItem";
            resources.ApplyResources(this.listeDesVLANToolStripMenuItem, "listeDesVLANToolStripMenuItem");
            this.listeDesVLANToolStripMenuItem.Click += new System.EventHandler(this.listeDesVLANToolStripMenuItem_Click);
            // 
            // iPAUTOToolStripMenuItem
            // 
            this.iPAUTOToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activerToolStripMenuItem,
            this.désactiverToolStripMenuItem});
            this.iPAUTOToolStripMenuItem.Name = "iPAUTOToolStripMenuItem";
            resources.ApplyResources(this.iPAUTOToolStripMenuItem, "iPAUTOToolStripMenuItem");
            // 
            // activerToolStripMenuItem
            // 
            this.activerToolStripMenuItem.Name = "activerToolStripMenuItem";
            resources.ApplyResources(this.activerToolStripMenuItem, "activerToolStripMenuItem");
            this.activerToolStripMenuItem.Click += new System.EventHandler(this.activerToolStripMenuItem_Click);
            // 
            // désactiverToolStripMenuItem
            // 
            this.désactiverToolStripMenuItem.Name = "désactiverToolStripMenuItem";
            resources.ApplyResources(this.désactiverToolStripMenuItem, "désactiverToolStripMenuItem");
            this.désactiverToolStripMenuItem.Click += new System.EventHandler(this.désactiverToolStripMenuItem_Click);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            resources.ApplyResources(this.quitterToolStripMenuItem, "quitterToolStripMenuItem");
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // BTN_ADD_VLAN
            // 
            resources.ApplyResources(this.BTN_ADD_VLAN, "BTN_ADD_VLAN");
            this.BTN_ADD_VLAN.Name = "BTN_ADD_VLAN";
            this.BTN_ADD_VLAN.UseVisualStyleBackColor = true;
            this.BTN_ADD_VLAN.Click += new System.EventHandler(this.BTN_ADD_VLAN_Click);
            // 
            // BTN_SUPP_VLAN
            // 
            resources.ApplyResources(this.BTN_SUPP_VLAN, "BTN_SUPP_VLAN");
            this.BTN_SUPP_VLAN.Name = "BTN_SUPP_VLAN";
            this.BTN_SUPP_VLAN.UseVisualStyleBackColor = true;
            this.BTN_SUPP_VLAN.Click += new System.EventHandler(this.BTN_SUPP_VLAN_Click);
            // 
            // BTN_MODIF_VLAN
            // 
            resources.ApplyResources(this.BTN_MODIF_VLAN, "BTN_MODIF_VLAN");
            this.BTN_MODIF_VLAN.Name = "BTN_MODIF_VLAN";
            this.BTN_MODIF_VLAN.UseVisualStyleBackColor = true;
            this.BTN_MODIF_VLAN.Click += new System.EventHandler(this.BTN_MODIF_VLAN_Click);
            // 
            // txtB_ID_VLAN
            // 
            resources.ApplyResources(this.txtB_ID_VLAN, "txtB_ID_VLAN");
            this.txtB_ID_VLAN.Name = "txtB_ID_VLAN";
            this.txtB_ID_VLAN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtB_ID_VLAN_KeyDown);
            // 
            // txtB_Name_VLAN
            // 
            resources.ApplyResources(this.txtB_Name_VLAN, "txtB_Name_VLAN");
            this.txtB_Name_VLAN.Name = "txtB_Name_VLAN";
            this.txtB_Name_VLAN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtB_Name_VLAN_KeyDown);
            // 
            // lb_ID_VLAN
            // 
            resources.ApplyResources(this.lb_ID_VLAN, "lb_ID_VLAN");
            this.lb_ID_VLAN.Name = "lb_ID_VLAN";
            // 
            // lb_Name_VLAN
            // 
            resources.ApplyResources(this.lb_Name_VLAN, "lb_Name_VLAN");
            this.lb_Name_VLAN.Name = "lb_Name_VLAN";
            // 
            // listView_Vlan_Liste
            // 
            resources.ApplyResources(this.listView_Vlan_Liste, "listView_Vlan_Liste");
            this.listView_Vlan_Liste.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Column_ID_VLAN,
            this.Column_Name_VLAN,
            this.Column_IP_Adresse});
            this.listView_Vlan_Liste.FullRowSelect = true;
            this.listView_Vlan_Liste.GridLines = true;
            this.listView_Vlan_Liste.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_Vlan_Liste.HideSelection = false;
            this.listView_Vlan_Liste.Name = "listView_Vlan_Liste";
            this.listView_Vlan_Liste.Scrollable = false;
            this.listView_Vlan_Liste.UseCompatibleStateImageBehavior = false;
            this.listView_Vlan_Liste.View = System.Windows.Forms.View.Details;
            this.listView_Vlan_Liste.SelectedIndexChanged += new System.EventHandler(this.listView_Vlan_Liste_SelectedIndexChanged);
            this.listView_Vlan_Liste.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_Vlan_Liste_KeyDown);
            // 
            // Column_ID_VLAN
            // 
            resources.ApplyResources(this.Column_ID_VLAN, "Column_ID_VLAN");
            // 
            // Column_Name_VLAN
            // 
            resources.ApplyResources(this.Column_Name_VLAN, "Column_Name_VLAN");
            // 
            // Column_IP_Adresse
            // 
            resources.ApplyResources(this.Column_IP_Adresse, "Column_IP_Adresse");
            // 
            // lb_ip_adresse
            // 
            resources.ApplyResources(this.lb_ip_adresse, "lb_ip_adresse");
            this.lb_ip_adresse.Name = "lb_ip_adresse";
            // 
            // txtB_IP_Adresse
            // 
            resources.ApplyResources(this.txtB_IP_Adresse, "txtB_IP_Adresse");
            this.txtB_IP_Adresse.Name = "txtB_IP_Adresse";
            this.txtB_IP_Adresse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtB_IP_Adresse_KeyDown);
            // 
            // txtB_CIDR
            // 
            resources.ApplyResources(this.txtB_CIDR, "txtB_CIDR");
            this.txtB_CIDR.Name = "txtB_CIDR";
            this.txtB_CIDR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtB_CIDR_KeyDown);
            // 
            // lb_cidr
            // 
            resources.ApplyResources(this.lb_cidr, "lb_cidr");
            this.lb_cidr.Name = "lb_cidr";
            // 
            // lb_ip_mode
            // 
            resources.ApplyResources(this.lb_ip_mode, "lb_ip_mode");
            this.lb_ip_mode.Name = "lb_ip_mode";
            // 
            // cB_ip_static
            // 
            resources.ApplyResources(this.cB_ip_static, "cB_ip_static");
            this.cB_ip_static.Checked = true;
            this.cB_ip_static.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cB_ip_static.Name = "cB_ip_static";
            this.cB_ip_static.UseVisualStyleBackColor = true;
            this.cB_ip_static.CheckedChanged += new System.EventHandler(this.cB_ip_static_CheckedChanged);
            // 
            // cB_ip_dhcp
            // 
            resources.ApplyResources(this.cB_ip_dhcp, "cB_ip_dhcp");
            this.cB_ip_dhcp.Name = "cB_ip_dhcp";
            this.cB_ip_dhcp.UseVisualStyleBackColor = true;
            this.cB_ip_dhcp.CheckedChanged += new System.EventHandler(this.cB_ip_dhcp_CheckedChanged);
            this.cB_ip_dhcp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cB_ip_dhcp_KeyDown);
            // 
            // VlanChangerApp
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cB_ip_dhcp);
            this.Controls.Add(this.cB_ip_static);
            this.Controls.Add(this.lb_ip_mode);
            this.Controls.Add(this.lb_cidr);
            this.Controls.Add(this.txtB_CIDR);
            this.Controls.Add(this.lb_ip_adresse);
            this.Controls.Add(this.txtB_IP_Adresse);
            this.Controls.Add(this.listView_Vlan_Liste);
            this.Controls.Add(this.lb_Name_VLAN);
            this.Controls.Add(this.lb_ID_VLAN);
            this.Controls.Add(this.txtB_Name_VLAN);
            this.Controls.Add(this.txtB_ID_VLAN);
            this.Controls.Add(this.BTN_MODIF_VLAN);
            this.Controls.Add(this.BTN_SUPP_VLAN);
            this.Controls.Add(this.BTN_ADD_VLAN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "VlanChangerApp";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VlanChangerApp_FormClosing);
            this.Load += new System.EventHandler(this.VlanChangerApp_Load);
            this.Click += new System.EventHandler(this.VlanChangerApp_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VlanChangerApp_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem parametreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem carteRéseauToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listeDesVLANToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vLANToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem vL1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL6ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL7ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL11ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL15ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL17ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL18ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL20ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL12ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL13ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL14ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vL19ToolStripMenuItem;
        private System.Windows.Forms.Button BTN_ADD_VLAN;
        private System.Windows.Forms.Button BTN_SUPP_VLAN;
        private System.Windows.Forms.Button BTN_MODIF_VLAN;
        private System.Windows.Forms.TextBox txtB_ID_VLAN;
        private System.Windows.Forms.TextBox txtB_Name_VLAN;
        private System.Windows.Forms.Label lb_ID_VLAN;
        private System.Windows.Forms.Label lb_Name_VLAN;
        private System.Windows.Forms.ListView listView_Vlan_Liste;
        private System.Windows.Forms.ColumnHeader Column_ID_VLAN;
        private System.Windows.Forms.ColumnHeader Column_Name_VLAN;
        private System.Windows.Forms.ToolStripMenuItem network1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem network2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem network3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem network4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem network5ToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader Column_IP_Adresse;
        private System.Windows.Forms.Label lb_ip_adresse;
        private System.Windows.Forms.TextBox txtB_IP_Adresse;
        private System.Windows.Forms.TextBox txtB_CIDR;
        private System.Windows.Forms.Label lb_cidr;
        private System.Windows.Forms.Label lb_ip_mode;
        private System.Windows.Forms.CheckBox cB_ip_static;
        private System.Windows.Forms.CheckBox cB_ip_dhcp;
        private System.Windows.Forms.ToolTip vlanToolTip;
        private System.Windows.Forms.ToolStripMenuItem iPAUTOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem désactiverToolStripMenuItem;
    }
}

