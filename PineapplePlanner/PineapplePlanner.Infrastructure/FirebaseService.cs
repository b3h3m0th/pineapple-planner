using Google.Cloud.Firestore;

namespace PineapplePlanner.Infrastructure
{
    public sealed class FirebaseService
    {
        private FirestoreDb _firestoreDb;

        public FirestoreDb FirestoreDb
        {
            get { return _firestoreDb; }
            set { _firestoreDb = value; }
        }

        public FirebaseService()
        {
            string projectId = "";
            _firestoreDb = FirestoreDb.Create(projectId);
        }
    }
}
