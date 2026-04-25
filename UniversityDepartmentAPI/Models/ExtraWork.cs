namespace UniversityDepartmentAPI.Models;

public class ExtraWork
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int? DefaultHours { get; set; }
    public List<TeacherExtraWork>? TeacherExtraWorks { get; set; }
}