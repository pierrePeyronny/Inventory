using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class TypeMaterielService : ITypeMaterielService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<TypeMateriel> typeMaterielRepository;
        private IGenericRepository<Materiel> materielRepository;
        private ILoggerService logService;

        public TypeMaterielService(IUnitOfWork unitOfWork, IGenericRepository<TypeMateriel> typeMaterielRepository, IGenericRepository<Materiel> materielRepository, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.typeMaterielRepository = typeMaterielRepository;
            this.materielRepository = materielRepository;
            this.logService = logService;
        }

        public IEnumerable<TypeMateriel> GetTypeMateriels()
        {
            return this.typeMaterielRepository.Get(
                orderBy: o =>o.OrderBy(f =>f.Nom)
                );
        }

        public IEnumerable<TypeMateriel> GetTypeMaterielByDomaine(MATERIEL_TYPE_DOMAINE domaine)
        {
            return this.typeMaterielRepository.Get(
                filter: f =>f.Domaine == domaine
                );
        }

        public TypeMateriel GetTypeMaterielById(object id)
        {
            return this.typeMaterielRepository.GetByID(id);
        }

        public ActionControllerResult InsertTypeMateriel(TypeMateriel unTypeMateriel, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                this.typeMaterielRepository.Insert(unTypeMateriel);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.TypeMateriel, null, "Erreur Lors de la création d'un Type de Matériel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult UpdateTypeMateriel(TypeMateriel unTypeMateriel, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                this.typeMaterielRepository.Update(unTypeMateriel);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.TypeMateriel, null, "Erreur Lors de la modification d'un Type de Matériel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public void DeleteTypeMateriel(int id)
        {
            this.typeMaterielRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public ActionControllerResult GenerationTypeMaterielFromMateriel()
        {
            ActionControllerResult result;
            try
            {
                TypeMateriel unTypeMateriel;
                IEnumerable<Materiel> listMateriels = this.materielRepository.Get();

                foreach(Materiel unMateriel in listMateriels)
                {
                    if(unMateriel.TypeMateriel == null)
                    {
                        //Recherche l'existance d'un type materiel correspondant
                        unTypeMateriel = this.typeMaterielRepository.Get(
                            filter: f => f.Nom == unMateriel.Nom
                            ).SingleOrDefault();

                        if(unTypeMateriel == null)
                        {
                            //Creation d'un type Matériel
                            unTypeMateriel = new TypeMateriel();                        
                            unTypeMateriel.Nom = unMateriel.Nom;
                            
                            switch(unMateriel.BlocInventaire.Inventaire.Nom)
                            {
                                case "FPT":
                                    unTypeMateriel.Domaine = MATERIEL_TYPE_DOMAINE.Incendie;
                                    break;

                                case "VSAV":
                                    unTypeMateriel.Domaine = MATERIEL_TYPE_DOMAINE.Secourisme;
                                    break;

                                case "VTUT":
                                    unTypeMateriel.Domaine = MATERIEL_TYPE_DOMAINE.Divers;
                                    break;

                                case "RSR":
                                    unTypeMateriel.Domaine = MATERIEL_TYPE_DOMAINE.Divers;
                                    break;

                                case "Prompts de Secours":
                                    unTypeMateriel.Domaine = MATERIEL_TYPE_DOMAINE.Secourisme;
                                    break;

                                default:
                                    unTypeMateriel.Domaine = MATERIEL_TYPE_DOMAINE.Divers;
                                    break;
                            }
                            this.InsertTypeMateriel(unTypeMateriel);                       
                        }
                        //Affectation du type matériel au Matériel
                        unMateriel.TypeMateriel = unTypeMateriel;
                        this.materielRepository.Update(unMateriel);
                        this.unitOfWork.Save();
                    }
                }
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.TypeMateriel, null, "Erreur Lors de la Génération des Types de Matériel", ex.Message, null);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }


        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
