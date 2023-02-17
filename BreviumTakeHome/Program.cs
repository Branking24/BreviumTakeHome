using BreviumTakeHome;
using System.ComponentModel;

var Program = new ScheduleProgram();

Program.StartProgram();

var curSchedule = Program.GetCurrentSchedule().Result;
var curAppointment = new NewAppointmentRequest();


while (curAppointment.requestId != -1) {
    curAppointment = await Program.GetNextAppointment();

    if (curAppointment.requestId == -1)
    {
        break;
    }
    //TODO: Check if patient is new
    foreach (var t in curAppointment.preferredDays)
    {
        if (CheckPatientAvailability(1, t, curSchedule))
        {
            foreach (var doctor in curAppointment.preferredDocs)
            {
                if (CheckDoctorAvailability(doctor, t, curSchedule))
                {
                    var markAppointment = new MarkAppointmentRequest();
                    markAppointment.isNewPatientAppointment = curAppointment.isNew;
                    markAppointment.appointmentTime = t;
                    markAppointment.personId = curAppointment.personId;
                    markAppointment.requestId = curAppointment.requestId;
                    markAppointment.doctorId = doctor;
                    Program.MarkAppointment(markAppointment);
                    break;
                }
            }
            break;
        };
    }

    //TODO: Check if any appointment has been marked, if not then start checking every availability
}

Program.EndProgram();

bool CheckDoctorAvailability(DoctorId id, string preferred, Schedule curAvailability)
{
    foreach (var a in curAvailability.Appointments)
    {
        if (a.doctorId == id && a.appointmentTime == preferred)
        {
            return false;
        }
    }

    return true;
}

bool CheckPatientAvailability(int id, string preferred, Schedule curAvailability)
{
    DateTime preferredTime = DateTime.Parse(preferred);
    foreach (var a in curAvailability.Appointments)
    {
        DateTime curTime = DateTime.Parse(a.appointmentTime);
        if (Math.Abs(DateTime.Compare(curTime, preferredTime)) < 7)
        {
            return false;
        }
    }
    return true;
}