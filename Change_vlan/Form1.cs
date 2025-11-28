using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Change_vlan
{
    public partial class VlanChangerApp : Form
    {
        // Chemins des fichiers de configuration
        private const string ListeVlanPath = "C://Program Files/Vlan_Changer/VlanListe.csv";
        private const string NetworkNamePath = "C://Program Files/Vlan_Changer/NameNetworkCard.csv";
        private const string AutoIPFilePath = "C://Program Files/Vlan_Changer/autoip.txt";

        // Déclaration de variables utilisées dans plusieurs méthodes
        string VlanSelect;
        string VlanDesc;
        string VlanIDDesc;
        string VlanIP;
        string VlanCIDR;
        string VlanModeIP;
        string NameNetworkCard;
        int close = 0;
        int autoip = 0;
        string _vlanRegistryKeyword;

        class VlanEntry // Représente une ligne du CSV
        {
            public string Id { get; set; }       // ex: "10"
            public string Name { get; set; }     // ex: "Administratif"
            public string ModeIp { get; set; }   // "STATIC" ou "DHCP"
            public string Ip { get; set; }       // ex: "192.168.1.197" ou ""
            public string Cidr { get; set; }     // ex: "24" ou ""
        }

        public VlanChangerApp()
        {
            InitializeComponent();
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new VlanChangerApp());

            // Définir KeyPreview sur true pour que le formulaire puisse gérer les événements clavier en premier
            this.KeyPreview = true;

            // Lier l'événement KeyDown au gestionnaire d'événements
            this.KeyDown += VlanChangerApp_KeyDown;

            // Ajouter le gestionnaire d'événements pour la zone en dehors du menu de sélection de VLAN
            this.Click += VlanChangerApp_Click;


            // Initialiser le ToolTip
            vlanToolTip = new ToolTip();

            // Ajouter des gestionnaires d'événements MouseHover pour chaque élément du menu
            vL1ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL2ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL3ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL4ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL5ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL6ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL7ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL8ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL9ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL10ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL11ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL12ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL13ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL14ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL15ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL16ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL17ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL18ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL19ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;
            vL20ToolStripMenuItem.MouseHover += VlanMenuItem_MouseHover;

            // Ajouter des gestionnaires d'événements Click pour chaque élément du menu vlan
            vL1ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL2ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL3ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL4ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL5ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL6ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL7ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL8ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL9ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL10ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL11ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL12ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL13ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL14ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL15ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL16ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL17ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL18ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL19ToolStripMenuItem.Click += VlanMenuItem_Click;
            vL20ToolStripMenuItem.Click += VlanMenuItem_Click;

            // Ajouter des gestionnaires d'événements Click pour chaque élément du menu carte réseau
            network1ToolStripMenuItem.Click += NetworkMenuItem_Click;
            network2ToolStripMenuItem.Click += NetworkMenuItem_Click;
            network3ToolStripMenuItem.Click += NetworkMenuItem_Click;
            network4ToolStripMenuItem.Click += NetworkMenuItem_Click;
            network5ToolStripMenuItem.Click += NetworkMenuItem_Click;

            // Charger la configuration
            LoadConfig();

            // Mettre à jour l'état des boutons en fonction de la configuration
            if (autoip == 1)
            {
                activerToolStripMenuItem.Checked = true;
                désactiverToolStripMenuItem.Checked = false;
            }
            else
            {
                activerToolStripMenuItem.Checked = false;
                désactiverToolStripMenuItem.Checked = true;
            }
        }

        private void VlanChangerApp_Load(object sender, EventArgs e) // Méthode appelée lors du chargement de l'application
        {

            ActualizeVlanListe(); // Actualise la liste des VLAN

            ChangeNetworkInterface(); // Affiche le nom de la carte réseau dans le ToolStripMenu

            PopulateNetworkMenu(); // Recupere le nom des cartes réseau de la machine et peuple les boutons

            GetSelectedNetworkAdapterVlanID(); // Récupère l'ID VLAN actuel et l'affiche

            if (FormWindowState.Minimized == this.WindowState) // Active l'icône de notification à côté de l'horloge
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = true;
            }
        }

        // *********************** FORM *********************** //

        private void BTN_ADD_VLAN_Click(object sender, EventArgs e) // Méthode appelée lorsqu'un nouvel élément VLAN est ajouté
        {
            AddVlanList();
        }

        private void BTN_SUPP_VLAN_Click(object sender, EventArgs e)
        {
            DeleteSelectedVlans();
        }

        private void BTN_MODIF_VLAN_Click(object sender, EventArgs e) // Méthode appelée lorsqu'un élément VLAN est modifié
        {
            if (listView_Vlan_Liste.SelectedItems.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un élément à modifier.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Récupérer l'ID de l'ancien VLAN (ligne sélectionnée)
            string oldID = listView_Vlan_Liste.SelectedItems[0].Text;
            string newID = txtB_ID_VLAN.Text;
            string newName = txtB_Name_VLAN.Text;
            string newModeIP;
            string newIP = txtB_IP_Adresse.Text;
            string newCIDR = txtB_CIDR.Text;

            // Déterminer le mode IP et valider éventuellement
            if (cB_ip_dhcp.Checked)
            {
                newModeIP = "DHCP";
                newIP = "";
                newCIDR = "";
            }
            else
            {
                newModeIP = "STATIC";

                if (!IsValidIPAddress(newIP))
                {
                    MessageBox.Show("L'adresse IP n'est pas valide.",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(newCIDR, out int cidrVal) || cidrVal < 0 || cidrVal > 32)
                {
                    MessageBox.Show("Le CIDR n'est pas valide.",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                newCIDR = cidrVal.ToString();
            }

            // Charger tous les VLAN
            var vlans = LoadVlans();

            // Vérifier qu'on ne crée pas un doublon d'ID (si l'ID change)
            if (newID != oldID && vlans.Any(v => v.Id == newID))
            {
                MessageBox.Show("L'ID VLAN entré est déjà utilisé.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Trouver le VLAN à modifier
            var vlanToEdit = vlans.FirstOrDefault(v => v.Id == oldID);
            if (vlanToEdit == null)
            {
                MessageBox.Show("Le VLAN sélectionné n'a pas été trouvé dans le fichier.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Appliquer les modifications
            vlanToEdit.Id = newID;
            vlanToEdit.Name = newName;
            vlanToEdit.ModeIp = newModeIP;
            vlanToEdit.Ip = newIP;
            vlanToEdit.Cidr = newCIDR;

            // (optionnel) retrier par ID
            vlans = vlans
                .OrderBy(v => int.TryParse(v.Id, out int idVal) ? idVal : int.MaxValue)
                .ToList();

            // Sauvegarder
            SaveVlans(vlans);

            // Rafraîchir la ListView
            ActualizeVlanListe();

            // Réinitialiser l'UI
            EscapeSelection();
        }

        private void listView_Vlan_Liste_SelectedIndexChanged(object sender, EventArgs e) // Méthode appelée lorsqu'un élément est sélectionné dans la ListView
        {
            // Vérifie si un élément est sélectionné dans la ListView
            if (listView_Vlan_Liste.SelectedItems.Count > 0)
            {
                // Obtient l'élément sélectionné
                ListViewItem selectedItem = listView_Vlan_Liste.SelectedItems[0];

                // Obtient les valeurs VLANID et DescVLAN de l'élément sélectionné
                string VLANID = selectedItem.Text; // La première colonne correspond à l'élément Text du ListViewItem
                string DescVLAN = selectedItem.SubItems[1].Text; // La deuxième colonne correspond au premier sous-élément
                string ModeIP = selectedItem.SubItems[2].Text; // La troisième colonne correspond au deuxième sous-élément

                // Affiche les valeurs dans les TextBox
                txtB_ID_VLAN.Text = VLANID;
                txtB_Name_VLAN.Text = DescVLAN;
                if (ModeIP == "DHCP")
                {
                    cB_ip_dhcp.Checked = true;
                    cB_ip_static.Checked = false;
                }
                else
                {
                    cB_ip_dhcp.Checked = false;
                    cB_ip_static.Checked = true;
                    string[] IpCIDR = ModeIP.Split('/');
                    txtB_IP_Adresse.Text = IpCIDR[0];
                    txtB_CIDR.Text = IpCIDR[1];
                }

                BTN_MODIF_VLAN.Enabled = true;
                BTN_SUPP_VLAN.Enabled = true;
                BTN_ADD_VLAN.Enabled = false;
            }
        }

        private void VlanChangerApp_KeyDown(object sender, KeyEventArgs e) // Méthode appelée lorsqu'une touche est enfoncée sur le formulaire
        {
            if (e.KeyCode == Keys.Escape)
            {
                EscapeSelection(); // Désélectionne l'élément actuellement sélectionné
                e.Handled = true; // Indique que l'événement a été géré
            }
        }

        private void VlanChangerApp_Click(object sender, EventArgs e) => EscapeSelection(); // Méthode appelée lorsqu'un clic est effectué en dehors de la zone de sélection de VLAN

        private void VlanChangerApp_FormClosing(object sender, FormClosingEventArgs e) // Méthode appelée lors de la fermeture de l'application
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (close == 1) // Verfie la valeur de close pour fermer le logiciel
                {
                    e.Cancel = false; // Autorise la fermeture
                }
                else
                {
                    e.Cancel = true; // Annule la fermeture par défaut
                    this.WindowState = FormWindowState.Minimized; // Minimise la fenêtre
                    notifyIcon1.Visible = true; // Affiche l'icône dans la barre d'état système
                    this.Hide(); // Cache la fenêtre
                }
            }
        }

        private void cB_ip_dhcp_CheckedChanged(object sender, EventArgs e) // Méthode pour activer ou désactiver les champs d'adresse IP et CIDR
        {
            if (cB_ip_dhcp.CheckState == CheckState.Checked)
            {
                cB_ip_static.Checked = false;
                txtB_IP_Adresse.Enabled = false;
                txtB_CIDR.Enabled = false;
            }
            else if (cB_ip_dhcp.CheckState == CheckState.Unchecked)
            {
                cB_ip_dhcp.Checked = false;
                cB_ip_static.Checked = true;
                txtB_IP_Adresse.Enabled = true;
                txtB_CIDR.Enabled = true;
            }
        }

        private void cB_ip_static_CheckedChanged(object sender, EventArgs e) // Méthode pour activer ou désactiver les champs d'adresse IP et CIDR
        {
            if (cB_ip_static.CheckState == CheckState.Checked)
            {
                cB_ip_dhcp.Checked = false;
                txtB_IP_Adresse.Enabled = true;
                txtB_CIDR.Enabled = true;
            }
            else if (cB_ip_static.CheckState == CheckState.Unchecked)
            {
                cB_ip_dhcp.Checked = true;
                cB_ip_static.Checked = false;
                txtB_IP_Adresse.Enabled = false;
                txtB_CIDR.Enabled = false;
            }
        }

        private void txtB_ID_VLAN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddVlanList();
            }
        }

        private void txtB_Name_VLAN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddVlanList();
            }
        }

        private void txtB_IP_Adresse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddVlanList();
            }
        }

        private void txtB_CIDR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddVlanList();
            }
        }

        private void cB_ip_dhcp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddVlanList();
            }
        }

        private void listView_Vlan_Liste_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                DeleteSelectedVlans();
                e.SuppressKeyPress = true; // éviter un beep
            }
        }

        // *********************** ICONE *********************** //

        private void VlanMenuItem_Click(object sender, EventArgs e) // Méthode pour changer le VLAN de la carte réseau sélectionnée
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                VlanSelect = menuItem.Text;
                UncheckStripMenuVLAN();
                ChangeVlan();
                menuItem.Checked = true;
            }
        }

        private async void ChangeVlan() // Méthode pour changer le VLAN de la carte réseau sélectionnée
        {
            try
            {
                string selectedVlan = VlanSelect;
                string vlanKeyword = GetOrDetectVlanRegistryKeyword();
                if (vlanKeyword == null)
                {
                    return;
                }

                // On prend une copie des variables utilisées dans le Task.Run
                string adapterNameTarget = NameNetworkCard;

                UseWaitCursor = true;
                Enabled = false;

                await Task.Run(() =>
                {
                    // Utilisez WMI pour rechercher la carte réseau par son nom
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher(
                            @"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId = """ + adapterNameTarget + @"""");

                    ManagementObjectCollection networkAdapters = searcher.Get();

                    bool found = false;

                    foreach (ManagementObject mo in networkAdapters)
                    {
                        string adapterName = mo["NetConnectionId"]?.ToString();
                        if (adapterName == adapterNameTarget)
                        {
                            found = true;
                            using (PowerShell PowerShellInstance = PowerShell.Create())
                            {
                                string script =
                                    $@"Set-NetAdapterAdvancedProperty -Name '{adapterNameTarget}' -RegistryKeyword '{vlanKeyword}' -DisplayValue {selectedVlan}";
                                PowerShellInstance.AddScript(script);
                                PowerShellInstance.Invoke();
                            }

                            // Petite pause le temps que Windows applique le VLAN
                            Thread.Sleep(2000);
                        }
                    }

                    if (!found)
                    {
                        // On ne peut pas appeler directement MessageBox.Show dans le Task.Run
                        // donc on remonte ça en exception
                        throw new Exception("Aucune carte réseau correspondant à " + adapterNameTarget + " n'a été trouvée.");
                    }
                });

                // Ici on est revenu sur le thread UI

                VlanIDDesc = selectedVlan;
                GetDescriptionVlan(VlanIDDesc);

                // Gestion auto IP (ici encore synchrone, mais beaucoup plus rapide que le changement de VLAN)
                if (autoip == 1)
                {
                    if (VlanModeIP == "DHCP")
                    {
                        SetNetworkInterfaceToDhcp(NameNetworkCard);
                    }
                    else
                    {
                        ChangeIpAddress(NameNetworkCard, VlanIP, VlanCIDR);
                    }
                }

                // Affichez un message de succès dans les notifications
                notifyIcon1.BalloonTipTitle = "Vlan Changer App";
                notifyIcon1.BalloonTipText = $"Vlan {selectedVlan} , {VlanDesc} modifié sur {NameNetworkCard} !";
                notifyIcon1.ShowBalloonTip(500);

                GetSelectedNetworkAdapterVlanID();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            finally
            {
                UseWaitCursor = false;
                Enabled = true;
            }
        }


        private void GetSelectedNetworkAdapterVlanID() // Méthode pour obtenir l'ID VLAN de la carte réseau sélectionnée
        {
            try
            {
                string vlanKeyword = GetOrDetectVlanRegistryKeyword();
                if (vlanKeyword == null)
                {
                    notifyIcon1.Text = "Vlan inconnu";
                    ActualizeVlanListe();
                    return;
                }

                // Utilisez WMI pour rechercher la carte réseau par son nom
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId =" + "\"" + NameNetworkCard + "\"");
                ManagementObjectCollection networkAdapters = searcher.Get();

                foreach (ManagementObject mo in networkAdapters)
                {

                    string adapterName = mo["NetConnectionId"].ToString();
                    if (adapterName == NameNetworkCard)
                    {
                        using (PowerShell PowerShellInstance = PowerShell.Create())
                        {
                            PowerShellInstance.AddScript($@"(Get-NetAdapterAdvancedProperty -Name '{NameNetworkCard}' -RegistryKeyword '{vlanKeyword}').DisplayValue");
                            var output = PowerShellInstance.Invoke();
                            foreach (var o in output)
                            {
                                string resultGet = o.ToString();
                                notifyIcon1.Text = "Vlan " + resultGet;
                            }
                            //   PowerShellInstance.AddScript("(Get-NetAdapterAdvancedProperty -Name " + "\'" + NameNetworkCard + "\'" + " -RegistryKeyword 'RegVlanid').DisplayValue");
                            //   Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                            //   foreach (PSObject PSOutputItem in PSOutput)
                            //   {
                            //       string ResultGet = PSOutputItem.ToString();
                            //       notifyIcon1.Text = "Vlan " + ResultGet;
                            //   }
                        }
                    }
                    ActualizeVlanListe();
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private string GetDescriptionVlan(string vlanID = "defaultVlanID") // Méthode pour définir la description du VLAN
        {
            var vlans = LoadVlans();

            foreach (var v in vlans)
            {
                if (v.Id == vlanID)
                {
                    // Mets aussi à jour les variables globales si tu en as besoin
                    VlanDesc = v.Name;
                    VlanIP = v.Ip;
                    VlanCIDR = v.Cidr;
                    VlanModeIP = v.ModeIp;

                    return v.Name;
                }
            }

            return "Description non trouvée";
        }

        private void NetworkMenuItem_Click(object sender, EventArgs eventArgs) // Méthode pour changer la carte réseau sélectionnée
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                File.WriteAllText(NetworkNamePath, menuItem.Text);
                _vlanRegistryKeyword = null;
                UncheckStripMenuNetwork();
                UncheckStripMenuVLAN();
                ChangeNetworkInterface();
                menuItem.Checked = true;
            }
        }

        public void SetNetworkInterfaceToDhcp(string interfaceName) // Méthode pour définir l'interface réseau sur DHCP
        {
            string script = $@"
                $interfaceName = '{interfaceName}'

                # Activer DHCP pour l'adresse IP
                Set-NetIPInterface -InterfaceAlias $interfaceName -Dhcp Enabled

                # Activer DHCP pour le serveur DNS
                Set-DnsClientServerAddress -InterfaceAlias $interfaceName -ResetServerAddresses
                ";
            try
            {
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(script);
                    powerShell.Invoke();
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private string DetectVlanRegistryKeyword(string interfaceName)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                // 1) Test Realtek : RegVlanId
                ps.Commands.Clear();
                ps.AddScript($@"Get-NetAdapterAdvancedProperty -Name '{interfaceName}' -RegistryKeyword 'RegVlanId' -ErrorAction SilentlyContinue");
                var outputRealtek = ps.Invoke();

                if (outputRealtek != null && outputRealtek.Count > 0)
                {
                    return "RegVlanId";   // Realtek
                }

                // 2) Test Broadcom : VlanID
                ps.Commands.Clear();
                ps.AddScript($@"Get-NetAdapterAdvancedProperty -Name '{interfaceName}' -RegistryKeyword 'VlanID' -ErrorAction SilentlyContinue");
                var outputBroadcom = ps.Invoke();

                if (outputBroadcom != null && outputBroadcom.Count > 0)
                {
                    return "VlanID";      // Broadcom
                }
            }

            // Rien trouvé : on ne sait pas gérer cette carte pour l’instant
            MessageBox.Show(
            $"Carte réseau \"{interfaceName}\" non compatible ou non gérée.\n" +
            "Merci de transmettre ce message au support.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return null;
        }

        private string GetOrDetectVlanRegistryKeyword()
        {
            if (!string.IsNullOrEmpty(_vlanRegistryKeyword))
            {
                return _vlanRegistryKeyword;
            }
            else 
            {
                _vlanRegistryKeyword = DetectVlanRegistryKeyword(NameNetworkCard);
                return _vlanRegistryKeyword; 
            }
        }

        private List<string> GetNameNetworkInterface()
        {
            var result = new List<string>();

            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript("Get-NetAdapter | Select-Object -ExpandProperty Name");
                var output = ps.Invoke();

                foreach (var obj in output)
                {
                    var name = obj?.ToString();
                    if (!string.IsNullOrWhiteSpace(name))
                        result.Add(name);
                }
            }

            return result;
        }

        private void PopulateNetworkMenu()
        {
            var adapters = GetNameNetworkInterface();

            int i = 1;
            foreach (var adapterName in adapters)
            {
                string menuFieldName = "network" + i + "ToolStripMenuItem";
                FieldInfo field = GetType().GetField(menuFieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic);

                if (field != null && field.FieldType == typeof(ToolStripMenuItem))
                {
                    var item = (ToolStripMenuItem)field.GetValue(this);
                    item.Text = adapterName;
                    item.Visible = true;

                    if (nToolStripMenuItem.Text == adapterName)
                    {
                        UncheckStripMenuNetwork();
                        item.Checked = true;
                    }
                }

                i++;
            }
        }

        public void ChangeIpAddress(string interfaceName, string newIpAddress, string newSubnetMask) // Méthode pour changer l'adresse IP de la carte réseau
        {
            string script = $@"
                $interfaceName = '{interfaceName}'
                $newIpAddress = '{newIpAddress}'
                $newSubnetMask = '{newSubnetMask}'

                # Changer l'adresse IP
                New-NetIPAddress -InterfaceAlias $interfaceName -IPAddress $newIpAddress -PrefixLength $newSubnetMask
                ";
            try
            {
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(script);
                    powerShell.Invoke();
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private void ChangeNetworkInterface() // Méthode pour changer la carte réseau sélectionné dans le toolStrip
        {
            if (!File.Exists(NetworkNamePath))
            {
                var adapters = GetNameNetworkInterface();
                if (adapters.Count > 0)
                {
                    NameNetworkCard = adapters[0];
                    File.WriteAllText(NetworkNamePath, NameNetworkCard);
                    _vlanRegistryKeyword = null;
                }
                else
                {
                    MessageBox.Show("Aucune carte réseau détectée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                string[] lignesNetwork = File.ReadAllLines(NetworkNamePath);
                if (lignesNetwork.Length == 0)
                {
                    var adapters = GetNameNetworkInterface();
                    if (adapters.Count > 0)
                    {
                        NameNetworkCard = adapters[0];
                        File.WriteAllText(NetworkNamePath, NameNetworkCard);
                        _vlanRegistryKeyword = null;
                    }
                    else
                    {
                        MessageBox.Show("Aucune carte réseau détectée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    NameNetworkCard = lignesNetwork[0];
                }
            }

            // Mise à jour du texte du ToolStripMenuItem avec le nom de la carte réseau
            nToolStripMenuItem.Text = NameNetworkCard;

            // Appel de la méthode pour obtenir l'ID VLAN de l'adaptateur réseau sélectionné
            GetSelectedNetworkAdapterVlanID();
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e) => ActualizeVlanListe(); // Méthode appelée avant l'ouverture du menu contextuel

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e) // Méthode appelée pour quitter l'application
        {
            close = 1;
            this.Close();
        }

        private void listeDesVLANToolStripMenuItem_Click(object sender, EventArgs e) // Méthode appelée pour ouvrir la fenêtre de gestion des VLAN
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) // Méthode appelée lors d'un double-clic sur l'icône de notification
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            ActualizeVlanListe();
        }

        private void VlanMenuItem_MouseHover(object sender, EventArgs e) // Méthode pour afficher le vlan acctuel au survol
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                string vlanID = menuItem.Text;
                string description = GetDescriptionVlan(vlanID);
                vlanToolTip.SetToolTip(menuItem.GetCurrentParent(), description);
            }
        }

        private void activerToolStripMenuItem_Click(object sender, EventArgs e) // Méthode pour activer l'ip auto
        {
            autoip = 1;
            activerToolStripMenuItem.Checked = true;
            désactiverToolStripMenuItem.Checked = false;
            SaveConfig();
        }

        private void désactiverToolStripMenuItem_Click(object sender, EventArgs e) // Méthode pour désactiver l'ip auto
        {
            autoip = 0;
            désactiverToolStripMenuItem.Checked = true;
            activerToolStripMenuItem.Checked = false;
            SaveConfig();
        }

        private void UncheckStripMenuVLAN() // Méthode pour désélectionner tous les éléments du menu de sélection de VLAN
        {
            for (int i = 1; i <= 20; i++)
            {
                string VlanMenuItem = "vL" + i + "ToolStripMenuItem";
                FieldInfo field = GetType().GetField(VlanMenuItem, BindingFlags.Instance | BindingFlags.NonPublic);

                // Vérifiez si le champ a été trouvé
                if (field != null && field.FieldType == typeof(ToolStripMenuItem))
                {
                    // Obtenez la valeur du champ (c'est-à-dire le ToolStripMenuItem)
                    ToolStripMenuItem item = (ToolStripMenuItem)field.GetValue(this);

                    item.Checked = false;
                }
            }
        }

        private void UncheckStripMenuNetwork() // Méthode pour désélectionner tous les éléments du menu de sélection de VLAN
        {
            for (int i = 1; i <= 5; i++)
            {
                string VlanMenuItem = "network" + i + "ToolStripMenuItem";
                FieldInfo field = GetType().GetField(VlanMenuItem, BindingFlags.Instance | BindingFlags.NonPublic);

                // Vérifiez si le champ a été trouvé
                if (field != null && field.FieldType == typeof(ToolStripMenuItem))
                {
                    // Obtenez la valeur du champ (c'est-à-dire le ToolStripMenuItem)
                    ToolStripMenuItem item = (ToolStripMenuItem)field.GetValue(this);

                    item.Checked = false;
                }
            }
        }

        // *********************** METHODE GLOBALE *********************** //

        private void ActualizeVlanListe() // Méthode pour actualiser la liste des VLAN dans l'application (listView et toolStrip)
        {
            listView_Vlan_Liste.Items.Clear();

            List<VlanEntry> vlans = LoadVlans();

            int i = 1;
            foreach (var vlan in vlans)
            {
                string VLANID = vlan.Id;
                string DescVLAN = vlan.Name;
                string ModeIP = vlan.ModeIp;
                string IpAdresse = vlan.Ip;
                string CIDR = vlan.Cidr;
                string IPCIDR = IpAdresse + "/" + CIDR;

                string VlanMenuItem = "vL" + i + "ToolStripMenuItem"; // Ajoute le bon nombre de toolstrip par rapport au nombre de vlan
                FieldInfo field = GetType().GetField(VlanMenuItem, BindingFlags.Instance | BindingFlags.NonPublic);

                // Vérifiez si le champ a été trouvé
                if (field != null && field.FieldType == typeof(ToolStripMenuItem))
                {
                    // Obtenez la valeur du champ (c'est-à-dire le ToolStripMenuItem)
                    ToolStripMenuItem item = (ToolStripMenuItem)field.GetValue(this);
                    item.Text = VLANID;
                    item.Visible = true;//Affiche le toolstrip correspondant au vlan

                    if (notifyIcon1.Text == "Vlan " + VLANID) //Coche le vlan déjà appliqué
                    {
                        UncheckStripMenuVLAN();
                        item.Checked = true;
                    }

                    ListViewItem item1 = new ListViewItem(VLANID); // Crée un ListViewItem avec VLANID dans la première colonne
                    item1.SubItems.Add(DescVLAN); // Ajoute DescVLAN comme sous-élément dans la deuxième colonne
                    if (ModeIP == "DHCP")
                    {
                        item1.SubItems.Add(ModeIP); // Ajoute ModeIP comme sous-élément dans la troisième colonne
                    }
                    else item1.SubItems.Add(IPCIDR); // Ajoute ModeIP comme sous-élément dans la troisième colonne

                    listView_Vlan_Liste.Items.Add(item1); // Ajoute l'élément à la ListView
                    i++;
                }
            }
        }

        private void SaveConfig() // Méthode pour sauvegarder la configuration AUTOIP
        {
            try
            {
                using (var writer = new StreamWriter(AutoIPFilePath, false))
                {
                    writer.WriteLine(autoip);
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private void LoadConfig() // Méthode pour charger la configuration AUTOIP
        {
            try
            {
                if (File.Exists(AutoIPFilePath))
                {
                    using (StreamReader reader = new StreamReader(AutoIPFilePath))
                    {
                        if (int.TryParse(reader.ReadLine(), out int value))
                        {
                            autoip = value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private List<string> ReadLinesFromFile(string filePath) // Méthode pour lire les lignes d'un fichier
        {
            // Initialise une liste pour stocker les lignes lues à partir du fichier
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                // Lit chaque ligne du fichier et l'ajoute à la liste
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines; // Retourne la liste des lignes lues
        }

        static bool IsValidIPAddress(string ipAddress)  // Tente de parser l'adresse IP en IPv4 ou IPv6
        {
            return IPAddress.TryParse(ipAddress, out _);
        }

        private void EscapeSelection() // Méthode pour annuler la sélection actuelle dans l'application
        {
            // Vider les champs de texte
            txtB_ID_VLAN.Text = "";
            txtB_Name_VLAN.Text = "";

            // Déverrouiller le bouton "ajouter"
            BTN_ADD_VLAN.Enabled = true;

            // Verrouller les boutons "Supprimer et modfiier"
            BTN_MODIF_VLAN.Enabled = false;
            BTN_SUPP_VLAN.Enabled = false;

            // Désélectionner tous les éléments de la ListView
            listView_Vlan_Liste.SelectedItems.Clear();

            cB_ip_dhcp.Checked = false;
            cB_ip_static.Checked = true;
            txtB_IP_Adresse.Enabled = true;
            txtB_CIDR.Enabled = true;
            txtB_IP_Adresse.Text = "";
            txtB_CIDR.Text = "";
        }

        private void HandleException(Exception e) // Méthode pour gérer les exceptions
        {
            string LogFilePath = "C://Program Files/Vlan_Changer/error.log";
            // Log l'exception dans un fichier ou via un framework de logging
            string errorMessage = $"[ERREUR] {DateTime.Now}\n" +
                                  $"Message: {e.Message}\n" +
                                  $"Source: {e.Source}\n" +
                                  $"StackTrace: {e.StackTrace}\n" +
                                  (e.InnerException != null ? $"InnerException: {e.InnerException.Message}\n" : "") +
                                  "---------------------------------------------------\n";
            try
            {
                File.AppendAllText(LogFilePath, errorMessage);
            }
            catch { /* au pire on ignore */ }
            Console.WriteLine(errorMessage);
            MessageBox.Show(errorMessage, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddVlanList() //Méthode pour ajouter un vlan dans la liste
        {
           
            // 1) Vérifier les champs obligatoires
            if (string.IsNullOrWhiteSpace(txtB_ID_VLAN.Text) ||
                string.IsNullOrWhiteSpace(txtB_Name_VLAN.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter un nouvel élément.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Vérifier que l'ID est un entier
            if (!int.TryParse(txtB_ID_VLAN.Text, out int vlanID))
            {
                MessageBox.Show("L'ID VLAN doit être un nombre entier.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3) Vérifier qu'au moins un mode IP est coché
            if (!cB_ip_dhcp.Checked && !cB_ip_static.Checked)
            {
                MessageBox.Show("Veuillez choisir un mode IP (DHCP ou STATIC).","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4) Charger les VLAN existants depuis le CSV
            var vlans = LoadVlans();

            // 5) Vérifier unicité de l'ID
            if (vlans.Any(v => v.Id == txtB_ID_VLAN.Text))
            {
                MessageBox.Show("L'ID VLAN entré est déjà utilisé.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string modeIP;
            string ip = "";
            string cidr = "";

            // 6) Gérer le cas DHCP
            if (cB_ip_dhcp.Checked)
            {
                modeIP = "DHCP";
                // ip et cidr restent vides
            }
            else // 7) Gérer le cas STATIC
            {
                modeIP = "STATIC";

                // Validation IP
                if (!IsValidIPAddress(txtB_IP_Adresse.Text))
                {
                    MessageBox.Show("L'adresse IP n'est pas valide.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtB_IP_Adresse.Text = "";
                    txtB_CIDR.Text = "";
                    return;
                }

                // Validation CIDR
                if (!int.TryParse(txtB_CIDR.Text, out int cidrValue) ||
                    cidrValue < 0 || cidrValue > 32)
                {
                    MessageBox.Show("Le CIDR n'est pas valide.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtB_CIDR.Text = "";
                    return;
                }

                ip = txtB_IP_Adresse.Text;
                cidr = cidrValue.ToString();
            }

            // 8) Construire le nouvel objet VLAN
            var newVlan = new VlanEntry
            {
                Id = txtB_ID_VLAN.Text,
                Name = txtB_Name_VLAN.Text,
                ModeIp = modeIP,
                Ip = ip,
                Cidr = cidr
            };

            // 9) Ajouter à la liste et trier par ID
            vlans.Add(newVlan);

            vlans = vlans
                .OrderBy(v => int.TryParse(v.Id, out int idVal) ? idVal : int.MaxValue)
                .ToList();

            // 10) Sauvegarder dans le CSV
            SaveVlans(vlans);

            // 11) Mettre à jour la ListView
            ActualizeVlanListe();

            // 12) Réinitialiser l’UI
            EscapeSelection();
        }

        private List<VlanEntry> LoadVlans()  // Charge tous les VLAN depuis le CSV dans une liste d'objets
        {
            var vlans = new List<VlanEntry>();

            if (!File.Exists(ListeVlanPath))
                return vlans;

            foreach (var line in File.ReadAllLines(ListeVlanPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                if (parts.Length < 5)
                    continue; // ligne invalide

                vlans.Add(new VlanEntry
                {
                    Id = parts[0].Trim(),
                    Name = parts[1].Trim(),
                    ModeIp = parts[2].Trim(),
                    Ip = parts[3].Trim(),
                    Cidr = parts[4].Trim()
                });
            }

            return vlans;
        }

        private void SaveVlans(IEnumerable<VlanEntry> vlans) // Sauvegarde la liste des VLAN dans le CSV
        {
            var lines = vlans.Select(v =>
                $"{v.Id};{v.Name};{v.ModeIp};{v.Ip};{v.Cidr}");
            File.WriteAllLines(ListeVlanPath, lines);
        }

        private void DeleteSelectedVlans() // Methode pour supprimer un vlan de la liste
        {
            if (listView_Vlan_Liste.SelectedItems.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins un élément à supprimer.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Récupère les IDs sélectionnés
            var selectedIds = listView_Vlan_Liste.SelectedItems
                .Cast<ListViewItem>()
                .Select(item => item.Text)
                .ToHashSet();

            // Charge tous les VLAN
            var vlans = LoadVlans();

            // Filtre ceux à garder
            var remaining = vlans
                .Where(v => !selectedIds.Contains(v.Id))
                .ToList();

            // Sauvegarde
            SaveVlans(remaining);

            // Supprime de la ListView
            foreach (ListViewItem item in listView_Vlan_Liste.SelectedItems.Cast<ListViewItem>().ToList())
            {
                listView_Vlan_Liste.Items.Remove(item);
            }

            BTN_ADD_VLAN.Enabled = true;
            EscapeSelection();
        }

    }
}
// ****** Nouvelle version ****** //
// ****** WINFORM ****** //
// - Ancrage des éléments de l’interface
// - Touche Entrée mappée sur l’ajout de VLAN
// - Sélection multiple pour la suppression dans la ListView
// - Suppression avec la touche "Suppr" et "Backspace"
// - Refactor de la gestion du CSV : VlanEntry + LoadVlans/SaveVlans + DeleteSelectedVlans + BTN_MODIF_VLAN
// ****** ICONE ****** //
// - Détection automatique du mot-clé VLAN selon le driver (RegVlanId / VlanID)
// - Remplissage dynamique du menu des cartes réseau (Get-NetAdapter)
// - Gestion automatique de la première carte réseau si aucun fichier NameNetworkCard ou fichier vide