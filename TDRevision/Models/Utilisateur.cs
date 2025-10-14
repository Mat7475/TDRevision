using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDRevision.Models
{
    [Table("T_E_UTILISATEUR_UTI")]
    public class Utilisateur
    {
        [Key]
        [Column("UTI_ID")]
        public int IdUtilisateur { get; set; }

        [Column("UTI_NOM")]
        public string? Nom { get; set; }

        [Column("UTI_PRENOM")]
        public string? Prenom { get; set; }

        [Column("UTI_NUMERORUE")]
        [StringLength(10)]
        public string? NumeroRue { get; set; }

        [Column("UTI_RUE")]
        public string? Rue { get; set; }

        [Column("UTI_CODEPOSTAL")]
        [MinLength(5)]
        [MaxLength(5)]
        public int CodePostal { get; set; }

        [Column("UTI_VILLE")]
        [DefaultValue("Annecy")]
        public string? Ville { get; set; }

        [InverseProperty(nameof(Commande.CommandeNav))]
        public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();

    }
}
