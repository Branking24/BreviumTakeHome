using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreviumTakeHome
{
    public class MarkAppointmentRequest
    {
        public DoctorId doctorId { get; set; }

        public int personId { get; set; }

        public string appointmentTime { get; set; }

        public bool isNewPatientAppointment { get; set; }

        public int requestId { get; set; }
    }
}
