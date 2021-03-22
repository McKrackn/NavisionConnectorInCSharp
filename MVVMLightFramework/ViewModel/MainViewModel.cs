using System.Net;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMLightFramework.WebService;

namespace MVVMLightFramework.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand FireClickCommand { get; set; }
        public RelayCommand PromoteClickCommand { get; set; }
        private SPS[] _spsArray;
        public SPS[] SpsArray
        {
            get => _spsArray;
            set { _spsArray = value; RaisePropertyChanged(); }
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

            FireClickCommand = new RelayCommand(() =>
            {
            }, () =>
            {
                return true;
            });
            PromoteClickCommand = new RelayCommand(() =>
            {
            }, () =>
            {
                return true;
            });
        }
    }
}