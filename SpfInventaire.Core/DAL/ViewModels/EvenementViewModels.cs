using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.ViewModels
{
    public class FormEvenementViewModels
    {
        public Evenement unEvenement { get; set; }
        public bool isEdit { get; set; }
    }
}
