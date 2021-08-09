using INF272AdminSubsystem.Models;
using INF272AdminSubsystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace INF272AdminSubsystem.Controllers
{
    public class AdminController : Controller
    {
        // EXPERT  Actions
       
       //7.1. View Schedule 


        //Returns a view where expert records in the database can be CRUD-ed
        public ActionResult Educator(string showPassword, string SearchCriteria, string sortOrder)
        {
            ProjectEntities db = new ProjectEntities();
            List<Expert> expertsList = db.Experts.Include("SystemUser").ToList();



            List<ExpertsVM> expertView = new List<ExpertsVM>();
            foreach (Expert e in expertsList)
            {
                ExpertsVM item = new ExpertsVM();
                item.UserId = Convert.ToInt32(e.SystemUser_ID);
                item.ExpertId = Convert.ToInt32(e.Expert_ID);

                item.UserName = db.SystemUsers.Where(zz => zz.SystemUser_ID == e.SystemUser_ID).Select(zz => zz.SystemUser_Name).FirstOrDefault();

                ExpertModule eModule = db.ExpertModules.Where(zz => zz.Expert_ID == item.ExpertId).FirstOrDefault();
                int moduleid = (int)eModule.Module_ID;

              
                item.UserPassword = db.SystemUsers.Where(zz => zz.SystemUser_ID == e.SystemUser_ID).Select(zz => zz.SystemUser_Password).FirstOrDefault();
                item.UserSurname = db.SystemUsers.Where(zz => zz.SystemUser_ID == e.SystemUser_ID).Select(zz => zz.SystemUserSurname).FirstOrDefault();
                item.ModuleName = db.Modules.Where(zz => zz.Module_ID == moduleid).Select(zz => zz.Module_Name).FirstOrDefault();
                expertView.Add(item);
            }
           
            //ShowPassword
            if (showPassword == "Show")
            {
                ViewBag.Show = true;
            }
            else
            {
                ViewBag.Show = false;
            }


            if (!String.IsNullOrEmpty(SearchCriteria))
            {
                expertView = expertView.Where(zz => zz.UserName.Contains(SearchCriteria) || zz.UserSurname.Contains(SearchCriteria) || zz.ModuleName.Contains(SearchCriteria) || zz.UserId.ToString().Contains(SearchCriteria)).ToList();
            }


            switch (sortOrder)
            {
                case "SortById":
                    expertView =  expertView.OrderBy(zz => zz.UserId).ToList();
                    break;

                case "SortBySurname":
                    expertView = expertView.OrderBy(zz => zz.UserSurname).ToList();
                    break;

                default:
                    expertView = expertView.OrderBy(zz => zz.ExpertId).ToList();
                    break;
            }


            ViewBag.Experts = expertView;
            List<Module> modulelist = db.Modules.ToList();
            ViewBag.Modules = modulelist;
         
            return View();
        }
        public ActionResult SearchEducators()
        {
            return View();
        }

        //Returns a form where expert details can be added
        public ActionResult AddEducator()
        {
            ProjectEntities db = new ProjectEntities();

            List<Module> modulelist = db.Modules.ToList();
            ViewBag.Modules = modulelist;
            return View(modulelist);
        }

        //Adds the new expert info to the database. It returns an "expert added successfully" notification 
        public ActionResult EducatorAdded(string ExpertName, string ExpertSurname, int ModuleId)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                var random = new Random();
                string password = ExpertName.Substring(0,2) +  Convert.ToString(random.Next(1000, 9999));
               


                SystemUser user = new SystemUser { SystemUser_Name = ExpertName, SystemUserSurname = ExpertSurname, SystemUser_Password = password.ToString() };
                Expert expert = new Expert { SystemUser_ID = user.SystemUser_ID };
                ExpertModule expertModule = new ExpertModule { Expert_ID = expert.Expert_ID, Module_ID = ModuleId };

                db.SystemUsers.Add(user);
                db.Experts.Add(expert);
                db.ExpertModules.Add(expertModule);

                db.SaveChanges();

                ViewBag.Password = password;
            }
            catch (Exception error)
            {
                ViewBag.Message = error.Message;
            }
           
            return View();
        }

        //Returns a form where the user can select to proceed with the cancellation or to go back
        public ActionResult CancelAddEducator()
        {
            return View();
        }

        
        //Returns page that allows the user to enter updated educator info
        public ActionResult UpdateEducator(int id)
        {

                ProjectEntities db = new ProjectEntities();
                Expert expert = db.Experts.Find(id);
                int userid = (int)expert.SystemUser_ID;
                SystemUser user = db.SystemUsers.Find(userid);


                ViewBag.ExpertId = id;
                ViewBag.UserId = userid;
                ViewBag.FirstName = user.SystemUser_Name;
                ViewBag.Surname = user.SystemUserSurname;
                List<Module> modulelist = db.Modules.ToList();
                ViewBag.Modules = modulelist;
           
            

            return View();
        }
        //Updated an educator record using the information provided
        public ActionResult EducatorUpdated(int ExpertID, int UserID, string ExpertName, string ExpertSurname, int ModuleId)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                SystemUser user = db.SystemUsers.Find(UserID);
                Expert expert = db.Experts.Find(ExpertID);
                ExpertModule expertModule = db.ExpertModules.Where(zz => zz.Expert_ID == ExpertID).FirstOrDefault();

                user.SystemUser_Name = ExpertName.ToString();
                user.SystemUserSurname = ExpertSurname.ToString();
                expertModule.Module_ID = ModuleId;
                db.SaveChanges();
            }
            catch (Exception error)
            {
                ViewBag.Message = error.Message;

            }
              
               
            
            return View();


        }
        //Returns a form where the user can select to proceed with the cancellation or to go back
        public ActionResult CancelUpdateEducator()
        {
            return View();
        }
        //Returns a view that asks the user if they are sure they want to delete the record
        public ActionResult DeleteEducator(int expertid)
        {


            ViewBag.ID = expertid;
            

            return View();
        }
        //EducatorDeleted removes the record from the database and displays a notification that the record has been deleted. 
        public ActionResult EducatorDeleted(int id)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();

                Expert delExpert = db.Experts.Find(id);
                int userid = Convert.ToInt32(delExpert.SystemUser_ID);
                SystemUser user = db.SystemUsers.Find(userid);
                ExpertModule expertModule = db.ExpertModules.Where(zz => zz.Expert_ID == id).FirstOrDefault();

                db.SystemUsers.Remove(user);
                db.Experts.Remove(delExpert);
                db.ExpertModules.Remove(expertModule);
                db.SaveChanges();
            }
            catch (Exception error)
            {

                ViewBag.Message = error.Message;
            } 
            return View();
        }





        //  SCHOOL actions
        public ActionResult School(string SearchCriteria, string sortOrder)
        {
            ProjectEntities db = new ProjectEntities();
            List < School > schoolsList = db.Schools.ToList();

            if (!String.IsNullOrEmpty(SearchCriteria))
            {
                schoolsList = schoolsList.Where(zz => zz.School_Name.Contains(SearchCriteria) || zz.School_Address.Contains(SearchCriteria) || zz.School_ID.ToString().Contains(SearchCriteria)).ToList();
            }

            switch (sortOrder)
            {
                case "ByID":
                    schoolsList =  schoolsList.OrderBy(zz => zz.School_ID).ToList();
                    break;

                case "ByName":
                    schoolsList = schoolsList.OrderBy(zz => zz.School_Name).ToList();
                    break;

                default:
                    schoolsList = schoolsList.OrderBy(zz => zz.School_ID).ToList();
                    break;
            }

            ViewBag.Schools = schoolsList;

            return View();
        }
        public ActionResult AddSchool()
        {

            return View();
        }
        public ActionResult SchoolAdded(string SchoolName, string Email, string PhoneNumber, string Address )
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                School school = new School { School_Name = SchoolName, School_Email = Email, School_Phone = PhoneNumber, School_Address = Address };
                db.Schools.Add(school);
                db.SaveChanges();
            }
            catch (Exception error)
            {
                ViewBag.Message = error.Message;

            } 
            return View();
        }
        public ActionResult CancelAddSchool()
        {

            return View();
        }

       
        public ActionResult UpdateSchool(int id)
        {
            ProjectEntities db = new ProjectEntities();
            School school = db.Schools.Find(id);
            ViewBag.Name = school.School_Name;
            ViewBag.Email = school.School_Email;
            ViewBag.Phone = school.School_Phone;
            ViewBag.Address = school.School_Address;
            ViewBag.Id = school.School_ID;
            return View();
        }
        public ActionResult SchoolUpdated(int SchoolID, string SchoolName, string Email, string PhoneNumber, string Address)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                School school = db.Schools.Find(SchoolID);
                school.School_Name = SchoolName.ToString();
                school.School_Email = Email.ToString();
                school.School_Phone = PhoneNumber.ToString();
                school.School_Address = Address.ToString();
                db.SaveChanges();
            }
            catch (Exception error)
            {
                ViewBag.Message = error.Message;

            } 
            return View();
        }
        public ActionResult CancelUpdateSchool(int id)
        {
            ViewBag.id = id;
            return View();
        }
        
        public ActionResult DeleteSchool(int id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult SchoolDeleted(int id)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                School school = db.Schools.Find(id);
                db.Schools.Remove(school);
                db.SaveChanges();
            }
            catch (Exception error)
            {
                ViewBag.Message = error;
            } 
           
            return View();
        }

         




        //   TEACHER Actions
        public ActionResult Teacher(string showPassword, string SearchCriteria, string sortOrder)
        {
            ProjectEntities db = new ProjectEntities();
            List<Teacher> teacherlist = db.Teachers.Include("SystemUser").ToList();

            

            List<TeacherVM> teacherView = new List<TeacherVM>();
            foreach(Teacher t in teacherlist)
            {
                TeacherVM teach = new TeacherVM();
                teach.UserId = (int)t.SystemUser_ID;
                teach.TeacherId = (int)t.Teacher_ID;
                teach.UserName = (string)t.SystemUser.SystemUser_Name;
                teach.UserSurname = (string)t.SystemUser.SystemUserSurname;
                int SchoolId = (int)t.School_ID;
                teach.SchoolName = db.Schools.Where(zz => zz.School_ID == t.School_ID).Select(zz => zz.School_Name).FirstOrDefault();
                teach.UserPassword = db.SystemUsers.Where(zz => zz.SystemUser_ID == t.SystemUser_ID).Select(zz => zz.SystemUser_Password).FirstOrDefault();
                teacherView.Add(teach);
            }

            if (!String.IsNullOrEmpty(SearchCriteria))
            {
                teacherView = teacherView.Where(zz => zz.UserName.Contains(SearchCriteria) || zz.UserSurname.Contains(SearchCriteria) || zz.SchoolName.Contains(SearchCriteria) || zz.UserId.ToString().Contains(SearchCriteria)).ToList();
            }

            switch (sortOrder)
            {
                case "UserId":
                    teacherView = teacherView.OrderBy(zz => zz.UserId).ToList();
                    break;

                case "Surname":
                    teacherView = teacherView.OrderBy(zz => zz.UserSurname).ToList();
                    break;

                default:
                    teacherView = teacherView.OrderBy(zz => zz.TeacherId).ToList();
                    break;

            }

            if (showPassword == "Show")
            {
                ViewBag.Show = true;
            }
            else
            {
                ViewBag.Show = false;
            }

            ViewBag.Teachers = teacherView;
            return View();
        }
        public ActionResult AddTeacher()
        {

            ProjectEntities db = new ProjectEntities();
            List<School> schoollist = db.Schools.ToList();
            ViewBag.Schools = schoollist;

            return View();
        }
        public ActionResult TeacherAdded(string TeacherName, string TeacherSurname, int School_ID)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                var random = new Random();
                string password = TeacherName.Substring(0, 2) + Convert.ToString(random.Next(1000, 9999));


                SystemUser user = new SystemUser { SystemUser_Name = TeacherName, SystemUserSurname = TeacherSurname, SystemUser_Password = password };
                Teacher teacher = new Teacher { SystemUser_ID = user.SystemUser_ID, School_ID = School_ID };
                db.SystemUsers.Add(user);
                db.Teachers.Add(teacher);
                db.SaveChanges();
            }
            catch (Exception error)
            {

                ViewBag.Message = error;
            } 
            return View();
        }
        public ActionResult CancelAddTeacher()
        {
            return View();
        }
      
        public ActionResult UpdateTeacher(int id)
        {
            ProjectEntities db = new ProjectEntities();
            Teacher teacher = db.Teachers.Find(id);
            SystemUser user = db.SystemUsers.Find(teacher.SystemUser_ID);
            School school = db.Schools.Find(teacher.School_ID);

            List<School> schoollist = db.Schools.ToList();
            ViewBag.Schools = schoollist;

            ViewBag.TeacherId = teacher.Teacher_ID;
            ViewBag.Name = user.SystemUser_Name;
            ViewBag.Surname = user.SystemUserSurname;
            ViewBag.School = school.School_Name;
            return View();
        }
        public ActionResult TeacherUpdated(int TeacherId, string Name, string Surname, int SchoolId)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                Teacher teacher = db.Teachers.Find(TeacherId);
                SystemUser user = db.SystemUsers.Find(teacher.SystemUser_ID);
                teacher.School_ID = SchoolId;
                user.SystemUser_Name = Name;
                user.SystemUserSurname = Surname;
                db.SaveChanges();
            }
            catch (Exception error)
            {

                ViewBag.Message = error;
            } 
            return View();
        }
        public ActionResult CancelUpdateTeacher(int id)
        {
            ViewBag.TeacherId = id;
            return View();
        }
        public ActionResult DeleteTeacher(int id)
        {
            ViewBag.TeacherId = id;
            return View();
        }
        public ActionResult TeacherDeleted(int id)
        {
            try
            {
                ProjectEntities db = new ProjectEntities();
                Teacher teacher = db.Teachers.Find(id);
                SystemUser user = db.SystemUsers.Find(teacher.SystemUser_ID);

                db.SystemUsers.Remove(user);
                db.Teachers.Remove(teacher);
                db.SaveChanges();
            }
            catch (Exception error)
            {

                ViewBag.Message = error;
            }
            
            return View();
        }



    }
}