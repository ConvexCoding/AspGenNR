﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AspGen.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int NumberOfDrawLensRays {
            get {
                return ((int)(this["NumberOfDrawLensRays"]));
            }
            set {
                this["NumberOfDrawLensRays"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public double DrawLensPreRayDistance {
            get {
                return ((double)(this["DrawLensPreRayDistance"]));
            }
            set {
                this["DrawLensPreRayDistance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DrawLensDrawCL {
            get {
                return ((bool)(this["DrawLensDrawCL"]));
            }
            set {
                this["DrawLensDrawCL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int InputRaySet {
            get {
                return ((int)(this["InputRaySet"]));
            }
            set {
                this["InputRaySet"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int ExtSrcNoofBaseRays {
            get {
                return ((int)(this["ExtSrcNoofBaseRays"]));
            }
            set {
                this["ExtSrcNoofBaseRays"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int ExtSrcNoofAngles {
            get {
                return ((int)(this["ExtSrcNoofAngles"]));
            }
            set {
                this["ExtSrcNoofAngles"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.001")]
        public double ExtScrBinSize {
            get {
                return ((double)(this["ExtScrBinSize"]));
            }
            set {
                this["ExtScrBinSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int NoOfDigitsSpotDiagLabels {
            get {
                return ((int)(this["NoOfDigitsSpotDiagLabels"]));
            }
            set {
                this["NoOfDigitsSpotDiagLabels"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public int WFE2DMapSize {
            get {
                return ((int)(this["WFE2DMapSize"]));
            }
            set {
                this["WFE2DMapSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1024")]
        public int PSFTotalGridSize {
            get {
                return ((int)(this["PSFTotalGridSize"]));
            }
            set {
                this["PSFTotalGridSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int PSFBeamGridSize {
            get {
                return ((int)(this["PSFBeamGridSize"]));
            }
            set {
                this["PSFBeamGridSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int SpotDiagramSpokesInc {
            get {
                return ((int)(this["SpotDiagramSpokesInc"]));
            }
            set {
                this["SpotDiagramSpokesInc"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int SpotDiagramRadialSamples {
            get {
                return ((int)(this["SpotDiagramRadialSamples"]));
            }
            set {
                this["SpotDiagramRadialSamples"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ExtScrVerticalAverage1D {
            get {
                return ((int)(this["ExtScrVerticalAverage1D"]));
            }
            set {
                this["ExtScrVerticalAverage1D"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("21")]
        public int TransversDataPoints {
            get {
                return ((int)(this["TransversDataPoints"]));
            }
            set {
                this["TransversDataPoints"] = value;
            }
        }
    }
}