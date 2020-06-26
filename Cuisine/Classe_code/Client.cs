using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cooking
{
    class Client
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);
        #endregion

        #region attribut, proprietes, constructeur, affichage

        public string id_client;
        public string mot_passe;
        public string nom_client;
        public string prenom_client;
        public string telephone_client;
        public string adresse_client;
        public string ville_client;
        public int createur_recette;// 0 non 1 oui 
        public int solde_cook;
        public int compteur_cdr;

        public Client(string idc = "", string mdp = "",string nc = "", string pc = "", string tc = "", string adc = "", string vc = "", int cr = 0, int sc = 100,int cdr=0)
            {
            this.id_client = idc;
            this.mot_passe = mdp;
            this.nom_client = nc;
            this.prenom_client = pc;
            this.telephone_client = tc;
            this.adresse_client = adc;
            this.ville_client = vc;
            this.createur_recette = cr;
            this.solde_cook = sc;
            this.compteur_cdr = cdr;
            }

        public Client(MySqlDataReader reader)
        {
            this.id_client = reader.GetString(0);
            this.mot_passe = reader.GetString(1);
            this.nom_client = reader.GetString(2);
            this.prenom_client = reader.GetString(3);
            this.telephone_client = reader.GetString(4);
            this.adresse_client = reader.GetString(5);
            this.ville_client = reader.GetString(6);
            this.createur_recette = reader.GetInt32(7);
            this.solde_cook = reader.GetInt32(8);
            this.compteur_cdr = reader.GetInt32(9);
        }

        public string Id_client { get { return this.id_client; } set { this.id_client = value; } }
        public string Mot_passe { get { return this.mot_passe; } set { this.mot_passe = value; } }
        public string Nom_client { get { return this.nom_client; } set { this.nom_client = value; } }
        public string Prenom_client { get { return this.prenom_client; } set { this.prenom_client = value; } }
        public string Telephone_client { get { return this.telephone_client; } set { this.telephone_client = value; } }
        public string Adresse_client { get { return this.adresse_client; } set { this.adresse_client = value; } }
        public string Ville_client { get { return this.ville_client; } set { this.ville_client = value; } }
        public int Createur_recette { get { return this.createur_recette; } set{ this.createur_recette=value; } }
        public int Solde_cook { get { return this.solde_cook; } set { this.solde_cook = value; } }
        public int Compteur_cdr { get { return this.compteur_cdr; } set { this.compteur_cdr = value; } }

        public override string ToString()
        {
            return "\nId client : " + this.id_client + "\nMot de passe : " + this.mot_passe
                + "\nNom client : " + this.nom_client + "\nPrenom client : " + this.prenom_client + "\nTelephone : " + this.telephone_client
                + "\nAdresse : " + this.adresse_client + "\nVille : " + this.ville_client + "\nCreateur recette : " + this.createur_recette
                + "\nSolde cook : " + this.solde_cook+"\nCompteur cdr : "+this.Compteur_cdr;
        }



        #endregion
        
        #region Fonction connexion base de donnee
        public void ConnexionCooking(string requeteExecute)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requeteExecute; //On lance la requete lors de l'appel de la fonction
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" Erreur : " + e.ToString());
                Console.WriteLine("\n Id de recette deja pris ou identifiant produit erroné ! ");
                Console.ReadLine();
            }
        }

        #endregion

        #region Creation client
        public void CreationClientDansBDD()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select id_client from client order by id_client;"; //On lance la requete lors de l'appel de la fonction
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("\nId de client deja utilisés:");
            while (reader.Read())
            {
                Console.Write(reader.GetString(0) + " / ");
            }
            connection.Close();
            
                Console.WriteLine("\n\nSaisir l'ID (CXXXX) :");
                string id = Console.ReadLine();
                Console.WriteLine("Saisir le mot de passe :");
                string mdp = Console.ReadLine();
                Console.WriteLine("Saisir le nom :");
                string nom = Console.ReadLine();
                Console.WriteLine("Saisir le prenom :");
                string prenom = Console.ReadLine();
                Console.WriteLine("Saisir le telephone :");
                string telephone = Console.ReadLine();
                Console.WriteLine("Saisir l'adresse :");
                string adresse = Console.ReadLine();
                Console.WriteLine("Saisir la ville :");
                string ville = Console.ReadLine();
                Console.WriteLine("Saisir createur de recette (0 non/1 oui) :");
                int createur =int.Parse( Console.ReadLine());
                ConnexionCooking("INSERT INTO client values(\'" + id + "\',\'" + mdp + "\',\'" + nom + "\',\'" + prenom + "\',\'"
                    + telephone + "\',\'" + adresse + "\',\'" + ville + "\',\'" + createur + "\',\'" + 100 + "\',\'" + 0 + "\');");   
        
            
            }
        #endregion

        #region Creation d'une liste des clients de la base de donnee
        public List<Client> ListeClient(string requeteExecute)
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;
            MySqlDataReader reader = command.ExecuteReader();

            List<Client> listeclient = new List<Client>();

            while (reader.Read())
            {
                listeclient.Add(new Client(reader));
            }
            connection.Close();
            return listeclient;
        }
        #endregion

        #region Createur de recette Max
        public void CDR_Max() //celui qui a le plus grand nombre de Cdr
        {
            Console.Clear();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText=("Select id_client,prenom_client,nom_client, compteur_cdr from client order by compteur_cdr desc limit 1;");
            MySqlDataReader reader = command.ExecuteReader();

            string pre;string nom;int comp;
            while(reader.Read())
            {
                pre = reader.GetString(1);
                nom = reader.GetString(2); comp = reader.GetInt32(3);
                Console.WriteLine(pre + " " + nom + " possède : " + comp+" CDR");
                Console.ReadKey();
            }
        }
        #endregion

        #region Suppression d'un cuisinier 
        //Ejecter de la base des clients

        public void SuppressionClientCreateurdeRecette()
        {
            //Commande pour afficher les clients createurs
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select id_client,nom_client,prenom_client from client where createur_recette=1;");
            MySqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                Console.WriteLine("Id : " + reader.GetString(0) + "\nNom : " + reader.GetString(1) + "\nPrenom : " + reader.GetString(2) + "\n____");
            }

            //Supprimer dans composition tous les id_recette du createur de recette
            //Dans createur et dans commande tous les id_recettes et tous les id_client 
            Console.WriteLine("\nSaisir l'Id du client a supprimer : ");
            string id_client_suppression = Console.ReadLine();
            Recette recette_id_client = new Recette();
            List<Recette> liste_recette_id_client = new List<Recette>();
            liste_recette_id_client = recette_id_client.ListeRecette("Select * from recette where id_client=\'" + id_client_suppression + "\';");

            Commande commande_id_client = new Commande();
            Creation creation_id_client = new Creation();
            List<Creation> liste_creation_id_client = new List<Creation>();
            List<Commande> liste_commande_id_client = new List<Commande>();

            this.ConnexionCooking("DELETE FROM commande where id_client=\'" + id_client_suppression + "\';");
            this.ConnexionCooking("DELETE FROM creation where id_client=\'" + id_client_suppression + "\';");
            for (int i = 0; i < liste_recette_id_client.Count; i++)
            {
                this.ConnexionCooking("DELETE FROM commande where id_recette=\'" + liste_recette_id_client[i].Id_recette + "\';");
                this.ConnexionCooking("DELETE FROM creation where id_recette=\'" + liste_recette_id_client[i].Id_recette + "\';");
                this.ConnexionCooking("DELETE FROM composition where id_recette=\'" + liste_recette_id_client[i].Id_recette + "\';");
            }

            //Supprimer les id_client et recette dans recette 
            //et ensuite l'id_client final
            for (int i = 0; i < liste_recette_id_client.Count; i++)
            {
                this.ConnexionCooking("DELETE FROM recette where id_recette=\'" + liste_recette_id_client[i].Id_recette + "\';");
            }
            this.ConnexionCooking("DELETE FROM recette where id_client=\'" + id_client_suppression + "\';");

            this.ConnexionCooking("DELETE FROM client where id_client=\'" + id_client_suppression + "\';");
            Console.WriteLine("\nSuppression effectuée !");
        }

        #endregion

        #region Nombre de client
        public void NombreClient()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("SELECT count(*) from client;");
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("Le nombre de client est de : " + reader.GetInt32(0));
            }
            connection.Close();
        }
        #endregion

        #region Nombre de Createur de recette
        public void NombreDeCreateurDeRecette()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("SELECT count(*) from client where createur_recette=1;");
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("\nLe nombre de créateur de recette : " + reader.GetInt32(0)+"\n");
            }
            connection.Close();
        }
        #endregion

        #region Nom des Createurs de recette et nombre recettes commandées
        public void NomCreateurNombreCommandee()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select client.nom_client,count(commande.id_commande)from commande, creation,client where commande.id_recette=creation.id_recette and client.id_client=creation.id_client and client.createur_recette=1 group by client.id_client;");
            MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                Console.WriteLine(reader.GetString(0)+" est créateur de recette. Le nombre de ses recettes commandées est de : "+reader.GetInt32(1));
            }
            connection.Close();
        }
        #endregion

        #region Nombre de recette
        public void NombreRecette()
        {
            
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("SELECT count(*) from recette ;");
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                Console.WriteLine("Le nombre de recette : " + reader.GetInt32(0));
            }
            connection.Close();
        }
        #endregion


        #region 3 modes pour la console

        #region Connexion Client
        public void ConnexionDuClient()
        {
            List<Client> listeclient;
            Recette nouvelle_recette=new Recette();
            Commande commande=new Commande();
            listeclient = this.ListeClient("SELECT * from client ");

            Console.WriteLine("######################################");
            Console.WriteLine("# Bienvenue sur la plateforme client #");
            Console.WriteLine("######################################\n");
            Console.WriteLine("\t1) - Déjà client ");
            Console.WriteLine("\t2) - Pas encore client");
            Console.Write("\n___________________");
            Console.Write("\nExprimez-vous : ");
            int choixclient = Convert.ToInt32(Console.ReadLine());




            switch(choixclient)
            {
                case 1:
                        Console.Clear();
                        Console.WriteLine("\nQuel est votre identifiant :");
                        string identifiantclient = Console.ReadLine();
                        Console.WriteLine("Quel est votre mot de passe :");
                        string motdepasse = Console.ReadLine();
                        Client cliend_find = new Client() ;
                        Client bisclient = new Client();


                    for (int i = 0; i < listeclient.Count; i++)
                    {
                        if (listeclient[i].Id_client == identifiantclient && listeclient[i].Mot_passe == motdepasse)
                        {
                            cliend_find = listeclient[i];
                        }
                    }
                    if (cliend_find.Id_client == bisclient.Id_client ) { Console.WriteLine("\nDésolé, nous ne vous trouvons pas dans notre base");}
                    else
                    {
                        int choix_creation;

                        do
                        {
                            Console.Clear();
                            Console.WriteLine("\nBonjour " +  cliend_find.Prenom_client  + " (" + cliend_find.Id_client+ ") "+ ", vous voici dans votre espace de CREATION");

                            Console.WriteLine("\n\t1) - Creer une nouvelle recette");
                            Console.WriteLine("\t2) - Passer une commande");
                            Console.WriteLine("\t3) - Consulter vos recettes");
                            Console.WriteLine("\t4) - Consulter votre solde cook");
                            Console.WriteLine("\t5) - Pour sortir");
                            Console.Write("\n___________________");
                            Console.Write("\nExprimez-vous : ");
                            choix_creation = Convert.ToInt32(Console.ReadLine());

                            switch (choix_creation)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine("____________________________________");
                                    Console.WriteLine("Rappel de votre Id_client : " + cliend_find.Id_client);
                                    Console.WriteLine("____________________________________");
                                    nouvelle_recette.CreationNouvelleRecette();
                                    Console.ReadKey();
                                    break;

                                case 2:
                                    Console.Clear();
                                    commande.PasserUneCommande(cliend_find);
                                    Console.ReadKey();
                                    break;

                                case 3:
                                    Console.Clear();
                                    nouvelle_recette.ConsulterRecette(cliend_find);
                                    Console.ReadKey();
                                    break;

                                case 4:
                                    Console.Clear();
                                    Console.WriteLine("Solde cook de : " + cliend_find.Solde_cook);
                                    Console.ReadKey();
                                    break;

                            }
                        } while (choix_creation!=5);
                    }
                break;


                case 2:
                    Client nouveau_client = new Client();
                    Console.WriteLine("Désirez-vous vous inscrire : \n\t1) - oui\n\t2) - non ");
                    Console.Write("\n___________________");
                    Console.Write("\nExprimez-vous : ");
                    int choix_nvx_client = Convert.ToInt32(Console.ReadLine());
                    switch(choix_nvx_client)
                    {
                        case 1:
                            try
                            {
                                Console.Clear();
                                CreationClientDansBDD();
                                Console.WriteLine("\nFélicitation !!! \nBienvenue dans le monde cooking !");
                            }
                            catch (MySqlException e)
                            { 
                                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                                Console.ReadLine();
                            }
                            break;

                        case 2:
                            Console.WriteLine("Merci d'être passé !");
                            break;

                    }
                break;


            }

        }
        #endregion

        #region Connexion Admin
        //sont administrateur les personnes avec les id_client: C0000 / C0002
        public void ConnexionDeAdmin()
        {
            Recette recette = new Recette();
            Produit produit = new Produit();
            Console.WriteLine("#####################################");
            Console.WriteLine("# Bienvenue sur la plateforme Admin #");
            Console.WriteLine("#####################################\n");
            Console.WriteLine("Saisir votre identifiant : ");
            string id_admin = Console.ReadLine();
            Console.WriteLine("Saisir le mot de passe : ");
            string mot_admin = Console.ReadLine();
            int choix_admin;
            if (id_admin=="C0000" && mot_admin=="0000" || id_admin=="C0002" && mot_admin=="F2nicolas")
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("\nBonjour Agent " + id_admin + " vous voici connecté dans l'espace administrateur");
                    Console.WriteLine("\nQue souhaitez vous faire ?\n ");
                    Console.WriteLine("\n\t1) - Tableau de bord de la semaine");
                    Console.WriteLine("\t2) - Mise à jour stock min stock max ");
                    Console.WriteLine("\t3) - XML Commande aux fournisseurs ");
                    Console.WriteLine("\t4) - Supprimer une recette ");
                    Console.WriteLine("\t5) - Supprimer un cuisinier (créateur de recette) ");
                    Console.WriteLine("\t6) - Pour sortir");

                    Console.Write("\n___________________");
                    Console.Write("\nExprimez-vous : ");
                    choix_admin = int.Parse(Console.ReadLine());

                    switch (choix_admin)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("\n\t1) - Afficher le cdr le plus commandé  ");
                            Console.WriteLine("\t2) - Top 5 recettes les plus commandées ");
                            Console.WriteLine("\t3) - Le createur de recette d'or " +
                                "");
                            Console.Write("\n___________________");
                            Console.Write("\nExprimez-vous : ");
                            int choix_admin_tab_bord = int.Parse(Console.ReadLine());

                            switch(choix_admin_tab_bord)
                            {
                                case 1:
                                    CDR_Max();
                                    Console.Clear();
                                    break;

                                case 2:
                                    recette.Top5_CDR();
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case 3:
                                    Console.Clear();
                                    recette.CreateurOr();
                                    Console.ReadKey();
                                    break;
                            }
                            break;

                        case 2:
                            Console.Clear();
                            produit.MAJ();
                            Console.ReadKey();
                            break;

                        case 3:
                            Console.Clear();
                            produit.ProduitSemaineXML();
                            Console.ReadKey();
                            break;

                        case 4:
                            Console.Clear();
                            recette.SupprimerUneRecette();
                            Console.ReadKey();
                            break;

                        case 5:
                            Console.Clear();
                            this.SuppressionClientCreateurdeRecette();
                            Console.ReadKey();
                            break;
                    }
                }
                while (choix_admin != 6);
            }
            else { Console.WriteLine("Vous n'êtes pas administrateur"); }

        }
        #endregion

        #region Connexion Mode demo 
        public void ModeDemo()
        {
            Produit produit = new Produit();
            Console.WriteLine("##############################");
            Console.WriteLine("# Bienvenue sur le Mode Démo #");
            Console.WriteLine("##############################\n");
            Console.WriteLine("\n\t 1) - Nombre de client ");
            NombreClient();
            Console.WriteLine("______________________________");
            Console.ReadKey();
            Console.WriteLine("\n\t 2) - Nombre & nom des CdR (nombre total des recettes commandées) ");
            NombreDeCreateurDeRecette();
            NomCreateurNombreCommandee();
            Console.WriteLine("______________________________");
            Console.ReadKey();
            Console.WriteLine("\n\t 3) - Nombre de recette ");
            NombreRecette();
            Console.WriteLine("______________________________");
            Console.ReadKey();
            Console.WriteLine("\n\t 4) - Liste produit quantité stock (<=2 * quantité minimale) et affichage");
            produit.ProduitQuantiteInfEtAffichage();
            Console.ReadKey();
        }

        #endregion

        #region Connexion Mode demo plus personnalisé
        public void ModeDemo1()
        {
            int choix_demo;
            do
            {
                Produit produit = new Produit();
                Console.WriteLine("##############################");
                Console.WriteLine("# Bienvenue sur le Mode Démo #");
                Console.WriteLine("##############################\n");
                Console.WriteLine("\n\t 1) - Nombre de client ");
                Console.WriteLine("\t 2) - Nombre & nom des CdR (nombre total des recettes commandées) ");
                Console.WriteLine("\t 3) - Nombre de recette ");
                Console.WriteLine("\t 4) - Liste produit quantité stock (<=2 * quantité minimale) et affichage");
                Console.WriteLine("\t 5) - Pour sortir");

                Console.Write("\n___________________");
                Console.Write("\nExprimez-vous : ");
                choix_demo = int.Parse(Console.ReadLine());

                switch (choix_demo)
                {
                    case 1:
                        Console.Clear();
                        NombreClient();
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 2:
                        Console.Clear();
                        NombreDeCreateurDeRecette();
                        NomCreateurNombreCommandee();
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 3:
                        Console.Clear();
                        NombreRecette();
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 4:
                        Console.Clear();
                        produit.ProduitQuantiteInfEtAffichage();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }

            } while (choix_demo != 5);

        }
        #endregion

        #endregion
    }
}
