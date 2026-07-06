using task02;
namespace task02tests;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.2).ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(a => a.Grades.Average() >= 4.2));
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsCorrectList()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        var names = result.Select(a => a.Name).ToList();
        var alphabetical_names = new List<string> {"Анна", "Иван", "Петр"};
        Assert.Equal(alphabetical_names, names); 
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectList()
    {
        var result = _service.GroupStudentsByFaculty().ToList();
        var FIT = result.Single(a => a.Key == "ФИТ");
        var Economy = result.Single(a => a.Key == "Экономика");
        Assert.Contains(FIT, c => c.Name == "Иван");
        Assert.Contains(FIT, d => d.Name == "Анна");
        Assert.Contains(Economy, e => e.Name == "Петр");
        Assert.Equal(2, result.Single(g => g.Key == "ФИТ").Count());
        Assert.Single(result.Single(g => g.Key == "Экономика"));
    }
}
