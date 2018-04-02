using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpfInventaire.Core.DAL.ViewModels
{
    public class FormMaterielViewModels
    {
        public Materiel unMateriel { get; set; }
        public SelectList listBlocInventaire { get; set; }
        public SelectList listTypeMateriel { get; set; }
        public bool isEdit { get; set; }
    }

    public class FormTypeMaterielViewModels
    {
        public TypeMateriel unTypeMateriel { get; set; }
        public bool isEdit { get; set; }
    }

}
