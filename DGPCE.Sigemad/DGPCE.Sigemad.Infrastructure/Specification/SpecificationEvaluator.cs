using DGPCE.Sigemad.Application.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Infrastructure.Specification;

public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            inputQuery = inputQuery.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            inputQuery = inputQuery.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.IsPagingEnable)
        {
            inputQuery = inputQuery.Skip(spec.Skip.Value).Take(spec.Take.Value);
        }

        inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

        // Aplicar Includes basados en strings
        inputQuery = spec.IncludeStrings.Aggregate(inputQuery, (current, include) => current.Include(include));


        return inputQuery;
    }

}
