using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;

namespace Cooking
{

    class Program
    {
        #region Run final
        static void RunFinal()
        {
            Console.WriteLine("                                                                            _     ");
            Console.WriteLine("                                   |_|_|                                   | |    ");
            Console.WriteLine("                                     |    BIENVENUE DANS LE MONDE COOKING  \\ |  ");
            Console.WriteLine("                                     |                                      ||   ");
            Console.WriteLine("                                     |                                      ||   ");
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.ReadKey();
            Console.WriteLine("\n\nSouhaitez-vous accéder à l'espace:");
            Console.WriteLine("\n\t(1) - Client ");
            Console.WriteLine("\t(2) - Administrateur");
            Console.WriteLine("\t(3) - Mode démo");

            int choix_user = int.Parse(Console.ReadLine());
            Client client = new Client();


            switch (choix_user)
            {
                case 1:
                    Console.Clear();
                    client.ConnexionDuClient();
                    break;
                case 2:
                    Console.Clear();
                    client.ConnexionDeAdmin();
                    break;
                case 3:
                    Console.Clear();
                    client.ModeDemo();
                    //client.ModeDemo1(); //avec un menu déroulant
                    break;
            }
        }
        #endregion

        #region Test fonction
        static void test()
        {

            Composition compo = new Composition();
            Recette recette = new Recette("R0001", 12, "Pate carbonara", "pate lardon", "plat","C0001");
            Recette r=new Recette("R0002",15,"Lasagne","pate fine bolognaise et gruyere","plat","C0001");
            Recette ri = new Recette();

            Console.WriteLine(compo.CommandePasseeAcceptee(r));
            Console.ReadKey();
            Console.Clear();
            ri.Top5_CDR();
            Console.ReadKey();
            Console.Clear();
            ri.CreateurOr();
            Console.ReadKey();
            Console.Clear();
            Client c = new Client();
            c.SuppressionClientCreateurdeRecette();
            Console.ReadKey();
            Console.Clear();

        }
        #endregion

        static void Main(string[] args)
        {

            /// <summary>
            /// Test De quelques fonctions
            /// </summary>

            //test();


            
            /// <summary>
            /// Lancement de l'application cooking
            /// </summary>

            RunFinal();


            Console.ReadKey();
        }
    }
}
