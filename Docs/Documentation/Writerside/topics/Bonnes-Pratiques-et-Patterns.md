# Bonnes Pratiques et Patterns

## Bonnes pratiques

### Git

- **Commit régulièrement** : il est important de faire des commits réguliers afin de pouvoir revenir en arrière si besoin. Cela permet également de mieux suivre l'évolution du projet.
- **Faire des commits cohérents** : il est important de faire des commits cohérents, c'est-à-dire que chaque commit doit être indépendant des autres. Cela permet de pouvoir revenir en arrière sur un commit sans avoir à revenir sur les autres.

### Code

- **Utiliser des noms explicites** : il est important d'utiliser des noms de variables/fonctions explicites afin de pouvoir comprendre le code plus facilement. Cela permet également de pouvoir faire une documentation plus facilement.
- **Utiliser des interfaces** : il est important d'utiliser des interfaces afin de pouvoir réutiliser du code plus facilement. Cela permet également de pouvoir comprendre le code plus facilement.
- **Utiliser des tests unitaires** : il est important d'utiliser des tests unitaires afin de pouvoir tester le code plus facilement. Cela permet également de pouvoir comprendre le code plus facilement.

## Patterns

### MVC

Le pattern MVC (Modèle-Vue-Contrôleur) est un pattern qui permet de séparer les données, la logique et l'interface graphique d'une application.

- **Modèle** : le modèle représente les données de l'application. Il contient les données ainsi que les méthodes permettant de les manipuler.
- **Vue** : la vue représente l'interface graphique de l'application. Elle contient les éléments graphiques ainsi que les méthodes permettant de les manipuler.
- **Contrôleur** : le contrôleur représente la logique de l'application. Il contient les méthodes permettant de manipuler les données et l'interface graphique.

Dans ce projet, le modèle est séparé dans un projet à part, afin que l'API et la WebApp puissent l'utiliser. La vue est séparée dans un projet à part, afin que la WebApp puisse l'utiliser.

### ORM

L'utilisation d'un ORM (Object-Relational Mapping) permet de manipuler les données de la base de données comme des objets. Cela permet de ne pas avoir à écrire de requêtes SQL.
J'ai utilisé l'ORM Entity Framework Core, qui est un ORM open-source développé par Microsoft.

### Repository

Le pattern Repository permet de séparer la logique de l'application de la logique de la base de données. Cela permet de pouvoir changer de base de données plus facilement.
