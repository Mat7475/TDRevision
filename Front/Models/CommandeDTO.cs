using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Front.Models.DTO
{
    public class CommandeDTO
    {
        public string? Montant { get; set; }

        public string? NomArticle { get; set; }

        public string? IdUtilisateur { get; set; }
    }
}
