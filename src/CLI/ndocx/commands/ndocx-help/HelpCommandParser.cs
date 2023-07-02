// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Novo.Docx.Cli;

namespace Novo.Docx.Tools.Help
{
    internal static class HelpCommandParser
    {
        public static readonly string DocsLink = "https://blahblah.github.com/ndocx-help";

        public static readonly Argument<string> Argument = new(LocalizableStrings.CommandArgumentName)
        {
            Description = LocalizableStrings.CommandArgumentDescription,
            Arity = ArgumentArity.ZeroOrOne
        };

        private static readonly Command Command = ConstructCommand();

        public static Command GetCommand()
        {
            return Command;
        }

        private static Command ConstructCommand()
        {
            var command = new DocumentedCommand("help", DocsLink, LocalizableStrings.AppFullName);

            command.AddArgument(Argument);

            command.SetHandler(HelpCommand.Run);

            return command;
        }
    }
}

