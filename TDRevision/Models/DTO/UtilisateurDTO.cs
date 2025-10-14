using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDRevision.Models.DTO
{
    public class UtilisateurDTO
    {

        public int IdUtilisateur { get; set; }

        public string? Nom { get; set; }

        public string? Prenom { get; set; }

        public string? Ville { get; set; }

        public int NbCommandes { get; set; }
    }
}
