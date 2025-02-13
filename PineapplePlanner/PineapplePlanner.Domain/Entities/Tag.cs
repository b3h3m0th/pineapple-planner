using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class Tag : IBaseFirestoreData
    {
        [FirestoreProperty]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public string? Color { get; set; }

        [FirestoreProperty]
        public string? UserUid { get; set; }
    }
}

