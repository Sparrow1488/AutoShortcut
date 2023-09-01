using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services.Options;

public interface IUploadFilesOptions
{
    IEnumerable<string> IgnoreFileTypes { get; }
    Func<IFile, UploadFileAction> OnUploadedIgnoreFile { get; set; }
    IUploadFilesOptions Ignore(string fileType);
}