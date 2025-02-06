using Google.Cloud.Firestore;

namespace PineapplePlanner.Infrastructure
{
    public sealed class FirestoreService
    {
        private FirestoreDb _firestoreDb;

        public FirestoreDb FirestoreDb
        {
            get { return _firestoreDb; }
            set { _firestoreDb = value; }
        }

        public FirestoreService()
        {
            string projectId = "";
            _firestoreDb = FirestoreDb.Create(projectId);
        }
    }
}
