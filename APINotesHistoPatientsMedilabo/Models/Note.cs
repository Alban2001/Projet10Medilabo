using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APINotesHistoPatientsMedilabo.Models
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int MedecinId { get; set; }

        public DateTime DateHeure { get; set; }

        public string UneNote { get; set; } = string.Empty;
    }
}
