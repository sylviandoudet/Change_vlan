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

            // Affiche le nom de la carte réseau dans le ToolStripMenu
            ChangeNetworkInteface();

            // Utilisez PowerShell pour récupérer la liste des noms des adaptateurs réseau
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("Get-NetAdapter | Select-Object -ExpandProperty Name");
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                // Initialisez une chaîne pour stocker les noms des adaptateurs réseau
                string networkAdapterNames = "";

                // Parcourez chaque objet PSObject dans PSOutput et ajoutez le nom de l'adaptateur réseau à la chaîne
                foreach (PSObject PSOutputItem in PSOutput)
                {
                    networkAdapterNames += PSOutputItem.ToString() + "\n";
                }

                // Divisez la chaîne en lignes individuelles
                string[] lines = networkAdapterNames.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Obtenez la taille indexée du tableau lines[]
                int indexedSize = lines.Length - 1;

                // Répétez la diminution de indexedSize tant qu'il est supérieur ou égal à 0
                do
                {
                    int i = 1;
                    // Parcourez les lignes et traitez les données
                    foreach (string ligne in lines)
                    {
                        // Divisez la ligne en colonnes en utilisant la virgule comme délimiteur
                        string[] colonnes = ligne.Split(';');
                        // Accédez aux valeurs individuelles des colonnes
                        string NameInterface = colonnes[0];
                        string NetworkInterfaceMenuItem = "network" + i + "ToolStripMenuItem"; // Ajoute le bon nombre de toolstrip par rapport au nombre d'inteface réseau
                        FieldInfo field = GetType().GetField(NetworkInterfaceMenuItem, BindingFlags.Instance | BindingFlags.NonPublic);

                        // Vérifiez si le champ a été trouvé
                        if (field != null && field.FieldType == typeof(ToolStripMenuItem))
                        {
                            // Obtenez la valeur du champ (c'est-à-dire le ToolStripMenuItem)
                            ToolStripMenuItem item = (ToolStripMenuItem)field.GetValue(this);
                            item.Text = NameInterface;
                            item.Visible = true;// Affiche le toolstrip correspondant au nom de l'interface

                            if (nToolStripMenuItem.Text == NameInterface) // Coche l'interface déja sélectionné
                            {
                                UncheckStripMenuNetwork();
                                item.Checked = true;
                            }
                            i++;
                        }
                    }
                    // Diminuez indexedSize
                    indexedSize--;
                }
                while (indexedSize >= 0);

            }

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

        private void BTN_ADD_VLAN_Click(object sender, EventArgs e) // Méthode appelée lorsqu'un nouvel élément VLAN est ajouté
        {
            // Vérifiez si les champs de texte ne sont pas vides
            if (!string.IsNullOrEmpty(txtB_ID_VLAN.Text) && !string.IsNullOrEmpty(txtB_Name_VLAN.Text))
            {
                // Vérifiez si l'ID VLAN est un nombre
                if (int.TryParse(txtB_ID_VLAN.Text, out int vlanID))
                {
                    // Vérifiez si l'ID VLAN n'est pas déjà utilisé
                    bool idExists = listView_Vlan_Liste.Items.Cast<ListViewItem>().Any(item => item.Text == txtB_ID_VLAN.Text);
                    if (!idExists)
                    {
                        string newLine = "";

                        if (cB_ip_dhcp.Checked || cB_ip_static.Checked)
                        {
                            string modeIP = "STATIC;"; //Valeur par defaut

                            if (cB_ip_dhcp.Checked)
                            {
                                modeIP = "DHCP";
                                newLine = txtB_ID_VLAN.Text + ";" + txtB_Name_VLAN.Text + ";" + modeIP;

                                // Ajoutez l'élément à la ListView
                                ListViewItem newItem = new ListViewItem(txtB_ID_VLAN.Text);
                                newItem.SubItems.Add(txtB_Name_VLAN.Text);
                                newItem.SubItems.Add(modeIP);
                                listView_Vlan_Liste.Items.Add(newItem);
                            }

                            if (cB_ip_static.Checked)
                            {
                                modeIP = "STATIC";

                                if (IsValidIPAddress(txtB_IP_Adresse.Text))
                                {
                                    newLine = txtB_ID_VLAN.Text + ";" + txtB_Name_VLAN.Text + ";" + modeIP + ";" + txtB_IP_Adresse.Text + ";" + txtB_CIDR.Text;

                                    // Ajoutez l'élément à la ListView
                                    ListViewItem newItem = new ListViewItem(txtB_ID_VLAN.Text);
                                    newItem.SubItems.Add(txtB_Name_VLAN.Text);
                                    newItem.SubItems.Add(txtB_IP_Adresse.Text + "/" + txtB_CIDR.Text);
                                    listView_Vlan_Liste.Items.Add(newItem);

                                    // Lire toutes les lignes du fichier CSV
                                    List<string> lines = File.ReadAllLines(ListeVlanPath).ToList();

                                    // Ajouter la nouvelle ligne
                                    lines.Add(newLine);

                                    // Trier les lignes par ordre alphabétique du premier élément
                                    lines = lines.OrderBy(line => line.Split(';')[0]).ToList();

                                    // Réécrire toutes les lignes dans le fichier CSV
                                    File.WriteAllLines(ListeVlanPath, lines);

                                    // Vider les champs de texte
                                    EscapeSelection();

                                    ActualizeVlanListe(); // Actualise la liste des VLAN
                                }
                                else
                                {
                                    MessageBox.Show("L'adresse IP n'est pas valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    // Vider les champs de texte conf ip
                                    txtB_IP_Adresse.Text = "";
                                    txtB_CIDR.Text = "";
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("L'ID VLAN entré est déjà utilisé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("L'ID VLAN doit être un nombre entier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter un nouvel élément.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_SUPP_VLAN_Click(object sender, EventArgs e) // Méthode appelée lorsqu'un élément VLAN est supprimé
        {
            // Vérifiez si un élément est sélectionné dans la ListView
            if (listView_Vlan_Liste.SelectedItems.Count > 0)
            {
                // Supprimez l'élément du fichier CSV
                string selectedID = listView_Vlan_Liste.SelectedItems[0].Text;
                List<string> lines = File.ReadAllLines(ListeVlanPath).ToList();
                lines.RemoveAll(line => line.StartsWith(selectedID));
                File.WriteAllLines(ListeVlanPath, lines);

                // Supprimez l'élément de la ListView
                listView_Vlan_Liste.Items.Remove(listView_Vlan_Liste.SelectedItems[0]);

                // Déverrouiller le bouton "ajouter"
                BTN_ADD_VLAN.Enabled = true;

                EscapeSelection(); // Annule la sélection
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un élément à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_MODIF_VLAN_Click(object sender, EventArgs e) // Méthode appelée lorsqu'un élément VLAN est modifié
        {
            // Vérifiez si un élément est sélectionné dans la ListView
            if (listView_Vlan_Liste.SelectedItems.Count > 0)
            {
                // Met à jour l'élément dans le fichier CSV
                string oldID = listView_Vlan_Liste.SelectedItems[0].Text;
                string newID = txtB_ID_VLAN.Text;
                string newName = txtB_Name_VLAN.Text;
                string newModeIP;
                string newIP = txtB_IP_Adresse.Text;
                string newCIDR = txtB_CIDR.Text;

                if (cB_ip_dhcp.Checked == true)
                {
                    listView_Vlan_Liste.SelectedItems[0].SubItems[2].Text = "DHCP";
                    newModeIP = "DHCP";
                }
                else
                {
                    listView_Vlan_Liste.SelectedItems[0].SubItems[2].Text = txtB_IP_Adresse.Text + "/" + txtB_CIDR.Text;
                    newModeIP = "STATIC";
                }

                List<string> lines = File.ReadAllLines(ListeVlanPath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith(oldID))
                    {
                        lines[i] = newID + ";" + newName + ";" + newModeIP + ";" + newIP + ";" + newCIDR;
                        break;
                    }
                }
                File.WriteAllLines(ListeVlanPath, lines);

                // Met à jour l'élément dans la ListView
                listView_Vlan_Liste.SelectedItems[0].Text = txtB_ID_VLAN.Text;
                listView_Vlan_Liste.SelectedItems[0].SubItems[1].Text = txtB_Name_VLAN.Text;

                EscapeSelection(); // Annule la sélection
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un élément à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void ChangeVlan() // Méthode pour changer le VLAN de la carte réseau sélectionnée
        {
            try
            {
                string selectedVlan = VlanSelect;

                // Utilisez WMI pour rechercher la carte réseau par son nom
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId =" + "\"" + NameNetworkCard + "\"");
                ManagementObjectCollection networkAdapters = searcher.Get();

                foreach (ManagementObject mo in networkAdapters)
                {

                    string adapterName = mo["NetConnectionId"].ToString();
                    // Vérifiez si le nom de la carte réseau correspond à "INT VLAN X"
                    if (adapterName == NameNetworkCard)
                    {
                        using (PowerShell PowerShellInstance = PowerShell.Create())
                        {
                            PowerShellInstance.AddScript("Set-NetAdapterAdvancedProperty -Name " + "\'" + NameNetworkCard + "\'" + " -RegistryKeyword 'RegVlanid' -DisplayValue " + selectedVlan);
                            Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                            foreach (PSObject PSOutputItem in PSOutput)
                            {
                                PSOutputItem.ToString();
                            }
                        }

                        Thread.Sleep(2000);

                        VlanIDDesc = selectedVlan;
                        GetDescriptionVlan();

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
                        notifyIcon1.BalloonTipText = "Vlan " + selectedVlan + " ,  " + VlanDesc + " modifié sur " + adapterName + " !";
                        notifyIcon1.ShowBalloonTip(500);
                    }
                    else
                    {
                        MessageBox.Show("Aucune carte réseau correspondant à " + NameNetworkCard + " n'a été trouvée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                GetSelectedNetworkAdapterVlanID();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private void GetSelectedNetworkAdapterVlanID() // Méthode pour obtenir l'ID VLAN de la carte réseau sélectionnée
        {
            try
            {
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
                            PowerShellInstance.AddScript("(Get-NetAdapterAdvancedProperty -Name " + "\'" + NameNetworkCard + "\'" + " -RegistryKeyword 'RegVlanid').DisplayValue");
                            Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                            foreach (PSObject PSOutputItem in PSOutput)
                            {
                                string ResultGet = PSOutputItem.ToString();
                                notifyIcon1.Text = "Vlan " + ResultGet;
                            }
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
            string SelectVlanID = VlanIDDesc;

            List<string> lignes = ReadLinesFromFile(ListeVlanPath);

            // Parcourez les lignes et traitez les données
            foreach (string ligne in lignes)
            {
                // Divisez la ligne en colonnes en utilisant le point virgule comme délimiteur
                string[] colonnes = ligne.Split(';');

                // Accédez aux valeurs individuelles des colonnes
                string VLANID = colonnes[0];
                string Description = colonnes[1];
                string ModeIP = colonnes[2];
                string IpAdresse = colonnes[3];
                string CIDR = colonnes[4];

                if (SelectVlanID == VLANID) // Vérifie l'id sélectionné par rapport au fichier csv et ajoute la description
                {
                    VlanDesc = Description;
                    VlanIP = IpAdresse;
                    VlanCIDR = CIDR;
                    VlanModeIP = ModeIP;
                }

                if (colonnes[0] == vlanID)
                {
                    return colonnes[1]; // Retourne la description du VLAN
                }
            }
            return "Description non trouvée";
        }

        private void ActualizeVlanListe() // Méthode pour actualiser la liste des VLAN dans l'application (listView et toolStrip)
        {
            listView_Vlan_Liste.Items.Clear();

            List<string> lignes = ReadLinesFromFile(ListeVlanPath);

            int i = 1;
            // Parcourez les lignes et traitez les données
            foreach (string ligne in lignes)
            {
                // Divisez la ligne en colonnes en utilisant la virgule comme délimiteur
                string[] colonnes = ligne.Split(';');
                int taille = colonnes.Length;
                // Accédez aux valeurs individuelles des colonnes
                string VLANID = colonnes[0];
                string DescVLAN = colonnes[1];
                string ModeIP = colonnes[2];
                string IpAdresse = colonnes[3];
                string CIDR = colonnes[4];
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

        private void ChangeNetworkInteface() // Méthode pour changer la carte réseau sélectionné dans le toolStrip
        {
            // Lecture du fichier contenant le nom de la carte réseau
            string[] lignesNetwork = File.ReadAllLines(NetworkNamePath);

            // Récupération du nom de la carte réseau à partir de la première ligne du fichier
            NameNetworkCard = lignesNetwork[0];

            // Mise à jour du texte du ToolStripMenuItem avec le nom de la carte réseau
            nToolStripMenuItem.Text = NameNetworkCard;

            // Appel de la méthode pour obtenir l'ID VLAN de l'adaptateur réseau sélectionné
            GetSelectedNetworkAdapterVlanID();
        }

        private void NetworkMenuItem_Click(object sender, EventArgs eventArgs) // Méthode pour changer la carte réseau sélectionnée
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                File.WriteAllText(NetworkNamePath, menuItem.Text);
                UncheckStripMenuNetwork();
                UncheckStripMenuVLAN();
                ChangeNetworkInteface();
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

        private void VlanChangerApp_KeyDown(object sender, KeyEventArgs e) // Méthode appelée lorsqu'une touche est enfoncée sur le formulaire
        {
            if (e.KeyCode == Keys.Escape)
            {
                EscapeSelection(); // Désélectionne l'élément actuellement sélectionné
                e.Handled = true; // Indique que l'événement a été géré
            }
        }

        private void VlanChangerApp_Click(object sender, EventArgs e) => EscapeSelection(); // Méthode appelée lorsqu'un clic est effectué en dehors de la zone de sélection de VLAN

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e) => ActualizeVlanListe(); // Méthode appelée avant l'ouverture du menu contextuel

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

        private void VlanMenuItem_MouseHover(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                string vlanID = menuItem.Text;
                string description = GetDescriptionVlan(vlanID);
                vlanToolTip.SetToolTip(menuItem.GetCurrentParent(), description);
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

        private void SaveConfig() // Méthode pour sauvegarder la configuration AUTOIP
        {
            try
            {
                if (File.Exists(AutoIPFilePath))
                {
                    using (StreamWriter writer = new StreamWriter(AutoIPFilePath))
                    {
                        writer.WriteLine(autoip);
                    }
                }
                else
                {
                    File.Create(AutoIPFilePath);
                    using (StreamWriter writer = new StreamWriter(AutoIPFilePath))
                    {
                        writer.WriteLine(autoip);
                    }
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

        private void UncheckStripMenuVLAN() // Méthode pour désélectionner tous les éléments du menu de sélection de VLAN
        {
            for (int i = 0; i < 20; i++)
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
            for (int i = 0; i < 5; i++)
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
            // Log l'exception dans un fichier ou via un framework de logging
            string errorMessage = $"[ERREUR] {DateTime.Now}\n" +
                                  $"Message: {e.Message}\n" +
                                  $"Source: {e.Source}\n" +
                                  $"StackTrace: {e.StackTrace}\n" +
                                  (e.InnerException != null ? $"InnerException: {e.InnerException.Message}\n" : "");

            Console.WriteLine(errorMessage);
            MessageBox.Show(errorMessage, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

                
    }
}