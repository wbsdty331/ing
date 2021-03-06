﻿using System.IO;
using System.Threading.Tasks;

namespace Sakuno.ING.Bootstrap
{
    public interface IPackageStorage
    {
        string[] SupportedTargetFrameworks { get; }

        Task StageAsync(string id, string version, Stream stream);

        void Remove(string id, string version);
    }
}
