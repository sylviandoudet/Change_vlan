using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace Change_vlan
{
    public partial class VlanChangerApp : Form
    {
        // Chemins des fichiers de configuration
        private const string ListeVlanPath = "C://Program Files/Vlan_Changer/VlanListe.csv";
        private const string NetworkNamePath = "C://Program Files/Vlan_Changer/NameNetworkCard.csv";

        // Déclaration de variables utilisées dans plusieurs méthodes
        string VlanSelect;
        string VlanDesc;
        string VlanIDDesc;
        string NameNetworkCard;
        int close = 0;
       

       

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
                int indexedSize = lines.Length -1 ;

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
                        // Ajoutez l'élément à la ListView
                        ListViewItem newItem = new ListViewItem(txtB_ID_VLAN.Text);
                        newItem.SubItems.Add(txtB_Name_VLAN.Text);
                        listView_Vlan_Liste.Items.Add(newItem);

                        // Ajouter l'élément au fichier CSV
                        string newLine = txtB_ID_VLAN.Text + ";" + txtB_Name_VLAN.Text;

                        // Lire toutes les lignes du fichier CSV
                        List<string> lines = File.ReadAllLines(ListeVlanPath).ToList();

                        // Ajouter la nouvelle ligne
                        lines.Add(newLine);

                        // Trier les lignes par ordre alphabétique du premier élément
                        lines = lines.OrderBy(line => line.Split(';')[0]).ToList();

                        // Réécrire toutes les lignes dans le fichier CSV
                        File.WriteAllLines(ListeVlanPath, lines);

                        // Vider les champs de texte
                        txtB_ID_VLAN.Text = "";
                        txtB_Name_VLAN.Text = "";

                        ActualizeVlanListe(); // Actualise la liste des VLAN
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
                // Mettez à jour l'élément dans la ListView
                listView_Vlan_Liste.SelectedItems[0].Text = txtB_ID_VLAN.Text;
                listView_Vlan_Liste.SelectedItems[0].SubItems[1].Text = txtB_Name_VLAN.Text;

                // Mettez à jour l'élément dans le fichier CSV
                string oldID = listView_Vlan_Liste.SelectedItems[0].Text;
                string newID = txtB_ID_VLAN.Text;
                string newName = txtB_Name_VLAN.Text;
                List<string> lines = File.ReadAllLines(ListeVlanPath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith(oldID))
                    {
                        lines[i] = newID + ";" + newName;
                        break;
                    }
                }
                File.WriteAllLines(ListeVlanPath, lines);

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

                // Affiche les valeurs dans les TextBox
                txtB_ID_VLAN.Text = VLANID;
                txtB_Name_VLAN.Text = DescVLAN;
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

        private void vL1ToolStripMenuItem_Click(object sender, EventArgs e) // Méthode appelée lors du clic sur un élément du menu de sélection de VLAN
        {
            VlanSelect = vL1ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL1ToolStripMenuItem.Checked = true;
        }


        private void vL2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL2ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL2ToolStripMenuItem.Checked = true;
        }

        private void vL3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL3ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL3ToolStripMenuItem.Checked = true;
        }

        private void vL4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL4ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL4ToolStripMenuItem.Checked = true;
        }

        private void vL5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL5ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL5ToolStripMenuItem.Checked = true;
        }

        private void vL6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL6ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL6ToolStripMenuItem.Checked = true;
        }

        private void vL7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL7ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL7ToolStripMenuItem.Checked = true;
        }

        private void vL8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL8ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL8ToolStripMenuItem.Checked = true;
        }

        private void vL9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL9ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL9ToolStripMenuItem.Checked = true;
        }

        private void vL10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL10ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL10ToolStripMenuItem.Checked = true;
        }

        private void vL11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL11ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL11ToolStripMenuItem.Checked = true;
        }

        private void vL12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL12ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL12ToolStripMenuItem.Checked = true;
        }

        private void vL13ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL13ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL13ToolStripMenuItem.Checked = true;
        }

        private void vL14ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL14ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL14ToolStripMenuItem.Checked = true;
        }

        private void vL15ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL15ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL15ToolStripMenuItem.Checked = true;
        }

        private void vL16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL16ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL16ToolStripMenuItem.Checked = true;
        }

        private void vL17ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL17ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL17ToolStripMenuItem.Checked = true;
        }

        private void vL18ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL18ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL18ToolStripMenuItem.Checked = true;
        }

        private void vL19ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL19ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL19ToolStripMenuItem.Checked = true;
        }

        private void vL20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VlanSelect = vL20ToolStripMenuItem.Text;
            UncheckStripMenuVLAN();
            ChangeVlan();
            vL20ToolStripMenuItem.Checked = true;
        }

        private void network1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(NetworkNamePath, network1ToolStripMenuItem.Text);
            UncheckStripMenuNetwork();
            UncheckStripMenuVLAN();
            ChangeNetworkInteface();
            network1ToolStripMenuItem.Checked = true;
        }

        private void network2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(NetworkNamePath, network2ToolStripMenuItem.Text);
            UncheckStripMenuNetwork();
            UncheckStripMenuVLAN();
            ChangeNetworkInteface();
            network2ToolStripMenuItem.Checked = true;
        }

        private void network3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(NetworkNamePath, network3ToolStripMenuItem.Text);
            UncheckStripMenuNetwork();
            UncheckStripMenuVLAN();
            ChangeNetworkInteface();
            network3ToolStripMenuItem.Checked = true;
        }

        private void network4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(NetworkNamePath, network4ToolStripMenuItem.Text);
            UncheckStripMenuNetwork();
            UncheckStripMenuVLAN();
            ChangeNetworkInteface();
            network4ToolStripMenuItem.Checked = true;
        }

        private void network5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(NetworkNamePath, network5ToolStripMenuItem.Text);
            UncheckStripMenuNetwork();
            ChangeNetworkInteface();
            network5ToolStripMenuItem.Checked = true;
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
                string errorMessage = $"[ERREUR] {DateTime.Now}\n"
                        + $"Message: {e.Message}\n"
                        + $"Source: {e.Source}\n"
                        + $"StackTrace: {e.StackTrace}\n"
                        + (e.InnerException != null ? $"InnerException: {e.InnerException.Message}\n" : "");

                // Afficher dans la console
                Console.WriteLine(errorMessage);

                // Afficher dans un MessageBox (utile pour les applications GUI)
                MessageBox.Show(errorMessage, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string errorMessage = $"[ERREUR] {DateTime.Now}\n"
                        + $"Message: {e.Message}\n"
                        + $"Source: {e.Source}\n"
                        + $"StackTrace: {e.StackTrace}\n"
                        + (e.InnerException != null ? $"InnerException: {e.InnerException.Message}\n" : "");

                // Afficher dans la console
                Console.WriteLine(errorMessage);

                // Afficher dans un MessageBox (utile pour les applications GUI)
                MessageBox.Show(errorMessage, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetDescriptionVlan() // Méthode pour définir la description du VLAN
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

                if (SelectVlanID == VLANID) // Vérifie l'id sélectionné par rapport au fichier csv et ajoute la description
                {
                    VlanDesc = Description;
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
                string VlanMenuItem = "vL" + i + "ToolStripMenuItem"; // Ajoute le bon nombre de toolstrip par rapport au nombre de vlan
                FieldInfo field = GetType().GetField(VlanMenuItem, BindingFlags.Instance | BindingFlags.NonPublic);

                // Vérifiez si le champ a été trouvé
                if (field != null && field.FieldType == typeof(ToolStripMenuItem))
                {
                    // Obtenez la valeur du champ (c'est-à-dire le ToolStripMenuItem)
                    ToolStripMenuItem item = (ToolStripMenuItem)field.GetValue(this);
                    item.Text = VLANID;
                    item.Visible = true;//Affiche le toolstrip correspondant au vlan

                    if(notifyIcon1.Text == "Vlan " + VLANID) //Coche le vlan déjà appliqué
                    {
                        UncheckStripMenuVLAN();
                        item.Checked = true;
                    }

                    ListViewItem item1 = new ListViewItem(VLANID); // Crée un ListViewItem avec VLANID dans la première colonne
                    item1.SubItems.Add(DescVLAN); // Ajoute DescVLAN comme sous-élément dans la deuxième colonne
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
    }
}