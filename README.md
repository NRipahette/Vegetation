# Introduction
Ce rapport a pour objectif de décrire le travail que nous avons effectué dans le cadre de l’enseignement Mondes Virtuels sur la génération procédurale d’objets 3D avec le moteur Unity.
# Objectifs
Notre projet porte spécifiquement sur la génération procédurale de végétaux en employant un L-Système.

Le L-Système est un algorithme qui génère une chaîne de caractères à partir d’une chaîne de départ par itérations en se basant sur un alphabet défini. A chaque itération la phrase est lue en remplaçant chaque symbole (ou lettre) par la chaîne de caractère qui lui est associée, on appelle ces associations des règles.

Nous nous sommes concentrés sur la génération d’arbres.

# Symboles
Pour la création de nos arbres, nous avons créé un alphabet constitué de tous ces symboles. La plupart des symboles ont une version minuscule qui signifie qu’ils ont été “traités”, ils sont associés à des règles qui ne font que faire subsister le symbole dans la chaîne générée, ce qui permet qu’au fur et à mesure des itérations, les règles ne se réappliquent pas sur les parties de l’arbre déjà traitées.

Ici nous allons détailler les symboles et leurs représentations dans Unity lors de la génération des arbres à partir de la chaîne.

T = Tronc 

Le Tronc est la partie la plus large de l’arbre et sert de base aux branches principales.
C’est un cylindre de rayon 1 et de hauteur 2, avec une texture de bois importée de l’asset store. Toutes les parties du tronc sont générées en ligne droite et réduit son diamètre progressivement au fur et à mesure que l'arbre grandit.

# Règles
# Fonctionnement et Utilisation
# Bibliographie
