﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Novo.Docx.Tools.Help {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class LocalizableStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LocalizableStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Novo.Docx.Cli.commands.ndocx_help.LocalizableStrings", typeof(LocalizableStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .NET CLI help utility.
        /// </summary>
        internal static string AppFullName {
            get {
                return ResourceManager.GetString("AppFullName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SDK command to launch online help for..
        /// </summary>
        internal static string CommandArgumentDescription {
            get {
                return ResourceManager.GetString("CommandArgumentDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to COMMAND_NAME.
        /// </summary>
        internal static string CommandArgumentName {
            get {
                return ResourceManager.GetString("CommandArgumentName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified command &apos;{0}&apos; is not a valid SDK command. Specify a valid SDK command. For more information, run dotnet help..
        /// </summary>
        internal static string CommandDoesNotExist {
            get {
                return ResourceManager.GetString("CommandDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NDocx commands.
        /// </summary>
        internal static string Commands {
            get {
                return ResourceManager.GetString("Commands", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Execute an ndocx commands..
        /// </summary>
        internal static string CommandsUsageDescription {
            get {
                return ResourceManager.GetString("CommandsUsageDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Populates a docx document using parameters specificed in a file..
        /// </summary>
        internal static string PopulateCommandDescription {
            get {
                return ResourceManager.GetString("PopulateCommandDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Run &apos;ndocx [command] --help&apos; for more information on a command..
        /// </summary>
        internal static string RunCommandHelpForMore {
            get {
                return ResourceManager.GetString("RunCommandHelpForMore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage:.
        /// </summary>
        internal static string Usage {
            get {
                return ResourceManager.GetString("Usage", resourceCulture);
            }
        }
    }
}
