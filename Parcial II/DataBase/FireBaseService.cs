using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Firebase.Auth.Providers;
using Firebase.Auth;
using Newtonsoft.Json;
using Google.Cloud.Firestore;
using Google.Type;
using static Google.Cloud.Firestore.V1.StructuredAggregationQuery.Types.Aggregation.Types;
using System.Xml.Linq;
using Parcial_II.Models;
using Google.Cloud.Firestore.V1;

namespace Parcial_II.DataBase
{
    public class FireBaseService : Controller
    {
        private readonly FirestoreDb firestoreDb;
        public static class FirebaseAuthHelper
        {
            public const string firebaseAppId = "tasks-d6588";
            public const string firebaseApiKey = "AIzaSyCvH0yEn7zgupiALf90lXl5pGjYWQt196c";

            public static FirebaseAuthClient setFirebaseAuthClient()
            {
                var response = new FirebaseAuthClient(new FirebaseAuthConfig
                {
                    ApiKey = firebaseApiKey,
                    AuthDomain = $"{firebaseAppId}.firebaseapp.com",
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    }
                });
                return response;
            }

        }

        public async Task UpdateDocumentAsync(string collectionName, string documentId, Dictionary<string, object> updates)
        {
            try
            {
                // Ensure firestoreDb is not null before calling the Collection method
                if (firestoreDb == null)
                {
                    Console.WriteLine("FirestoreDb is not initialized.");
                    return;
                }

                // Specify the document path
                DocumentReference docRef = firestoreDb.Collection(collectionName).Document(documentId);

                // Perform the update
                await docRef.UpdateAsync(updates);

                Console.WriteLine($"Document {documentId} in collection {collectionName} updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"Error updating document: {ex.Message}");
                throw; // Re-throw the exception if needed
            }
        }

    }
}
