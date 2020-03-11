namespace EnvironmentalInspectionManagement.Utilities
{
    #region Usings
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Objects;
    #endregion

    public static class LanguageExtensions
    {
        #region Greek Characters Mapping
        private static readonly List<KeyValue> GrLettersMap = new List<KeyValue>
        {
            new KeyValue { Key = @"ά", Value = @"α" },
            new KeyValue { Key = @"έ", Value = @"ε" },
            new KeyValue { Key = @"ί", Value = @"ι" },
            new KeyValue { Key = @"ϊ", Value = @"ι" },
            new KeyValue { Key = @"ΐ", Value = @"ι" },
            new KeyValue { Key = @"ή", Value = @"η" },
            new KeyValue { Key = @"ύ", Value = @"υ" },
            new KeyValue { Key = @"ϋ", Value = @"υ" },
            new KeyValue { Key = @"ΰ", Value = @"υ" },
            new KeyValue { Key = @"ό", Value = @"ο" },
            new KeyValue { Key = @"ώ", Value = @"ω" },
            new KeyValue { Key = @"Ά", Value = @"Α" },
            new KeyValue { Key = @"Έ", Value = @"Ε" },
            new KeyValue { Key = @"Ί", Value = @"Ι" },
            new KeyValue { Key = @"Ϊ", Value = @"Ι" },
            new KeyValue { Key = @"Ή", Value = @"Η" },
            new KeyValue { Key = @"Ύ", Value = @"Υ" },
            new KeyValue { Key = @"Ϋ", Value = @"Υ" },
            new KeyValue { Key = @"Ό", Value = @"Ο" },
            new KeyValue { Key = @"Ώ", Value = @"Ω" },
        };
        #endregion

        public static string NoPunctuationGr(this string inStr)
        {
            var strBuilder = new StringBuilder();
            inStr.Select(x => x.ToString()).ToList().ForEach(x =>
            {
                var nonPunctuated = GrLettersMap
                    .FirstOrDefault(c => x.Equals(c.Key));

                if (nonPunctuated != null)
                    strBuilder.Append(nonPunctuated.Value);
                else strBuilder.Append(x);
            });

            return strBuilder.ToString();
        }
    }
}
