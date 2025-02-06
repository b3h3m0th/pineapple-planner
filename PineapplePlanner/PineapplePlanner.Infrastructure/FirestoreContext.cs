using Google.Cloud.Firestore;

namespace PineapplePlanner.Infrastructure
{
    public sealed class FirestoreContext
    {
        private FirestoreDb _firestoreDb;

        public FirestoreDb FirestoreDb
        {
            get { return _firestoreDb; }
            set { _firestoreDb = value; }
        }

        public FirestoreContext()
        {
            string projectId = "your-firebase-project-id";
            _firestoreDb = FirestoreDb.Create(projectId);
        }
    }
}
