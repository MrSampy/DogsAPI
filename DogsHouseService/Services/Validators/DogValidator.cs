using Services.Abstractions.DTOs;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class DogValidator : IDogValidator
    {
        public bool IsValid(DogDTO dog)
        {
            return !string.IsNullOrEmpty(dog.Name) &&
                   IsValidColor(dog.Color) &&
                   dog.TailLength > 0 &&
                   dog.Weight > 0;
        }

        public bool IsValidColor(string color)
        {
            var amountOfCharacres = color.Where(x => x == '&').Count();
            var colors = color.Split('&');
            if (colors == null || amountOfCharacres != (colors.Length - 1) || colors.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                return false;
            }
            return true;
        }
    }

}
