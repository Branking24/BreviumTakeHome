namespace BreviumTakeHome
{
    public class Appointment
    {
        public DoctorId doctorId { get; set; }

        public int personId { get; set; }

        public string appointmentTime { get; set; }

        public bool isNewPatientAppointment { get; set; }
    }
}