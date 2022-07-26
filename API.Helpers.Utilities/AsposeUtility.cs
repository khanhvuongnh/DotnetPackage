using Aspose.Cells;
using Aspose.Cells.Drawing;

namespace API.Helpers.Utilities;
public static class AsposeUtility
{
    public static void SetLicense(string filePath)
    {
        try
        {
            var cellsLicense = new Aspose.Cells.License();
            cellsLicense.SetLicense(filePath);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static Style SetAllBorders(this Style style)
    {
        style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
        style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        return style;
    }

    public static Style SetAlignCenter(this Style style)
    {
        style.IsTextWrapped = true;
        style.HorizontalAlignment = TextAlignmentType.Center;
        style.VerticalAlignment = TextAlignmentType.Center;
        return style;
    }

    public static Picture SetPictureSize(this Picture picture, int customWidth = 100)
    {
        var originWidth = picture.Width;
        var originHeight = picture.Height;
        var customHeight = (int)(originHeight * customWidth / originWidth);
        picture.Top = 5;
        picture.Left = 5;
        picture.Width = customWidth;
        picture.Height = customHeight;
        return picture;
    }
}