using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMLightFramework.WebService;

namespace MVVMLightFramework.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand FireClickCommand { get; set; }
        public RelayCommand PromoteClickCommand { get; set; }
        public bool IsConnected { get; set; } = false;
        private SPS[] _spsArray;
        public SPS[] SpsArray
        {
            get => _spsArray;
            set { _spsArray = value; RaisePropertyChanged(); }
        }


        public MainViewModel()
        {
            SPS_Binding service = new SPS_Binding();
            service.UseDefaultCredentials = true;

            SpsArray = service.ReadMultiple(null, null, 100);
            foreach (SPS actSPS in SpsArray)
            {

            }

            FireClickCommand = new RelayCommand(() =>
            {
                IsConnected = true;
            }, () =>
            {
                return !IsConnected;
            });
            PromoteClickCommand = new RelayCommand(() =>
            {
                IsConnected = true;
            }, () =>
            {
                return !IsConnected;
            });
        }
    }
}