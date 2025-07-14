using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AspNetUsersSpec : BaseSpecification<AspNetUser>
{
    public AspNetUsersSpec(int skip, int take)
    {
        ApplyPaging(skip, take);
    }

    public AspNetUsersSpec() { }
}
