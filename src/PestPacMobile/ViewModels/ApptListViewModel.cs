using System;
using System.Collections.ObjectModel;
using Marathon.Mobile.Models;
using System.Collections.Generic;
using PestPac.Mobile.Core;
using System.Threading.Tasks;

namespace PestPacMobile
{
    public class ApptListViewModel : ViewModelBase
    {
        public AppointmentProxy AppointmentProxy { get; set; }

        ObservableCollection<Appointment> _Appointment;

        public ObservableCollection<Appointment> Appointments { get { return _Appointment; } set { SetRaiseIfPropertyChanged(ref _Appointment, value); } }

        DateTime _Date;

        public DateTime Date { get { return _Date; } set { SetRaiseIfPropertyChanged(ref _Date, value); } }

        public ApptListViewModel(AppointmentProxy appointmentProxy)
        {
            Date = DateTime.Today;
            AppointmentProxy = appointmentProxy;
        }

        public IEnumerable<Appointment> GetAppointments()
        {
//            Random rnd = new Random();
//            return new []
//            { 
//                new Appointment { Id = Guid.NewGuid().ToString(), WorkOrderId = rnd.Next(9999), ServiceName = "Inspection" },
//                new Appointment { Id = Guid.NewGuid().ToString(), WorkOrderId = rnd.Next(9999), ServiceName = "Service" },
//                new Appointment { Id = Guid.NewGuid().ToString(), WorkOrderId = rnd.Next(9999), ServiceName = "Clean Up" },
//                new Appointment { Id = Guid.NewGuid().ToString(), WorkOrderId = rnd.Next(9999), ServiceName = "Weekly Checkin" }
//            };
            var result = Task.Run(async () =>
                {
                    return await AppointmentProxy.GetAppointmentsAsync();
                }).GetAwaiter().GetResult();
            return result;
        }
    }
}

