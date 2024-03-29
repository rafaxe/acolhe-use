﻿using ifsp.acolheuse.mobile.Core.Domain;
using ifsp.acolheuse.mobile.Core.Repositories;
using ifsp.acolheuse.mobile.Core.Settings;
using ifsp.acolheuse.mobile.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace ifsp.acolheuse.mobile.ViewModels
{
    public class ScheduleResponsiblePageViewModel : ViewModelBase
    {
        #region properties

        private ObservableCollection<ScheduleAppointment> meetings;
        private Responsible responsible;

        public ObservableCollection<ScheduleAppointment> Meetings
        {
            get { return meetings; }
            set { meetings = value; RaisePropertyChanged(); }
        }
        public Responsible Responsible
        {
            get { return responsible; }
            set { responsible = value; RaisePropertyChanged(); }
        }
        #endregion

        private INavigationService navigationService;
        private IResponsibleRepository responsibleRepository;
        private IAppointmentRepository appointmentRepository;
        private IInternRepository internRepository;

        public ScheduleResponsiblePageViewModel(INavigationService navigationService, IResponsibleRepository responsibleRepository, IAppointmentRepository appointmentRepository, IInternRepository internRepository) :
          base(navigationService)
        {
            this.navigationService = navigationService;
            this.responsibleRepository = responsibleRepository;
            this.appointmentRepository = appointmentRepository;
            this.internRepository = internRepository;
            Meetings = new ObservableCollection<ScheduleAppointment>();
            BuscarAppointmentsAsync();
        }

        /// <summary>
        /// Creates meetings and stores in a collection.  
        /// </summary>
        public async void VisualizeAppointments(string appointmentNotes)
        {
            Appointment appointment = (await appointmentRepository.GetAllAsync()).FirstOrDefault(x => x.Id == appointmentNotes);
            if (await MessageService.Instance.ShowAsyncYesNo("Deseja visualizar os details do atendimento?"))
            {
                IEnumerable<Intern> interns = (await internRepository.GetAllAsync()).Where(x => appointment.InternsIdCollection.Contains(x.Id));

                var navParameters = new NavigationParameters();
                navParameters.Add("interns", interns);
                navParameters.Add("patient", appointment.Patient);
                navParameters.Add("schedule", appointment.StartTime);
                navParameters.Add("type_consultation", appointment.ConsultationType);
                await navigationService.NavigateAsync("DetailsSchedulePage", navParameters);
            }
        }

        public async void BuscarAppointmentsAsync()
        {
            IsBusy = true;
            Meetings = new ObservableCollection<ScheduleAppointment>();
            Responsible = await responsibleRepository.GetAsync(Settings.UserId);


            IEnumerable<Appointment> appointments = await appointmentRepository.GetAllByResponsibleId(Responsible.Id);

            GetAppointments(appointments.Where(x => x.ConsultationType != Appointment._GRUPO));

            IEnumerable<Appointment> groupAppointments = appointments
                .Where(x => x.ConsultationType == Appointment._GRUPO)
                .GroupBy(x => x.StartTime)
                .Select(g => new Appointment()
                {
                    Canceled = g.FirstOrDefault().Canceled,
                    Confirmed = g.FirstOrDefault().Confirmed,
                    StartTime = g.FirstOrDefault().StartTime,
                    EndTime = g.FirstOrDefault().EndTime,
                    Id = g.FirstOrDefault().Id,
                    EventName = g.FirstOrDefault().EventName,
                    IdResponsible = g.FirstOrDefault().IdResponsible,
                    Patient = g.FirstOrDefault().Patient,
                    IdAction = g.FirstOrDefault().IdAction,
                    Capacity = g.FirstOrDefault().Capacity,
                    AllDay = g.FirstOrDefault().AllDay,
                    Repeat = g.FirstOrDefault().Repeat,
                    ConsultationType = g.FirstOrDefault().ConsultationType,
                    InternsIdCollection = g.FirstOrDefault().InternsIdCollection,
                });

            GetAppointments(groupAppointments);

           
            IsBusy = false;
        }
        private void GetAppointments(IEnumerable<Appointment> appointments)
        {
            for (int i = 0; i < appointments.Count(); i++)
            {
                ScheduleAppointment appointment = new ScheduleAppointment();
                appointment.Subject = appointments.ElementAt(i).DescricaoConsultation;
                appointment.Color = appointments.ElementAt(i).Cor;
                appointment.StartTime = appointments.ElementAt(i).StartTime;
                appointment.EndTime = appointments.ElementAt(i).EndTime;
                appointment.Notes = appointments.ElementAt(i).Id;
                if (appointments.ElementAt(i).Repeat)
                {
                    RecurrenceProperties recurrenceProperties = new RecurrenceProperties();
                    recurrenceProperties.RecurrenceType = RecurrenceType.Weekly;
                    recurrenceProperties.RecurrenceRange = RecurrenceRange.NoEndDate;

                    switch (appointment.StartTime.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            recurrenceProperties.WeekDays = WeekDays.Monday;
                            break;
                        case DayOfWeek.Tuesday:
                            recurrenceProperties.WeekDays = WeekDays.Tuesday;
                            break;
                        case DayOfWeek.Wednesday:
                            recurrenceProperties.WeekDays = WeekDays.Wednesday;
                            break;
                        case DayOfWeek.Thursday:
                            recurrenceProperties.WeekDays = WeekDays.Thursday;
                            break;
                        case DayOfWeek.Friday:
                            recurrenceProperties.WeekDays = WeekDays.Friday;
                            break;
                    }

                    appointment.RecurrenceRule = DependencyService.Get<IRecurrenceBuilder>().RRuleGenerator(recurrenceProperties, appointment.StartTime, appointment.EndTime);
                }

                Meetings.Add(appointment);

            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
