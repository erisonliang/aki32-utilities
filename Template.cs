using System;

public class Template
{

    public static void RunExampleModel(FileInfo? outputImageFile = null, bool preview = true)
    {
        outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));


    }


}
