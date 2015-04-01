using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Devoxx.Model;

namespace Devoxx.Data
{
    public class AgendaDataSource
    {
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private static AgendaDataSource _agendaDataSource = new AgendaDataSource();

        private ObservableCollection<Slot> agenda = new ObservableCollection<Slot>();
        public ObservableCollection<Slot> Agenda
        {
            get { return this.agenda; }
        }

        public static async Task AddSlotAsync(Slot slot)
        {
            if (!_agendaDataSource.agenda.Contains(slot))
            {
                _agendaDataSource.agenda.Add(slot);
                _agendaDataSource.SaveAsync(_agendaDataSource.agenda);
            }
        }

        public static async Task DeleteSlotAsync(Slot slot)
        {
            _agendaDataSource.agenda.Remove(slot);
            _agendaDataSource.SaveAsync(_agendaDataSource.agenda);
        }

        public async static Task<IEnumerable<Slot>> GetAgendaByDayAsync(string day)
        {
            if (!_agendaDataSource.agenda.Any())
            {
                await _agendaDataSource.LoadAsync();
            }

            if(_agendaDataSource.agenda != null)
                return _agendaDataSource.agenda.Where(slot => slot.Day == day);

            return new List<Slot>();
        }

        private async void SaveAsync(IEnumerable<Slot> slots)
        {
            var file = await localFolder.CreateFileAsync("Agenda.json", CreationCollisionOption.ReplaceExisting);

                var text = Utils.SerializeAgenda(slots);
                await FileIO.WriteTextAsync(file, text, UnicodeEncoding.Utf8);
        }

        private async Task LoadAsync()
        {
            if (localFolder != null)
            {
                try
                {
                    var file = await localFolder.GetFileAsync("Agenda.json");
                    var agendaText = await FileIO.ReadTextAsync(file);

                    var slots = Utils.DeserializeAgenda(agendaText);
                    _agendaDataSource.agenda = slots;
                }
                catch (Exception e)
                {
                    
                }
                
            }
        }
    }
}
