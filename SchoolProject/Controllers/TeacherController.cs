using SchoolProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        // GET: /Teacher/List/{SearchKey}
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }
        // GET: /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);
            return View(SelectedTeacher);
        }

        // GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        // POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //Get: /Teacher/New
        public ActionResult New()
        {

            return View();
        }

        //Post: /Teacher/Create
        [HttpPost]
        [EnableCors(origins:"*", methods:"*", headers:"*")]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, decimal? Salary)
        {
            // identify that this method is running
            // identify the input provided from the form

            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = new Teacher();
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.Salary = Salary;
            controller.AddTeacher(NewTeacher);
            return RedirectToAction("List");


        }

        /// <summary>
        /// Receives the id of the teacher and shows a page showing their information
        /// </summary>
        /// <param name="id">Id of the Teacher</param>
        /// <returns>
        /// A dynamic page showing a form with the current information of the teacher and it asks to update the teacher's information
        /// </returns>
        ///<example>Get: /Teacher/Update/{id}</example> 
        [HttpGet]
        public ActionResult Update(int id)
        {

            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);


        }

        /// <summary>
        /// Receives a post request containing information about an exisiting teacher in the database and the new information contains updated values about the teacher. 
        /// This new information is sent to the Api, then it redirects back to the "Teacher show" page of the updated teacher
        /// </summary>
        /// <param name="id">Id of the teacher to update</param>
        /// <param name="TeacherFname">The new updated first name</param>
        /// <param name="TeacherLname">The new updated last name</param>
        /// <param name="EmployeeNumber">The new updated employee number</param>
        /// <param name="Salary">The new salary</param>
        /// <returns>A dynamic webpage which contains the updated information of the teacher</returns>
        /// <example>POST : /Teacher/Update/4
        /// FORM DATA / POST DATA / REQUET BODY
        /// {
        /// "TeacherFname":"Idayat",
        /// "TeacherLname":"Sanni",
        /// "EmployeeNumber":"T790",
        /// "Salary":90.00
        /// }
        /// </example>

        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, decimal? Salary)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher InfoTeacher = new Teacher();
            InfoTeacher.EmployeeNumber = EmployeeNumber;
            InfoTeacher.TeacherFname = TeacherFname;
            InfoTeacher.TeacherLname = TeacherLname;
            InfoTeacher.Salary = Salary;
            controller.UpdateTeacher(id, InfoTeacher);

            return RedirectToAction("Show/" + id); 

        }
    }
}