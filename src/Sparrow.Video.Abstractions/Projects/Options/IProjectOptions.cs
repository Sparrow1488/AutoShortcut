﻿using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Projects.Options
{
    public interface IProjectOptions
    {
        IFilesStructure Structure { get; }
        IProjectOptions StructureBy(IFilesStructure structure);
    }
}
