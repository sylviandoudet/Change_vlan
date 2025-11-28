# Changelog

Toutes les versions avant 3.0 sont reconstruites a posteriori à partir de l’historique Git
et peuvent être incomplètes.

---

## [3.0.0] - 2025-11-28

### Ajouté
- Exécution du changement de VLAN en tâche de fond (`async/await` + `Task.Run`) pour ne plus
  bloquer l’interface pendant l’appel à PowerShell.
- Curseur d’attente et désactivation temporaire de la fenêtre pendant l’application du VLAN
  pour mieux indiquer à l’utilisateur qu’une action est en cours.
- Mise en cache du mot-clé de registre VLAN (`RegVlanId` / `VlanID`) par carte réseau via
  `_vlanRegistryKeyword` + méthode centrale `GetOrDetectVlanRegistryKeyword()`.
- Réinitialisation automatique de ce cache lorsque la carte réseau sélectionnée change.
- Gestion d’erreurs plus robuste :
  - journalisation détaillée dans `C:\Program Files\Vlan_Changer\error.log`
  - séparation claire du message (Message, Source, StackTrace, InnerException, etc.).
- Amélioration du message d’erreur lorsque la carte réseau n’expose pas de mot-clé VLAN :
  le nom de la carte est inclus et le texte invite à contacter le support.
- Sélection de la première carte réseau détectée si `NameNetworkCard.csv` est absent **ou vide**
  (cas mieux géré qu’avant).
  
### Modifié
- Centralisation de la détection du mot-clé VLAN dans `DetectVlanRegistryKeyword()` +
  `GetOrDetectVlanRegistryKeyword()` au lieu d’appeler la détection brute partout.
- `ChangeVlan()` :
  - refactorisée pour utiliser le cache de mot-clé VLAN,
  - recherche WMI et appel PowerShell déplacés dans un `Task.Run` pour ne pas bloquer le thread UI,
  - gestion claire du cas “aucune carte trouvée” (exception → `HandleException`).
- `GetSelectedNetworkAdapterVlanID()` utilise désormais aussi `GetOrDetectVlanRegistryKeyword()` pour
  être cohérent avec `ChangeVlan()`.
- `NetworkMenuItem_Click()` remet `_vlanRegistryKeyword` à `null` pour forcer une redétection correcte
  lors du changement de carte.
- `ChangeNetworkInterface()` :
  - remet `_vlanRegistryKeyword` à `null` lorsque la carte par défaut est sélectionnée parce que
    le fichier est absent ou vide,
  - renforce la gestion du cas “aucune carte réseau détectée”.

### Corrigé
- Cas où `NameNetworkCard.csv` existe mais est vide : la carte réseau n’était pas toujours correctement
  réinitialisée, ce qui pouvait casser la détection de VLAN.
- Risque de message d’erreur appelé depuis un thread non-UI pendant le `Task.Run` sur `ChangeVlan()`
  (désormais géré proprement via exception remontée et `await`).
- Divers petits comportements de bord liés à la sélection de la carte réseau et à la détection du VLAN.

---

## [2.1.0] - 2025-02-17

> Version précédente à 3.0, telle que présente sur GitHub (AssemblyVersion `2.1.0.*`).

### Ajouté
- Améliorations de la gestion de la liste des VLAN :
  - édition des VLAN existants,
  - suppression de plusieurs lignes sélectionnées dans la `ListView`,
  - validation de base sur l’ID VLAN et le format IP/CIDR.
- Mise à jour de la documentation (README / README.txt).

### Modifié
- Ajustements de l’interface (menus, icône de notification, textes).
- Nettoyage de code et petites refactorisations autour de la gestion de la liste (chargement/sauvegarde CSV).

### Corrigé
- Corrections de bugs mineurs sur la sélection de VLAN et l’affichage de la description.

---

## [2.0.0] - 2025-01-31

> Première version 2.x, basée sur l’appli de systray existante.

### Ajouté
- Version “stabilisée” de l’interface de gestion des VLAN (ajout / modification / suppression).
- Intégration plus propre de l’icône dans la zone de notification et du menu contextuel associé.
- Support de la configuration automatique via `VlanListe.csv` et `NameNetworkCard.csv`.

### Modifié
- Réorganisation des fichiers projet et du packaging.
- Ajustements visuels de la fenêtre principale.

### Corrigé
- Quelques corrections autour de la persistance des VLANs et du nom de la carte réseau.

---

## [1.0.0] - 2024-04-29

> Première publication sur GitHub.

### Ajouté
- Application de changement de VLAN pour Windows :
  - icône dans la zone de notification,
  - menu permettant de sélectionner un VLAN,
  - prise en charge de cartes réseau compatibles Realtek/Broadcom via propriété avancée.
- Stockage de la liste des VLANs dans `VlanListe.csv`.
- Sélection de la carte réseau utilisée via `NameNetworkCard.csv`.

---

*(Les sections 1.x / 2.x sont volontairement générales : l’historique détaillé n’ayant pas été
tenu à l’époque, elles sont reconstruites d’après les commits Git et peuvent être incomplètes.)*
