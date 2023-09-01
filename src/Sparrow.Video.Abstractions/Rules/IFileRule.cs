﻿namespace Sparrow.Video.Abstractions.Rules;

public interface IFileRule : IProcessingRule
{
    bool IsApplied { get; }
    void Applied();
    new IFileRule Clone();
}