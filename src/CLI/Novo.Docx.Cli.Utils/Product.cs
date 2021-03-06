// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Novo.Docx.Cli.Utils
{
    public class Product
    {
        public static string LongName => LocalizableStrings.NDocxInfo;
        public static readonly string Version = GetProductVersion();

        private static string GetProductVersion()
        {
            NDocxVersionFile versionFile = NDocxFiles.VersionFileObject;
            return versionFile.BuildNumber ?? string.Empty;
        }
    }
}
