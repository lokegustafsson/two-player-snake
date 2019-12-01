using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace TwoPlayerSnake.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void Notify([CallerMemberName] string property = null)
        {
            Program.Log(this).Debug("Notifying {property}", property);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}