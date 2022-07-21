namespace Sparrow.Video.Abstractions.Enums
{
    public class UploadFileAction
    {
        private UploadFileAction(string actionName)
        {
            ActionName = actionName;
        }

        public string ActionName { get; }

        public static readonly UploadFileAction Exception = new(nameof(Exception));
        public static readonly UploadFileAction NoAction = new(nameof(NoAction));
        public static readonly UploadFileAction Skip = new(nameof(Skip));
    }
}
