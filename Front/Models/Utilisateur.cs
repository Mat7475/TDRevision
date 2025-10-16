using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Front.Models
{
    public class Utilisateur
    {

        public int IdUtilisateur { get; set; }

        public string? Nom { get; set; }

        public string? Prenom { get; set; }

        public string? NumeroRue { get; set; }

        public string? Rue { get; set; }

        public int CodePostal { get; set; }

        public string? Ville { get; set; }


    }
}
