## Function Utility

```cs
Task<string> UploadAsync(IFormFile file, string subfolder, string rawFileName);
Task<string> UploadAsync(string file, string subfolder, string rawFileName);
string RemoveUnicode(string str);
```

## Pagination Utility

```cs
Task<PaginationUtility<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
PaginationUtility<T> Create(List<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
```

## Mail Utility

```cs
void SendMail(MailRequest request)
async Task SendMailAsync(MailRequest request)
```

## Aspose Utility

```cs
static Style SetAllBorders(this Style style)
static Style SetAlignCenter(this Style style)
static Picture SetPictureSize(this Picture picture, int customWidth = 100)
```

## Jwt Utility

```cs
string GenerateJwtToken(Claim[] claims, string secretKey, DateTime expires);
string ValidateJwtToken(string token, string secretKey, string claimType = "nameid");
RefreshToken GenerateRefreshToken(string username, DateTime expires);
```

## Operation Result

```cs
public class OperationResult<T>
{
    public string Error { set; get; }
    public bool IsSuccess { set; get; }
    public T Data { set; get; }
}
```