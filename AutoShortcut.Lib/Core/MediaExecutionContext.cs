using ExecutionContext = AutoShortcut.Lib.Contracts.Core.ExecutionContext;

namespace AutoShortcut.Lib.Core;

public record MediaExecutionContext(
    MediaStoreType StoreType,
    string FileName,
    bool InsertSavePath = true
) : ExecutionContext;
