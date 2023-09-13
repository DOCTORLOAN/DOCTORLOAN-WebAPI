namespace DoctorLoan.Domain.Common;

public interface IBaseEntity<T>
{
    T Id { get; set; }
}

public abstract class BaseEntity<T> : IBaseEntity<T>
{
    public T Id { get; set; }
}

public abstract class BaseEntityAudit<T> : AuditableEntity, IBaseEntity<T>
{
    public T Id { get; set; }
}

public abstract class AuditableEntity
{
    public DateTimeOffset Created { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public int? LastModifiedBy { get; set; }
}