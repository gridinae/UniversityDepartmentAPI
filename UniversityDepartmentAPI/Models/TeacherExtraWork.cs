namespace UniversityDepartmentAPI.Models;

public class TeacherExtraWork
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int ExtraWorkId { get; set; }
    public string AcademicYear { get; set; }
    public int Semester { get; set; }
    public int? Status { get; set; }
    public Teacher? Teacher { get; set; }
    public ExtraWork? ExtraWork { get; set; }
}