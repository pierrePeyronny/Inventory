using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpfInventaire.Core.DAL.ViewModels
{

    public class ItemListOrderCreationInventaireViewModels
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Ordre { get; set; }
    }

    public class ListOrderCreationInventaireViewModels
    {
        public List<ItemListOrderCreationInventaireViewModels> ListOrder { get; set; }
        public string entity { get; set; }
    }

    public class SaisieInventaireViewModels
    {
        public SelectList listInventaires { get; set; }

        public Evenement desinfection { get; set; }
    }
}
