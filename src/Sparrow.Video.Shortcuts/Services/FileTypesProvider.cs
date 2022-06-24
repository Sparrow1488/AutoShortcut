using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Exceptions;

namespace Sparrow.Video.Shortcuts.Services
{
    public class FileTypesProvider : IFileTypesProvider
    {
        public FileTypesProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Get <see cref="FileType"/> variable by <paramref name="extensions"/>
        /// </summary>
        /// <param name="extensions">File extension</param>
        /// <returns>Variable in <see cref="FileType"/></returns>
        /// <exception cref="FileTypeIdentifyException">Not identified</exception>
        public string GetFileType(string extensions)
        {
            var fileType = GetFileTypeOrUndefined(extensions);
            return fileType == FileType.Undefined 
                ? throw new FileTypeIdentifyException(
                    $"Not identified type of file by {nameof(extensions)} {extensions}") 
                : fileType;
        }

        /// <summary>
        ///     Get <see cref="FileType"/> variable by <paramref name="extensions"/>
        /// </summary>
        /// <param name="extensions">File extension</param>
        /// <returns>Varialbe in <see cref="FileType"/> or <see cref="FileType.Undefined"/></returns>
        public string GetFileTypeOrUndefined(string extensions)
        {
            string fileTypeResult = FileType.Undefined;
            var fileTypes = GetFileTypesSection();
            foreach (var typeSection in fileTypes)
            {
                var typeExtensions = typeSection.GetRequiredSection("Extensions");
                var extensionsValues = typeExtensions.Get<string[]>();
                if (extensionsValues.Contains(extensions))
                {
                    fileTypeResult = typeSection.Key;
                    break;
                }
            }
            return fileTypeResult;
        }

        private IEnumerable<IConfigurationSection> GetFileTypesSection()
        {
            var fileTypesSection = _configuration.GetRequiredSection("Store:FileTypes");
            var fileTypes = fileTypesSection.GetChildren();
            return fileTypes;
        }
    }
}
