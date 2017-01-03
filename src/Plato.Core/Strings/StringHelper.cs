// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Plato.Core.Strings
{
    /// <summary>
    /// Summary description for StringHelper.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Ifs the null or empty use default.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string IfNullOrEmptyUseDefault(string str, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(str) ? defaultValue : str;
        }

        /// <summary>
        /// Fulls the trim.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string FullTrim(string str)
        {
            return str?.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Removes the multiple spaces.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string RemoveMulitpleSpaces(string str)
        {
            var options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            return regex.Replace(str, " ");
        }

        /// <summary>
        /// Determines whether [is URL format valid] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsUrlFormatValid(string url)
        {
            url = url.Trim().ToLower();

            if (url.IndexOf("http://") > 0 || url.IndexOf("https://") > 0 || url.IndexOf("ftp://") > 0)
            {
                return false;
            }

            if (url.IndexOf("http://") == -1 && url.IndexOf("https://") == -1 && url.IndexOf("ftp://") == -1)
            {
                url = string.Format("http://{0}", url);
            }

            return new Regex(@"(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?").IsMatch(url);
        }

        /// <summary>
        /// Determines whether [is valid ip address] [the specified address].
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string address)
        {
            return new Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b").IsMatch(address);
        }

        /// <summary>
        /// Check whether the provided string is a strong password.
        /// The string must contain at least one uppercase, one lowercase,
        /// one numeral with no white spaces.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="bSpecialChars">If true, then one special character must exist.</param>
        /// <param name="minSize">Min size of password allowed.</param>
        /// <param name="maxSize">Max size of password allowed.</param>
        /// <returns>
        /// True if the password is a strong password, false otherwise.
        /// </returns>
        public static bool IsStrongPassword(string password, bool bSpecialChars, int minSize, int maxSize)
        {
            if (password.IndexOf(" ") > 0)
            {
                return false;
            }

            var ex1 = @"^ " + @"(?=.*\d)" + @"(?=.*[a-z])" + @"(?=.*[A-Z])" + @"{0}" + @".{{{1},{2}}}" + @"$";

            string specialChar;
            if (bSpecialChars)
            {
                specialChar = @"(?=.*[\-\+\?\*\$\[\]\^\.\(\)\|`~!@#%&_ ={}:;  ',/])";
            }
            else
            {
                specialChar = string.Empty;
            }

            var pattern = string.Format(ex1, specialChar, minSize, maxSize);
            var regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            var m = regex.Match(password);

            return m.Success;
        }

        /// <summary>
        /// To the title case.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ToTitleCase(CultureInfo culture, string str)
        {
            return culture.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// To the title case.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ToTitleCase(string str)
        {
            return ToTitleCase(CultureInfo.CurrentCulture, str);
        }

        /// <summary>
        /// Splits the word phrase by.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="maxPhraseSize">Maximum size of the phrase.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitWordPhraseBy(string phrase, int maxPhraseSize)
        {
            const char space = ' ';
            var phrases = new List<string>();
            var remaining = phrase.Length;
            var next = 0;

            while (remaining > 0)
            {
                var maxSize = Math.Min(remaining, maxPhraseSize);
                var nextText = phrase.Substring(next, maxSize);

                if (maxSize < remaining && nextText[nextText.Length - 1] != space)
                {
                    var testMaxSize = nextText.LastIndexOf(space) + 1;
                    if (testMaxSize > 0)
                    {
                        nextText = phrase.Substring(next, testMaxSize);
                        maxSize = testMaxSize;
                    }
                }

                phrases.Add(nextText);

                remaining -= maxSize;
                next += maxSize;
            }

            return phrases;
        }
    }
}
