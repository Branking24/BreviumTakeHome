using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreviumTakeHome
{
    public class NewAppointmentRequest
    {
        public int requestId { get; set; }

        public int personId { get; set; }

        public List<string> preferredDays { get; set; }

        public List<DoctorId> preferredDocs { get; set; }

        public bool isNew { get; set; }
    }
}
