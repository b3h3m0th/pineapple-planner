namespace PineapplePlanner.Infrastructure
{
    public sealed class FirebaseDbService
    {
        private static FirebaseDbService _instance = null;
        private static readonly object _lock = new object();
        private readonly FirestoreDb _firestoreDb;

        // Private constructor to prevent direct instantiation
        private FirebaseDbService()
        {
            string projectId = "your-firebase-project-id";
            _firestoreDb = FirestoreDb.Create(projectId);
            Console.WriteLine("Firestore initialized.");
        }

        // Public property to access the singleton instance
        public static FirebaseDbService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FirebaseDbService();
                        }
                    }
                }
                return _instance;
            }
        }

        public FirestoreDb GetDb() => _firestoreDb;
    }
}
