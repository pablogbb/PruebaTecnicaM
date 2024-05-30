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
        void AssingUserToOrg(int userId, int organizationId);
        IEnumerable<Organization> GetOrganizationsByUser(int userId);
        //User? GetUserByEmailAndPassword(string email, string password);
    }
    public class UserRepository : GenericRepository<User, OrganizationAndUsersEfContext>, IUserRepository
    {
        public UserRepository(OrganizationAndUsersEfContext dbContext) : base(dbContext)
        {
        }

        public void AssingUserToOrg(int userId, int organizationId)
        {
            _context.UserOrganizations.Add(new UserOrganization
            {
                UserId = userId,
                OrganizationId = organizationId
            });
            _context.SaveChanges();
        }

        //public User? GetUserByEmailAndPassword(string email, string password)
        //{
        //    return _context.Users.Include(u => u.Organizations).SingleOrDefault(x => x.Email == email && x.Password == password);
        //}

        public IEnumerable<Organization> GetOrganizationsByUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.userOrganizations)
                .Select(uc => uc.Organization)
                .ToList();
        }
    }
}
