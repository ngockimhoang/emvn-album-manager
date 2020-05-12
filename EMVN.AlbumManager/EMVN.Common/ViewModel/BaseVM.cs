using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.Common.ViewModel
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public event ExceptionDelegate OnException;        

        protected virtual void RaiseException(Exception ex)
        {
            if (OnException != null)
            {
                OnException(ex);
            }
        }
    }

    public delegate void ExceptionDelegate(Exception ex);
}
