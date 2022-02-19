using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspGen
{
    /// Note July 2021 - this class was added to the Asphere Design program without modification.
    /// 
    /// <summary>
    /// Class WithIn can be used to verify that a given value is within certain boundaries.
    /// This class will accept objects that can be casted to int, double, bool.
    /// The class can easily be expanded to include other data types.
    /// 
    /// WithIn class is geared towards validating User Settings found in the Properties Settings,
    /// </summary>
    public class WithIn
    {
        public string SettingStr { get; set; }
        public object Lower { get; set; }
        public object Upper { get; set; }
        public object Default { get; set; }
        public object Value { get; set; }
        public Type SType { get; set; }

        public WithIn(string _setstr, object _lower, object _upper, object _def)
        {
            this.SettingStr = _setstr;
            this.Lower = _lower;
            this.Upper = _upper;
            this.Default = _def;
            this.SType = _lower.GetType();
        }

        public bool IsWithIn(object value)
        {
            if ((Lower.GetType()).Equals(typeof(int)))
            {
                int ivalue;
                if ( (int.TryParse(value.ToString(), out ivalue)) && 
                     (ivalue >= (int)Lower && ivalue <= (int)Upper) )
                {
                    this.Value = ivalue;
                    return true;
                }
            }

            if ((Lower.GetType()).Equals(typeof(double)))
            {
                double xvalue;
                if ( (double.TryParse(value.ToString(), out xvalue)) &&
                     (xvalue >= (double)Lower && xvalue <= (double)Upper) )
                {
                    this.Value = xvalue;
                    return true;
                }
            }

            if (Lower.GetType().Equals(typeof(bool)))
            {
                bool strvalue;
                if ( (bool.TryParse(value.ToString(), out strvalue) ) && 
                     ( (strvalue == true) || (strvalue == false)) )
                {
                    this.Value = strvalue;
                    return true;
                }
            }

            this.Value = this.Default;
            return false;
        }

    }

    public class BackUpSetting
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }

        public BackUpSetting(string name, string dtype, string val)
        {
            this.Name = name;
            this.DataType = dtype;
            this.Value = val;
        }
    }

}
