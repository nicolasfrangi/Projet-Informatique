using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cooking
{
    class Creation
    {
        //  Modifier le mot de passe dans la region ci-dessous
        #region CONNEXION 
        static string connectionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = MDP";
        MySqlConnection connection = new MySqlConnection(connectionString);
        #endregion

        #region attribut, propriete, constructeur, affichage
        public string id_creation;
        public DateTime date_creation;
        public string id_client;
        public string id_recette;

        public Creation(string id="", DateTime dc=new DateTime(),string idc="",string idr="")
        {
            this.id_creation = id;
            this.date_creation = dc;
            this.id_client = idc;
            this.id_recette = idr;
        }

        public Creation(MySqlDataReader reader)
        {
            this.id_creation = reader.GetString(0);
            this.date_creation = reader.GetDateTime(1);
            this.id_client = reader.GetString(2);
            this.id_recette = reader.GetString(3);
        }

        public string Id_creation { get { return this.id_creation; } set { this.id_creation = value; } }
        public DateTime Date_creation { get { return this.date_creation; } set { this.date_creation = value; } }
        public string Id_client { get { return this.id_client; } set { this.id_client = value; } }
        public string Id_recette { get { return this.id_recette; } set { this.id_recette = value; } }

        public override string ToString()
        {
            return "\nId creation : " + this.id_creation + "\nDate creation : " + this.date_creation + "\n Id client : " + this.id_client
                +"\nId recette : "+this.id_recette;
        }
        #endregion

        #region Liste de creation issu de la base de donnee
        public List<Creation> ListeCreation(string requeteExecute)
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requeteExecute;
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            List<Creation> listeCreation = new List<Creation>();
            while (reader.Read())
            {
                listeCreation.Add(new Creation(reader));
            }
            connection.Close();
            return listeCreation;
        }

        #endregion
    }
}
