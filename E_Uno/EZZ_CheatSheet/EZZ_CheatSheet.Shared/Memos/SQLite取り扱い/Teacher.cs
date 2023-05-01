

namespace Aki32Utilities.Uno.CheatSheet;
public class Teacher
{
    public int TeacherId { get; private set; }  //主キー（変更不可）
    public string Name { get; set; }
    public int Age { get; set; }

    public Teacher() { }
    public Teacher(int TeacherId) => this.TeacherId = TeacherId;
}
