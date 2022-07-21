using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Abstractions.Services.Options;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Exceptions;
using System.Collections.ObjectModel;
using SFile = Sparrow.Video.Shortcuts.Primitives.File;

namespace Sparrow.Video.Shortcuts.Services
{
    public class UploadFilesService : IUploadFilesService
    {
        public UploadFilesService(IFileTypesProvider typesProvider)
        {
            TypesProvider = typesProvider;
        }

        public IFileTypesProvider TypesProvider { get; }
        public event Action<IFile> OnUploadedFile;

        public IFile GetFile(string filePath)
        {
            var stringPath = StringPath.Create(filePath);
            var fileInfo = new FileInfo(stringPath.Value);
            var file = new SFile()
            {
                Extension = fileInfo.Extension,
                FileType = TypesProvider.GetFileType(fileInfo.Extension),
                Name = Path.GetFileNameWithoutExtension(filePath),
                Path = stringPath.Value
            };
            return file;
        }

        public ICollection<IFile> GetFiles(string path, IUploadFilesOptions uploadFilesOptions)
        {
            var ignoreFilesCollection = new Collection<IFile>();
            OnUploadedFile += file =>
            {
                if (uploadFilesOptions.IgnoreFileTypes.Contains(file.FileType))
                {
                    var action = uploadFilesOptions.OnUploadedIgnoreFile.Invoke(file);
                    switch (action.ActionName)
                    {
                        case "NoAction": break;
                        case "Skip": ignoreFilesCollection.Add(file); break;
                        case "Exception": throw new IgnoreFileUploadedException("Uploaded file that marked as ignore " + file.FileType);
                    }
                }
            };
            var uploadedFiles = GetFiles(path).ToList();
            var removedCount = uploadedFiles.RemoveAll(x => ignoreFilesCollection.Contains(x));
            return uploadedFiles;
        }

        /// <summary>
        ///     Get files from path on your computer
        /// </summary>
        /// <param name="path">Root files directory</param>
        /// <returns>Directory files</returns>
        /// <exception cref="InvalidInputPathException">Invalid path</exception>
        public ICollection<IFile> GetFiles(string path)
        {
            var filesList = new List<IFile>();
            filesList.AddRange(CreateDirectoryFiles(path));
            var insideDirs = Directory.GetDirectories(path);
            foreach (var dir in insideDirs)
                filesList.AddRange(CreateDirectoryFiles(dir));
            return filesList;
        }

        private IEnumerable<IFile> CreateDirectoryFiles(string dirPath)
        {
            var filesCollection = new Collection<IFile>();
            var files = Directory.GetFiles(dirPath);
            var directoryName = Path.GetFileName(dirPath);
            foreach (var file in files)
                filesCollection.Add(CreateFileUsingPath(file, fileGroup: directoryName));
            return filesCollection;
        }

        public Task<ICollection<IFile>> GetFilesAsync(
            string path, CancellationToken token = default)
        {
            return Task.FromResult(GetFiles(path));
        }

        private IFile CreateFileUsingPath(string path, string fileGroup = "")
        {
            var fileExtension = Path.GetExtension(path);
            var file = new SFile()
            {
               Name = Path.GetFileNameWithoutExtension(path),
               Extension = fileExtension,
               FileType = TypesProvider.GetFileTypeOrUndefined(fileExtension),
               Path =  path,
               Group = fileGroup
            };
            OnUploadedFile?.Invoke(file);
            return file;
        }

        public Task<ICollection<IFile>> GetFilesAsync(string path, IUploadFilesOptions uploadFilesOptions, CancellationToken token = default)
        {
            return Task.FromResult(GetFiles(path, uploadFilesOptions));
        }
    }
}
