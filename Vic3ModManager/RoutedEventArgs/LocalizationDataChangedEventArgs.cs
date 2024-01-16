using System.Windows;

namespace Vic3ModManager
{
    public class LocalizationDataChangedEventArgs : RoutedEventArgs
    {
        public bool IsLanguageDeleted { get; private set; }

        public LocalizationDataChangedEventArgs(RoutedEvent routedEvent, bool isLanguageDeleted) : base(routedEvent)
        {
            IsLanguageDeleted = isLanguageDeleted;
        }
    }
}
