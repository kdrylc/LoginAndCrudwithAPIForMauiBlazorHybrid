using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForMauiBlazorHybrid.Models
{
    public class UpdateStudentDTO

    {
        public int StudentID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
}
