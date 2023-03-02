using dotnetConf2023.Domain;
using dotnetConf2023.Shared.Abstraction.Contexts;
using dotnetConf2023.Shared.Abstraction.Databases;
using dotnetConf2023.Shared.Abstraction.Entities;
using dotnetConf2023.Shared.Abstraction.Time;
using Microsoft.EntityFrameworkCore;

namespace dotnetConf2023.Persistence;

public class SqlServerDbContext : DbContext, IDbContext
{
    private readonly IContext? _context;
    private readonly IClock _clock;

    public SqlServerDbContext(
        DbContextOptions<SqlServerDbContext> options,
        IContext? context,
        IClock clock)
        : base(options)
    {
        _context = context;
        _clock = clock;
    }

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : BaseEntity =>
        base.Set<TEntity>();

    public void Insert<TEntity>(TEntity entity)
        where TEntity : BaseEntity =>
        Set<TEntity>().Add(entity);

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : BaseEntity =>
        Set<TEntity>().Remove(entity);

    public void DetachEntities()
    {
        var changedEntriesCopy = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }

    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var contextExist = !(_context is null || !_context.Identity.IsAuthenticated);

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    if (contextExist)
                    {
                        entry.Entity.CreatedBy = _context!.Identity.Id.ToString();
                        entry.Entity.CreatedByName = _context.Identity.Username;
                    }

                    entry.Entity.CreatedDt = _clock.CurrentDate();
                    entry.Entity.CreatedDtServer = _clock.CurrentServerDate();
                    break;
                }
                case EntityState.Modified:
                    if (contextExist)
                    {
                        entry.Entity.LastUpdatedBy = _context!.Identity.Id.ToString();
                        entry.Entity.LastUpdatedByName = _context.Identity.Username;
                    }
                    else
                    {
                        if (entry.Entity.LastUpdatedBy is not null)
                        {
                            entry.Entity.LastUpdatedBy = null;
                            entry.Entity.LastUpdatedByName = null;
                        }
                    }

                    entry.Entity.LastUpdatedDt = _clock.CurrentDate();
                    entry.Entity.LastUpdatedDtServer = _clock.CurrentServerDate();

                    if (entry.Entity.DeletedByDt.HasValue || entry.Entity.DeletedByDtServer.HasValue)
                    {
                        entry.Entity.DeletedByDt ??= _clock.CurrentDate();
                        entry.Entity.DeletedByDtServer ??= _clock.CurrentServerDate();

                        if (contextExist)
                        {
                            entry.Entity.DeletedBy = _context!.Identity.Id.ToString();
                            entry.Entity.DeletedByName = _context.Identity.Username;
                        }
                    }

                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    throw new Exception("Deleted is not acceptable");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Marker).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}