using System;
using System.Windows.Controls;

namespace Vic3ModManager
{
    public partial class CustomPage : Page
    {
        public event Action<string>? RequestPageChange;

        protected void OnRequestPageChange(string pageName)
        {
            RequestPageChange?.Invoke(pageName);
        }
    }

}
