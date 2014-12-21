using System.ComponentModel;

namespace ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // CallerMemberName geht erst ab .net 4.5 oder mit einem Patch
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyname = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}