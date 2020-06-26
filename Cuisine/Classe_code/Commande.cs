using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;


namespace Cooking
{
    class Commande
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);

        static string co = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection conn = new MySqlConnection(co);

        static string con = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection conne = new MySqlConnection(con);

        static string connec = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connect = new MySqlConnection(connec);
        #endregion

        #region attribut, propriete, constructeur, affichage
        public int id_commande;
        public float prix_commande;
        public int quantite_commande;
        public DateTime date_commande;
        public string id_client;
        public string id_recette;

        public Commande(int idcom=0,float prix=0,int qte=0,DateTime dt=new DateTime(),string idc="",string idr="")
        {
            this.id_commande = idcom;
            this.prix_commande = prix;
            this.quantite_commande = qte;
            this.date_commande = dt;
            this.id_client = idc;
            this.id_recette = idr;
        }

        public Commande(MySqlDataReader reader)
        {
            this.id_commande = reader.GetInt32(0);
            this.prix_commande = reader.GetFloat(1);
            this.quantite_commande = reader.GetInt32(2);
            this.date_commande = reader.GetDateTime(3);
            this.id_client = reader.GetString(4);
            this.id_recette = reader.GetString(5);
        }

        public int Id_commande { get { return this.id_commande; } set { this.id_commande = value; } }
        public float Prix_commande { get { return this.prix_commande; } set { this.prix_commande = value; } }
        public int Quantite_commande { get { return this.quantite_commande; } set { this.quantite_commande = value; } }
        public DateTime Date_commande { get { return this.date_commande; } set { this.date_commande = value; } }
        public string Id_client { get { return this.id_client; } set { this.id_client = value; } }
        public string Id_recette { get { return this.id_recette; } set { this.id_recette = value; } }

        public override string ToString()
        {
            return "\nId commande : " + this.id_commande + "\nPrix : " + this.prix_commande + "\nQuantite : " + this.quantite_commande
                + "\nDate commande : " + this.date_commande + "\nId client : " + this.id_client + "\nId recette : " + this.id_recette;
        }
        #endregion

        #region Fonction connexion base de donnee
        public void ConnexionCooking(string requeteExecute)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requeteExecute;
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" Erreur : " + e.ToString());
                Console.ReadLine();
            }
        }

        #endregion

        #region Listes des commandes
        public List<Commande> listeCommande(string requeteExecute)
        {
            List<Commande> listeCommande = new List<Commande>();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                listeCommande.Add(new Commande(reader));
            }
            connection.Close();
            return listeCommande;

        }
        #endregion

        #region entree/plat/dessert
        public void entree()
        {
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = "select nom_recette,description_recette,id_recette from recette where type_recette='entree';";
            MySqlDataReader reader = comm.ExecuteReader();
            string nom = ""; string descr = ""; int compt = 1;
            Console.Clear();
            Console.WriteLine("\nListe des entrees:\n");
            while (reader.Read())
            {

                nom = reader.GetString(0);
                descr = reader.GetString(1);
                string id_recette = reader.GetString(2);
                Console.WriteLine(compt + " -->  " + id_recette + " - "+ nom + " \tinfo : " + descr + " ");
                compt++;
            }
            conn.Close();
        }
        public void plat()
        {
            conne.Open();
            MySqlCommand comma = conne.CreateCommand();
            comma.CommandText = "select nom_recette,description_recette,id_recette from recette where type_recette='plat';";
            MySqlDataReader read = comma.ExecuteReader();
            string nom1 = ""; string descr1 = ""; int compt1 = 1;
            Console.Clear();
            Console.WriteLine("\nListe des plats:\n");
            while (read.Read())
            {

                nom1 = read.GetString(0);
                descr1 = read.GetString(1);
                string id_recette1 = read.GetString(2);
                Console.WriteLine(compt1 + " -->  " + id_recette1 + " - " + nom1 + " \tinfo : " + descr1 + " ");
                compt1++;
            }
            conne.Close();
        }

        public void dessert()
        {
            connect.Open();
            MySqlCommand command = connect.CreateCommand();
            command.CommandText = "select nom_recette,description_recette,id_recette from recette where type_recette='dessert';";
            MySqlDataReader readerr = command.ExecuteReader();
            string nom_ = ""; string descr_ = ""; int compt_ = 1;
            Console.Clear();
            Console.WriteLine("\nListe des desserts:\n");
            while (readerr.Read())
            {

                nom_ = readerr.GetString(0);
                descr_ = readerr.GetString(1);
                string id_recette_ = readerr.GetString(2);
                Console.WriteLine(compt_+ " -->  " + id_recette_ + " - " + nom_ + " \tinfo : " + descr_ + " ");
                compt_++;
            }
            connect.Close();
        }
        #endregion

        #region Commande lancee ?!
        public void CommandeLancee(string id_rece,List<Recette> listeRecette,Client client,List<Commande> listeCommande,string type)
        {
            for (int i=0;i<listeRecette.Count;i++)
            {
                if (listeRecette[i].Id_recette == id_rece && listeRecette[i].Type_recette==type)//type ajouté car si l'individu tape un autre id que voulu, la commande ne se lance pas
                {
                    Recette new_recette = new Recette(listeRecette[i].Id_recette, listeRecette[i].Prix_recette, listeRecette[i].Nom_recette, listeRecette[i].Description_recette, listeRecette[i].Type_recette, listeRecette[i].Id_client);
                    Composition composition = new Composition();
                    //assez de stock pour commaander
                    if (composition.CommandePasseeAcceptee(new_recette) == true)
                    {
                        if(client.Solde_cook>new_recette.Prix_recette)
                        {
                            //actualise le solde cook client (il achete, le solde cook diminue)
                            client.Solde_cook -= new_recette.Prix_recette;
                            this.ConnexionCooking("UPDATE client SET solde_cook='"+client.Solde_cook+"' where id_client='"+client.Id_client+"';");

                            List<Client> listeClient = new List<Client>();
                            listeClient = client.ListeClient("SELECT * from client;");

                            for (int j=0; j<listeClient.Count;j++)
                            {
                                if(listeClient[j].Id_client==new_recette.Id_client)
                                {
                                    //recompense de +2 cooks pour une commande d'une recette d'un client
                                    Client client_recette_recompense = new Client(listeClient[j].Id_client, listeClient[j].Mot_passe, listeClient[j].Nom_client, listeClient[j].Prenom_client, listeClient[j].Telephone_client, listeClient[j].Adresse_client, listeClient[j].Ville_client, listeClient[j].Createur_recette, listeClient[j].Solde_cook,listeClient[j].Compteur_cdr);
                                    client_recette_recompense.Solde_cook += 2;
                                    this.ConnexionCooking("UPDATE client SET solde_cook='" + client_recette_recompense.Solde_cook + "' where id_client='" + client_recette_recompense.Id_client + "';");
                                    int compt = client_recette_recompense.Compteur_cdr;
                                    compt+= 1;
                                    this.ConnexionCooking("UPDATE client SET compteur_cdr='" + compt + "' where id_client='" + client.Id_client + "';");
                                }
                            }

                            int nouveaux_id_commande = listeCommande[listeCommande.Count-1].Id_commande+1;
                            this.ConnexionCooking("INSERT INTO commande VALUES(\'" + nouveaux_id_commande + "\',\'" + new_recette.Prix_recette + "\',\'" + 1 + "\',\'"+DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "\',\'" + new_recette.Id_recette+ "\',\'" + client.Id_client+ "\');");

                            //MAJ DES STOCKS
                            Composition compo = new Composition();
                            List<Composition> produitPourRecette = new List<Composition>();
                            List<Composition> produitDejaPresent = new List<Composition>();
                            produitDejaPresent = compo.listeCompositionRecette();

                            Produit produit = new Produit();
                            List<Produit> listeProduit = new List<Produit>();
                            listeProduit = produit.ListeProduit("Select * from produit;");

                            for(int k=0;k<produitDejaPresent.Count;k++)
                            {
                                if (produitDejaPresent[k].Id_recette == new_recette.Id_recette)
                                {
                                    produitPourRecette.Add(produitDejaPresent[k]);
                                }
                            }
                            for (int l = 0; l < produitPourRecette.Count; l++)
                            {
                                string produit_nom = produitPourRecette[l].Id_produit;
                                int produit_quantite = produitPourRecette[l].Quantite_produit;
                                for (int m = 0; m < listeProduit.Count; m++)
                                {
                                    if (listeProduit[m].Id_produit == produit_nom)
                                    {
                                        string id_prod = listeProduit[m].Id_produit;
                                        int stock_prod = listeProduit[m].Stock_produit;
                                        stock_prod -= produit_quantite;
                                        this.ConnexionCooking("UPDATE produit SET stock_produit='" + stock_prod + "' where id_produit='" + id_prod+ "';");
                                        this.ConnexionCooking("UPDATE fourniture SET quantite_fourniture='" + stock_prod + "' where id_produit='" + id_prod + "';");
                                    }
                                }
                            }
                            Console.WriteLine("Recette trouvee, Commande envoyee !");
                        }
                        else
                        {
                            Console.WriteLine("SCI !");//Solde cook insuffisant 
                        }
                    }
                    else
                    {
                        Console.WriteLine("SI !");//Stock insuffisant
                    }
                }
                if(listeRecette[i].Id_recette != id_rece)
                {
                    Console.WriteLine(" - RNT");//Recette non trouvee
                }
            }
        }
        #endregion

        #region Passer une commande
        public void PasserUneCommande(Client client)
        {
            Console.WriteLine("\n ### Menu ### ");
            Console.WriteLine("\n1) - une entree \n2) - un plat  \n3) - un dessert ");
            int choix_commande = Convert.ToInt32(Console.ReadLine());
            Recette recette = new Recette();
            List<Recette> listeRecette = recette.ListeRecette("SELECT * from recette;");
            List<Commande> listeCommande = this.listeCommande("SELECT * from commande;");
            switch (choix_commande)
            {

                case 1:
                    entree();
                    Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                    string choix_entree = Console.ReadLine();
                    Console.WriteLine("\n");
                    if (choix_entree != "")
                    {
                        CommandeLancee(choix_entree, listeRecette, client, listeCommande, "entree");
                        Console.ReadKey();
                        plat();
                        Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                        string choix_plat = Console.ReadLine();
                        if (choix_plat != "")
                        {
                            CommandeLancee(choix_plat, listeRecette, client, listeCommande, "plat");
                            Console.ReadKey();
                            dessert();
                            Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                            string choix_dessert = Console.ReadLine();
                            if (choix_dessert != "")
                            {
                                CommandeLancee(choix_dessert, listeRecette, client, listeCommande, "dessert");
                                Console.ReadKey();
                                Console.WriteLine("\nBon appetit");
                            }
                            else
                            {
                                Console.WriteLine("\nBon appetit");
                            }
                        }
                        else
                        {
                            dessert();
                            Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                            Console.ReadKey();
                            string choix_dessert1 = Console.ReadLine();
                            if (choix_dessert1 != "")
                            {
                                CommandeLancee(choix_dessert1, listeRecette, client, listeCommande, "dessert");
                                Console.ReadKey();
                                Console.WriteLine("\nBon appetit");
                            }
                            else
                            {
                                Console.WriteLine("\nBon appetit");
                            }
                        }
                    }
                    else
                    {
                        plat();
                        Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                        string choix_plat2 = Console.ReadLine();
                        if (choix_plat2 != "")
                        {
                            CommandeLancee(choix_plat2, listeRecette, client, listeCommande, "plat");
                            Console.ReadKey();
                            dessert();
                            Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                            string choix_dessert3 = Console.ReadLine();
                            if (choix_dessert3 != "")
                            {
                                CommandeLancee(choix_dessert3, listeRecette, client, listeCommande, "dessert");
                                Console.ReadKey();
                                Console.WriteLine("\nBon appetit");
                            }
                            else
                            {
                                Console.WriteLine("\nBon appetit");
                            }
                        }
                        else
                        {
                            dessert();
                            Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                            string choix_dessert4 = Console.ReadLine();
                            if (choix_dessert4 != "")
                            {
                                CommandeLancee(choix_dessert4, listeRecette, client, listeCommande, "dessert");
                                Console.ReadKey();
                                Console.WriteLine("\nBon appetit");
                            }
                            else
                            {
                                Console.WriteLine("\nBon appetit");
                            }
                        }
                    }

                    break;

                case 2:
                    plat();
                    Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                    string choix_plat_ = Console.ReadLine();
                    if (choix_plat_ != "")
                    {
                        CommandeLancee(choix_plat_, listeRecette, client, listeCommande, "plat");
                        Console.ReadKey();
                        dessert();
                        Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                        string choix_dessert_ = Console.ReadLine();
                        if (choix_dessert_ != "")
                        {
                            CommandeLancee(choix_dessert_, listeRecette, client, listeCommande, "dessert");
                            Console.ReadKey();
                            Console.WriteLine("\nBon appetit");
                        }
                        else
                        {
                            Console.WriteLine("\nBon appetit");
                        }
                    }
                    else
                    {
                        dessert();
                        Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                        string choix_dessert__ = Console.ReadLine();
                        if (choix_dessert__ != "")
                        {
                            CommandeLancee(choix_dessert__, listeRecette, client, listeCommande, "dessert");
                            Console.ReadKey();
                            Console.WriteLine("\nBon appetit");
                        }
                        else
                        {
                            Console.WriteLine("\nBon appetit");
                        }
                    }

                    break;

                case 3:
                    dessert();
                    Console.WriteLine("\nQue souhaitez-vous (entrer pour ne rien prendre sinon taper l'ID) ?");
                    string choix_dessertx = Console.ReadLine();
                    if (choix_dessertx != "")
                    {
                        CommandeLancee(choix_dessertx, listeRecette, client, listeCommande, "dessert");
                        Console.ReadKey();
                        Console.WriteLine("\nBon appetit");
                    }
                    else
                    {
                        Console.WriteLine("\nBon appetit");
                    }
                    break;



            }
        }
        #endregion
    }
}
