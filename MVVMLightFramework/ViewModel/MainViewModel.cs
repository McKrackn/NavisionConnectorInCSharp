using System;
using System.Collections.ObjectModel;
using System.Net;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMLightFramework.WebService;

namespace MVVMLightFramework.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand NAVConnectCommand { get; set; }
        public RelayCommand<string> FireClickCommand { get; set; }
        public RelayCommand<string> PromoteClickCommand { get; set; }
        public string Pass { get; set; } = "zak";
        public string User { get; set; } = "DESKTOP-2K6GVM4\\mckracken";
        public string HeaderMessage { get; set; } = "... ready to connect ...";
        private string _url = "http://192.168.0.192:7047/DynamicsNAV110/WS/ERPSPEZ/";
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                RaisePropertyChanged();
            }
        }
        private string _nameVisibility = "True";
        public string NameVisibility
        {
            get => _nameVisibility;
            set { _nameVisibility = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<SalesPerson> SalesPersons { get; set; } = new ObservableCollection<SalesPerson>();
        private CreateTask[] _taskArray;
        public CreateTask[] TaskArray
        {
            get => _taskArray;
            set { _taskArray = value; RaisePropertyChanged(); }
        }
        public decimal SumAmounts { get; set; }
        public MainViewModel()
        {
            NAVConnectCommand = new RelayCommand(() =>
                {
                    SPS_Binding service = new SPS_Binding();
                    service.Url = Url + "Page/SPS";
                    service.UseDefaultCredentials = false;
                    service.Credentials = new NetworkCredential(User, Pass);

                    try
                    {
                        SPS[] spsArray = service.ReadMultiple(null, null, 100);
                        foreach (SPS spse in spsArray)
                        {
                            SalesPerson tmp = new SalesPerson(spse);
                            SalesPersons.Add(tmp);
                        }
                        foreach (SalesPerson actSalesperson in SalesPersons)
                        {
                            SumAmounts += actSalesperson.Sps.Calcd_Current_Value_LCY;
                        }
                        NameVisibility = "False";
                        HeaderMessage = "Calculated Revenues per Salesperson, total " + SumAmounts.ToString("C2");
                        RaisePropertyChanged("HeaderMessage");
                        NAVConnectCommand.RaiseCanExecuteChanged();
                    }
                    catch (WebException)
                    {
                        HeaderMessage = "Connection failed";
                        RaisePropertyChanged("HeaderMessage");
                    }
                },
                () => NameVisibility == "True");
            

            FireClickCommand = new RelayCommand<string>((tt) =>
            {
                decimal ownAmount = 0;
                foreach (SalesPerson actSalesPerson in SalesPersons)
                {
                    if (actSalesPerson.Sps.Code == tt)
                    {
                        ownAmount = actSalesPerson.Sps.Calcd_Current_Value_LCY;
                        actSalesPerson.Sentmessage = "FIRED";
                    }
                }
                CreateTaskTry_Binding taskBinding = new CreateTaskTry_Binding
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(User, Pass)
                };
                CreateTaskTry newCreateTaskTry = new CreateTaskTry
                {
                    Salesperson_Code = tt,
                    Interaction_Template_Code = "FIRED",
                    Contact_No = "KT000666",
                    Subject = "Probleme!!",
                    Description = "Nur " + ownAmount + " Euro von " + SumAmounts + "?! Verschwinde!"
                };
                taskBinding.Create(ref newCreateTaskTry);
            }, (tt) =>
            {
                return true;
            });
            PromoteClickCommand = new RelayCommand<string>((tt) =>
            {
                decimal ownAmount=0;
                foreach (SalesPerson actSalesPerson in SalesPersons)
                {
                    if (actSalesPerson.Sps.Code == tt)
                    {
                        ownAmount = actSalesPerson.Sps.Calcd_Current_Value_LCY;
                        actSalesPerson.Sentmessage = "FEIERT";
                    }
                }
                CreateTaskTry_Binding taskBinding = new CreateTaskTry_Binding
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(User,Pass)
                };
                CreateTaskTry newCreateTaskTry = new CreateTaskTry
                {
                    Salesperson_Code = tt,
                    Interaction_Template_Code = "FEIERT",
                    Contact_No = "KT000666",
                    Subject = "Well done!",
                    Description = "Party! Du hast " + ownAmount + " Euro von " + SumAmounts + " gemacht"
                };
                taskBinding.Create(ref newCreateTaskTry);
            }, (tt) =>
            {
                return true;
            });
        }
    }
}