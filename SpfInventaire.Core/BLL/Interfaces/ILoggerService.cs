using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface ILoggerService : IDisposable
    {
        void LogEvenement(LOG_TYPE_EVENT typeEvent, LOG_TYPE_OBJECT? typeObject, int? idObject, string description, string exception, string user_id);

        void LogErreur(LOG_TYPE_OBJECT? typeObject, int? idObject, string description, string exception, string user_id);

        IEnumerable<Log> GetLogs();

        ActionControllerResult Delete(int id);

        Log GetLogById(object id);

        ActionControllerResult DeleteLogsByUserId(string userId);
        ActionControllerResult DeleteLogsAnterieurDate(DateTime date);
    }
}