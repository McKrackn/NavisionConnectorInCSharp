using GalaSoft.MvvmLight;
using MVVMLightFramework.WebService;

namespace MVVMLightFramework.ViewModel
{
    public class SalesPerson : ViewModelBase
    {
        private SPS _sps;
        public SPS Sps
        {
            get => _sps;
            set
            {
                _sps = value;
                RaisePropertyChanged();
            }
        }
        private string _sentMessage;
        public string Sentmessage
        {
            get => _sentMessage;
            set
            {
                _sentMessage = value;
                RaisePropertyChanged();
            }
        }
        public SalesPerson(SPS sps)
        {
            Sps = sps;
        }
    }
}