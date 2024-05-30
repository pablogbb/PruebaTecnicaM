using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SlugTenant { get; set; }
        public List<UserOrganization> userOrganizations { get; set; } = [];
    }
}
