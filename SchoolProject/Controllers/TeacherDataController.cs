﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;



namespace SchoolProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the teachers table of our school database.
        /// <summary>
        /// Retrieves any teacher from the database that matches the values in the search key. The search key input can be the teacher's first name, last name, hire date, or salary.
        /// </summary>
        /// <param name="SearchKey">The search key used to filter teachers. Can be a string, integer, decimal, and DateTime types</param>
        /// <example>
        /// GET /TeacherData/ListTeachers?SearchKey=hawkins -> {"EmployeeNumber":"T393",HireDate":"2016-08-10T00:00:00","Salary":54.45,"TeacherFname":"Thomas", "TeacherId":6,"TeacherLname":"Hawkins"}
        /// GET /TeacherData/ListTeachers?SearchKey=74.20 -> {"EmployeeNumber":"T385",HireDate":"2014-06-22T00:00:00","Salary":74.20,"TeacherFname":"Lauren", "TeacherId":4,"TeacherLname":"Smith"}
        /// GET /TeacherData/ListTeachers?SearchKey=tar -> {"EmployeeNumber":"T505",HireDate":"2015-10-23T00:00:00","Salary":79.63,"TeacherFname":"John", "TeacherId":10,"TeacherLname":"Taram"}
        /// </example>
        /// </example>
        /// <returns>
        /// A list of teachers (first names and last names) and teachers matching the search criteria
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or salary = @salary or teacherid = @id or hiredate = @hiredate or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            // Prepare the search key
            if (decimal.TryParse(SearchKey, out decimal salary))
            {
                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Parameters.AddWithValue("@salary", salary);
            }
            else
            {
                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Parameters.AddWithValue("@salary", DBNull.Value);
            }
            
            if (DateTime.TryParse(SearchKey, out DateTime hiredate))
            {
                cmd.Parameters.AddWithValue("@hireDate", hiredate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@hireDate", DBNull.Value);
            }

            if ((int.TryParse(SearchKey, out int teacherId)))
            {
                cmd.Parameters.AddWithValue("@id", teacherId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@id", DBNull.Value);
            }
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>{};

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {

                int TeacherId = ResultSet["teacherid"] != DBNull.Value ? Convert.ToInt32(ResultSet["teacherid"]) : 0;
                decimal Salary = ResultSet["salary"] != DBNull.Value ? Convert.ToDecimal(ResultSet["salary"]) : 0.0m;
                string TeacherFname = ResultSet["teacherfname"] != DBNull.Value ? Convert.ToString(ResultSet["teacherfname"]) : string.Empty;
                string TeacherLname = ResultSet["teacherlname"] != DBNull.Value ? Convert.ToString(ResultSet["teacherlname"]) : string.Empty;
                string EmployeeNumber = (string)ResultSet["employeenumber"];

                // convert date to string
                DateTime HireDate;
                DateTime.TryParse(ResultSet["hiredate"].ToString(), out HireDate);

                // creating a teacher object
                Teacher NewTeacher = new Teacher();
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return Teachers;
        }

        /// <summary>
        /// Finds a teacher using the id from the database
        /// </summary>
        /// <param name="id">The id to match against a primary key record in the MySQL Database</param>
        /// <example>api/TeacherData/FindTeacher/6 -> {Author Object}</example>
        /// <returns>A teacher object</returns>

        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            // create an instance of the teacher
            Teacher NewTeacher = new Teacher();
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where teacherid = " +id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();


            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {

                int TeacherId = ResultSet["teacherid"] != DBNull.Value ? Convert.ToInt32(ResultSet["teacherid"]) : 0;
                decimal Salary = ResultSet["salary"] != DBNull.Value ? Convert.ToDecimal(ResultSet["salary"]) : 0.0m;
                string TeacherFname = ResultSet["teacherfname"] != DBNull.Value ? Convert.ToString(ResultSet["teacherfname"]) : string.Empty;
                string TeacherLname = ResultSet["teacherlname"] != DBNull.Value ? Convert.ToString(ResultSet["teacherlname"]) : string.Empty;
                string EmployeeNumber = (string)ResultSet["employeenumber"];


                // convert date to string
                DateTime HireDate;
                DateTime.TryParse(ResultSet["hiredate"].ToString(), out HireDate);

               
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;          

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the teacher object
            return NewTeacher;
        }

        /// <summary>
        /// Deletes a teacher from the connected MySQL Database if the ID of that Article exists. Does NOT maintain relational integrity. 
        /// </summary>
        /// <param name="id">The ID of the Article.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Adds a teacher to the MySQL Database. 
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the Teacher's table. </param>
        /// <example>
        /// POST api/TeacherData/AddTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Idayat",
        ///	"TeacherLname":"Sanni",
        ///	"EmployeeNumber":"T903",
        ///	"Salary":90.00,
        ///	"HireDate":currentdate,
        /// }
        /// </example>
        [HttpPost]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Exit method if model fields are not included.
            if (!NewTeacher.IsValid()) return;
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber, CURRENT_DATE(), @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Updates a teacher in the MySQL Database. 
        /// </summary>
        /// <param name="id">id of the teacher we want to update</param>
        /// <param name="InfoTeacher">An object that contains new information of the Teacher in the already in the database.</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        /// "TeacherId" => 4
        ///	"TeacherFname":"Idayat",
        ///	"TeacherLname":"Sanni",
        ///	"EmployeeNumber":"T903",
        ///	"Salary":90.00
        /// }
        /// </example>
        public void UpdateTeacher(int id, [FromBody] Teacher InfoTeacher)
        {
            
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber, salary=@Salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", InfoTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", InfoTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", InfoTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", InfoTeacher.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();
        }

    }
}
