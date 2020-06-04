using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

using System.Threading;
using System.Threading.Tasks;

namespace Jasily.EF.MigrationHistoryBuilder
{
    public interface IHistoryBuilder
    {
        DatabaseFacade DatabaseFacade { get; }

        IHistoryRepository HistoryRepository { get; }

        IHistoryBuilder AddAsTrustedApplied<T>() where T : Migration;

        Task<IHistoryBuilder> AddAsTrustedAppliedAsync<T>(CancellationToken cancellationToken = default) where T : Migration;
    }
}
