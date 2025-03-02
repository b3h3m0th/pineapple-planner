using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class Event : Entry, IBaseFirestoreData
    {
        [FirestoreProperty]
        public DateTime? StartDate { get; set; }

        [FirestoreProperty]
        public DateTime? EndDate { get; set; }

        public Event() : base(nameof(Event)) { }
    }
}

