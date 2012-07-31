﻿using System;
using System.Windows.Input;
using wp7_api_demos.Model;
using System.ComponentModel;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Windows;

namespace wp7_api_demos.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private INavigationService navigationService;

        private bool isBusy;

        private String busyMessage;

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public String BusyMessage
        {
            get
            {
                return this.busyMessage;
            }

            set
            {
                this.busyMessage = value;
                RaisePropertyChanged("BusyMessage");
            }
        }

        public ICommand CreateNewSessionCommand
        {
            get
            {
                return new DelegateCommand(this.CreateNewSession);
            }
        }

        public ICommand ConnectToSessionCommand
        {
            get
            {
                return new DelegateCommand(this.ConnectToExistingSession);
            }
        }

        public int SessionCode { get; set; }

        private void CreateNewSession(object arg)
        {
            this.BusyMessage = "Creating new demo session...";
            this.IsBusy = true;

            CreateSessionTask createTask = new CreateSessionTask((sessionCode) =>
                {
                    this.IsBusy = false;
                    if (sessionCode == null)
                    {
                        navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotCreateSession);
                    }
                    else
                    {
                        this.SessionCode = Int32.Parse(sessionCode);
                        this.navigationService.Navigate(new Uri(String.Format("/View/NewSessionPage.xaml?SessionCode={0}", this.SessionCode), UriKind.Relative));
                    }
                });
            createTask.Execute();
        }

        private void ConnectToExistingSession(object arg)
        {
            this.BusyMessage = "Logging in...";
            this.IsBusy = true;
            Mobeelizer.Login(SessionCode.ToString(), Resources.Config.c_userALogin, Resources.Config.c_userAPassword, (result) =>
                {
                    this.IsBusy = false;
                    try
                    {
                        var status = result.GetLoginStatus();
                        switch (status)
                        {
                            case MobeelizerLoginStatus.OK:
                                navigationService.Navigate(new Uri("TODO Explorer page"));
                                break;
                            case MobeelizerLoginStatus.MISSING_CONNECTION_FAILURE:
                                this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_missingConnection);
                                break;
                            case MobeelizerLoginStatus.CONNECTION_FAILURE:
                            case MobeelizerLoginStatus.AUTHENTICATION_FAILURE:
                            case MobeelizerLoginStatus.OTHER_FAILURE:
                                this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                                break;
                        }
                    }
                    catch
                    {
                        this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                    }
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
