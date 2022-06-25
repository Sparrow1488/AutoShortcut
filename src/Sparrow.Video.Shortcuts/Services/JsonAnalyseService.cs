using Newtonsoft.Json.Linq;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Primitives;
using System.Globalization;

namespace Sparrow.Video.Shortcuts.Services
{
    public class JsonAnalyseService : IJsonFileAnalyseService
    {
        public JsonAnalyseService(IFileTypesProvider typesProvider)
        {
            _typesProvider = typesProvider;
        }

        private readonly IFileTypesProvider _typesProvider;

        public IFileAnalyse Analyse(string variables) =>
            AnalyseByJson(variables);

        public IFileAnalyse AnalyseByJson(string jsonAnalyse)
        {
            var fileAnalyse = new FileAnalyse();
            var jObject = JObject.Parse(jsonAnalyse);
            var streamObjects = jObject["streams"].AsEnumerable();
            foreach (var streamObject in streamObjects)
            {
                var codecType = streamObject["codec_type"].Value<string>();
                var lowerCodeType = codecType.ToLower();
                if (lowerCodeType == "video")
                    fileAnalyse.StreamAnalyses.Add(streamObject.ToObject<VideosStreamAnalyse>());
                if (lowerCodeType == "audio")
                    fileAnalyse.StreamAnalyses.Add(streamObject.ToObject<StreamAnalyse>());
            }
            fileAnalyse.Format = GetFormatByJObject(jObject);
            fileAnalyse.FileType = GetFileTypeByAnalyse(fileAnalyse);
            return fileAnalyse;
        }

        private IFileFormat GetFormatByJObject(JObject jsonObject)
        {
            var jFormat = jsonObject["format"];
            var fileFormat = jFormat.ToObject<FileFormat>();
            return fileFormat ?? FileFormat.Default;
        }

        private string GetFileTypeByAnalyse(IFileAnalyse analyse)
        {
            if (analyse.StreamAnalyses.Any(x => x.CodecType.ToLower() == "video"))
            {
                return FileType.Video;
            }
            if (analyse.StreamAnalyses.Any(x => x.CodecType.ToLower() == "audio"))
            {
                return FileType.Audio;
            }
            return FileType.Undefined;
        }
    }
}
