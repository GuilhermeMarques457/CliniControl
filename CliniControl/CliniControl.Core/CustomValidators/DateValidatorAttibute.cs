using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.CustomValidators
{
    public class DateValidatorAttibute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue.Date >= DateTime.Now.Date)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
