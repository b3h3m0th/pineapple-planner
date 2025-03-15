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
            GoogleCredential credential = GoogleCredential.FromFile("firebase-adminsdk-key.json");
            FirestoreClientBuilder builder = new() { Credential = credential };

            _firestoreDb = FirestoreDb.Create("pineapple-planner", builder.Build());
        }
    }
}
