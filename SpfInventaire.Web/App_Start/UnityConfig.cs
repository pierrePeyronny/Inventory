using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SpfInventaire.Core.Repositories.Interfaces;
using SpfInventaire.Core.Repositories.Repositories;
using SpfInventaire.Core.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Common;
using SpfInventaire.Core.DAL;
using SpfInventaire.Web.Controllers;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.BLL.Services;
using SpfInventaire.Web.Helpers.Interfaces;
using SpfInventaire.Web.Helpers;

namespace SpfInventaire.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            ///////////////////////////////////////////////////////////////////////////////
            //Identity
            ////////////////////////////////////////////////////////////////////////////////
            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());       
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            
            

            ///////////////////////////////////////////////////////////////////////////////
            //UnitOfWork And Repositories
            ////////////////////////////////////////////////////////////////////////////////
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            ///////////////////////////////////////////////////////////////////////////////
            //Helpers
            ////////////////////////////////////////////////////////////////////////////////
            container.RegisterType<ISelectListHelper, SelectListHelper>();

            ///////////////////////////////////////////////////////////////////////////////
            //Services
            ////////////////////////////////////////////////////////////////////////////////
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IInventaireService, InventaireService>();
            container.RegisterType<IBlocInventaireService, BlocInventaireService>();
            container.RegisterType<IMaterielService, MaterielService>();
            container.RegisterType<ITicketIncidentService, TicketIncidentService>();
            container.RegisterType<IEvenementService, EvenementService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IValidationInventaireService, ValidationInventaireService>();

            container.RegisterType<ITypeMaterielService, TypeMaterielService>();
            container.RegisterType<IStockMaterielService, StockMaterielService>();
            container.RegisterType<ISortieStockService, SortieStockService>();

            container.RegisterType<IEnginService, EnginService>();

        }
    }
}
