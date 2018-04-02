using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public class Engin
    {
        public int ID { get; set; }

        [Required]
        [StringLength(15)]
        public string Nom { get; set; }

        [Display(Name = "Numéro")]
        [StringLength(10)]
        public string Numero { get; set; }

        [StringLength(15)]
        public string Immatriculation { get; set; }

        [Required]
        [Display(Name = "Code Conf")]
        [StringLength(4)]
        public string CodeConf { get; set; }

        [Required]
        [Display(Name = "Code Chauff")]
        [StringLength(4)]
        public string CodeChauff { get; set; }

        public virtual ICollection<PleinEssence> PleinEssences { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Date modification")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModification { get; set; }
    }
}
