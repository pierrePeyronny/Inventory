using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpfInventaire.Core.DAL.ViewModels
{
    public class DetailUserViewModels
    {
        public ApplicationUser unUser { get; set; }

        public IEnumerable<IdentityRole> listRolesUser {get; set;}

        public SelectList listAllRoles { get; set; }
    }
}
