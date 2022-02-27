// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Novo.Docx.Cli.Utils
{
    public interface IReporter
    {
        void WriteLine(string message);

        void WriteLine();

        void Write(string message);
    }
}
