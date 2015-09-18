using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace LunchPac
{
    public class ViewModelBase : IViewModel
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IViewModel implementation

        public void SetState<T>(Action<T> action) where T : class, IViewModel
        {
            action(this as T);
        }

        string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetRaiseIfPropertyChanged(ref _title, value);
            }
        }

        #endregion

        protected virtual bool SetRaiseIfPropertyChanged<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

