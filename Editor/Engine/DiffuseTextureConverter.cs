using System;
using System.ComponentModel;

namespace Editor.Engine
{
    // Provides texture options for the PropertyGrid dropdown
    internal class DiffuseTextureConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // Enables a list of predefined values
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // Allows only values from dropdown
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "Grass", "HeightMap", "Metal" }); // Returns the 3 texture options
        }
    }
}