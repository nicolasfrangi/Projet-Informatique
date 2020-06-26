using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Collections;
using System.Xml;

namespace Cooking
{
    class Produit
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);

        static string connectionString1 = "SERVER=localhost ; DATABASE=Cooking; UID=root; PASSWORD=MDP;";
        MySqlConnection connection1 = new MySqlConnection(connectionString1);
        #endregion

        #region attribut, proprietes, constructeur, affichage
        public string id_produit;
        public string nom_produit;
        public string categorie_produit;
        public int stock_produit;
        public int stock_min;
        public int stock_max;

        public Produit(string id = "", string cat = "", string nom = "", int sp = 0, int smin = 0, int smax = 0)
        {
            this.id_produit = id;
            this.categorie_produit = cat;
            this.nom_produit = nom;
            this.stock_produit = sp;
            this.stock_min = smin;
            this.stock_max = smax;
        }

        public Produit(MySqlDataReader reader)
        {
            this.id_produit = reader.GetString(0);
            this.categorie_produit = reader.GetString(1);
            this.nom_produit = reader.GetString(2);
            this.stock_produit = reader.GetInt32(3);
            this.stock_min = reader.GetInt32(4);
            this.stock_max = reader.GetInt32(5);
        }

        public string Id_produit { get { return this.id_produit; } set { this.id_produit = value; } }
        public string Nom_produit { get { return this.nom_produit; }set { this.nom_produit = value; } }
        public string Categorie_produit { get { return this.categorie_produit; } set { this.categorie_produit = value; } }
        public int Stock_produit { get { return this.stock_produit; } set { this.stock_produit = value; } }
        public int Stock_min { get { return this.Stock_min; } set { this.stock_min = value; } }
        public int Stock_max { get { return this.stock_max; } set { this.stock_max = value; } }

        public override string ToString()
        {
            return "\nId produit : " + this.id_produit + "\nNom : " + this.nom_produit + "\nCategorie : " + this.categorie_produit
                + "\nStock produit : " + this.stock_produit + "\nStock min : " + this.stock_min + "\nStock max : " + this.stock_max;
        }
        #endregion

        #region Connexion requête vers BDD
        public void ConnexionCooking(string requeteExecute)
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
        }

        #endregion

        #region Creation d'une liste de produit issu de la base de donnee

        public List<Produit> ListeProduit(string requeteExecute) //on crée une liste contenant toutes les produits de la base de donnée
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<Produit> produits = new List<Produit>();

            while (reader.Read())
            {
                produits.Add(new Produit(reader));
            }

            connection.Close();
            return produits;

        }
        #endregion

        #region Mise a jour des quantités
        public void MAJ()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select DATE_FORMAT(commande.date_commande,'%y-%m-%d') ,commande.id_commande,recette.id_recette,produit.id_produit from produit,composition,recette,commande where produit.id_produit=composition.id_produit and composition.id_recette=recette.id_recette and recette.id_recette=commande.id_recette  group by id_recette order by commande.date_commande asc ; ");
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                DateTime date = reader.GetDateTime(0);
                string id_prod = reader.GetString(3); 
                TimeSpan dif = date - DateTime.Now;
                if (Math.Abs(dif.Days)>30)
                {
                    Console.WriteLine(date + " --- " + id_prod +"  ont vu leur stock modifié.");
                    this.ConnexionCooking("update produit set stock_min=stock_min/2 where id_produit=\'"+id_prod+"\';");
                    this.ConnexionCooking("update produit set stock_max=stock_max/2 where id_produit=\'" + id_prod + "\';");
                }
            }
        }



        #endregion

        #region Produit quantite inferieur a 2*Stock minimal et affichage une des recettes
        public void ProduitQuantiteInfEtAffichage()
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ("select * from produit where stock_produit<=2*stock_min; ");
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("Stock dont la quantité est inférieure à 2*stock minimale \n");

            while (reader.Read())
            {
                Console.WriteLine("Id produit : " + reader.GetString(0) + "\nNom produit : " + reader.GetString(2)
                    + "\nStock produit : " + reader.GetInt32(3) + "\nStock min : " + reader.GetInt32(4) + "\nStock max : " + reader.GetInt32(5)+"\n___");
            }
            connection.Close();
            Console.WriteLine("\nSaisir l'Id produit pour afficher les recettes où ce produit apparait : ");
            string id_pro = Console.ReadLine();
            
            connection1.Open();
            MySqlCommand command1 = connection1.CreateCommand();
            command1.CommandText = ("select produit.id_produit, recette.nom_recette,composition.quantite_produit from produit, composition, recette where produit.id_produit=composition.id_produit and composition.id_recette=recette.id_recette and produit.id_produit=\'"+id_pro+"\'; ");
            MySqlDataReader reader1 = command1.ExecuteReader();
            while(reader1.Read())
            {
                Console.WriteLine("\nId produit : " + reader1.GetString(0) + "\nNom recette : " + reader1.GetString(1)
                    + "\nQuantité utilisée : " + reader1.GetInt32(2)+"\n___");
            }
            connection1.Close();
        }
        #endregion

        #region Commande XML 
        public void ProduitSemaineXML()
        {
            //Liste produit en faible quantité
            List<Produit> listeProduitFaibleQuantite = this.ListeProduit("Select * from produit where stock_produit<stock_min;");
            for(int i=0;i<listeProduitFaibleQuantite.Count;i++)
            {
                int nombre_manquant = Math.Abs(listeProduitFaibleQuantite[i].Stock_produit - listeProduitFaibleQuantite[i].Stock_max);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                if (listeProduitFaibleQuantite[i].Categorie_produit == "Legume")
                {
                    string id_fou = "F0001";
                    command.CommandText = ("Insert into fourniture values(\'" + nombre_manquant + "\',\'" + listeProduitFaibleQuantite[i].Id_produit + "\',\'" + id_fou + "\');");
                    MySqlDataReader reader;
                    reader = command.ExecuteReader();
                }
                if (listeProduitFaibleQuantite[i].Categorie_produit == "Fruit")
                {
                    string id_fou = "F0004";
                    command.CommandText = ("Insert into fourniture values(\'" + nombre_manquant + "\',\'" + listeProduitFaibleQuantite[i].Id_produit + "\',\'" + id_fou + "\');");
                    MySqlDataReader reader;
                    reader = command.ExecuteReader();
                }
                if (listeProduitFaibleQuantite[i].Categorie_produit == "Viande")
                {
                    string id_fou = "F0002";
                    command.CommandText = ("Insert into fourniture values(\'" + nombre_manquant + "\',\'" + listeProduitFaibleQuantite[i].Id_produit + "\',\'" + id_fou + "\');");
                    MySqlDataReader reader;
                    reader = command.ExecuteReader();
                }
                if (listeProduitFaibleQuantite[i].Categorie_produit == "Poisson")
                {
                    string id_fou = "F0003";
                    command.CommandText = ("Insert into fourniture values(\'" + nombre_manquant + "\',\'" + listeProduitFaibleQuantite[i].Id_produit + "\',\'" + id_fou + "\');");
                    MySqlDataReader reader;
                    reader = command.ExecuteReader();
                }

                connection.Close();
            }

            List<Fourniture> listeFourniture = new List<Fourniture>();
            Fourniture fourniture = new Fourniture();
            listeFourniture = fourniture.ListeFourniture("Select * from fourniture;");


            XmlDocument docXml = new XmlDocument();
            //creation noeud racine
            XmlElement racine = docXml.CreateElement("Commande");
            docXml.AppendChild(racine);
            //création de l'en-tête XML 
            XmlDeclaration xmldecl = docXml.CreateXmlDeclaration("1.0", "UTF-8", "no");
            docXml.InsertBefore(xmldecl, racine);

            //date de la commande
            string aujdhui1 = Convert.ToString(DateTime.Now.ToString());
            XmlElement date = docXml.CreateElement("Date");
            date.InnerText = aujdhui1;
            racine.AppendChild(date);


            for (int i = 0; i < listeFourniture.Count; i++)
            {
                XmlElement balise_id_fournisseur = docXml.CreateElement("Id_Produit");
                balise_id_fournisseur.InnerText = listeFourniture[i].Id_fournisseur;
                XmlElement balise_id_produit = docXml.CreateElement("Id_Fournisseur");
                balise_id_produit.InnerText = listeFourniture[i].Id_produit;
                racine.AppendChild(balise_id_produit);
                balise_id_produit.AppendChild(balise_id_fournisseur);

                XmlElement balise_quantite_fourniture = docXml.CreateElement("Quantite_Fourniture");
                balise_quantite_fourniture.InnerText = Convert.ToString(listeFourniture[i].Quantite_fourniture);
                balise_id_produit.AppendChild(balise_quantite_fourniture);

            }

            docXml.Save("ListeACommander.xml");
            Console.WriteLine("Le fichier a été créé ");


        }
        #endregion
    }
}
