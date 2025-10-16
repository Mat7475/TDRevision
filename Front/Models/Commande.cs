using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Front.Models
{
    public class Commande
    {

        public int IdCommande{ get; set; }

        public string? NomArticle { get; set; }


        public int IdUtilisateur { get; set; }


        public string? Montant { get; set; }


    }
}
