using System.Text;
using System.Text.RegularExpressions;

namespace Roxana.Application.Core.Extensions
{
    public static class StringExtensions
    {
        public static string CommonPrefix(this string str, params string[] more)
        {
            var prefixLength = str.TakeWhile((c, i) => more.All(s => i < s.Length && s[i] == c))
                .Count();

            return str.Substring(0, prefixLength);
        }
        
        public static string ToKebabCase(this string input)
        {
            var str = Regex.Replace(input.Replace(" ", "-"), 
                "([A-Z])", "-$0", RegexOptions.Compiled);
            if(str.StartsWith("-"))
                str = str.Substring(1);
            return str;
        }
        
        public static string ToSnakeCase(this string input)
        {
            var str = Regex.Replace(input.Replace(" ", "_"), "([A-Z])", "_$0", RegexOptions.Compiled);
            if(str.StartsWith("_"))
                str = str.Substring(1);
            return str;
        }
        
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            char[] a = input.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ConvertDigitsToLatin(this string input)
        {
            var sb = new StringBuilder();
            foreach (var t in input)
            {
                switch (t)
                {
                    //Persian digits
                    case '\u06f0':
                        sb.Append('0');
                        break;
                    case '\u06f1':
                        sb.Append('1');
                        break;
                    case '\u06f2':
                        sb.Append('2');
                        break;
                    case '\u06f3':
                        sb.Append('3');
                        break;
                    case '\u06f4':
                        sb.Append('4');
                        break;
                    case '\u06f5':
                        sb.Append('5');
                        break;
                    case '\u06f6':
                        sb.Append('6');
                        break;
                    case '\u06f7':
                        sb.Append('7');
                        break;
                    case '\u06f8':
                        sb.Append('8');
                        break;
                    case '\u06f9':
                        sb.Append('9');
                        break;

                    //Arabic digits    
                    case '\u0660':
                        sb.Append('0');
                        break;
                    case '\u0661':
                        sb.Append('1');
                        break;
                    case '\u0662':
                        sb.Append('2');
                        break;
                    case '\u0663':
                        sb.Append('3');
                        break;
                    case '\u0664':
                        sb.Append('4');
                        break;
                    case '\u0665':
                        sb.Append('5');
                        break;
                    case '\u0666':
                        sb.Append('6');
                        break;
                    case '\u0667':
                        sb.Append('7');
                        break;
                    case '\u0668':
                        sb.Append('8');
                        break;
                    case '\u0669':
                        sb.Append('9');
                        break;
                    default:
                        sb.Append(t);
                        break;
                }
            }

            return sb.ToString();
        }
        
        public static string RemoveSpecialCharacters(this string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }
    }
}