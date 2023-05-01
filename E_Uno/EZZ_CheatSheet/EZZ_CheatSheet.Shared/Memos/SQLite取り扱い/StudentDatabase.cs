using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Aki32Utilities.Uno.CheatSheet;
class StudentDatabase
{

    /// <summary>
    /// 学生リスト（DBではなくCS側）
    /// </summary>
    public static ObservableCollection<Student> student_list { get; private set; }
    public static ObservableCollection<Teacher> teacher_list { get; private set; }

    public static void InitForAndroid()
    {
        StudentContext.InitConnectionStringForAndroid();
    }

    public static void InitForIOS()
    {
        SQLitePCL.Batteries.Init();
        StudentContext.InitConnectionStringForIOS();
    }

    /// <summary>
    /// データベースの読み込み（初期化したリストを返す）
    /// </summary>
    /// <returns></returns>
    public static async Task<Tuple<ObservableCollection<Student>, ObservableCollection<Teacher>>> LoadAsync()
    {
        // ★初期化
        using (var context = new StudentContext())
        {
#if DEBUG
            //★DELETE DB (if DB exists)
            context.Database.EnsureDeleted(); // DB 削除
#endif

            //★CREATE DB (if no DB exists)
            await context.Database.EnsureCreatedAsync(CancellationToken.None);
            //データベースに最初の要素を加える（定義は真下にある）
            await EnsureInitialDataAsync();
            //それをリストに変換
            student_list = new ObservableCollection<Student>(await context.students.ToListAsync());
            teacher_list = new ObservableCollection<Teacher>(await context.teachers.ToListAsync());
            return new Tuple<ObservableCollection<Student>, ObservableCollection<Teacher>>(student_list, teacher_list);

            async Task EnsureInitialDataAsync()
            {
                if (await context.students.FirstOrDefaultAsync() != null)
                    return;

                context.students.Add(new Student()
                {
                    Name = "アキミツタイチ",
                    Age = 23,
                });
                context.students.Add(new Student()
                {
                    Name = "ヤマモトレナ",
                    Age = 21,
                });
                context.students.Add(new Student()
                {
                    Name = "ダレカ",
                    Age = 50,
                });
                context.teachers.Add(new Teacher()
                {
                    Name = "田中先生",
                    Age = 222,
                });
                var count = await context.SaveChangesAsync(CancellationToken.None);
#if DEBUG
                Console.WriteLine("{0} records saved to database", count);
#endif

            }

        }
    }

    #region このプログラム用のお遊びクラスたち

    /// <summary>
    /// 判断してDBへのUPDATEかINSERT
    /// 仮のデータのupdateは、DBへの新規登録となり、ArticleIdが採番される。
    /// </summary>
    /// <param name="original_student"></param>
    /// <param name="new_student"></param>
    /// <returns></returns>
    public static async Task<Student> UpdateAsync(Student original_student, Student new_student)
    {
        if (!student_list.Contains(original_student))
            throw new ArgumentException("指定された学生データはリストに含まれていません。");

        if (original_student.StudentId != new_student.StudentId)
            throw new ArgumentException("2つの引数で学生IDが違います。");

        if (original_student == new_student)
            return original_student;

        Student modified_student;
            modified_student = await UpdateDBAsync(original_student, new_student);

        // ObservableCollectionのデータを入れ替える（これでNotifyChangedが画面側に飛ぶ）
        int old_item_position = student_list.IndexOf(original_student);
        student_list.RemoveAt(old_item_position);
        student_list.Insert(old_item_position, modified_student);

        return modified_student;
    }

    #endregion


    #region データベース操作

    /// <summary>
    /// ★INSERT（IDは自動で採番される！）
    /// </summary>
    /// <param name="new_item"></param>
    /// <returns></returns>
    public static async Task<Student> InsertDBAsync(Student new_item)
    {
        // 新規データを追加（自動採番）
        using (var context = new StudentContext())
        {
            var newEntry = context.students.Add(new Student
            {
                Name = new_item.Name,
                Age = new_item.Age,
            });
            await context.SaveChangesAsync(CancellationToken.None);

            return newEntry.Entity;
        }
    }

    /// <summary>
    /// ★UPDATE
    /// </summary>
    /// <param name="target_item"></param>
    /// <param name="new_item"></param>
    /// <returns></returns>
    public static async Task<Student> UpdateDBAsync(Student target_item, Student new_item)
    {
        // 既存データの更新
        using (var context = new StudentContext())
        {
            // データベースから更新対象のデータを取ってくる
            var targetItem = await context.students.SingleAsync(x => x.StudentId == target_item.StudentId);

            // 更新対象のデータを書き換え
            targetItem.Name = new_item.Name;
            targetItem.Age = new_item.Age;

            // データベースに反映
            await context.SaveChangesAsync(CancellationToken.None);

            return targetItem;
        }
    }

    /// <summary>
    /// ★DELETE
    /// </summary>
    /// <param name="target_item"></param>
    /// <returns></returns>
    public static async Task DeleteDBAsync(Student target_item)
    {
        if (!student_list.Contains(target_item))
            throw new ArgumentException("指定された学生データはリストに含まれていません。");

        if (target_item.StudentId > 0)
        {
            using (var context = new StudentContext())
            {
                var targetItem = await context.students.SingleAsync(x => x.StudentId == target_item.StudentId);

                context.students.Remove(targetItem);
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        student_list.Remove(target_item);
    }

    /// <summary>
    /// ★SQLをコマンド入力！
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static async Task CommandDBAsync_OutPutOnConsole(string command = "SELECT * FROM students")
    {
        using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(StudentContext._connectionString))
        {
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = command;
            using (var reader = cmd.ExecuteReader())
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write(reader.GetInt32(i) + ", ");
                    Console.WriteLine();
                }
        }
    }

    #endregion

}
