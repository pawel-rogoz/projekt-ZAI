using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;

namespace IoTAnalyzer.Infrastructure.Persistance.Repositories;

public class EfRepository<T>(IoTAnalyzerDbContext dbContext)
    : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot;
