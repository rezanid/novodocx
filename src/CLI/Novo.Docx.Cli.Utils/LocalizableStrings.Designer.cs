﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Novo.Docx.Cli.Utils
{
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Novo.Docx.Cli.Utils.LocalizableStrings", typeof(LocalizableStrings).Assembly);
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
        ///   Looks up a localized string similar to Novo Docx.
        /// </summary>
        internal static string NDocxInfo {
            get {
                return ResourceManager.GetString("NDocxInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Novo Docx:.
        /// </summary>
        internal static string NDocxInfoLabel {
            get {
                return ResourceManager.GetString("NDocxInfoLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Runtime Environment:.
        /// </summary>
        internal static string NDocxRuntimeInfoLabel {
            get {
                return ResourceManager.GetString("NDocxRuntimeInfoLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Process ID: {0}.
        /// </summary>
        internal static string ProcessId {
            get {
                return ResourceManager.GetString("ProcessId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for debugger to attach. Press ENTER to continue.
        /// </summary>
        internal static string WaitingForDebuggerToAttach {
            get {
                return ResourceManager.GetString("WaitingForDebuggerToAttach", resourceCulture);
            }
        }
    }
}