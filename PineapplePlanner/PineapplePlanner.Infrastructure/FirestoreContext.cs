using Google.Cloud.Firestore;

namespace PineapplePlanner.Infrastructure
{
    public sealed class FirestoreContext
    {
        private static FirestoreContext? _instance = null;
        private static readonly object _lock = new object();
        private readonly FirestoreDb _firestoreDb;

        // Private constructor to prevent direct instantiation
        private FirestoreContext()
        {
            string projectId = "your-firebase-project-id";
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public static FirestoreContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FirestoreContext();
                        }
                    }
                }
                return _instance;
            }
        }

        public FirestoreDb GetDb() => _firestoreDb;
    }
}
