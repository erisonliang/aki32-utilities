

# 🌟 Numbering Rule

 - Prefix: Depends on their input.

 - Num: 100 * α + β
   
   - α: Depends on input and output<br/>
     = 0 (both are FileSystemInfo)<br/>
     = 1 (either)<br/>
     = 2 (neither)

   - β: Incremental num
 



### For example, "B102 ReadObjectFromLocal"

 - input is FileSystem → classified to "B - FileSystem" → "B---"

 - output is FileSystemInfo and input is not. → "B1--"

 - B101 is taken, and B102 is available → "B102"



# 🌟 Creating Rule

 - Template exists in Z999.

 - First, accept and return as FileInfo or DirectoryInfo if possible.

 - Second, accept and return as Image, Size or Point if possible.


# 🌟 memo

- for playing sound in console, you can use Argus.Audio.NAudio NuGet

- for handling spectrogram, you can use Spectrogram NuGet
