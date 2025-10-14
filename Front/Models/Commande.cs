using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Front.Models
{
    [Table("T_E_COMMANDE_COM")]
    public class Commande
    {
        [Key]
        [Column("COM_ID")]
        public int IdCommande{ get; set; }

        [Column("COM_NOMARTICLE")]
        public string? NomArticle { get; set; }

        [Column("UTI_ID")]
        public int IdUtilisateur { get; set; }

        [Column("COM_MONTANT")]
        public string? Montant { get; set; }


    }
}
