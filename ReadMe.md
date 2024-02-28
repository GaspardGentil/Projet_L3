# SA1. Modélisation distribuée d’un jeu stratégique - Exemple du tactical RPG

1. [Subject](#subject)
2. [Members](#members)
3. [Description](#description)
4. [Objectives](#objectives)
	- [How to use ?](#how-to-use)

## Subject

L’objectif de ce projet est de proposer une modélisation distribuée d’un jeu stratégique. 
Nous prendrons comme exemple le tactical RPG. Nous considérerons des personnages qui évoluent sur un environnement dynamique. 
Les personnages appartiennent à plusieurs groupes. Les personnages peuvent se déplacer en même temps selon des règles préétablies. 
Nous implémenterons une méthode de résolution distribuée avec des entités autonomes dotée de comportements intelligents. 
Nous commencerons par définir des stratégies de résolution simples dans lesquelles les personnages effectuent des déplacements uniquement sur la base de leur perception. 
Dans une deuxième étape, nous définirons des stratégies de résolution plus fines permettant aux personnages de construire des stratégies de jeu collectives en interagissant avec leur voisinage. 
Plusieurs exemples de jeux ont été déjà implémentés par les étudiants. Nous pourrons mettre a disposition leur code. 
Des exemples de développements se trouvent a cette adresse : https://perso.liris.cnrs.fr/samir.aknine/L3/

## Members

- Bouhouch Roua
- Mao Manita
- Salah Mansour Masten
- Gentil Gaspard

## Description

We are using unity to create a basic life simulation game.

### How to use ? (cloning the project)

You need to have Unity installed first (recommended 2022.3.18f1) with the module microsoft visual studio.
Now open visual studio and use git to clone the depository (URL recommended) to the directory of your choice.
Now in Unity Hub, select Projects -> Add -> Add project from disk, and select "unity_project" in the "lifprojet" folder cloned.
You should now be able to launch the project and edit it in the Unity editor as a normal project.
After modification, simply use visual studio's git tab to commit and/or push to the Gitlab repository..

## Objectives (any contributor can add it's own objective)

- Individual behavior 
- Group behavior
- Navigation

