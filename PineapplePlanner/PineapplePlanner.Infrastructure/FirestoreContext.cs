using Google.Cloud.Firestore;

namespace PineapplePlanner.Infrastructure
{
    public sealed class FirestoreContext
    {
        private readonly FirestoreDb _firestoreDb;

        // Private constructor to prevent direct instantiation
        public FirestoreContext()
        {
            string projectId = "your-firebase-project-id";
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public FirestoreDb GetDb() => _firestoreDb;
    }
}
