using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

public static class SummaryListHelper
{
    public static List<LambdaExpression> For<TModel>(params Expression<Func<TModel, object>>[] properties)
        => new(properties);
}