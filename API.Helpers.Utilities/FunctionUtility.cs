
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace API.Helpers.Utilities;

public interface IFunctionUtility
{
    Task<string> UploadAsync(IFormFile file, string subfolder, string rawFileName);
    Task<string> UploadAsync(string file, string subfolder, string rawFileName);
    string RemoveUnicode(string str);
}

public class FunctionUtility : IFunctionUtility
{
    private string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    public async Task<string> UploadAsync(IFormFile file, string subfolder, string rawFileName)
    {
        if (file == null)
        {
            return null;
        }

        var folderPath = Path.Combine(webRootPath, subfolder);
        var fileName = file.FileName;
        var extension = Path.GetExtension(file.FileName);

        if (string.IsNullOrEmpty(extension))
        {
            return null;
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (!string.IsNullOrEmpty(rawFileName))
        {
            fileName = $"{rawFileName}{extension}";
        }

        var filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        try
        {
            using (FileStream fs = File.Create(filePath))
            {
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
            }

            return fileName;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<string> UploadAsync(string file, string subfolder, string rawFileName)
    {
        if (string.IsNullOrEmpty(file))
        {
            return null;
        }

        var folderPath = Path.Combine(webRootPath, subfolder);
        var extension = $".{file.Split(';')[0].Split('/')[1]}";

        if (string.IsNullOrEmpty(extension))
        {
            return null;
        }

        var fileName = $"{Guid.NewGuid().ToString()}{extension}";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (!string.IsNullOrEmpty(rawFileName))
        {
            fileName = $"{rawFileName}{extension}";
        }

        var filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var base64String = file.Substring(file.IndexOf(',') + 1);
        var fileData = Convert.FromBase64String(base64String);

        try
        {
            await File.WriteAllBytesAsync(filePath, fileData);
            
            return fileName;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static readonly string[] VietnameseChars =
    [
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    ];

    public string RemoveUnicode(string text)
    {
        for (int i = 1; i < VietnameseChars.Length; i++)
        {
            for (int j = 0; j < VietnameseChars[i].Length; j++)
                text = text.Replace(VietnameseChars[i][j], VietnameseChars[0][i - 1]);
        }
        
        text = Regex.Replace(text, "[^0-9a-zA-Z]+", " ").ToLower();
        
        return text;
    }
}
