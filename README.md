# Change_vlan

Petit utilitaire Windows pour changer rapidement de VLAN sur une carte réseau via l’icône de la zone de notification.

---

## Prérequis

- Windows 10 / 11
- Lancer l’application **en tant qu’administrateur**
- Carte réseau avec option VLAN dans les propriétés avancées  
  (mot-clé `RegVlanId` ou `VlanID`)

---

## Installation

1. Copier le dossier de l’application dans :

   ```text
   C:\Program Files\Vlan_Changer\
   ```

2. Vérifier/Créer les fichiers de configuration dans ce dossier :

   - `VlanListe.csv`  
     - Une ligne par VLAN :  
       `ID;Nom;ModeIP;IP;CIDR`  
       - Exemple DHCP :  
         `10;Administratif;DHCP;;`  
       - Exemple IP fixe :  
         `20;Pedago;STATIC;192.168.20.10;24`
   - `NameNetworkCard.csv` (optionnel au premier lancement)  
     - Contient le **nom de la carte réseau** à utiliser.  
     - Si le fichier n’existe pas ou est vide, l’app choisit la **première** carte trouvée.
   - `autoip.txt` (optionnel au premier lancement)  
     - Contient `0` ou `1`  
     - `0` = ne touche pas à l’IP (recommandé si vous avez déjà une configuration multi‑IP)  
     - `1` = applique automatiquement le mode IP défini dans `VlanListe.csv`
       (DHCP ou IP fixe) **uniquement utile si vous passez vraiment de DHCP à STATIC
       et que la carte n’est pas déjà configurée en multi‑IP**.

3. Lancer `Change_vlan` **en administrateur**.

4. Une icône **Vlan Changer App** apparaît dans la zone de notification.

---

## Utilisation

### Changer de VLAN

1. Clic droit sur l’icône **Vlan Changer App**.
2. Dans le menu **VLAN**, choisir l’ID souhaité (1 à 20 affichés selon `VlanListe.csv`).
3. Le VLAN est appliqué à la carte réseau sélectionnée.
4. Une notification apparaît :  
   `Vlan XX, NomDuVlan modifié sur NomDeLaCarte !`

Si `autoip` est activé (`autoip.txt` = `1`) :
- Si le VLAN est en **DHCP** → l’interface passe en DHCP (IP + DNS).
- Si le VLAN est en **STATIC** → l’IP et le CIDR définis dans `VlanListe.csv` sont appliqués.

> ⚠️ Dans la plupart des usages « avancés » (multi‑IP déjà configuré sur la carte), il est conseillé de laisser `autoip` à `0`.  
> L’option est surtout prévue pour les postes qui passent régulièrement de DHCP à une IP fixe et inversement.

---

### Choisir la carte réseau

1. Clic droit sur l’icône → menu **Carte réseau**.
2. Sélectionner la carte souhaitée.
3. Le choix est sauvegardé dans `NameNetworkCard.csv`.

---

### Gérer la liste des VLAN

1. Clic droit sur l’icône → **Liste des VLAN**.
2. Fenêtre principale :
   - **Ajouter** : saisir ID, Nom, choisir DHCP ou STATIC (avec IP/CIDR si STATIC).
   - **Modifier** : sélectionner un VLAN dans la liste, modifier les champs, puis **Modifier**.
   - **Supprimer** : sélectionner un ou plusieurs VLAN et cliquer sur **Supprimer**.
3. Les modifications sont automatiquement enregistrées dans `VlanListe.csv`.

Raccourcis :
- **Entrée** dans un champ → ajoute le VLAN.
- **Suppr / Backspace** dans la liste → supprime le(s) VLAN sélectionné(s).
- **Échap** → annule la sélection / réinitialise le formulaire.

---

### Mode IP automatique

Dans le menu contextuel :

- **Activer l’IP auto** : écrit `1` dans `autoip.txt` et permet à l’app
  d’appliquer automatiquement le mode IP du VLAN (DHCP ou STATIC).
- **Désactiver l’IP auto** : écrit `0` dans `autoip.txt` et laisse votre
  configuration IP existante telle quelle.

Si vous utilisez déjà une configuration **multi‑IP** sur la carte réseau,
il est en général préférable de laisser **l’IP auto désactivée**.

---

### Limitations

- L’application gère les cartes dont le paramètre VLAN est exposé via :
  - `RegVlanId` (souvent Realtek)
  - `VlanID` (souvent Broadcom)
- Si aucune de ces propriétés n’est trouvée, un message indique que la carte n’est pas gérée.
