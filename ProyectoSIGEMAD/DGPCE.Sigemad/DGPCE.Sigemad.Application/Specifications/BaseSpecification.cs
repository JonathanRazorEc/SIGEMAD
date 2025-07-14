using DGPCE.Sigemad.Application.Extensions;
using System.Linq.Expressions;
namespace DGPCE.Sigemad.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification() { }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public List<Func<T, T>> PostFilters { get; } = new();
        public Expression<Func<T, bool>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; private set; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new(); // Para soportar Includes por string

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public BaseSpecification<T> AddCriteria(Expression<Func<T, bool>> additionalCriteria) 
        {
            if(Criteria == null)
            {
                Criteria = additionalCriteria;
            }
            else
            {
                Criteria = Criteria.AndAlso(additionalCriteria);
            }
            return this;
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }


        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnable = true;
        }

        protected void ApplyPaging(SpecificationParams specParams)
        {
            ApplyPaging((specParams.PageIndex) * specParams.PageSize, specParams.PageSize);
        }

        public bool IsPagingEnable { get; private set; }


        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // Método para agregar Includes por string
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new BaseSpecification<T>(this.Criteria.AndAlso(other.Criteria))
            {
                OrderBy = this.OrderBy ?? other.OrderBy,
                OrderByDescending = this.OrderByDescending ?? other.OrderByDescending,
                Includes = this.Includes.Concat(other.Includes).ToList(),
                Skip = Skip ?? other.Skip,
                Take = Take ?? other.Take
            };
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new BaseSpecification<T>(this.Criteria.OrElse(other.Criteria))
            {
                OrderBy = this.OrderBy ?? other.OrderBy,
                OrderByDescending = this.OrderByDescending ?? other.OrderByDescending,
                Includes = this.Includes.Concat(other.Includes).ToList(),
                Skip = Skip ?? other.Skip,
                Take = Take ?? other.Take
            };
        }

        public ISpecification<T> Not()
        {
            return new BaseSpecification<T>(Expression.Lambda<Func<T, bool>>(Expression.Not(Criteria.Body), Criteria.Parameters))
            {
                OrderBy = OrderBy,
                OrderByDescending = OrderByDescending,
                Includes = Includes,
                Skip = Skip,
                Take = Take
            };
        }


        protected void AddPostFilter(Func<T, T> filter)
        {
            PostFilters.Add(filter);
        }

    }
}
