

namespace Aki32Utilities.Uno.CheatSheet;
public class Student
{
    public int StudentId { get; private set; }  //主キー（変更不可）
    public string Name { get; set; }
    public int Age { get; set; }

    public Student() { }
    public Student(int StudentId) => this.StudentId = StudentId;

}
