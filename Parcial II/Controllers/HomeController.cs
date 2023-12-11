using Microsoft.AspNetCore.Mvc;
using Parcial_II.Models;
using System.Diagnostics;
using Google.Cloud.Firestore;
using static Parcial_II.DataBase.FireBaseService;
using FirebaseAdmin.Messaging;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using Parcial_II.DataBase;

namespace Parcial_II.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static FirestoreDb firestoreDb;

        public async Task<ActionResult> Index()
        {
            List<Tasks> tasksList = new List<Tasks>();
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task");

            if (query != null)
            {
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                if (querySnapshot.Documents.Count > 0)
                {
                    foreach (var document in querySnapshot.Documents)
                    {
                        Dictionary<string, object> data = document.ToDictionary();

                        Tasks task = new Tasks
                        {
                            DocumentID = document.Id.ToString(),
                            ID = GetValueOrDefault(data, "ID", string.Empty),
                            Name = GetValueOrDefault(data, "Name", string.Empty),
                            Description = GetValueOrDefault(data, "Description", string.Empty),
                            DueDate = GetValueOrDefault(data, "DueDate", DateTime.MinValue),
                            Status = GetValueOrDefault(data, "Status", string.Empty),
                            Priority = GetValueOrDefault(data, "Priority", string.Empty)
                        };

                        tasksList.Add(task);
                    }

                    ViewBag.TaskData = tasksList;

                    return View();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTask(string docu, string name, string Description, string duedate, string priority)
        {
            try
            {
                if (Convert.ToDateTime(duedate) < System.DateTime.Now)
                {
                    string message = "The date is invalid. It cannot be earlier than the current date.";
                    ErrorController error = new ErrorController();
                    ViewBag.ErrorMessage = message;
                    return View("Error");
                }
                else
                {
                    // Use the FirestoreService to update the document
                    FireBaseService firestoreService = new FireBaseService();

                    string collectionName = "task";
                    var updates = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", Description },
                { "DueDate", duedate },
                { "Status", "Incomplete" },
                { "Priority", priority }
            };

                    await firestoreService.UpdateDocumentAsync(collectionName, docu, updates);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating document: {ex.Message}");
                ViewBag.ErrorMessage = $"Something went wrong while trying to update: {ex.Message}";
                return View("Error");
            }
        }



        private T GetValueOrDefault<T>(Dictionary<string, object> dictionary, string key, T defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                try
                {
                    return (T)Convert.ChangeType(dictionary[key], typeof(T));
                }
                catch (Exception)
                {
                    // Handle conversion errors if necessary
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public async Task<ActionResult> GetTask(int id)
        {
            try
            {
                // Retrieve data
                List<Tasks> tasksList = new List<Tasks>();
                Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task").WhereEqualTo("ID", id);

                if (query != null)
                {
                    QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                    if (querySnapshot.Documents.Count > 0)
                    {
                        foreach (var document in querySnapshot.Documents)
                        {
                            Dictionary<string, object> data = document.ToDictionary();

                            Tasks task = new Tasks
                            {
                                ID = data["ID"].ToString(),
                                DocumentID = document.Id.ToString(),
                                Name = data["Name"].ToString(),
                                Description = data["Description"].ToString(),
                                DueDate = Convert.ToDateTime(data["DueDate"].ToString()),
                                Status = data["Status"].ToString(),
                                Priority = data["Priority"].ToString()
                            };
                            tasksList.Add(task);
                        }

                        ViewData["TaskData"] = tasksList;

                        // Return the view with the retrieved data
                        return View("GetTask", tasksList);
                    }
                    else
                    {
                        // Handle case where no data is found
                        return View("Error");
                    }
                }
                else
                {
                    // Handle case where query is null
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Something went wrong while trying to retrieve data: {ex.Message}";
                return View("Error");
            }
        }


        public async Task<ActionResult> GetTaskByParameter(string parameter)
        {
            List<Tasks> tasksList = new List<Tasks>();
            Query query1 = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task").WhereEqualTo("Name", parameter);
            Query query2 = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("task").WhereEqualTo("Description", parameter);


            if (query1 != null && query2 != null)
            {
                QuerySnapshot querySnapshot1 = await query1.GetSnapshotAsync();
                QuerySnapshot querySnapshot2 = await query2.GetSnapshotAsync();

                if (querySnapshot1.Documents.Count > 0)
                {
                    foreach (var document in querySnapshot1.Documents)
                    {
                        Dictionary<string, object> data = document.ToDictionary();

                        Tasks task = new Tasks
                        {
                            ID = data["ID"].ToString(),
                            DocumentID = document.Id.ToString(),
                            Name = data["Name"].ToString(),
                            Description = data["Description"].ToString(),
                            DueDate = Convert.ToDateTime(data["DueDate"].ToString()),
                            Status = data["Status"].ToString(),
                            Priority = data["Priority"].ToString()
                        };
                        tasksList.Add(task);

                    }
                    ViewBag.TaskData = tasksList;

                    return View("Index");
                }
                else if (querySnapshot2.Documents.Count > 0)
                {
                    foreach (var document in querySnapshot2.Documents)
                    {
                        Dictionary<string, object> data = document.ToDictionary();

                        Tasks task = new Tasks
                        {
                            ID = data["ID"].ToString(),
                            DocumentID = document.Id.ToString(),
                            Name = data["Name"].ToString(),
                            Description = data["Description"].ToString(),
                            DueDate = Convert.ToDateTime(data["DueDate"].ToString()),
                            Status = data["Status"].ToString(),
                            Priority = data["Priority"].ToString()
                        };
                        tasksList.Add(task);
                    }
                    ViewBag.TaskData = tasksList;

                    return View("Index");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
