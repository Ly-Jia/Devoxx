using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Devoxx.Common;
using Devoxx.Data;
using Devoxx.Model;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Devoxx
{
    public sealed partial class PivotPage : Page
    {
        private const string FirstGroupName = "wednesday";
        private const string SecondGroupName = "thursday";
        private const string ThirdGroupName = "friday";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var sampleDataGroup = await ScheduleDataSource.GetScheduleAsync(FirstGroupName);
            this.DefaultViewModel[FirstGroupName] = sampleDataGroup;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((Slot)e.ClickedItem).Id;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await ScheduleDataSource.GetScheduleAsync(SecondGroupName);
            this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }

        private async void ThirdPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await ScheduleDataSource.GetScheduleAsync(ThirdGroupName);
            this.DefaultViewModel[ThirdGroupName] = sampleDataGroup;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void GoToScheduleByHourPage(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(ScheduleByHourPage), e))
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

        private void GoToAboutPage(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(AboutPage), e))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void GoToAgendaPage(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof (AgendaPage), e))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void AddAgendaSlot(object sender, RoutedEventArgs e)
        {
            var slot = (e.OriginalSource as FrameworkElement).DataContext;
            AgendaDataSource.AddSlotAsync(slot as Slot);
        }
    }
}
