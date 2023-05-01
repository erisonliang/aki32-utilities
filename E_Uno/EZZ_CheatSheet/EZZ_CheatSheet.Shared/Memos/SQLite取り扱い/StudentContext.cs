using System;
using System.Data.Entity;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Aki32Utilities.Uno.CheatSheet;
internal class StudentContext : DbContext // ← データベースの文脈を継承できる？
{
    /// <summary>
    /// 学生リスト（DB側のコンテキスト）
    /// </summary>
    public DbSet<Student> students { get; set; }
    public DbSet<Teacher> teachers { get; set; }

    /// <summary>
    /// データベースの名前命名
    /// </summary>
    const string DbName = "students.db";

    /// <summary>
    /// 接続文字列（UWPとWebASM）（自動的にこれで初期化）
    /// </summary>
    internal static string _connectionString = $"data source={DbName}";

    /// <summary>
    /// 接続文字列（Android）
    /// </summary>
    public static void InitConnectionStringForAndroid()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DbName);
        _connectionString = $"filename={dbPath}";
    }

    /// <summary>
    /// 接続文字列（iOS）
    /// </summary>
    public static void InitConnectionStringForIOS()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DbName);
        _connectionString = $"filename={dbPath}";
    }

    /// <summary>
    /// DbContextの設定（SQLiteを使用）
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

}
