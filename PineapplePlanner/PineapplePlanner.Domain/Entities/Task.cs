using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class Task : Entry, IBaseFirestoreData
    {
        [FirestoreProperty]
        public DateTime? CompletedAt { get; set; }

        public bool IsCompleted { get => CompletedAt != null; }

        [FirestoreProperty]
        public DateTime? DateDue { get; set; }

        public Task() : base(nameof(Task)) { }
    }
}

