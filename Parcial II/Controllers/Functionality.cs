using Google.Cloud.Firestore;
using Google.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using static Parcial_II.DataBase.FireBaseService;

namespace Parcial_II.Controllers
{
    public class Functionality : Controller
    {

        public async Task<ActionResult> Index(string name, string description, string duedate, string priority)
        {
            if (Convert.ToDateTime(duedate) < System.DateTime.Now)
            {
                string message = "Fecha invalida. Selecciona una fecha en el futuro.";
                ErrorController error = new ErrorController();
                return error.Index(message);
            }
            else
            {
                string id = new Random().Next(1000, 9999).ToString();
                DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task")
                    .AddAsync(new Dictionary<string, object>
                    {
                    { "DocumentId", ""},
                    { "ID", id},
                    { "Name", name},
                    { "Description", description},
                    { "DueDate", duedate},
                    { "Status", "Incomplete"},
                    { "Priority", priority}
                    });
                FirestoreDb db = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId);
                DocumentReference dref = db.Collection("task").Document(docRef.Id);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    { "DocumentId", docRef.Id },
                };
                WriteResult result = await dref.UpdateAsync(dataToUpdate);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> CompleteTask(string doc)
        {
            try
            {
                CollectionReference collection = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task");
                DocumentReference document = collection.Document(doc);

                var updatedData = new Dictionary<string, object>
                {
                    { "Status", "Completed" },
                };

                await document.SetAsync(updatedData, SetOptions.MergeAll);
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Something went wrong: {ex.Message}";
                return View("Error");
            }

        }

        // GET: Functionality/Details/5
        public async Task<ActionResult> DeleteTask(string doc)
        {
            CollectionReference collection = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task");

            DocumentReference document = collection.Document(doc);
            await document.DeleteAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
