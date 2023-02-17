using BreviumTakeHome;
using System.Net;
using System.Text.Json;

public class ScheduleProgram
{

    public const string token = "token=47b4fbde-0e6d-45f2-8dab-0ac28a61a70a";

    public string baseUri = "http://scheduling-interview-2021-265534043.us-west-2.elb.amazonaws.com";

    public string startUri = "/api/Scheduling/Start";

    public string doneUri = "/api/Scheduling/Stop";

    public string getAppointmentUri = "/api/Scheduling/AppointmentRequest";

    public string getScheduleUri = "/api/Scheduling/Schedule";

    public string markAppointmentUri = "/api/Scheduling/Schedule";

    public void StartProgram()
    {
        WebRequest startRequest = WebRequest.Create(baseUri + startUri + "?" + token);
        startRequest.Method = "POST";
        try
        {
            var startResponse = (HttpWebResponse)startRequest.GetResponse();
            if (startResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(startResponse.StatusCode.ToString());
            }
            startResponse.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        

        return;
    }

    public async void EndProgram()
    {
        WebRequest doneRequest = WebRequest.Create(baseUri + doneUri + "?" + token);
        doneRequest.Method = "POST";
        doneRequest.ContentType = "application/json";
        try
        {
            var doneResponse = (HttpWebResponse)doneRequest.GetResponse();
            if (doneResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(doneResponse.StatusCode.ToString());
            }
            using (var stream = doneResponse.GetResponseStream())
            {
                List<Appointment> appointments = await JsonSerializer.DeserializeAsync<List<Appointment>>(stream);

            }
            doneResponse.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<Schedule> GetCurrentSchedule()
    {
        WebRequest currentScheduleRequest = WebRequest.Create(baseUri + getScheduleUri + "?" + token);
        currentScheduleRequest.Method = "GET";
        currentScheduleRequest.ContentType = "application/json";

        try
        {
            var currentScheduleResponse = (HttpWebResponse)currentScheduleRequest.GetResponse();
            if (currentScheduleResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(currentScheduleResponse.StatusCode.ToString());
            }
            Schedule curSchedule = new Schedule();
            using (var stream = currentScheduleResponse.GetResponseStream())
            {
                List<Appointment> appointments = await JsonSerializer.DeserializeAsync<List<Appointment>>(stream);
                curSchedule.Appointments = appointments;
            }
            currentScheduleResponse.Close();

            return curSchedule;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return new Schedule();
        }
    }
    public async Task<NewAppointmentRequest> GetNextAppointment() 
    {
        WebRequest getAppointmentRequest = WebRequest.Create(baseUri + getAppointmentUri + "?" + token);
        getAppointmentRequest.Method = "GET";
        getAppointmentRequest.ContentType = "application/json";

        try
        {
            var getAppointmentResponse = (HttpWebResponse)getAppointmentRequest.GetResponse();

            if (getAppointmentResponse.StatusCode == HttpStatusCode.NoContent)
            {
                var finishedRequest = new NewAppointmentRequest();
                finishedRequest.requestId = -1;
                return finishedRequest;
            }
            else if (getAppointmentResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(getAppointmentResponse.StatusCode.ToString());
            }

            var appointmentRequest = new NewAppointmentRequest();

            using (var stream = getAppointmentResponse.GetResponseStream())
            {
                appointmentRequest = await JsonSerializer.DeserializeAsync<NewAppointmentRequest>(stream);
            }

            getAppointmentResponse.Close();

            return appointmentRequest;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return new NewAppointmentRequest();
        }
    }

    public async void MarkAppointment(MarkAppointmentRequest req) 
    {
        WebRequest markAppointmentRequest = WebRequest.Create(baseUri + markAppointmentUri + "?" + token);
        markAppointmentRequest.Method = "POST";
        markAppointmentRequest.ContentType = "application/json";
        using (var stream = new StreamWriter(markAppointmentRequest.GetRequestStream()))
        {
            stream.Write(JsonSerializer.Serialize<MarkAppointmentRequest>(req));
        }

            try
            {
                var markAppointmentResponse = (HttpWebResponse)markAppointmentRequest.GetResponse();

                if (markAppointmentResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(markAppointmentResponse.StatusCode.ToString());
                }

                markAppointmentResponse.Close();

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
    }
}