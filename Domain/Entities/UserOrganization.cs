using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserOrganization
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
