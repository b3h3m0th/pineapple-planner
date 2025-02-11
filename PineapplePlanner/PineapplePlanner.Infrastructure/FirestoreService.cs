using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

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
            // Credential file should be stored in %APPDATA%/PineapplePlanner/firebase-key.json
            String credentialPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PineapplePlanner\\firebase-key.json");
            GoogleCredential credential = GoogleCredential.FromFile(credentialPath);
            FirestoreClientBuilder builder = new FirestoreClientBuilder() { Credential = credential };

            _firestoreDb = FirestoreDb.Create("pineapple-planner", builder.Build());
        }
    }
}
