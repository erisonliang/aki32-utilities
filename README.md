# 💖 aki32-utilities

## ❗Abstract

All of my C# utilities







<br>

## 🌟 Solution Structure

<pre>

#_Aki32Utilities
 ┃
 ┣━ 0_Apps
 ┃   ┣━ 001_ConsoleMiniApps                ... Standalone C# small applications
 ┃   ┗━ 0AA_ResearchArticlesNodeController ... Handle research articles with node link GUI (using A11 and B01)
 ┃
 ┣━ 1_Examples
 ┃   ┣━ 101_ConsoleUtilExample             ... Usage examples for A
 ┃   ┣━ 102_NodeControllerExample          ... Usage examples for B01
 ┃   ┗━ 103_WFAControlExample              ... Usage examples for C01
 ┃
 ┣━ A_Console
 ┃   ┣━ A00_General                        ... C# utils
 ┃   ┣━ A01_AI                             ... Handle AI/ML models
 ┃   ┣━ A10_Structure                      ... Many structural engineering calculation models
 ┃   ┗━ A11_Research                       ... Handle research articles
 ┃
 ┣━ B_WPF                                     (Windows Presentation Foundation)
 ┃   ┣━ B00_General                        ... WPF utils
 ┃   ┗━ B01_NodeController                 ... Define node link graph
 ┃
 ┗━ C_WFA                                     (Windows Form Application)
     ┗━ C01_Control                        ... Original user controls

</pre>









<br>

## 🌟 Chainable Extensions

You can now handle files with beautiful method chain!!!

### 🎇 Example

```C#
new DirectoryInfo(@"C:\input")
  .CollectFiles(null, searchRegexen: $@"{eq}B\d*\.csv$")
  .RenameFiles()
  .ExtractCsvColumnsForMany_Loop(null, 6,
    ("i", new int[] { 0, 2 }, "t,μ"),
    ("j", new int[] { 0, 3 }, "t,μ"))
  .Rainflow_Loop(4, 1 / 3d)
  .CollectCsvColumns(null, 3)
  .RenameFile($"damage")
  .MoveTo(new DirectoryInfo(@"C:\result"))
  //.Csvs2ExcelSheets(null)
  ;
```

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00_Overview.jpg" width="666">



### 🎇 In A00 General

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00_1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A0001.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A0011.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A0021.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A0031.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00B1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00B2.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00C1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00D1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00E1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00E2.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00F1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00G1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00H1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00I1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00J1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00K1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00L1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00M1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A00X1.jpg" width="666">



### 🎇 In A10 Structural Engineering

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A10A1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/A10B1.jpg" width="666">








<br>

## 🌟 Awesome Models

### 🎇 In A00 General


- TimeHistoryModel

Model for handling time history data set (such as time-displacement relationship)













<br>

## ❗Clone

Since this repo has some git submodules, use following for correct cloning (* no submodules currently)

```bash
git clone --recursive https://github.com/aki32/aki32-utilities
```

If you've forgotten to use above to install submodules, use following
```bash
git submodule update --init --recursive
```









<br>

## ❗Notice

Any commitment to enhance this repo will be welcomed!







<br>

## ❗Japanese 日本語

C# の便利機能を実装した C# ソリューションです！

その他にも，個人の需要に応じて構造計算やAIなども随時実装しています。






<br>
