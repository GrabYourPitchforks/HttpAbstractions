// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Framework.Internal;
using Microsoft.Framework.WebEncoders;
using Microsoft.Security.Application;

namespace Microsoft.Framework.DependencyInjection
{
    internal sealed class AntiXssEncoder : IHtmlEncoder, IUrlEncoder
    {
        public string HtmlEncode(string value)
        {
            return (String.IsNullOrEmpty(value)) ? value : Encoder.HtmlEncode(value);
        }

        public void HtmlEncode([NotNull] string value, int startIndex, int charCount, [NotNull] TextWriter output)
        {
            string encoded = Encoder.HtmlEncode(value.Substring(startIndex, charCount));
            output.Write(encoded);
        }

        public void HtmlEncode([NotNull] char[] value, int startIndex, int charCount, [NotNull] TextWriter output)
        {
            string input = new string(value, startIndex, charCount);
            string encoded = Encoder.HtmlEncode(input);
            output.Write(encoded);
        }
        
        public string UrlEncode(string value)
        {
            return (String.IsNullOrEmpty(value)) ? value : Encoder.UrlEncode(value);
        }

        public void UrlEncode([NotNull] string value, int startIndex, int charCount, [NotNull] TextWriter output)
        {
            string encoded = Encoder.UrlEncode(value.Substring(startIndex, charCount));
            output.Write(encoded);
        }

        public void UrlEncode([NotNull] char[] value, int startIndex, int charCount, [NotNull] TextWriter output)
        {
            string input = new string(value, startIndex, charCount);
            string encoded = Encoder.UrlEncode(input);
            output.Write(encoded);
        }
    }
}
