using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.EF.MigrationHistoryBuilder
{
    class HistoryBuilder : IHistoryBuilder
    {
        private bool _isRepositoryCreated = false;

        public DatabaseFacade DatabaseFacade { get; }

        public IHistoryRepository HistoryRepository { get; }

        public HistoryBuilder(DatabaseFacade databaseFacade, IHistoryRepository historyRepository)
        {
            this.DatabaseFacade = databaseFacade;
            this.HistoryRepository = historyRepository;
        }

        private void EnsureRepositoryCreated()
        {
            if (!this._isRepositoryCreated)
            {
                this.DatabaseFacade.ExecuteSqlRaw(this.HistoryRepository.GetCreateIfNotExistsScript());
                this._isRepositoryCreated = true;
            }
        }

        private async ValueTask EnsureRepositoryCreatedAsync(CancellationToken cancellationToken)
        {
            if (!this._isRepositoryCreated)
            {
                await this.DatabaseFacade.ExecuteSqlRawAsync(
                    this.HistoryRepository.GetCreateIfNotExistsScript(), 
                    cancellationToken)
                    .ConfigureAwait(false);
                this._isRepositoryCreated = true;
            }
        }

        public IHistoryBuilder AddAsTrustedApplied<T>() where T : Migration
        {
            this.EnsureRepositoryCreated();

            var sql = this.HistoryRepository.GetInsertScript(
                new HistoryRow(typeof(T).GetCustomAttribute<MigrationAttribute>().Id, ProductInfo.GetVersion())
            );

            this.DatabaseFacade.ExecuteSqlRaw(sql);

            return this;
        }

        public async Task<IHistoryBuilder> AddAsTrustedAppliedAsync<T>(CancellationToken cancellationToken = default) where T : Migration
        {
            await this.EnsureRepositoryCreatedAsync(cancellationToken).ConfigureAwait(false);

            var sql = this.HistoryRepository.GetInsertScript(
                new HistoryRow(typeof(T).GetCustomAttribute<MigrationAttribute>().Id, ProductInfo.GetVersion())
            );

            await this.DatabaseFacade.ExecuteSqlRawAsync(sql, cancellationToken).ConfigureAwait(false);

            return this;
        }
    }
}
