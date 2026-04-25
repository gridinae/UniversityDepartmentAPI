namespace UniversityDepartmentAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public int Role { get; set; }
    public int? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}