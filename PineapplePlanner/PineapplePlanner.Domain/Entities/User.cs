using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class User : IBaseFirestoreData
    {
        [FirestoreProperty]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Username { get; set; }

        [FirestoreProperty]
        public bool IsDarkMode { get; set; }

        [FirestoreProperty]
        public required string UserUid { get; set; }
    }
}
