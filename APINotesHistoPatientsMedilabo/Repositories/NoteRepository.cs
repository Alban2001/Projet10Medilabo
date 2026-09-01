using MongoDB.Driver;
using APINotesHistoPatientsMedilabo.Models;

namespace APINotesHistoPatientsMedilabo.Repositories
{
    public class NoteRepository
    {
        private readonly IMongoCollection<Note> _collection;

        public NoteRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Note>("Notes");
        }

        public async Task<List<Note>> GetAllAsync()
        {
            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }
        public async Task<List<Note>> GetAllByPatientAsync(int idPatient)
        {
            return await _collection
                .Find(x => x.PatientId == idPatient)
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _collection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Note note)
        {
            await _collection.InsertOneAsync(note);
        }

        public async Task UpdateAsync(Note note)
        {
            var filter = Builders<Note>.Filter
                .Eq(x => x.Id, note.Id);

            await _collection.ReplaceOneAsync(filter, note);
        }

        public async Task DeleteAsync(int id)
        {
            var filter = Builders<Note>.Filter
                .Eq(x => x.Id, id);

            await _collection.DeleteOneAsync(filter);
        }
    }
}


