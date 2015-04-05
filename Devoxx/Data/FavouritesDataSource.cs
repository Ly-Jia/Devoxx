using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Devoxx.Model;

namespace Devoxx.Data
{
    public class FavouritesDataSource
    {
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private static FavouritesDataSource _favouritesDataSource = new FavouritesDataSource();

        private ObservableCollection<Slot> favourites = new ObservableCollection<Slot>();
        public ObservableCollection<Slot> Favourites
        {
            get { return this.favourites; }
        }

        private FavouritesDataSource()
        {
        
        }

        public static async Task AddSlotAsync(Slot slot)
        {
            if (!_favouritesDataSource.favourites.Any())
            {
                await _favouritesDataSource.LoadAsync();
            }

            if (!_favouritesDataSource.favourites.Contains(slot))
            {
                _favouritesDataSource.favourites.Add(slot);
                _favouritesDataSource.SaveAsync(_favouritesDataSource.favourites);
            }
        }

        public static async Task DeleteSlotAsync(Slot slot)
        {
            _favouritesDataSource.favourites.Remove(slot);
            _favouritesDataSource.SaveAsync(_favouritesDataSource.favourites);
        }

        public async static Task<IEnumerable<Slot>> GetFavouritesByDayAsync(string day)
        {
            if (!_favouritesDataSource.favourites.Any())
            {
                await _favouritesDataSource.LoadAsync();
            }

            if(_favouritesDataSource.favourites != null)
                return _favouritesDataSource.favourites.Where(slot => slot.Day == day).OrderBy(slot => slot.FromTimeToTime);

            return new List<Slot>();
        }

        private async void SaveAsync(IEnumerable<Slot> slots)
        {
            var file = await localFolder.CreateFileAsync("Agenda.json", CreationCollisionOption.ReplaceExisting);

            var favouritesText = Utils.SerializeFavourites(slots);
            await FileIO.WriteTextAsync(file, favouritesText, UnicodeEncoding.Utf8);
        }

        private async Task LoadAsync()
        {
            if (localFolder != null)
            {
                try
                {
                    var file = await localFolder.GetFileAsync("Agenda.json");
                    var favouritesText = await FileIO.ReadTextAsync(file);

                    var slots = Utils.DeserializeFavourites(favouritesText);
                    _favouritesDataSource.favourites = slots;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
