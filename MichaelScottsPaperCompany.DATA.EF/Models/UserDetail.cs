using System;
using System.Collections.Generic;

namespace MichaelScottsPaperCompany.DATA.EF.Models
{
    public partial class UserDetail
    {
        public UserDetail()
        {
            Sales = new HashSet<Sale>();
        }

        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
