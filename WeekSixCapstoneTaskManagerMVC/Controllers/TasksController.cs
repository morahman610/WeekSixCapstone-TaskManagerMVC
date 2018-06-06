using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeekSixCapstoneTaskManagerMVC.Models;

namespace WeekSixCapstoneTaskManagerMVC.Controllers
{
    public class TasksController : Controller
    {
        private TaskListEntities db = new TaskListEntities();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.User);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create(string id)
        {

            ViewBag.EmailAddress = new SelectList(db.Users, "EmailAddress", "EmailAddress");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,DueDate,Status,Name,EmailAddress")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmailAddress = new SelectList(db.Users, "EmailAddress", "Password", task.EmailAddress);
            return View(task);
        }

        [HttpPost]
        public ActionResult UserTasks(string EmailAddress, string Password)
        {

            TaskListEntities orm = new TaskListEntities();
            List<Task> TaskList = orm.Tasks.ToList();
            List<Task> LookupUser = new List<Task>();

            foreach (Task i in TaskList)
            {
                if (EmailAddress == i.EmailAddress)
                {
                    LookupUser.Add(i);

                }
            }

            ViewBag.EmailAddress = EmailAddress;
            ViewBag.Tasks = LookupUser.ToList();
            return View();
        }

        public ActionResult TaskSearch(string searching)
        {
            TaskListEntities orm = new TaskListEntities();
            List<Task> taskList = orm.Tasks.ToList();
            List<Task> searchResults = new List<Task>();

            var tasks = from s in orm.Tasks
                        select s;
            if(!String.IsNullOrEmpty(searching))
            {
                tasks = tasks.Where(s => s.Description.Contains(searching));
            }
            ViewBag.tasks = tasks;
            return View();
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmailAddress = new SelectList(db.Users, "EmailAddress", "Password", task.EmailAddress);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Description,DueDate,Status,Name,EmailAddress")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmailAddress = new SelectList(db.Users, "EmailAddress", "Password", task.EmailAddress);
            return View(task);
        }

        public ActionResult MarkComplete (string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            task.Status = "Complete";
            db.SaveChanges();
            ViewBag.EmailAddress = new SelectList(db.Users, "EmailAddress", "Password", task.EmailAddress);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkComplete ([Bind(Include = "Description,DueDate,Status,Name,EmailAddress")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmailAddress = new SelectList(db.Users, "EmailAddress", "Password", task.EmailAddress);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find((string)id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
