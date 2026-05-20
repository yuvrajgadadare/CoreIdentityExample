
using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERP_Services.Implementations
{
    public class BatchService : IBatchService
    {
        public BatchService()
        {

        }
        public async Task AddBatch(BatchModel batch)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@batch_id", batch.batch_id);
                  cmd.Parameters.AddWithValue("@branch_id", batch.branch_id);
                cmd.Parameters.AddWithValue("@topic_id", batch.topic_id);
                cmd.Parameters.AddWithValue("@employee_id", batch.employee_id);
                cmd.Parameters.AddWithValue("@start_date", batch.start_date);
                cmd.Parameters.AddWithValue("@end_date", batch.end_date);
                cmd.Parameters.AddWithValue("@batch_time", batch.batch_time);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddBatchSchedule(BatchScheduleModel schedule)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@batch_schedule_id", schedule.batch_schedule_id);
                cmd.Parameters.AddWithValue("@batch_id", schedule.batch_id);
                cmd.Parameters.AddWithValue("@content_id", schedule.content_id);
                cmd.Parameters.AddWithValue("@expected_date", schedule.expected_date);
                //    cmd.Parameters.AddWithValue("@actual_date", schedule.actual_date);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddBatchStudent(BatchStudentModel student)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch_student", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@batch_student_id", student.batch_student_id);
                cmd.Parameters.AddWithValue("@batch_id", student.batch_id);
                cmd.Parameters.AddWithValue("@registration_id", student.registration_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task<List<BatchModel>> GetAllBatches(int branch_id)
        {
            List<BatchModel> lst = new List<BatchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@batch_id", 0);
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    //int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    string playlist_title = dr["playlist_title"].ToString();
                    string playlist_key = dr["playlist_key"].ToString();
                    //int total_students = Convert.ToInt32(dr["total_students"].ToString());
                    int total_leactures = Convert.ToInt32(dr["total_leactures"].ToString());
                    int attended_leactures = Convert.ToInt32(dr["attended_leactures"].ToString());
                    List<BatchScheduleModel> schedule = await GetBatchWiseSchedule(id);
                    bool status = false;
                    if (schedule.Count() > 0)
                    {
                        status = true;
                    }
                    int remaining_leatures = 0;
                    remaining_leatures = total_leactures - attended_leactures;
                    float per = 0;
                    if (total_leactures > 0)
                    {
                        per = (remaining_leatures * 100) / total_leactures;
                    }

                    int cnt = 0;

                    List<BatchStudentModel> students = await GetBatchWiseStudents(id);
                    if (students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    string batch_status = "";
                    if (total_leactures == attended_leactures)
                    {
                        batch_status = "completed";
                    }
                    else if (attended_leactures > 0 && attended_leactures < total_leactures)
                    {
                        batch_status = attended_leactures + " Leactures Finished";
                    }
                    else
                    {
                        batch_status = "Not Yet Started";
                    }
                    BatchModel bm = new BatchModel()
                    {
                        batch_id = id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        is_schedule_generated = status,
                        total_students = cnt,
                        total_leactures = total_leactures,
                        attended_leatures = attended_leactures,
                        remaining_leatures = remaining_leatures,
                        completed_percentage = per,
                         branch_id = branch_id,
                          branch_name = branch_name,
                           batch_status=batch_status,
                            PlayListKey=playlist_key,
                             PlayListTitle=playlist_title
                            

                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<List<BatchModel>> GetAllDeletedBatches(int branch_id)
        {
            List<BatchModel> lst = new List<BatchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_deleted_batches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                   // int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    BatchModel bm = new BatchModel()
                    {
                        batch_id = id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                         branch_id = branch_id,
                          branch_name = branch_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<List<BatchScheduleModel>> GetAllBatchSchedules(int branch_id)
        {

            List<BatchScheduleModel> lst = new List<BatchScheduleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatch_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", 0);
                cmd.Parameters.AddWithValue("@branch_id", branch_id);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_schedule_id = Convert.ToInt32(dr["batch_schedule_id"].ToString());
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    DateTime? actual_date = null;
                    //  string expected_date = dr["expected_date"].ToString();

                    ////  DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    //string actual_date = dr["actual_date"].ToString();

                    //if (expected_date != "")
                    //{
                    //    expected_date = Convert.ToDateTime(expected_date).ToShortDateString();
                    //}
                    //  DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    //string actual_date = dr["actual_date"].ToString();
                    //if (actual_date != "")
                    //{
                    //    actual_date = Convert.ToDateTime(actual_date).ToShortDateString();
                    //}

                    string status = "Not Conducted";
                    if (actual_date != null)
                    {
                        status = "Conducted";
                    }


                    string batch_time = dr["batch_time"].ToString();
                    BatchScheduleModel bm = new BatchScheduleModel()
                    {
                        batch_schedule_id = batch_schedule_id,
                        batch_id = batch_id,
                        batch_name = batch_name,
                        actual_date = actual_date,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        batch_time = batch_time,
                        status = status,
                         branch_name=branch_name,
                          branch_id=branch_id
                         
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<List<BatchStudentModel>> GetAllBatchStudents(int branch_id)
        {

            List<BatchStudentModel> lst = new List<BatchStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatch_students", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();


                    BatchStudentModel bm = new BatchStudentModel()
                    {

                        batch_id = batch_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        batch_student_id = batch_student_id,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<List<EmployeeModel>> GetAllTrainers(int branch_id)
        {

            List<EmployeeModel> lst = new List<EmployeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltrainers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string gender = dr["gender"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();

                    EmployeeModel bm = new EmployeeModel()
                    {

                        employee_id = employee_id,
                        employee_name = employee_name,
                        email_address = email_address,
                        gender = gender,
                        mobile_number = mobile_number,
                        profile_photo = profile_photo,
                        //qualification = qualification,
                         branch_id = branch_id,
                          branch_name = branch_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<BatchModel> GetBatch(int batch_id)
        {

            BatchModel bm = new BatchModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                //cmd.Parameters.AddWithValue("@branch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    //int total_students = Convert.ToInt32(dr["total_students"].ToString());
                    int total_leactures = Convert.ToInt32(dr["total_leactures"].ToString());
                    int attended_leactures = Convert.ToInt32(dr["attended_leactures"].ToString());
                    List<BatchScheduleModel> schedule = await GetBatchWiseSchedule(batch_id);
                    bool status = false;
                    if (schedule.Count() > 0)
                    {
                        status = true;
                    }
                    int remaining_leatures = 0;
                    remaining_leatures = total_leactures - attended_leactures;
                    float per = 0;
                    if (total_leactures > 0)
                    {
                        per = ((float)attended_leactures * 100) /(float) total_leactures;
                    }

                    int cnt = 0;

                    List<BatchStudentModel> students = await GetBatchWiseStudents(batch_id);
                    if (students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    string batch_status = "";
                    if (total_leactures == attended_leactures)
                    {
                        batch_status = "completed";
                    }
                    else if (attended_leactures > 0 && attended_leactures < total_leactures)
                    {
                        batch_status = attended_leactures + " Leactures Finished";
                    }
                    else
                    {
                        batch_status = "Not Yet Started";
                    }
                    bm = new BatchModel()
                    {
                        batch_id = batch_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        is_schedule_generated = status,
                        total_students = cnt,
                        total_leactures = total_leactures,
                        attended_leatures = attended_leactures,
                        remaining_leatures = remaining_leatures,
                        completed_percentage = per,
                         branch_id = branch_id,
                          branch_name = branch_name

                    };
                }
            }
            return bm;
        }
        public async Task<List<BatchScheduleModel>> GetBatchWiseSchedule(int batch_id)
        {
            List<BatchScheduleModel> lst = new List<BatchScheduleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_wise_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_schedule_id = Convert.ToInt32(dr["batch_schedule_id"].ToString());
                    int b_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString(); DateTime? actual_date = null;
                    DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    // string expected_date =  dr["expected_date"].ToString() ;
                    //if (expected_date != "")
                    //{
                    //    expected_date=Convert.ToDateTime(expected_date).ToShortDateString();
                    //}
                    if (dr["actual_date"].ToString() != "")
                    {
                        actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    }
                    //  string actual_date =  dr["actual_date"].ToString() ;
                    //if (actual_date != "")
                    //{
                    //    actual_date = Convert.ToDateTime(actual_date).ToShortDateString();
                    //}
                    string status = "Not Conducted";
                    if (actual_date != null)
                    {
                        status = "Conducted";
                    }
                    string batch_time = dr["batch_time"].ToString();
                    BatchScheduleModel bm = new BatchScheduleModel()
                    {
                        batch_schedule_id = batch_schedule_id,
                        batch_id = b_id,
                        batch_name = batch_name,
                        //  actual_date = actual_date,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        actual_date = actual_date,
                        batch_time = batch_time,
                        status = status,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<BatchScheduleModel> GetScheduleWiseSchedule(int batch_schedule_id)
        {
            BatchScheduleModel st = new BatchScheduleModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_schedule_id", batch_schedule_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //int batch_schedule_id = Convert.ToInt32(dr["batch_schedule_id"].ToString());
                    int b_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();

                    DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    // string expected_date = dr["expected_date"].ToString();
                    //if (expected_date != "")
                    //{
                    //    expected_date = Convert.ToDateTime(expected_date).ToShortDateString();
                    //}
                    DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    //string actual_date = dr["actual_date"].ToString();
                    //if (actual_date != "")
                    //{
                    //    actual_date = Convert.ToDateTime(actual_date).ToShortDateString();
                    //}


                    string status = "Not Conducted";
                    if (actual_date != null)
                    {
                        status = "Conducted";
                    }
                    string batch_time = dr["batch_time"].ToString();
                    st = new BatchScheduleModel()
                    {
                        batch_schedule_id = batch_schedule_id,
                        batch_id = b_id,
                        batch_name = batch_name,
                        //  actual_date = actual_date,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        actual_date = actual_date,
                        batch_time = batch_time,
                        status = status,
                    };
                }
            }
            return st;
        }
        public async Task<List<BatchStudentModel>> GetBatchWiseStudents(int batch_id)
        {
            List<BatchStudentModel> lst = new List<BatchStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_wise_students", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
                    int b_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    //DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); 
                    //int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string course_name = dr["course_name"].ToString();


                    BatchStudentModel bm = new BatchStudentModel()
                    {

                        batch_id = b_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        start_date = start_date,
                        batch_student_id = batch_student_id,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name,
                        course_name = course_name,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public async Task<List<TopicStudentModel>> GetTopicWiseStudents(int batch_id)
        {
            BatchModel bm = await GetBatch(batch_id);
            List<BatchStudentModel> batchstudents = await GetBatchWiseStudents(batch_id);
            List<TopicStudentModel> lst = new List<TopicStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_students", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", bm.topic_id);
                cmd.Parameters.AddWithValue("@branch_id", bm.branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    int tp_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());


                    string branch_name = dr["branch_name"].ToString();

                    string student_name = dr["student_name"].ToString();
                    string parent_name = dr["student_name"].ToString();
                    string last_name = dr["student_name"].ToString();
                    string course_name = dr["course_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    BatchStudentModel bst = batchstudents.FirstOrDefault(e => e.registration_id.Equals(registration_id));
                    if (bst == null)
                    {
                        TopicStudentModel ts = new TopicStudentModel()
                        {
                            registration_id = registration_id,
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            topic_id = tp_id,
                            topic_name = topic_name,
                            branch_name = branch_name,
                            branch_id = branch_id
                        };
                        lst.Add(ts);
                    }
                }
            }
            return lst;
        }
        public async Task<List<BatchModel>> GetTrainerWiseBatches(int employee_id)
        {
            List<BatchModel> lst = new List<BatchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_trainer_wise_batches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    //  int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    int total_leactures = Convert.ToInt32(dr["total_leactures"].ToString());
                    int attended_leactures = Convert.ToInt32(dr["attended_leactures"].ToString());
                    List<BatchScheduleModel> schedule = await GetBatchWiseSchedule(id);
                    //bool status = false;
                    //if (schedule.Count() > 0)
                    //{

                    //    status = true;
                    //}

                    //int cnt = 0;
                    //List<BatchStudentModel> students =await GetBatchWiseStudents(id);
                    //if (students.Count() > 0)
                    //{
                    //    cnt = students.Count();
                    //}
                    //float remaining_leactures = total_leactures - attended_leactures;
                    //float per = attended_leactures * 100 / total_leactures;
                    //string batch_status = "";
                    //if (total_leactures == attended_leactures)
                    //{
                    //    batch_status = "completed";
                    //}
                    //else if (attended_leactures > 0 && attended_leactures < total_leactures)
                    //{
                    //    batch_status = attended_leactures + " Leactures Finished";
                    //}
                    //else
                    //{
                    //    batch_status = "Not Yet Started";
                    //}
                    bool status = false;
                    if (schedule.Count() > 0)
                    {
                        status = true;
                    }
                    int remaining_leatures = 0;
                    remaining_leatures = total_leactures - attended_leactures;
                    float per = 0;
                    if (total_leactures > 0)
                    {
                        per = (remaining_leatures * 100) / total_leactures;
                    }

                    int cnt = 0;

                    List<BatchStudentModel> students = await GetBatchWiseStudents(id);
                    if (students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    string batch_status = "";
                    if (total_leactures == attended_leactures)
                    {
                        batch_status = "completed";
                    }
                    else if (attended_leactures > 0 && attended_leactures < total_leactures)
                    {
                        batch_status = attended_leactures + " Leactures Finished";
                    }
                    else
                    {
                        batch_status = "Not Yet Started";
                    }
                    BatchModel bm = new BatchModel()
                    {
                        batch_id = id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        is_schedule_generated = status,
                        total_students = cnt,
                        total_leactures = total_leactures,
                        attended_leatures = attended_leactures,
                        batch_status = batch_status,
                        remaining_leatures = total_leactures - attended_leactures,
                        completed_percentage = per
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }

        public async Task MarkStudentScheduleAttendance(ScheduleAttendanceModel s)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("registration_id", typeof(int));
            dt.Columns.Add("is_present", typeof(int));
            dt.Columns.Add("flag", typeof(int));
            foreach (StudentAttendanceModel sam in s.students)
            {
                dt.Rows.Add(sam.registration_id, sam.is_present, 0);
            }
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch_schedule_attendance", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@schedule_attendance_id", 0);
                cmd.Parameters.AddWithValue("@batch_schedule_id", s.batch_schedule_id);
                //cmd.Parameters.AddWithValue("@batch_schedule_id", s.batch_schedule_id);
                cmd.Parameters.AddWithValue("@attendance_date", s.attendance_date);
                cmd.Parameters.AddWithValue("@id", 0);
                cmd.Parameters.AddWithValue("@attendance", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }




        public async Task<List<StudentMarkAttendance>> GetBatchWiseStudentAttendance(int batch_id, int registration_id)
        {
            List<StudentMarkAttendance> lst = new List<StudentMarkAttendance>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_and_batch_wise_attendance", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    //  int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    // int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    DateTime attendance_date = Convert.ToDateTime(dr["attendance_date"].ToString());
                    int is_present = Convert.ToInt32(dr["is_present"].ToString());
                    string attendance = dr["attendance"].ToString();
                    StudentMarkAttendance bm = new StudentMarkAttendance()
                    {
                        actual_date = actual_date,
                        attendance = attendance,
                        attendance_date = attendance_date,
                        batch_id = batch_id,
                        batch_name = batch_name,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date,
                        is_present = is_present,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name,
                        topic_id = topic_id,
                        topic_name = topic_name,

                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }

        public async Task<List<BatchStudentModel>> GetStudentWiseBatches(int student_id)
        {
            List<BatchStudentModel> lst = new List<BatchStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_student_wise_batches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    // DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); 
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    string batch_time = dr["batch_time"].ToString();
                    int s_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string playlist_title = dr["playlist_title"].ToString();
                    string playlist_key = dr["playlist_key"].ToString();
                    List<BatchScheduleModel> schedule = await GetBatchWiseSchedule(batch_id);
                    int total_leactures = schedule.Count();
                    int attended_leactures = schedule.Where(e => e.status.ToLower().Equals("conducted")).Count();
                    int remaining_leactures = total_leactures - attended_leactures;
                    List<StudentMarkAttendance> attendane = await GetBatchWiseStudentAttendance(batch_id, registration_id);
                    BatchStudentModel bm = new BatchStudentModel()
                    {

                        batch_id = batch_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        start_date = start_date,
                        batch_student_id = batch_student_id,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name,
                        registration_date = registration_date,
                        status = "",
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        schedule = schedule,
                        attendance = attendane,
                        total_leactures = total_leactures,
                        attended_leactures = attended_leactures,
                        remaining_leactures = remaining_leactures,
                        PlayListKey = playlist_key,
                        PlayListTitle = playlist_title
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        //public async Task<List<BatchStudentModel>> GetRegistrationAndTopicWiseBatch(int registration_id, int topic_id)
        //{
        //    List<BatchStudentModel> lst = new List<BatchStudentModel>();
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_registration_topic_wise_batch", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@registration_id", registration_id);
        //        cmd.Parameters.AddWithValue("@topic_id", topic_id);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
        //            int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
        //            string batch_name = dr["batch_name"].ToString();
        //            DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
        //            // DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); 
        //            DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());
        //           // int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
        //            string topic_name = dr["topic_name"].ToString();
        //            int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
        //            string employee_name = dr["employee_name"].ToString();
        //            int course_id = Convert.ToInt32(dr["course_id"].ToString());
        //            string course_name = dr["course_name"].ToString();
        //            string batch_time = dr["batch_time"].ToString();
        //            int s_id = Convert.ToInt32(dr["student_id"].ToString());
        //            int student_id = Convert.ToInt32(dr["student_id"].ToString());
        //            string student_name = dr["student_name"].ToString();
        //            List<BatchScheduleModel> schedule = await GetBatchWiseSchedule(batch_id);
        //            List<StudentMarkAttendance> attendane = await GetBatchWiseStudentAttendance(batch_id, registration_id);
        //            BatchStudentModel bm = new BatchStudentModel()
        //            {

        //                batch_id = batch_id,
        //                batch_name = batch_name,
        //                batch_time = batch_time,
        //                start_date = start_date,
        //                batch_student_id = batch_student_id,
        //                registration_id = registration_id,
        //                student_id = student_id,
        //                student_name = student_name,
        //                registration_date = registration_date,
        //                status = "",
        //                topic_id = topic_id,
        //                topic_name = topic_name,
        //                employee_id = employee_id,
        //                employee_name = employee_name,
        //                schedule = schedule,
        //                attendance = attendane
        //            };
        //            lst.Add(bm);
        //        }
        //    }
        //    return lst;
        //}

        public async Task<BatchModel> GetRegistrationAndTopicWiseBatch(int registration_id, int topic_id)
        {

            BatchModel bm = new BatchModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_registration_topic_wise_batch", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string batch_name = dr["batch_name"].ToString();
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    //int total_students = Convert.ToInt32(dr["total_students"].ToString());
                    int total_leactures = Convert.ToInt32(dr["total_leactures"].ToString());
                    int attended_leactures = Convert.ToInt32(dr["attended_leactures"].ToString());
                    List<BatchScheduleModel> schedule = await GetBatchWiseSchedule(batch_id);
                    //bool status = false;
                    //if (schedule.Count() > 0)
                    //{
                    //    status = true;
                    //}
                    //int remaining_leatures = total_leactures - attended_leactures;
                    //float per = (attended_leactures * 100) / total_leactures;
                    //int cnt = 0;
                    ////List<BatchStudentModel> students = await GetBatchWiseStudents(batch_id);
                    ////if (students.Count() > 0)
                    ////{
                    ////    cnt = students.Count();
                    ////}
                    //string batch_status = "";
                    //if (total_leactures == attended_leactures)
                    //{
                    //    batch_status = "completed";
                    //}
                    //else if (attended_leactures > 0 && attended_leactures < total_leactures)
                    //{
                    //    batch_status = attended_leactures + " Leactures Finished";
                    //}
                    //else
                    //{
                    //    batch_status = "Not Yet Started";
                    //}
                    bool status = false;
                    if (schedule.Count() > 0)
                    {
                        status = true;
                    }
                    int remaining_leatures = 0;
                    remaining_leatures = total_leactures - attended_leactures;
                    float per = 0;
                    if (total_leactures > 0)
                    {
                        per = (remaining_leatures * 100) / total_leactures;
                    }

                    int cnt = 0;

                    List<BatchStudentModel> students = await GetBatchWiseStudents(batch_id);
                    if (students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    string batch_status = "";
                    if (total_leactures == attended_leactures)
                    {
                        batch_status = "completed";
                    }
                    else if (attended_leactures > 0 && attended_leactures < total_leactures)
                    {
                        batch_status = attended_leactures + " Leactures Finished";
                    }
                    else
                    {
                        batch_status = "Not Yet Started";
                    }
                    bm = new BatchModel()
                    {
                        batch_id = batch_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        is_schedule_generated = status,
                        //  total_students = cnt,
                        total_leactures = total_leactures,
                        attended_leatures = attended_leactures,
                        remaining_leatures = remaining_leatures,
                        completed_percentage = per,
                         branch_id = branch_id,
                          branch_name = branch_name

                    };
                }
            }
            return bm;
        }
        public async Task DeleteBatch(int batch_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                cmd.Parameters.AddWithValue("@batch_name", "");
                cmd.Parameters.AddWithValue("@topic_id", 0);
                cmd.Parameters.AddWithValue("@employee_id", 0);
                cmd.Parameters.AddWithValue("@start_date", null);
                cmd.Parameters.AddWithValue("@end_date", null);
                cmd.Parameters.AddWithValue("@batch_time", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task RestoreBatch(int batch_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Restore");
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                cmd.Parameters.AddWithValue("@batch_name", "");
                cmd.Parameters.AddWithValue("@topic_id", 0);
                cmd.Parameters.AddWithValue("@employee_id", 0);
                cmd.Parameters.AddWithValue("@start_date", null);
                cmd.Parameters.AddWithValue("@end_date", null);
                cmd.Parameters.AddWithValue("@batch_time", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task DeleteBatchStudent(int student_id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BatchScheduleExamModel>> GetBatchWiseScheduledExams(int batch_id)
        {
            List<BatchScheduleExamModel> lst = new List<BatchScheduleExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_wise_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string batch_name = dr["batch_name"].ToString();
                   
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();

                    BatchScheduleExamModel bm = new BatchScheduleExamModel()
                    {
                        batch_name = batch_name,
                        exam_date = exam_date,
                        start_time = start_time,
                        batch_id = batch_id,

                        topic_id = topic_id,
                        topic_name = topic_name


                    };
                    lst.Add(bm);
                }
            }
            return lst;

        }

        public async Task SetPlayListTitle(BatchPlayListModel b)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_set_batchplaylist_key", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
               
                cmd.Parameters.AddWithValue("@batch_id", b.batch_id);
                cmd.Parameters.AddWithValue("@playlist_title",b.playlist_title);
                cmd.Parameters.AddWithValue("@playlist_key", b.playlist_key);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}