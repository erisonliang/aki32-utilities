# 💖 csharp-utilities

## ❗Abstract

C# utilities



<br>

## ❗Download

Since this repo has some git submodules, use following for correct cloning (* no submodules currently)

```bash
git clone --recursive https://github.com/aki32/aki32-utilities
```

If you've forgotten to use above to install submodules, use following
```bash
git submodule update --init --recursive
```







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

### 🎇 TimeHistoryModel

Model for handling time history data set (such as time-displacement relationship)





<br>

## 🌟 AI

Project for handling AI.


### 🎇 MLNetHandler

Summary of ML.NET 





<br>

## 🌟 Structural Engineering

Project for structural engineering






<br>

## 🌟 Research

Project for handling research papers






<br>

## 🌟 Node Controller

Project for handling node graph




<br>

## 🌟 Node Controller

Project for handling node graph






<br>

## ❗Notice

Any commitment to enhance this repo will be welcomed!




<br>

## ❗Japanese 日本語

C# の便利機能を実装した C# ソリューションです！

基本機能に加えて，構造計算やAIなども随時実装しています。

なかなか便利ですのでぜひお試しください！


<br>
