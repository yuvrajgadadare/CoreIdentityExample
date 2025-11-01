namespace ERP_Models
{
    public class RoleModel
    {
        public int employee_role_id { get; set; }

        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string role_id { get; set; }
        public string role_name { get; set; }
        public bool IsSelected{get;set;}
    }
}