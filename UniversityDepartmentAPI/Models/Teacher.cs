namespace UniversityDepartmentAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PassportNumber { get; set; }
        public string? Inn { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public decimal? Rate { get; set; }
        public int? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }
        public List<Teaching>? Teachings { get; set; }
        public List<TeacherExtraWork>? TeacherExtraWorks { get; set; }
    }
}
