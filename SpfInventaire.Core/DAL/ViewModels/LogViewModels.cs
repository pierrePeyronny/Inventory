using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.ViewModels
{
    public class LogSuppressionViewModel
    {
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime, ErrorMessage = "Ce n'est pas une Date Valide")]
        public DateTime DateSuppression { get; set; }
    }
}
