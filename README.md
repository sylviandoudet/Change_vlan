# VlanChangerApp

VlanChangerApp est une application Windows Forms permettant de gérer les VLANs et les configurations IP associées sur une carte réseau. Elle permet également de sélectionner une carte réseau et d'activer ou désactiver l'attribution automatique d'adresses IP.

## Prérequis

- .NET Framework 4.8
- Windows PowerShell


## Installation

### Étape 1 : Télécharger et installer l'application

1. Téléchargez l'archive ZIP contenant l'application.
2. Extrayez le contenu de l'archive dans un répertoire de votre choix, par exemple `C:\Program Files\Vlan_Changer`.

### Étape 2 : Créer les fichiers CSV de configuration

1. Créez un fichier nommé `VlanListe.csv` dans le répertoire `C:\Program Files\Vlan_Changer`.
2. Créez un fichier nommé `NameNetworkCard.csv` dans le répertoire `C:\Program Files\Vlan_Changer`.

### Étape 3 : Configurer les fichiers CSV

#### VlanListe.csv

Le fichier `VlanListe.csv` doit contenir les informations sur les VLANs. Chaque ligne doit être au format suivant : <VLAN_ID>;<Description>;<Mode_IP>;<Adresse_IP>;<CIDR>

Exemple : 1;VLAN 1;DHCP;; 2;VLAN 2;STATIC;192.168.1.10;24

#### NameNetworkCard.csv

Le fichier `NameNetworkCard.csv` doit contenir le nom de la carte réseau à utiliser. Le fichier doit contenir une seule ligne avec le nom de la carte réseau.

Exemple : Ethernet1


## Utilisation

### Lancer l'application

1. Double-cliquez sur `VlanChangerApp.exe` pour lancer l'application.
2. Une icône apparaîtra dans la barre des tâches à côté de l'horloge.

### Gestion des VLANs

#### Ajouter un VLAN

1. Faites un clic droit sur l'icône de l'application à côté de l'horloge. Ou double clic.
2. Sélectionnez `Paramètres` puis `Liste des VLANs`.
3. Remplissez les champs `ID VLAN` et `Nom VLAN`.
4. Sélectionnez le mode IP (`DHCP` ou `STATIC`).
   - Si vous sélectionnez `STATIC`, remplissez également les champs `Adresse IP` et `CIDR`. (Optionnel)
5. Cliquez sur le bouton `Ajouter`.

#### Modifier un VLAN

1. Faites un clic droit sur l'icône de l'application à côté de l'horloge. Ou double clic.
2. Sélectionnez `Paramètres` puis `Liste des VLANs`.
3. Sélectionnez un VLAN dans la liste.
4. Modifiez les champs `ID VLAN`, `Nom VLAN`, `Mode IP`, `Adresse IP` et `CIDR` selon vos besoins.
5. Cliquez sur le bouton `Modifier`.

#### Supprimer un VLAN

1. Faites un clic droit sur l'icône de l'application à côté de l'horloge. Ou double clic.
2. Sélectionnez `Paramètres` puis `Liste des VLANs`.
3. Sélectionnez un VLAN dans la liste.
4. Cliquez sur le bouton `Supprimer`.

### Activer l'IP automatique

1. Faites un clic droit sur l'icône de l'application à côté de l'horloge.
2. Sélectionnez `Paramètres` puis `IP AUTO`.
3. Sélectionnez `Activer` pour activer l'attribution automatique d'adresses IP.
4. Sélectionnez `Désactiver` pour désactiver l'attribution automatique d'adresses IP.

### Sélectionner une carte réseau

1. Faites un clic droit sur l'icône de l'application à côté de l'horloge.
2. Sélectionnez `Cartes Réseau`.
3. Sélectionnez la carte réseau souhaitée dans la liste.

### Changer de VLAN

1. Faites un clic droit sur l'icône de l'application à côté de l'horloge.
2. Sélectionnez `VLAN`.
3. Sélectionnez le VLAN souhaité dans la liste. Un petite bulle vous indique la description du VLAN.


## Dépannage

### Problèmes courants

- **Erreur lors de la lecture des fichiers CSV** : Assurez-vous que les fichiers `VlanListe.csv` et `NameNetworkCard.csv` existent et sont correctement formatés.
- **Erreur lors de l'exécution des scripts PowerShell** : Assurez-vous que PowerShell est installé et que l'exécution des scripts est autorisée (`Set-ExecutionPolicy RemoteSigned`).

### Contacter le support

Pour toute question ou problème, veuillez contacter le support à l'adresse suivante : sylvian.doudet@groupe-sb.org

## Licence

VlanChangerApp est distribué sous la licence MIT. Voir le fichier `LICENSE` pour plus d'informations.