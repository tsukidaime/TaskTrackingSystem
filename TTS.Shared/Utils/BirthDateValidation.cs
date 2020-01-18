using System;
using System.ComponentModel.DataAnnotations;
using Faisalman.AgeCalc;

namespace TTS.Shared.Utils
{
    public class BirthDateValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var bd = (DateTime) value;
            var calc = new Age(bd,DateTime.Now);
            return bd < DateTime.Now && calc.Years < 150;
        }
    }
}