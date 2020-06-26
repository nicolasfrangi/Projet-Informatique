Dans cette rubrique, nous allons voir comment constituer la base de donnée afin de la faire marcher avec notre projet C#

1) Dans un premier temps, il suffit de lancer l'application MySql Workbench et de vous y connecter.

2) Ensuite, veillez à créer un nouveau projet, le nommer comme bon vous semble ("cooking" pour plus de faciliter) et y copier/coller le code suivant.

<Code à coller>

create database cooking;
use cooking;
drop table if exists fourniture;
drop table if exists composition;
drop table if exists commande;
drop table if exists creation;
drop table if exists fournisseur;
drop table if exists produit;
drop table if exists recette;
drop table if exists client;
CREATE TABLE `cooking`.`client`(
	`id_client` VARCHAR(40) not null,
    `mot_passe` VARCHAR(40) not null,
    `nom_client` VARCHAR(40) not null,
    `prenom_client` VARCHAR(40) not null,
    `telephone_client` VARCHAR(40) not null,
    `adresse_client` VARCHAR(40) not null,
    `ville_client` VARCHAR(40) not null,
    `createur_recette` float null,
    `solde_cook` float null,
    `compteur_cdr` int null,
    PRIMARY KEY (`id_client`));




CREATE TABLE `cooking`.`recette`(
	`id_recette` VARCHAR(40) not null,
	`prix_recette` float null,
	`nom_recette` VARCHAR(40) not null,
    `description_recette`VARCHAR(40) not null,
    `type_recette`VARCHAR(40) not null,
    `id_client`VARCHAR(40) not null,
    PRIMARY KEY (`id_recette`));
ALTER TABLE recette ADD CONSTRAINT FK_recette_id_client foreign key(id_client) references client(id_client);


CREATE TABLE `cooking`.`commande`(
	`id_commande` int null,
    `prix_commande` float null,
    `quantite_commande` int null,
    `date_commande` dateTime null,
    `id_recette` VARCHAR(40) not null,
    `id_client` VARCHAR(40) not null);
ALTER TABLE commande ADD CONSTRAINT FK_commande_id_recette foreign key(id_recette) references recette(id_recette);
ALTER TABLE commande ADD CONSTRAINT FK_commande_id_client foreign key(id_client) references client(id_client);    

CREATE TABLE `cooking`.`creation`(
	`id_creation` VARCHAR(40) not null,
    `date_creation` dateTime null,
    `id_client` VARCHAR(40) not null,
    `id_recette` VARCHAR(40) not null);
ALTER TABLE creation ADD CONSTRAINT FK_creation_id_recette foreign key(id_recette) references recette(id_recette);
ALTER TABLE creation ADD CONSTRAINT FK_creation_id_client foreign key(id_client) references client(id_client);

CREATE TABLE `cooking`.`produit`(
	`id_produit` VARCHAR(40) not null,
    `categorie_produit` VARCHAR(40) not null,
	`nom_produit` VARCHAR(40) not null,
    `stock_produit` int null,
    `stock_min` int null,
    `stock_max` int  null,
    PRIMARY KEY (`id_produit`));
    
CREATE TABLE `cooking`.`composition`(
	`quantite_produit` int null,
    `id_recette` VARCHAR(40) not null,
    `id_produit` VARCHAR(40) not null);
ALTER TABLE composition ADD CONSTRAINT FK_composition_id_recette foreign key(id_recette) references recette(id_recette);
ALTER TABLE composition ADD CONSTRAINT FK_composition_id_produit foreign key(id_produit) references produit(id_produit);

CREATE TABLE `cooking`.`fournisseur`(
	`nom_fournisseur` VARCHAR(40),
    `id_fournisseur` VARCHAR(40),
    `telephone_fournisseur` VARCHAR(40),
    PRIMARY KEY(`id_fournisseur`));

CREATE TABLE `cooking`.`fourniture`(
	`quantite_fourniture` int null,
    `id_produit` VARCHAR(40) not null,
    `id_fournisseur` VARCHAR(40) not null);
ALTER TABLE fourniture ADD CONSTRAINT FK_fourniture_id_produit foreign key(id_produit) references produit(id_produit);
ALTER TABLE fourniture ADD CONSTRAINT FK_fourniture_id_fournisseur foreign key(id_fournisseur) references fournisseur(id_fournisseur);

-- insertion client 
INSERT INTO client values ('C0000','0000','0000','0000','0000','0000','0000',0,1000,10);
INSERT INTO client values ('C0001','R1clark','Clark','Richard','0728374636','1 boulevard des roches','Paris',1,100,7);
INSERT INTO client values ('C0002','F2nicolas','Frangi','Nicolas','0658372910','12 avenue des lilas','Paris',0,100,15);
INSERT INTO client values ('C0003','T3tom','Trevis','Tom','0736332718','3 chemin des araignees','Paris',1,100,8);
INSERT INTO client values ('C0004','P4chris','Polto','Chris','0722281909','54 boulevard Ruman','Paris',0,100,5);
INSERT INTO client values ('C0005','P5jean','Paco','Jean','0725432909','5 rue Durenche','Paris',0,100,6);
INSERT INTO client values ('C0006','D6mine','Dari','Mine','0748291909','6 boulevard Mars','Paris',0,100,5);
INSERT INTO client values ('C0007','F7phil','Frago','Phil','0722002989','54 boulevard Ruman','Paris',0,100,5);

-- insertion recette
INSERT INTO recette values('R0001',12,'Pate carbonara','pate lardon','plat','C0001');
INSERT INTO recette values('R0002',15,'Lasagne','pate fine, bolognaise et gruyere','plat','C0001');
INSERT INTO recette values('R0003',22,'Foie gras poele','foie gras poele','entree','C0002');
INSERT INTO recette values('R0004',10,'Salade nicoise','salade, noix, tomate, concombre','entree','C0002');
INSERT INTO recette values('R0005',10,'Tiramisu','boudoire mascarpone','dessert','C0002');
INSERT INTO recette values('R0006',16,'Truite','truitefumee','plat','C0002');
INSERT INTO recette values('R0007',6,'Oeuf mimosa','oeuf mimosa','entree','C0000');
INSERT INTO recette values('R0008',14,'Moule','Facon fifi','plat','C0000');
INSERT INTO recette values('R0009',20,'Caviar','Caviar','plat','C0000');
INSERT INTO recette values('R0010',20,'Soupe Legume','Multi de legumes','entree','C0002');
INSERT INTO recette values('R0011',20,'Soupe Legume bis','Duo de legumes','entree','C0002');



-- insertion produit
INSERT INTO produit values('P0001','Viande','poulet', 90, 10, 1000);
INSERT INTO produit values('P0002','Poisson','truite', 20, 4, 500);
INSERT INTO produit values('P0003','Feculent','pate', 150, 10, 2000);
INSERT INTO produit values('P0004','Legume','tomate', 200, 4, 2500);
INSERT INTO produit values('P0005','Legume','poivron', 100, 4, 2000);
INSERT INTO produit values('P0006','Legume','salade', 100, 4, 2000);
INSERT INTO produit values('P0007','Legume','courgette', 100, 4, 2000);
INSERT INTO produit values('P0008','Legume','castraveti', 100, 4, 2000);
INSERT INTO produit values('P0009','Legume','haricot', 100, 4, 2000);
INSERT INTO produit values('P0010','Legume','melon', 100, 4, 2000);
INSERT INTO produit values('P0011','Viande','boeuf', 90, 10, 1000);
INSERT INTO produit values('P0012','Poisson','saumon', 20, 4, 500);
INSERT INTO produit values('P0013','Fruit','fraise', 20, 4, 500);
INSERT INTO produit values('P0014','Fruit','peche', 20, 4, 500);
INSERT INTO produit values('P0015','Fruit','poire', 20, 4, 500);
INSERT INTO produit values('P0016','Fruit','ananas', 20, 4, 500);
INSERT INTO produit values('P0017','Biscuit','boudoire', 20, 4, 500);
INSERT INTO produit values('P0018','Creme','mascarpone', 20, 4, 500);
INSERT INTO produit values('P0019','Lard','lardon', 20, 4, 500);
INSERT INTO produit values('P0020','Poisson','caviar', 6, 4, 12);
INSERT INTO produit values('P0021','Legume','fenouille', 7, 4, 400);
INSERT INTO produit values('P0022','Legume','choux', 2, 10, 400);
INSERT INTO produit values('P0023','Fruit','raisin', 7, 10, 400);



-- insertion composition
INSERT INTO composition values(2,'R0003','P0008');
INSERT INTO composition values(2,'R0004','P0004');
INSERT INTO composition values(2,'R0004','P0005');
INSERT INTO composition values(2,'R0004','P0006');
INSERT INTO composition values(2,'R0004','P0007');
INSERT INTO composition values(2,'R0004','P0004');
INSERT INTO composition values(2,'R0006','P0002');
INSERT INTO composition values(2,'R0009','P0020');

INSERT INTO composition values(1,'R0002','P0011');
INSERT INTO composition values(2,'R0002','P0004');
INSERT INTO composition values(3,'R0005','P0017');
INSERT INTO composition values(1,'R0005','P0016');
INSERT INTO composition values(4,'R0002','P0004');
INSERT INTO composition values(2,'R0002','P0004');
INSERT INTO composition values(2,'R0001','P0019');
INSERT INTO composition values(2,'R0001','P0003');
INSERT INTO composition values(2,'R0010','P0021');
INSERT INTO composition values(2,'R0011','P0021');
INSERT INTO composition values(2,'R0010','P0005');
INSERT INTO composition values(2,'R0010','P0006');
INSERT INTO composition values(2,'R0010','P0007');

-- insert into commande
INSERT INTO commande values(1,2,1,'2019/1/1','R0001','C0000');
INSERT INTO commande values(2,2,1,'2020/1/2','R0003','C0001');
INSERT INTO commande values(3,2,1,'2020/1/2','R0003','C0000');
INSERT INTO commande values(4,2,1,'2020/1/4','R0002','C0002');
INSERT INTO commande values(5,2,1,'2020/1/6','R0003','C0000');
INSERT INTO commande values(6,2,1,'2020/1/6','R0002','C0000');
INSERT INTO commande values(7,2,1,'2020/1/6','R0002','C0005');
INSERT INTO commande values(8,2,1,'2020/1/6','R0002','C0006');
INSERT INTO commande values(9,2,1,'2020/1/6','R0004','C0002');
INSERT INTO commande values(10,2,1,'2020/1/7','R0004','C0002');
INSERT INTO commande values(11,2,1,'2020/1/7','R0005','C0003');
INSERT INTO commande values(12,2,1,'2020/1/8','R0006','C0001');
INSERT INTO commande values(13,2,1,'2020/1/8','R0005','C0000');
INSERT INTO commande values(14,2,1,'2020/1/9','R0003','C0000');
INSERT INTO commande values(15,2,1,'2020/1/9','R0003','C0002');
INSERT INTO commande values(16,2,1,'2020/1/10','R0007','C0002');

-- insertion creation
INSERT INTO creation values('1','2020/1/8','C0000','R0006');
INSERT INTO creation values('2','2020/1/9','C0002','R0004');
INSERT INTO creation values('3','2020/1/9','C0002','R0003');
INSERT INTO creation values('4','2020/1/9','C0001','R0002');
INSERT INTO creation values('5','2020/1/7','C0002','R0005');
INSERT INTO creation values('6','2020/1/9','C0001','R0001');
INSERT INTO creation values('7','2020/1/9','C0003','R0007');



-- insertion fournisseur
INSERT INTO fournisseur values('LegumeVerger','F0001','0738288192');
INSERT INTO fournisseur values('FruitLegume','F0002','0623131198');
INSERT INTO fournisseur values('Cueillette','F0003','0647739178');

</Code à coller>

3) Pensez à executer les quelques lignes de code ci-dessus dans votre sql workbench afin de créer les tables et les insertions correctement.

4) De là, il vous faut ensuite aller dans le projet C# et modifier dans chaque classe toutes les zones suivantes par: "SERVER = localhost; PORT = 3306; DATABASE = nom_projet; UID = root; PASSWORD = mot_de_passe";

5) Ensuite, il vous suffit simplement de lancer le projet C#. 
