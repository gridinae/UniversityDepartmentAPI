namespace UniversityDepartmentAPI.Models
{
    public class Discipline
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? HoursTotal { get; set; }
        public int? Semester { get; set; }
        public string? Code { get; set; }
        public List<Teaching>? Teachings { get; set; }
    }
}
