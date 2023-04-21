using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class I001_ImageInput
{
    public I001_ImageInput(byte[] image, string label, string imageFileName)
    {
        Image = image;
        Label = label;
        ImageFileName = imageFileName;
    }

    public readonly byte[] Image;
    public readonly string Label;
    public readonly string ImageFileName;

    public static IEnumerable<(FileInfo imageFile, string label)> LoadImagesFromDirectory(DirectoryInfo imagesDir, bool useFolderNameAsLabel)
    {
        var imagesPath = imagesDir.GetFiles_Images(SearchOption.AllDirectories);

        return useFolderNameAsLabel
            ? imagesPath.Select(imageFile => (imageFile, imageFile.Directory!.Name))
            : imagesPath.Select(imageFile =>
            {
                var label = imageFile.Name;
                for (var index = 0; index < label.Length; index++)
                {
                    if (!char.IsLetter(label[index]))
                    {
                        label = label[..index];
                        break;
                    }
                }
                return (imageFile, label);
            });
    }

    public static IEnumerable<I001_ImageInput> LoadInMemoryImagesFromDirectory(DirectoryInfo imagesDir, bool useFolderNameAsLabel = true)
    {
        return LoadImagesFromDirectory(imagesDir, useFolderNameAsLabel)
            .Select(x => new I001_ImageInput(image: x.imageFile.ReadAllBytes(), label: x.label, imageFileName: x.imageFile.Name));
    }
}
