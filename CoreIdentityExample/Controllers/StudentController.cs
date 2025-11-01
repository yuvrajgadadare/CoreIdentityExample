using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreIdentityExample.Controllers
{

    public class StudentController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IBatchService batchService;
        IExtraService extraService;
        private IWebHostEnvironment environment;
        EmailSettings _settings;
        IExamService examService;
        ICourseService courseService;
        ITopicService topicService;
        public StudentController(IMasterService masterService, IStudentService studentService, IBatchService batchService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IExamService examService, ICourseService courseService, ITopicService topicService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.batchService = batchService;
            this.environment = environment;
            this.extraService = extraService;
            this._settings = settings.Value;
            this.examService = examService;
            this.courseService = courseService;
            this.topicService = topicService;
        }
        //public IActionResult LoginHere()
        //{
        //    return View();
        //}
        //public IActionResult ProfileHere()
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    int student_id = (int)HttpContext.Session.GetInt32("student_id");
        //    StudentModel student = studentService.GetStudent(student_id);
        //    ViewBag.qualifications = studentService.GetStudentWiseQualifications(student_id);

        //    return View(student);
        //}
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = await studentService.GetStudent(student_id);
            ViewBag.qualifications = await studentService.GetStudentWiseQualifications(student_id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(StudentModel sm)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }

            await studentService.UpdateStudentDetails(sm);
            ViewBag.msg = "Profile Details updated successfully";
            StudentModel student = await studentService.GetStudent(sm.student_id);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Qualification(StudentQualificationModel sq)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            sq.student_id = student_id;
            studentService.AddQualification(sq);
            //ViewBag.msg = "Qualification added successfully";
            //ModelState.Clear();
            //int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //StudentModel student = studentService.GetStudent(student_id);
            //ViewBag.student = student;
            //ViewBag.qualifications = studentService.GetStudentWiseQualifications(student_id);
            //StudentQualificationModel q = new StudentQualificationModel() { student_id = student_id };
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> DeleteQualification(int id)
        {
            studentService.DeleteQualification(id);
            return RedirectToAction("Profile");

        }
        public async Task<IActionResult> Enrollment()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = await studentService.GetStudent(student_id);
            return View(student);
        }
        public async Task<IActionResult> CourseSyllabus(int id)
        {
            CourseModel c = await courseService.GetTrainingCourse(id);
            return View(c);

        }
        public async Task<IActionResult> BatchDetails()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);

            return View(batches);
        }
        public async Task<IActionResult> Login()
        {
            StudentLoginModel sm = new StudentLoginModel();
            return View(sm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(StudentLoginModel st)
        {
            if (!ModelState.IsValid)
            {
                return View(st);
            }
            StudentModel student = await masterService.CheckStudentLogin(st.email_address, st.password);
            if (student == null)
            {
                ViewBag.msg = "Invalid email address or password";
                StudentLoginModel sm = new StudentLoginModel();
                return View(sm);
            }
            else
            {
                StudentModel sm = await studentService.GetStudent(student.student_id);
                HttpContext.Session.SetString("student_name", student.student_name);
                HttpContext.Session.SetInt32("student_id", student.student_id);
                HttpContext.Session.SetString("student", JsonConvert.SerializeObject(sm));
                List<RegistrationModel> rst = await studentService.GetStudentWiseRegistrations(sm.student_id);
                RegistrationModel r = rst[0];
                HttpContext.Session.SetInt32("registration_id", r.registration_id);

                HttpContext.Session.SetString("registration", JsonConvert.SerializeObject(r));
                return Redirect("/dashboard");
            }
        }
        public async Task<IActionResult> ForgotPassword()
        {

            return View();
        }
        [HttpPost]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordEmailModel sm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                StudentModel student = await studentService.GetStudentByEmailAddress(sm.email_address);
                if (student != null)
                {
                    if (student.student_id != 0)
                    {
                        string encryptedtext = await extraService.Encrypt(student.student_id.ToString());
                        string passwordurl = DomainUrl.Url + "/Student/GenerateNewPassword?student=" + encryptedtext;
                        string msg = "<h4>Dear " + student.student_name + ",</h4><p>link to generate new password is</p><p><a href='" + passwordurl + "'>" + passwordurl + "</a></p><br/><br/><h5>Regards,</h5><h5>CIIT Training Institute Pvt Ltd</h5>";

                        EmailModel em = new EmailModel()
                        {
                            UserName = student.student_name,
                            EmailAddress = student.email_address,
                            Message = msg,
                            Subject = "Forgot Password link"
                        };
                        try
                        {
                            await extraService.SendEmail(em, _settings);
                            ViewBag.successmsg = "forgot password link has been sent your registered email address.";
                            return View();

                        }
                        catch (Exception ex)
                        {
                            ViewBag.msg = "unable to send forgot password link to your registered email address.Please contact to administrator";
                            return View();

                        }
                    }
                    else
                    {
                        ViewBag.msg = "user with given email address is not registered";
                        return View();

                    }

                }
                else
                {
                    ViewBag.msg = "user with given email address is not registered";
                    return View();

                }
            }
            return View();
        }
        public async Task<IActionResult> GenerateNewPassword(string student)
        {

            string student_id = await extraService.Decrypt(student);
            int sid = Convert.ToInt32(student_id);
            StudentModel std = await studentService.GetStudent(sid);
            ForgotPasswordModel sd = new ForgotPasswordModel()
            {
                student_id = std.student_id
            };
            return View(sd);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateNewPassword(ForgotPasswordModel f)
        {
            if (!ModelState.IsValid)
            {
                return View(f);
            }
            else
            {
                studentService.ChangeStudentPassword(f.student_id, f.password);
                ViewBag.msg = "Password changed successfully.";
                return View();
            }


        }
        public async Task<IActionResult> ViewTopics()
        {
            if (HttpContext.Session.GetString("student_name") == null)
            {
                return RedirectToAction("Login");

            }
            ViewBag.topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");
            return View();
        }

        public async Task<IActionResult> InitiateExam(int topic_id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            HttpContext.Session.SetInt32("topic_id", topic_id);
            int student_id = (int)(HttpContext.Session.GetInt32("student_id"));

            ExamModel exam = new ExamModel()
            {
                topic_id = topic_id,
                student_id = student_id,
                exam_date = DateTime.Now,
                start_time = DateTime.Now,

            };
            HttpContext.Session.SetString("exam", JsonConvert.SerializeObject(exam));

            List<TopicModel> topics = await topicService.GetTrainingTopics();
            TopicModel topic = topics.FirstOrDefault(e => e.topic_id.Equals(topic_id));
            ViewBag.topic = topic.topic_name;
            ViewBag.question_count = exam.total_questions;
            List<ContentQuestionModel> questions = await topicService.GetTopicWiseQuestions(topic_id, 5);
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questions));
            return View();
        }

        public async Task<string> ChangeProfilePhoto(IFormFile file)
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel d = await studentService.GetStudent(student_id);
            Random r = new Random();
            int n = r.Next(1, 1000);
            string imgname = d.student_name + "_" + n + Path.GetExtension(file.FileName);
            string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            file.CopyTo(fs);
            // d.profile_photo = imgname;
            studentService.ChangeStudentProfilePhoto(student_id, imgname);
            //string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            //string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            //if (System.IO.File.Exists(aadharpath))
            //{
            //    System.IO.File.Delete(aadharpath);
            //}
            //FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            //aadharcard.CopyTo(fsaadhar);
            //d.aadhar_card_photo = aadharname;
            d = await studentService.GetStudent(student_id);
            HttpContext.Session.SetString("student_name", d.student_name);
            HttpContext.Session.SetInt32("student_id", d.student_id);
            HttpContext.Session.SetString("student", JsonConvert.SerializeObject(d));

            return "Profile Photo Changed Successfully";

        }

        public async Task<string> ChangeAadharPhoto(IFormFile file)
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel d = await studentService.GetStudent(student_id);
            Random r = new Random();
            int n = r.Next(1, 1000);
            string imgname = d.student_name + "_" + n + Path.GetExtension(file.FileName);
            string imgpath = environment.WebRootPath + "/Students/AadharCards/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            file.CopyTo(fs);
            // d.profile_photo = imgname;
            studentService.ChangeStudentAadharPhoto(student_id, imgname);
            //string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            //string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            //if (System.IO.File.Exists(aadharpath))
            //{
            //    System.IO.File.Delete(aadharpath);
            //}
            //FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            //aadharcard.CopyTo(fsaadhar);
            //d.aadhar_card_photo = aadharname;
            d = await studentService.GetStudent(student_id);
            HttpContext.Session.SetString("student_name", d.student_name);
            HttpContext.Session.SetInt32("student_id", d.student_id);
            HttpContext.Session.SetString("student", JsonConvert.SerializeObject(d));

            return "Profile Photo Changed Successfully";

        }

        public async Task<string> ChangePassword(ChangePasswordModel p)
        {

            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel d = await studentService.GetStudent(student_id);
            StudentModel sm = await masterService.CheckStudentLogin(d.email_address, p.current_password);
            if (sm != null)
            {
                await studentService.ChangeStudentPassword(student_id, p.new_password);
                return "1";
            }
            else
            {
                return "0";

            }
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("student_id");
            HttpContext.Session.Remove("student_name");
            HttpContext.Session.Remove("student");
            return RedirectToAction("Login");

        }

        //public IActionResult StartExam()
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    string questions = HttpContext.Session.GetString("questions");
        //    List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
        //    ViewBag.questions = questionlist;
        //    ContentQuestionModel q = questionlist.First();

        //    ViewBag.prev = true;

        //    return View(q);
        //}
        //[HttpPost]
        //public IActionResult StartExam(ContentQuestionModel cm, string command)
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    string questions = HttpContext.Session.GetString("questions");
        //    List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
        //    ViewBag.questions = questionlist;

        //    ContentQuestionModel qs = questionlist.FirstOrDefault(e => e.question_id.Equals(cm.question_id));
        //    int index=questionlist.IndexOf(qs);
        //    qs.submitted_option_number = cm.submitted_option_number;
        //    questionlist[index] = qs;
        //    HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questionlist));
        //    ContentQuestionModel q=null;
        //    if (command == null)
        //    {
        //        q = questionlist[0];
        //    }
        //    else if (command == "Next")
        //    {
        //        cm.serial_number++;
        //        if (cm.serial_number < questionlist.Count)
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
        //            ViewBag.next = false;
        //        }
        //        else
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));

        //            ViewBag.next = true;
        //        }
        //    }
        //    else if (command == "Prev")
        //    {
        //        cm.serial_number--;
        //        if (cm.serial_number > 1)
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
        //            ViewBag.prev = false;
        //        }
        //        else
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
        //            ViewBag.prev = true;
        //        }
        //    }
        //    else if (command == "Submit")
        //    {
        //        string exatdata = HttpContext.Session.GetString("exam");
        //        ExamModel exam=(ExamModel)JsonConvert.DeserializeObject<ExamModel>(exatdata);
        //        exam.end_time = DateTime.Now;
        //        string questiondata = HttpContext.Session.GetString("questions");
        //        List<ContentQuestionModel> questionlistdata = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
        //        List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
        //        foreach(ContentQuestionModel c in questionlistdata)
        //        {
        //            ExamQuestionModel e = new ExamQuestionModel()
        //            {
        //                question_id = c.question_id,
        //                submitted_option_number = c.submitted_option_number
        //            };
        //            lst.Add(e);
        //        }
        //        exam.examQuestions = lst;
        //        masterService.SubmitExam(exam);
        //        return RedirectToAction("SubmitExam");
        //    }
        //    ModelState.Clear();
        //    return View(q);
        //}

        //public IActionResult SubmitExam()
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    ViewBag.msg = "Exam Submitted Successfully.Please check your mail for exam result.";
        //    return View();
        //}

        public async Task<IActionResult> Exams()
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<ExamModel> exams = await examService.GetStudentWiseExams(student_id);
            return View(exams);

        }
        public async Task<IActionResult> ViewExamDetails(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            ExamModel em = await examService.GetExam(id);
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
            return View(em);

        }
    }
}
