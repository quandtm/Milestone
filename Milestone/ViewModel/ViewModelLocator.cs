/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Milestone"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Windows.Threading;
using Milestone.Model;

namespace Milestone.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static Dispatcher _dispatcher;
        public static Dispatcher Dispatcher
        {
            get { return _dispatcher; }
            set
            {
                if (_dispatcher == value)
                    return;

                _dispatcher = value;
                if (_model != null)
                    _model.Dispatcher = _dispatcher;
            }
        }

        private static MainViewModel _main;

        private static GitHubModel _model;
        public static GitHubModel Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new GitHubModel();
                    _model.Load();
                }
                return _model;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            _main = new MainViewModel();
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }
    }
}