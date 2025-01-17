namespace Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Common
{
    public interface IEntity<out TId> : IAuditableEntity where TId : IStronglyTypedId
    {
        TId? Id { get; }
    }
}
