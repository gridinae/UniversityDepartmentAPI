namespace UniversityDepartmentAPI.Models
{
    public class Teaching
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public string AcademicYear { get; set; }
        public int Semester { get; set; }
        public int? Hours { get; set; }
        public Teacher? Teacher { get; set; }
        public Discipline? Discipline { get; set; }
    }
}
