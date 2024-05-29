using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {

    }
    public class UserRepository : GenericRepository<User, OrganizationAndUsersEfContext>, IUserRepository
    {
        public UserRepository(OrganizationAndUsersEfContext dbContext) : base(dbContext)
        {
        }
    }
}
