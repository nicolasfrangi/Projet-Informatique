using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cooking
{
    class Recette
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);

        static string connectionString1 = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection1 = new MySqlConnection(connectionString1);
        #endregion

        #region attribut, proprietes, constructeur, affichage
        public string id_recette;
        public string nom_recette;
        public string description_recette;
        public string type_recette;
        public int prix_recette;
        public string id_client;

        public Recette(string id = "", int p = 0, string n = "", string type = "", string d="",string idc="")
        {
            this.id_recette = id;
            this.nom_recette = n;
            this.type_recette = type;
            this.description_recette = d;
            this.prix_recette = p;
            this.id_client = idc;
        }
        public Recette(MySqlDataReader reader)
        {
            this.id_recette = reader.GetString(0);
            this.prix_recette = reader.GetInt32(1);
            this.nom_recette = reader.GetString(2);
            this.description_recette = reader.GetString(3);
            this.type_recette = reader.GetString(4);
            this.id_client = reader.GetString(5);
            
        }

        public string Id_recette { get { return this.id_recette; } set { this.id_recette = value; } }
        public string Nom_recette { get { return this.nom_recette; } set { this.nom_recette = value; } }
        public string Description_recette { get { return this.description_recette; } set { this.description_recette = value; } }
        public int Prix_recette { get { return this.prix_recette; } set { this.prix_recette = value; } }
        public string Id_client { get { return this.id_client; }set { this.id_client = value; } }
        public string Type_recette { get { return this.type_recette; } set { this.type_recette = value; } }

        public override string ToString()
        {
            return "\nId recette : " + this.id_recette + "\nNom : " + this.nom_recette + "\nDescription : " + this.description_recette
                + "\nPrix : " + this.prix_recette+"\nType : "+this.type_recette+ "\nId client : "+this.id_client;
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
 
        #region Creation d'une nouvelle recette
        public void CreationNouvelleRecette()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select id_recette from recette order by id_recette;"; //On lance la requete lors de l'appel de la fonction
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("\nId de recette deja utilisés:");
            while(reader.Read())
            {
                Console.Write(reader.GetString(0) + " / ");
            }
            connection.Close();
            try
            {
            Console.WriteLine("\n\nVeuillez saisir pour votre nouvelle recette --> ");    
            Console.WriteLine("\n\tl'ID recette (RXXXX) : ");
            string id = Console.ReadLine();
            Console.WriteLine("\tle nom : ");
            string nom = Console.ReadLine();
            Console.WriteLine("\tune description : ");
            string description = Console.ReadLine();
            Console.WriteLine("\ttype (entree/plat/dessert) : ");
            string type = Console.ReadLine();
            Console.WriteLine("\tle prix : ");
            int prix = int.Parse(Console.ReadLine());
            Console.WriteLine("\trappeler votre identifiant (ci-dessus ci besoin) : ");
            string identi_client = Console.ReadLine(); 
            this.ConnexionCooking("INSERT INTO recette VALUES(\'" + id + "\',\'" + prix + "\',\'" + nom + "\',\'" + description + "\',\'" + type + "\',\'" +identi_client+ "\');");
            Console.Clear();
            int choix_nb_ingredient;

            do
            {
                Produit nouveau_produit = new Produit();
                List<Produit> produit_recette_stock = new List<Produit>();
                Console.WriteLine("\nVoici les produits que vous pouvez selectionner :\n");
                produit_recette_stock = nouveau_produit.ListeProduit("SELECT * from produit order by categorie_produit;");
                for (int i =0;i< produit_recette_stock.Count;i++)
                {
                    Console.WriteLine(produit_recette_stock[i].Nom_produit+"  -  "+produit_recette_stock[i].Id_produit);                    
                }

                Console.WriteLine("\nQue voulez-vous? \nTaper l'ID du produit : ");
                string id_produit = Console.ReadLine();                
                Console.WriteLine("En quelle quantité ?:");
                int qte = Convert.ToInt32(Console.ReadLine());
                this.ConnexionCooking("INSERT INTO composition VALUES (\'" + qte + "\',\'" +id + "\',\'" +id_produit+ "\'); ");
                    Console.Clear();
                    Console.WriteLine("Souhaitez-vous d'autres  ingrédients pour la préparation :\n0) - oui \n1) - non\n");

                choix_nb_ingredient = int.Parse(Console.ReadLine());
            } while (choix_nb_ingredient != 1);
            Console.WriteLine("\nRECETTE VALIDEE");
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" Erreur : " + e.ToString());
                Console.WriteLine("\n Id de recette deja pris ou identifiant produit erroné ! ");
                Console.ReadLine();
            }
        }
        #endregion

        #region Creation d'une liste de recette issu de la base de donnee
        public List<Recette> ListeRecette(string requeteExecute)
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            List<Recette> listeRecette = new List<Recette>();
            while(reader.Read())
            {
                listeRecette.Add(new Recette(reader));
            }
            connection.Close();
            return listeRecette;
        }

        #endregion

        #region Consulter mes recettes
        public void ConsulterRecette(Client client)
        {
            List<Recette> listerecette ;
            listerecette = ListeRecette("SELECT  *  from recette;");
            for (int i=0;i<listerecette.Count;i++)
            {
                if(listerecette[i].Id_client==client.Id_client)
                {
                    Console.WriteLine(listerecette[i]);
                }
            }

        }
        #endregion

        #region Top5 des cdr
        public void Top5_CDR()
        {
            Console.Clear();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select commande.id_recette,count(commande.id_recette),recette.nom_recette,recette.description_recette,recette.type_recette from client,commande,recette where client.id_client=commande.id_client and commande.id_recette=recette.id_recette group by commande.id_recette order by count(commande.id_recette) desc limit 5;");
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("\nRecette les plus commandé\n");
            int i = 1;
            while (reader.Read())
            {
                Console.WriteLine("______________");
                Console.WriteLine(i+"-  id : "+reader.GetString(0)+ 
                    "\n nombre de fois : "+reader.GetString(1)+"\n nom : "+ reader.GetString(2) +"\n description : "+reader.GetString(3) +"\n type : "+ reader.GetString(4)+"\n");
                i++;
            }
            reader.Close();
        }
        #endregion

        #region Createur recette d'or
        public void CreateurOr()
        {
            Console.Clear();
           
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select creation.id_recette,recette.nom_recette,count(commande.id_recette),recette.id_client from client,commande,recette,creation where client.id_client = commande.id_client and commande.id_recette = recette.id_recette and creation.id_recette = recette.id_recette and creation.id_recette = commande.id_recette group by creation.id_recette order by count(commande.id_recette) desc limit 1;");
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("\nClient createur de recette dont une de ces recettes a été commandé le plus de fois");
            while (reader.Read())
            {
                Console.WriteLine("______________");
                Console.WriteLine("Id client: " + reader.GetString(3) +
                    "\nNombre de fois commandée: " + reader.GetString(2) + "\nNom recette: " + reader.GetString(1));
            }
            string recup_id = reader.GetString(3);//Recupere l'id du client cdr or pour afficher ces 5 recettes
            reader.Close();

            
            connection1.Open();
            MySqlCommand command1 = connection1.CreateCommand();
            command1.CommandText = ("select recette.nom_recette from client,recette where client.id_client=recette.id_client and recette.id_client=\'"+recup_id+"\' group by recette.nom_recette limit 5;");
            MySqlDataReader reader1 = command1.ExecuteReader();
            Console.WriteLine("\nLes recettes du client "+recup_id+" qui font fureur : \n");
            int i = 1;
            while(reader1.Read())
            {
                Console.WriteLine(i+") "+reader1.GetString(0));
                i++;
            }
            connection1.Close();

        }
        #endregion

        #region Suppression d'une recette
        public void SupprimerUneRecette()
        { 
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select id_recette,nom_recette from recette;");
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("Les recettes dont nous disposons : \n___");
            while (reader.Read())
            {
                Console.WriteLine("Id recette : " + reader.GetString(0) + "\nNom recette : " + reader.GetString(1) + "\n___");
            }
            Console.WriteLine("\nSaisir l'id de la recette que vous voulez supprimer : ");
            string choix_id_suppression = Console.ReadLine();
            if(choix_id_suppression!="")
            {
                this.ConnexionCooking("DELETE FROM composition where id_recette=\'" + choix_id_suppression + "\';");
                this.ConnexionCooking("DELETE FROM creation where id_recette=\'" + choix_id_suppression + "\';");
                this.ConnexionCooking("DELETE FROM commande where id_recette=\'" + choix_id_suppression + "\';");
                this.ConnexionCooking("DELETE FROM recette where id_recette=\'" + choix_id_suppression + "\';");
                Console.WriteLine("Recette supprimé !");
            }
            else 
            {
                Console.Clear();
                Console.WriteLine("Cette recette n'existe pas !");
            }
           
              
        }
        #endregion

    }
}
