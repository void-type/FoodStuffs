using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace DataMigratorV14.OldData;

public partial class FoodStuffsContext
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options, IDateTimeService dateTimeService, ICurrentUserAccessor currentUserAccessor)
        : base(options)
    {
        _dateTimeService = dateTimeService;
        _currentUserAccessor = currentUserAccessor;

        ChangeTracker.LazyLoadingEnabled = false;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.Entries().SetAuditableProperties(_dateTimeService, _currentUserAccessor.User.Login);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.Entries().SetAuditableProperties(_dateTimeService, _currentUserAccessor.User.Login);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
