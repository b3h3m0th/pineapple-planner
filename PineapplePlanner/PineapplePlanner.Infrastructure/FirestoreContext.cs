using Google.Cloud.Firestore;

namespace PineapplePlanner.Infrastructure
{
    public sealed class FirestoreContext
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreContext()
        {
            string projectId = "your-firebase-project-id";
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public FirestoreDb GetDb() => _firestoreDb;
    }
}
