using System;
using System.Net;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMLightFramework.WebService;

namespace MVVMLightFramework.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand<string> FireClickCommand { get; set; }
        public RelayCommand<string> PromoteClickCommand { get; set; }
        private SPS[] _spsArray;
        public SPS[] SpsArray
        {
            get => _spsArray;
            set { _spsArray = value; RaisePropertyChanged(); }
        }
        private CreateTask[] _taskArray;
        public CreateTask[] TaskArray
        {
            get => _taskArray;
            set { _taskArray = value; RaisePropertyChanged(); }
        }
        public decimal SumAmounts { get; set; }

        public MainViewModel()
        {
            SPS_Binding service = new SPS_Binding();
            service.UseDefaultCredentials = false;
            service.Credentials=new NetworkCredential("DESKTOP-2K6GVM4\\mckracken", "zak");
            
            SpsArray = service.ReadMultiple(null, null, 100);
            foreach (SPS actSPS in SpsArray)
            {
                SumAmounts += actSPS.Calcd_Current_Value_LCY;
            }

            FireClickCommand = new RelayCommand<string>((tt) =>
            {
                decimal ownAmount = 0;
                foreach (SPS actSPS in SpsArray)
                {
                    if (actSPS.Code == tt) ownAmount = actSPS.Calcd_Current_Value_LCY;
                }
                CreateTaskTry_Binding taskBinding = new CreateTaskTry_Binding
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("DESKTOP-2K6GVM4\\mckracken", "zak")
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
                foreach (SPS actSPS in SpsArray)
                {
                    if (actSPS.Code == tt) ownAmount = actSPS.Calcd_Current_Value_LCY;
                }
                CreateTaskTry_Binding taskBinding = new CreateTaskTry_Binding
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("DESKTOP-2K6GVM4\\mckracken", "zak")
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