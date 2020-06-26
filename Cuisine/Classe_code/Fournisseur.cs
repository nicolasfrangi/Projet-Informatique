using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cooking
{
    class Fournisseur
    {
        #region attribut, proprietes, constructeur, affichage
        public string id_fournisseur;
        public string nom_fournisseur;
        public string telephone_fournisseur;

        public Fournisseur(string id="",string nom="",string tel="")
        {
            this.id_fournisseur = id;
            this.nom_fournisseur = nom;
            this.telephone_fournisseur = tel;
        }

        public Fournisseur(MySqlDataReader reader)
        {
            this.id_fournisseur = reader.GetString(0);
            this.nom_fournisseur = reader.GetString(1);
            this.telephone_fournisseur = reader.GetString(2);
        }

        public string Id_fournisseur { get { return this.id_fournisseur; } set { this.id_fournisseur = value; } }
        public string Nom_fournisseur { get { return nom_fournisseur; } set { this.nom_fournisseur = value; } }
        public string Telephone_fournisseur { get { return this.telephone_fournisseur; } set { this.telephone_fournisseur = value; } }

        public override string ToString()
        {
            return "\nId fournisseur : " + this.id_fournisseur + "\nNom : " + this.nom_fournisseur + "\nTelephone : " + this.telephone_fournisseur;
        }
        #endregion
    }
}
