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

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100_Overview.jpg" width="666">



### 🎇 In 100 General

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100_1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/10001.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/10011.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/10021.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/10031.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100B1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100B2.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100C1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100D1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100E1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100E2.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100F1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100G1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100H1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100I1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100J1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100K1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100L1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100M1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/100X1.jpg" width="666">



### 🎇 In 110 Structural Engineering

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/110A1.jpg" width="666">
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/110B1.jpg" width="666">








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

## 🌟 NodeController

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
