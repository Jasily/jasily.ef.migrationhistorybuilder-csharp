using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.EF.MigrationHistoryBuilder
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Check whether the <see cref="DatabaseFacade"/> was called <see cref="DatabaseFacade.EnsureCreated"/>
        /// instead of call <see cref="RelationalDatabaseFacadeExtensions.Migrate"/> before.
        /// </summary>
        /// <param name="databaseFacade"></param>
        /// <returns></returns>
        public static bool IsCalledEnsureCreatedOnly(this DatabaseFacade databaseFacade)
        {
            if (databaseFacade is null)
                throw new ArgumentNullException(nameof(databaseFacade));

            if (!databaseFacade.GetService<IRelationalDatabaseCreator>().Exists())
                return false;

            return !databaseFacade.GetService<IHistoryRepository>().Exists();
        }

        /// <summary>
        /// Check whether the <see cref="DatabaseFacade"/> was called <see cref="DatabaseFacade.EnsureCreated"/>
        /// instead of call <see cref="RelationalDatabaseFacadeExtensions.Migrate"/> before.
        /// </summary>
        /// <param name="databaseFacade"></param>
        /// <returns></returns>
        public static async Task<bool> IsCalledEnsureCreatedOnlyAsync(this DatabaseFacade databaseFacade, 
            CancellationToken cancellationToken = default)
        {
            if (databaseFacade is null)
                throw new ArgumentNullException(nameof(databaseFacade));

            if (!await databaseFacade.GetService<IRelationalDatabaseCreator>().ExistsAsync(cancellationToken))
                return false;

            return !await databaseFacade.GetService<IHistoryRepository>().ExistsAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFacade"></param>
        /// <returns></returns>
        public static IHistoryBuilder GetHistoryBuilder(this DatabaseFacade databaseFacade)
        {
            if (databaseFacade is null)
                throw new ArgumentNullException(nameof(databaseFacade));

            return new HistoryBuilder(databaseFacade, databaseFacade.GetService<IHistoryRepository>());
        }
    }
}
