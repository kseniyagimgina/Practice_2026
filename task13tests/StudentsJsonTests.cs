using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using task13;
namespace task13tests;

public class StudentJsonTests
{
    private readonly JsonService service;

    public StudentJsonTests()
    {
        service = new JsonService();
    }

    [Fact]
    public void Serialize_ReturnsCorrectJson()
    {
        var student = new Student
        {
            FirstName = "Александр",
            LastName = "Сидоров",
            BirthDate = new DateTime(2006, 06, 24),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 5 },
                new Subject { Name = "Русский язык", Grade = 3 }
            },
            Information = null
        };
        string json = service.StudentSerialize(student);
        Assert.Contains("Александр", json);
        Assert.Contains("Сидоров", json);
        Assert.Contains("Математика", json);
        Assert.Contains("Русский язык", json);
        Assert.Contains("\"BirthDate\": \"2006-06-24\"", json);
        Assert.DoesNotContain("Information", json);
    }

    [Fact]
    public void Serialize_WithInformation()
    {
        var student = new Student
        {
            FirstName = "Варвара",
            LastName = "Иванова",
            BirthDate = new DateTime(2008, 02, 12),
            Grades = new List<Subject> 
            { 
                new Subject { Name = "Информатика", Grade = 5 },
                new Subject { Name = "Биология", Grade = 5 },
                new Subject { Name = "Литература", Grade = 5 }

            },
            Information = "Отличница"
        };
        string json = service.StudentSerialize(student);
        Assert.Contains("Варвара", json);
        Assert.Contains("Иванова", json);
        Assert.Contains("Информатика", json);
        Assert.Contains("Биология", json);
        Assert.Contains("Литература", json);
        Assert.Contains("\"BirthDate\": \"2008-02-12\"", json);
        Assert.Contains("Отличница", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsStudent()
    {
        string json = @"{
                ""FirstName"": ""Илья"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2007-05-22"",
                ""Grades"": 
                [
                { ""Name"": ""Физика"", ""Grade"": 4 },
                { ""Name"": ""Алгебра"", ""Grade"": 5 },
                { ""Name"": ""Геометрия"", ""Grade"": 4 }
                ]
            }";
        var student = service.StudentDeserialize(json);
        Assert.Equal("Илья", student.FirstName);
        Assert.Equal("Петров", student.LastName);
        Assert.Equal(new DateTime(2007, 5, 22), student.BirthDate);
        Assert.Equal(3, student.Grades.Count);
    }

    [Fact]
    public void Deserialize_InvalidDateFormat()
    {
        string json = @"{
                ""FirstName"": ""Екатерина"",
                ""LastName"": ""Владимирова"",
                ""BirthDate"": ""25-04-2010"",
                ""Grades"": 
                [
                { ""Name"": ""История"", ""Grade"": 5 },
                { ""Name"": ""Обществознание"", ""Grade"": 4 }
                ]
            }";
        var exception = Assert.Throws<JsonException>(() => service.StudentDeserialize(json));
        Assert.Contains("Неверный формат даты, верно:", exception.Message);
    }

    [Fact]
    public void Deserialize_EmptyFirstName()
    {
        string json = @"{
                ""FirstName"": """",
                ""LastName"": ""Сергеева"",
                ""BirthDate"": ""2005-01-01"",
                ""Grades"": 
                [
                { ""Name"": ""Химия"", ""Grade"": 4 },
                { ""Name"": ""Физика"", ""Grade"": 4 }
                ]
            }";
        var exception = Assert.Throws<ArgumentException>(() => service.StudentDeserialize(json));
        Assert.Contains("Имя", exception.Message);
    }

    [Fact]
    public void Deserialize_WithInformation()
    {
        string json = @"{
                ""FirstName"": ""Иван"",
                ""LastName"": ""Иванов"",
                ""BirthDate"": ""2005-01-01"",
                ""Grades"": 
                [
                { ""Name"": ""Математика"", ""Grade"": 2 },
                { ""Name"": ""Программирование"", ""Grade"": 3 }
                ],
                ""Information"": ""Отправлен на пересдачу""
            }";
        var student = service.StudentDeserialize(json);
        Assert.Equal(2, student.Grades[0].Grade);
        Assert.Equal("Отправлен на пересдачу", student.Information);
    }
    [Fact]
    public void Program_Run_PrintsStudentInfo()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        task13.Program.Main();
        var service = new JsonService();
        Assert.Contains("Алиса Смирнова", output.ToString());
        Assert.Contains("2007-07-07", output.ToString());
        Assert.Contains("Алгоритмизация и Программирование", output.ToString());
        Assert.Contains("Иностранный язык", output.ToString());
        Assert.Contains("Математическая логика", output.ToString());
        Assert.Contains("Алгебра", output.ToString());
        Assert.Contains("Практика речевой деятельности", output.ToString());
        Assert.Contains("Имеет повышенную стипендию за хорошую успеваемость", output.ToString());
        Assert.Contains("Информация сохранена в файл", output.ToString());
        Assert.Contains("Загружен студент: Алиса Смирнова", output.ToString());
        Assert.Contains("Дата рождения: 2007-07-07", output.ToString());
        Assert.Contains("\"FirstName\"", output.ToString());
        Assert.Contains("\"LastName\"", output.ToString());
        Assert.Contains("\"BirthDate\"", output.ToString());
        Assert.Contains("\"Grades\"", output.ToString());
        Assert.Contains("\"Information\"", output.ToString());
    }


}

