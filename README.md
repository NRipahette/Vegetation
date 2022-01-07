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

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image6.png)

t = Tronc "Traité"  
      
Le Tronc “Traité” est identique au tronc et trouve son utilité dans l’élaboration des règles.

C = Branche principale

Ces branches partent du tronc, ce sont des cylindres de rayon 0.2 et de hauteur 2, avec une texture de bois importée de l’asset store.

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image9.png)

c = Branche principale "traitée"

La branche principale “Traitée” est identique aux branches principales et trouve son utilité dans l’élaboration des règles.

B = Branche intermédiaire
 
Ces branches sont physiquement identiques aux branches principales mais sont les supports des petites branches qui vont terminer l’arborescence des branches.

b = Branche intermédiaire "traitée"

Les Branches intermédiaires “Traitée” sont identiques aux Branches intermédiaires et trouvent leur utilité dans l’élaboration des règles.

D = Petite Branche

La Petite branche ou “Branche Fine” est la dernière branche de l’arbre et est support des feuilles

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image8.png)

d = Petite Branche "traitée"

La Petite branches “Traitée” est identique à la petite branche et trouve son utilité dans l’élaboration des règles.

F = Feuille

C’est le sprite de feuille que nous avons utilisé pour notre arbre, nous avons choisis ce sprite car nous avons réalisé des règles qui s’appliquent plus à des sapins / conifères.

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image2.png)

Et pour chaque symbole F nous avons un prefab constitué de trois sprites pour obtenir un résultat plus touffu.

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image1.png)

Le reste des symboles “utilitaires” qui nous permettent de manipuler les prefabs représentés par les symboles précédents.

- + = Rotation sur l'axe X (sens positif ou négatif)

/ * = Rotation sur l'axe Z (sens positif ou négatif)

& = Rotation sur l'axe Y (positif)

Ces différents symboles permettent d’appliquer au GameObject courant une rotation selon divers axes avec un sens positif ou négatif. Cela permet d’orienter la construction de l’arbre au fur et à mesure des itérations.

[ ] = Hiérarchie de la chaîne

La hiérarchie permet d’enregistrer à un point donné la position et la rotation d’un prefab, comme un checkpoint, ensuite cela permet de générer une branche entièrement et de reprendre la génération au point où on l’avait laissé. Ici on se sert aussi de cette hiérarchie pour enregistrer une échelle, et au fur et à mesure des hiérarchies imbriquées, les modèles que nous faisons apparaître rétrécissent pour donner un effet plus réaliste.

# Règles

Tronc : T -> t[+C][-C][&+C][&-C][&&+C][&&-C]t[T]

Chaque tronc donne un objet tronc auquel on associe 5 branches principales avec des rotations pour les faire partir dans des directions différentes. On ajoute une instance de tronc à la suite du premier pour faire grandir l’arbre puis on termine avec un tronc itérable entre crochets pour permettre la mise à l’échelle progressive.

Branche principale : C -> c[&BF][&&&BF][C]

Chaque branche principale donne une instance de branche principale à laquelle on connecte 2 branches intermédiaires avec des rotations selon l’axe Y pour les distribuer sur les côtés de la branche principale. Enfin, on ajoute une future branche principale pour permettre d’allonger la branche lors de la prochaine itération.

Branche intermédiaire : B -> b[+D][D]

Chaque branche intermédiaire donne une instance de branche intermédiaire sur laquelle on connecte 2 petites branches qui restent itérables.

Petite branche : D ->  d[+F][-F]

Chaque petite branche donne une instance terminale de l’objet petite branche à laquelle sont reliés 2 feuilles.

# Fonctionnement et Utilisation

Pour utiliser notre script, il suffit de l’attacher à un GameObject dans la scène Unity. Chaque instance du script permet de générer un arbre.

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image4.png)

Le script possède plusieurs attributs paramétrables qui vont modifier le comportement du script et l’aspect de l'arbre créé.
Le premier paramètre est Save As Prefab, un booléen qui permet de choisir si on veut que notre arbre généré soit sauvegardé en tant que prefab.
Alphabet : C’est l’alphabet qui constitue la base de l’abre.
Branch weight: C'est l’angle vertical des branches principales par rapport au tronc, plus la valeur sera proche de 0, plus les branches seront dirigées vers le haut, et plus la valeur sera proche de 180, plus elles seront vers le bas.
Espace Branches Principales : C’est l’angle avec lequel les branches principales apparaissent autour du tronc. La variable est utilisée dans l’action du symbole &.
Espace Petite Branche : C’est l’angle avec lequel les petites branches apparaissent par rapport à la direction de la branche sur laquelle elles sont accrochées.
OBJ_Tronc: L’emplacement du prefab pour le tronc.
OBJ_Branche: L’emplacement du prefab pour les branches principales et intermédiaires.
OBJ_Branche Fine : L’emplacement du prefab pour les petites branches.
OBJ_Feuille: L’emplacement du prefab pour les feuilles.

![name-of-you-image](https://github.com/NRipahette/Vegetation/blob/main/image3.png)


# Bibliographie

L-Systems Unity Tutorial, Pete P

https://www.youtube.com/watch?v=tUbTGWl-qus

L-System User Notes, Paul Rourke

http://www.paulbourke.net/fractals/lsys/

L-system, Wikipedia

https://en.wikipedia.org/wiki/L-system
