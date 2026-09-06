# Projet 10 : Développez une solution en microservices pour votre client

**Date de création** : 06 septembre 2026

**Date de la dernière modification** : 06 septembre 2026

**Auteur** : Alban VOIRIOT

**Informations techniques** :

* **Technologies** : C#, HTML, CSS, Bootstrap, ASP.NET MVC, API REST, SQL Server, MongoDB
* **Version de .NET** : 8.0
* **Architecture** : Microservices, Docker, Docker Compose, Ocelot
* **Conteneurisation** : Docker / Docker Compose

## Sommaire

* [Contexte](#contexte)
* [Installation](#installation)

  * [Prérequis](#prérequis)
  * [Télécharger le projet](#télécharger-le-projet)
  * [Lancer le projet avec Docker](#lancer-le-projet-avec-docker)
  * [Accéder à l'application](#accéder-à-lapplication)
  * [Arrêter le projet](#arrêter-le-projet)
* [Green Code](#green-code)

  * [Qu'est-ce que le Green Code ?](#quest-ce-que-le-green-code-)
  * [Enjeux et objectifs](#enjeux-et-objectifs)
  * [Application du Green Code dans le projet](#application-du-green-code-dans-le-projet)
  * [Bonnes pratiques appliquées](#bonnes-pratiques-appliquées)

## Contexte

Ce projet a été conçu dans le cadre de ma formation de développeur d'applications Back-end .NET (OpenClassrooms).

L'objectif de ce projet est de développer une solution en architecture microservices pour l'application MediLabo.

L'application permet notamment de gérer les informations concernant des patients ainsi que leur historique avec les notes d'observation du médecin tout en évaluant le niveau de risque de développer un diabète de Type 2.

Le projet est composé de plusieurs services communiquant entre eux via des API REST.

Un API Gateway basé sur **Ocelot** permet de centraliser les communications entre le FrontPatient et les différents microservices.

Les données sont stockées dans différentes bases de données selon les besoins des services :

* **SQL Server** pour les informations des patients.
* **MongoDB** pour les notes et l'historique des patients.
* Et le rapport va communiquer avec les deux microservices (gestion patients et notes).

L'ensemble de l'application est conteneurisé avec **Docker** et orchestré avec **Docker Compose**.

## Installation

### Prérequis

Avant d'installer le projet, les éléments suivants doivent être installés sur votre ordinateur :

* **Git**
* **Docker Desktop**
* **Docker Compose** (inclus avec les versions récentes de Docker Desktop)

Aucune installation manuelle de SQL Server ou MongoDB n'est nécessaire pour lancer les services concernés dans Docker, car ils contiennent leurs propres conteneurs.

### Télécharger le projet

Pour télécharger le projet, ouvrez un terminal dans le dossier dans lequel vous souhaitez installer le projet puis exécutez la commande :

```bash
git clone https://github.com/Alban2001/Projet10Medilabo.git
```

Accédez ensuite au dossier du projet :

```bash
cd APIMedilabo
```

### Lancer le projet avec Docker

Vérifiez que **Docker Desktop est démarré**, puis placez-vous dans le dossier contenant le fichier `docker-compose.yml`.

Lancez ensuite l'ensemble des services avec :

```bash
docker compose up --build
```

L'option `--build` permet de reconstruire les images Docker à partir des Dockerfiles du projet.

Lors du premier lancement, Docker va :

1. Construire les différentes images des microservices.
2. Créer les conteneurs nécessaires.
3. Créer le réseau Docker utilisé par les différents services.
4. Démarrer les différentes API.
5. Démarrer l'API Gateway Ocelot.
6. Démarrer l'application FrontPatient.
7. Démarrer les conteneurs pour les bases de données MongoDB et SQLServer.
8. Créer et utiliser les volumes Docker nécessaires à la persistance des données.

Une fois les conteneurs démarrés, il est possible de vérifier leur état depuis Docker Desktop ou avec la commande :

```bash
docker compose ps
```

### Accéder à l'application

Une fois les conteneurs démarrés, l'application FrontPatient est accessible depuis un navigateur à l'adresse :

```text
http://localhost:32770/
```

La liste des patients est accessible à l'adresse :

```text
http://localhost:32770/patient/patients
```

L'API Gateway est accessible sur le port :

```text
http://localhost:32768/patient/patients
```

Les autres services utilisent également des ports dédiés définis dans le fichier `docker-compose.yml`.

### Arrêter le projet

Pour arrêter les conteneurs sans supprimer les données persistantes :

```bash
docker compose down
```

Pour redémarrer le projet :

```bash
docker compose up
```

Après une modification du code ou d'un Dockerfile, les images peuvent être reconstruites avec :

```bash
docker compose up --build
```

### Attention concernant les volumes Docker

SQLServer et MongoDB utilise un volume Docker afin de conserver les données lorsque les conteneurs sont arrêtés ou recréés afin d'éviter de les perdre et de retrouver avec des bases vides.

La commande :

```bash
docker compose down
```

arrête et supprime les conteneurs mais conserve les volumes.

En revanche :

```bash
docker compose down -v
```

supprime également les volumes Docker et donc les données persistantes associées à MongoDB et SQLServer

Il est donc recommandé de ne pas utiliser l'option `-v` si vous souhaitez conserver les données de la base MongoDB.

## Green Code

### Qu'est-ce que le Green Code ?

Le **Green Code** consiste à concevoir et développer des applications en cherchant à réduire leur impact environnemental.

L'utilisation d'une application nécessite des ressources informatiques : processeur, mémoire vive, stockage, réseau et énergie électrique.

Plus une application effectue de traitements inutiles ou transfère de grandes quantités de données, **plus elle consomme de ressources**.

L'objectif du Green Code est donc de concevoir des applications qui fournissent le même service tout en utilisant le moins de ressources informatiques possible.

Le Green Code ne concerne pas uniquement le code source. Il peut également être appliqué à :

* l'architecture de l'application ;
* la base de données ;
* les échanges réseau ;
* les fichiers et ressources utilisés ;
* les serveurs et conteneurs ;
* le déploiement de l'application ;
* le cycle de vie du logiciel.

Dans une architecture microservices, cette réflexion est particulièrement importante car plusieurs services peuvent fonctionner simultanément.

### Enjeux et objectifs

L'objectif principal du Green Code est de **réduire la consommation de ressources nécessaires au fonctionnement d'une application**.

Les principaux enjeux sont :

* réduire la consommation énergétique ;
* limiter les traitements CPU inutiles ;
* réduire l'utilisation de la mémoire ;
* limiter les échanges réseau ;
* réduire les volumes de données transférés ;
* optimiser les requêtes vers les bases de données ;
* limiter les ressources utilisées par les conteneurs ;
* prolonger la durée de vie des équipements informatiques ;
* améliorer les performances globales de l'application.

Le Green Code peut également avoir des effets positifs sur les performances et les coûts d'infrastructure.

Une application plus légère et mieux optimisée peut nécessiter moins de ressources pour fournir le même service.

### Application du Green Code dans le projet

Le projet MediLabo utilise une architecture composée de plusieurs microservices exécutés dans des conteneurs Docker.

Cette architecture permet de séparer les différentes responsabilités de l'application, mais elle peut également entraîner l'exécution simultanée de plusieurs services.

Plusieurs bonnes pratiques peuvent donc être appliquées afin de limiter les ressources utilisées.

#### Optimisation des appels API

Les appels vers les API doivent être limités aux données réellement nécessaires.

Il faut éviter de multiplier les appels pour récupérer plusieurs fois les mêmes informations.

Par exemple, lorsqu'une information peut être récupérée dans une seule requête, il est préférable d'éviter plusieurs appels successifs vers le même microservice.

L'utilisation d'appels asynchrones avec `async` et `await` permet également de ne pas bloquer inutilement les ressources lors des opérations d'entrée/sortie.

Exemple :

```csharp
var patients = await _patientService.GetPatients();
```

plutôt qu'une opération synchrone qui bloquerait inutilement un thread pendant l'attente de la réponse.

#### Optimisation des requêtes SQL

Les requêtes SQL doivent récupérer uniquement les informations nécessaires.

Il faut éviter de récupérer de grandes quantités de données lorsque seules quelques informations sont nécessaires.

Lorsque cela est possible, il est préférable d'utiliser des projections pour sélectionner uniquement les colonnes nécessaires.

Exemple :

```csharp
var patients = await _context.Patients
    .Select(p => new
    {
        p.Id,
        p.Nom,
        p.Prenom
    })
    .ToListAsync();
```

Cette approche permet de limiter les données récupérées depuis la base.

#### Optimisation de MongoDB

Le même principe est appliqué avec MongoDB.

Les recherches doivent être ciblées afin d'éviter de parcourir inutilement l'ensemble de la collection.

Dans le projet, les notes peuvent notamment être recherchées directement à partir de l'identifiant du patient :

```csharp
_collection
    .Find(x => x.PatientId == idPatient)
    .ToListAsync();
```

La création d'index adaptés peut également permettre d'accélérer les recherches et de réduire les ressources nécessaires aux requêtes.

#### Utilisation de Docker

Docker permet d'isoler les différents services du projet dans des conteneurs.

Afin de limiter la consommation de ressources, il est recommandé de :

* utiliser des images Docker légères ;
* utiliser des images de base adaptées au besoin ;
* utiliser des builds multi-stage pour les applications .NET ;
* ne pas installer de logiciels inutiles dans les images ;
* supprimer les fichiers temporaires et artefacts inutiles ;
* ne lancer que les services nécessaires.

Les images utilisées par le projet doivent donc rester aussi légères que possible.

#### Réduction des données transférées

Les communications entre les microservices passent par HTTP et peuvent donc générer des échanges réseau.

Il est préférable de ne pas transférer des données inutiles.

Les API doivent retourner uniquement les informations nécessaires au consommateur.

L'utilisation de DTO permet notamment de contrôler précisément les données retournées par les API.

#### Gestion des ressources

Les ressources utilisées par l'application doivent être libérées correctement.

Les connexions aux bases de données, les flux et les ressources réseau doivent notamment être correctement gérés.

L'utilisation des mécanismes fournis par .NET, comme `using`, `using var`, `async` et `await`, permet d'améliorer la gestion des ressources.

### Bonnes pratiques appliquées

Dans le cadre de ce projet, plusieurs bonnes pratiques de Green Code peuvent être mises en place :

* utiliser des opérations asynchrones pour les appels réseau et les accès aux bases de données ;
* limiter les appels vers les microservices ;
* éviter de récupérer des données inutiles depuis SQL Server et MongoDB ;
* utiliser des requêtes ciblées ;
* mettre en place des index sur les champs fréquemment recherchés dans MongoDB ;
* utiliser des DTO pour limiter les données transmises par les API ;
* optimiser la taille des images Docker ;
* utiliser des Dockerfiles multi-stage ;
* supprimer les dépendances et fichiers inutiles des images ;
* limiter le nombre de conteneurs exécutés lorsque certains services ne sont pas nécessaires ;
* conserver les données dans des volumes afin d'éviter des opérations de reconstruction inutiles ;
* éviter les traitements répétitifs ou inutiles ;
* limiter les logs produits en production.

L'objectif est de trouver un équilibre entre **performance, consommation de ressources, maintenabilité et fonctionnalités**.

Le Green Code est donc pris en compte dès la conception de l'architecture et doit continuer à être appliqué lors du développement et de la maintenance du projet.
