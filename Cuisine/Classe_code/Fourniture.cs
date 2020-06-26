using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cooking
{
    class Fourniture
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);
        #endregion

        #region attribut, propriete, constructeur, affichage
        public int quantite_fourniture;
        public string id_fournisseur;
        public string id_produit;

        public Fourniture(int qtef=0, string idf="",string idp="")
        {
            this.quantite_fourniture = qtef;
            this.id_fournisseur = idf;
            this.id_produit = idp;
        }

        public Fourniture(MySqlDataReader reader)
        {
            this.quantite_fourniture = reader.GetInt32(0);
            this.id_fournisseur = reader.GetString(1);
            this.id_produit = reader.GetString(2);
        }

        public int Quantite_fourniture { get { return this.quantite_fourniture; } set { this.quantite_fourniture = value; } }
        public string Id_fournisseur { get { return this.id_fournisseur; } set { this.id_fournisseur = value; } }
        public string Id_produit { get { return this.id_produit; } set { this.id_produit = value; } }

        public override string ToString()
        {
            return "\nQuantite fourniture : " + this.quantite_fourniture + "\nId fournisseur : " + this.id_fournisseur
                + "\nId produit : " + this.id_produit;
        }
        #endregion

        #region Liste fourniture dans la base de donnee
        public List<Fourniture> ListeFourniture(string requeteExecute) //on crée une liste contenant toutes les produits de la base de donnée
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<Fourniture> produits = new List<Fourniture>();

            while (reader.Read())
            {
                produits.Add(new Fourniture(reader));
            }

            connection.Close();
            return produits;

        }
        #endregion
    }
}
