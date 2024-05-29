using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IOrganizationRepository: IGenericRepository<Organization>
    {

    }
    public class OrganizationRepository : GenericRepository<Organization, OrganizationAndUsersEfContext>, IOrganizationRepository
    {
        public OrganizationRepository(OrganizationAndUsersEfContext dbContext) : base(dbContext)
        {
        }
    }
}
