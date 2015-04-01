using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Devoxx.Model;

namespace Devoxx.Data
{
    public class AgendaDataSource
    {
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private const string dataFolder = "Data";
        private const string agendaFile = "Agenda.json";

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
            var folder = await localFolder.CreateFolderAsync(dataFolder, CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(agendaFile, CreationCollisionOption.OpenIfExists);

            using (var s = await file.OpenStreamForWriteAsync())
            {
                var textBytes = Utils.SerializeAgenda(slots);
                s.Write(textBytes, 0, textBytes.Length);
            }
        }

        private async Task LoadAsync()
        {
            if (localFolder != null)
            {
                var folder = await localFolder.GetFolderAsync(dataFolder);
                var stream = await folder.OpenStreamForReadAsync(agendaFile);

                var slots = Utils.DeserializeAgenda(stream);
                _agendaDataSource.agenda = slots;
            }
        }
    }
}
