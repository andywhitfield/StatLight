﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StatLight.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("StatLight.Core.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;access-policy&gt;
        ///	&lt;cross-domain-access&gt;
        ///		&lt;policy&gt;
        ///			&lt;allow-from http-request-headers=&quot;*&quot;&gt;
        ///				&lt;domain uri=&quot;*&quot;/&gt;
        ///			&lt;/allow-from&gt;
        ///			&lt;grant-to&gt;
        ///				&lt;resource path=&quot;/&quot; include-subpaths=&quot;true&quot;/&gt;
        ///			&lt;/grant-to&gt;
        ///		&lt;/policy&gt;
        ///	&lt;/cross-domain-access&gt;
        ///&lt;/access-policy&gt;
        ///.
        /// </summary>
        public static string ClientAccessPolocy {
            get {
                return ResourceManager.GetString("ClientAccessPolocy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;cross-domain-policy&gt;
        ///	&lt;allow-access-from domain=&quot;*&quot; /&gt;
        ///&lt;/cross-domain-policy&gt;
        ///.
        /// </summary>
        public static string CrossDomain {
            get {
                return ResourceManager.GetString("CrossDomain", resourceCulture);
            }
        }
        
        public static System.Drawing.Icon FavIcon {
            get {
                object obj = ResourceManager.GetObject("FavIcon", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot; &gt;
        ///&lt;!-- saved from url=(0014)about:internet --&gt;
        ///&lt;head&gt;
        ///    &lt;title&gt;StatLight.IntegrationTests.Silverlight.MSTest&lt;/title&gt;
        ///    &lt;style type=&quot;text/css&quot;&gt;
        ///    html, body {
        ///	    height: 100%;
        ///	    overflow: auto;
        ///    }
        ///    body {
        ///	    padding: 0;
        ///	    margin: 0;
        ///    }
        ///    #silverlightControlHost {
        ///	    height: 100%;
        ///	    text-align:center [rest of string was truncated]&quot;;.
        /// </summary>
        public static string TestPage {
            get {
                return ResourceManager.GetString("TestPage", resourceCulture);
            }
        }
    }
}
