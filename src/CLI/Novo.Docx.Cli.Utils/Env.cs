﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Novo.Docx.Cli.Utils
{
    public static class Env
    {
        private static readonly IEnvironmentProvider _environment = new EnvironmentProvider();

        public static IEnumerable<string> ExecutableExtensions
        {
            get
            {
                return _environment.ExecutableExtensions;
            }
        }

        public static string GetCommandPath(string commandName, params string[] extensions)
        {
            return _environment.GetCommandPath(commandName, extensions);
        }

        public static string GetCommandPathFromRootPath(string rootPath, string commandName, params string[] extensions)
        {
            return _environment.GetCommandPathFromRootPath(rootPath, commandName, extensions);
        }

        public static string GetCommandPathFromRootPath(string rootPath, string commandName, IEnumerable<string> extensions)
        {
            return _environment.GetCommandPathFromRootPath(rootPath, commandName, extensions);
        }

        public static bool GetEnvironmentVariableAsBool(string name, bool defaultValue = false)
        {
            return _environment.GetEnvironmentVariableAsBool(name, defaultValue);
        }

        public static string GetEnvironmentVariable(string name)
        {
            return _environment.GetEnvironmentVariable(name);
        }
    }
}
