using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cooking
{
    class Composition
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);
        #endregion

        #region attribut, propriete, constructeur, affichage
        public int quantite_produit;
        public string id_recette;
        public string id_produit;

        public Composition(int qtep=0,string idr="",string idp="")
        {
            this.quantite_produit = qtep;
            this.id_recette = idr;
            this.id_produit = idp;
        }

        public Composition(MySqlDataReader reader)
        {
            this.quantite_produit = reader.GetInt32(0);
            this.id_recette = reader.GetString(1);
            this.id_produit = reader.GetString(2);
        }

        public int Quantite_produit { get { return this.quantite_produit; } set { this.quantite_produit = value; } }
        public string Id_recette { get { return this.id_recette; } set { this.id_recette = value; } }
        public string Id_produit { get { return this.id_produit; } set { this.id_produit = value; } }

        public override string ToString()
        {
            return "\nQuantite produit : " + this.quantite_produit + "\nId recette : " + this.id_recette + "\nId produit : " + this.id_produit;
        }
        #endregion

        #region Fonction connexion base de donnee
        public List<Composition> listeCompositionRecette()
        {
            List<Composition> listeCompo = new List<Composition>(); 
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * from composition;";
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                listeCompo.Add(new Composition(reader));
            }
            connection.Close();
            return listeCompo;
        }
        #endregion

        #region Le stock est suffisant pour passer une commande
        public bool CommandePasseeAcceptee(Recette recette)
        {
            bool reponse;
            List<Composition> listeComposition = new List<Composition>();
            List<Composition> listeProduitPourRecette = new List<Composition>();
            listeComposition = this.listeCompositionRecette();
            int compteur_recette = 0;
            for (int i=0; i<listeComposition.Count;i++)
            {
                if (listeComposition[i].Id_recette == recette.Id_recette)//on add les elements de la composition qui ont le meme id_recette que la recette que l'on veut
                {
                    listeProduitPourRecette.Add(listeComposition[i]);
                    compteur_recette++;
                }
            }
            Produit prod = new Produit() ;
            List<Produit> produits = prod.ListeProduit("SELECT * from produit;");
            int compteur_stock = 0;
            for (int j = 0; j < listeProduitPourRecette.Count; j++)
            {                
                string leProduit = listeProduitPourRecette[j].Id_produit;
                int stock_quantite = listeProduitPourRecette[j].Quantite_produit;

                for (int k = 0; k < produits.Count; k++)
                {
                    if (produits[k].Id_produit==leProduit)
                    {
                        if (produits[k].Stock_produit > stock_quantite )
                        {
                            compteur_stock++;
                        }
                    }
                }
            }
            if (compteur_recette == compteur_stock)
            {
                reponse = true;
            }
            else { reponse = false; }
            return reponse;
        }
        #endregion
    }
}
