using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using SpfInventaire.Core.BLL.Interfaces;
using System.Collections;
using System.Collections.Generic;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class LoggerService : ILoggerService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<Log> logRepository;
        private IGenericRepository<ApplicationUser> userRepository;

        public LoggerService(IUnitOfWork unitOfWork, IGenericRepository<Log> logRepository, IGenericRepository<ApplicationUser> userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.logRepository = logRepository;
            this.userRepository = userRepository;
        }

        public void LogEvenement(LOG_TYPE_EVENT typeEvent, LOG_TYPE_OBJECT? typeObject, int? idObject, string description, string exception, string user_id)
        {
            try
            {
                Log unLog = new Log();
                unLog.TypeEvt = typeEvent;
                unLog.TypeObject = typeObject;
                unLog.IdObject = idObject;
                unLog.Description = description;
                unLog.Exception = exception;
                unLog.DateCreation = DateTime.Now;
                unLog.Utilisateur = this.userRepository.GetByID(user_id);

                this.logRepository.Insert(unLog);
                this.unitOfWork.Save();
            }
            catch(Exception)
            {
                throw;
            }          

        }

        public void LogErreur(LOG_TYPE_OBJECT? typeObject, int? idObject, string description, string exception, string user_id)
        {
            try
            {
                Log unLog = new Log();

                unLog.TypeEvt = LOG_TYPE_EVENT.Erreur;
                unLog.TypeObject = typeObject;
                unLog.IdObject = idObject;
                unLog.Description = description;
                unLog.Exception = exception;
                unLog.DateCreation = DateTime.Now;
                unLog.Utilisateur = this.userRepository.GetByID(user_id);

                this.logRepository.Insert(unLog);
                this.unitOfWork.Save();
            }
            catch (Exception ex)
            {
            }
        }


        public IEnumerable<Log> GetLogs()
        {
            return this.logRepository.Get();
        }

        public Log GetLogById(object id)
        {
            return this.logRepository.GetByID(id);
        }


        public ActionControllerResult Delete(int id)
        {
            ActionControllerResult result;
            try
            {
                Log unLog = this.GetLogById(id);
                this.logRepository.Delete(unLog);
                this.unitOfWork.Save();

                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult DeleteLogsByUserId(string userId)
        {
            ActionControllerResult result;
            try
            {
                IEnumerable<Log> listLogs = this.logRepository.Get(
                    filter: f => f.Utilisateur.Id == userId
                    );

                foreach(Log unLog in listLogs)
                {
                    this.Delete(unLog.ID);
                }
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult DeleteLogsAnterieurDate(DateTime date)
        {
            ActionControllerResult result;
            try
            {
                IEnumerable<Log> listLogs = this.logRepository.Get(
                    filter: f =>f.DateCreation < date
                    );

                foreach (Log unLog in listLogs)
                {
                    this.Delete(unLog.ID);
                }
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
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
