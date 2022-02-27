// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using Novo.Docx.Cli.Utils;

namespace Novo.Docx.Cli
{
    internal static class NDocxFiles
    {
        private static string SdkRootFolder => Path.Combine(typeof(NDocxFiles).GetTypeInfo().Assembly.Location, "..");

        private static Lazy<NDocxVersionFile> s_versionFileObject =
            new(() => new NDocxVersionFile(VersionFile));

        /// <summary>
        /// NDocx ships with a .version file that stores the commit information and SDK version
        /// </summary>
        public static string VersionFile => Path.GetFullPath(Path.Combine(SdkRootFolder, ".version"));

        internal static NDocxVersionFile VersionFileObject
        {
            get { return s_versionFileObject.Value; }
        }
    }
}
