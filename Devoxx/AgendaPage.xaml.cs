using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Devoxx.Common;
using Devoxx.Data;
using Devoxx.Model;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Devoxx
{
    public sealed partial class AgendaPage : Page
    {
        private const string FirstGroupName = "wednesday";
        private const string SecondGroupName = "thursday";
        private const string ThirdGroupName = "friday";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public AgendaPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            var agenda = await AgendaDataSource.GetAgendaByDayAsync(FirstGroupName);
            this.DefaultViewModel[FirstGroupName] = agenda;
        }

        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var agenda = await AgendaDataSource.GetAgendaByDayAsync(SecondGroupName);
            this.DefaultViewModel[SecondGroupName] = agenda;
        }

        
        private async void ThirdPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var agenda = await AgendaDataSource.GetAgendaByDayAsync(ThirdGroupName);
            this.DefaultViewModel[ThirdGroupName] = agenda;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        
        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void SlotItem_Click(object sender, ItemClickEventArgs e)
        {
            var slot = ((Slot)e.ClickedItem);

            if (!Frame.Navigate(typeof(ItemPage), slot.Id))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void GoToScheduleByHourPage(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(ScheduleByHourPage), e))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void GoToAboutPage(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(AboutPage), e))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void RemoveAgendaSlot(object sender, RoutedEventArgs e)
        {
            var slot = (e.OriginalSource as FrameworkElement).DataContext;
            await AgendaDataSource.DeleteSlotAsync(slot as Slot);

            if (!Frame.Navigate(typeof(AgendaPage), e))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void GoToScheduleByRoomPage(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(ScheduleByRoomPage), e))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }
    }
}
