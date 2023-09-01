using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services.Options;
using System.Collections.ObjectModel;

namespace Sparrow.Video.Shortcuts.Services.Options;

public class UploadFilesOptions : IUploadFilesOptions
{
    protected ICollection<string> _ignoreFileTypes = new Collection<string>();

    public IEnumerable<string> IgnoreFileTypes => _ignoreFileTypes;
    public Func<IFile, UploadFileAction> OnUploadedIgnoreFile { get; set; } = file => UploadFileAction.Exception;

    public IUploadFilesOptions Ignore(string fileType)
    {
        _ignoreFileTypes.Add(fileType);
        return this;
    }
}