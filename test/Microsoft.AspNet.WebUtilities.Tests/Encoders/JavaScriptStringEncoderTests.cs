﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Xunit;

namespace Microsoft.AspNet.WebUtilities.Encoders
{
    public class JavaScriptStringEncoderTests
    {
        [Fact]
        public void Ctor_WithCustomFilters()
        {
            // Arrange
            CustomCodePointFilter filter1 = new CustomCodePointFilter('a', 'b');
            CustomCodePointFilter filter2 = new CustomCodePointFilter('\0', '&', '\uFFFF', 'd');
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder(filter1, filter2);

            // Act & assert
            Assert.Equal("a", encoder.JavaScriptStringEncode("a"));
            Assert.Equal("b", encoder.JavaScriptStringEncode("b"));
            Assert.Equal(@"\u0063", encoder.JavaScriptStringEncode("c"));
            Assert.Equal("d", encoder.JavaScriptStringEncode("d"));
            Assert.Equal(@"\u0000", encoder.JavaScriptStringEncode("\0")); // we still always encode control chars
            Assert.Equal(@"\u0026", encoder.JavaScriptStringEncode("&")); // we still always encode HTML-special chars
            Assert.Equal(@"\uFFFF", encoder.JavaScriptStringEncode("\uFFFF")); // we still always encode non-chars and other forbidden chars
        }

        [Fact]
        public void Ctor_WithEmptyParameters_DefaultsToNothing()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder(new ICodePointFilter[0]);

            // Act & assert
            Assert.Equal(@"\u0061", encoder.JavaScriptStringEncode("a"));
            Assert.Equal(@"\u00E9", encoder.JavaScriptStringEncode("\u00E9" /* LATIN SMALL LETTER E WITH ACUTE */));
            Assert.Equal(@"\u2601", encoder.JavaScriptStringEncode("\u2601" /* CLOUD */));
        }

        [Fact]
        public void Ctor_WithMultipleParameters_AllowsBitwiseOrOfCodePoints()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder(CodePointFilters.Latin1Supplement, CodePointFilters.MiscellaneousSymbols);

            // Act & assert
            Assert.Equal(@"\u0061", encoder.JavaScriptStringEncode("a"));
            Assert.Equal("\u00E9", encoder.JavaScriptStringEncode("\u00E9" /* LATIN SMALL LETTER E WITH ACUTE */));
            Assert.Equal("\u2601", encoder.JavaScriptStringEncode("\u2601" /* CLOUD */));
        }

        [Fact]
        public void Ctor_WithNoParameters_DefaultsToBasicLatin()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder();

            // Act & assert
            Assert.Equal("a", encoder.JavaScriptStringEncode("a"));
            Assert.Equal(@"\u00E9", encoder.JavaScriptStringEncode("\u00E9" /* LATIN SMALL LETTER E WITH ACUTE */));
            Assert.Equal(@"\u2601", encoder.JavaScriptStringEncode("\u2601" /* CLOUD */));
        }

        [Fact]
        public void Default_EquivalentToBasicLatin()
        {
            // Arrange
            JavaScriptStringEncoder controlEncoder = new JavaScriptStringEncoder(CodePointFilters.BasicLatin);
            JavaScriptStringEncoder testEncoder = JavaScriptStringEncoder.Default;

            // Act & assert
            for (int i = 0; i <= Char.MaxValue; i++)
            {
                if (!IsSurrogateCodePoint(i))
                {
                    string input = new String((char)i, 1);
                    Assert.Equal(controlEncoder.JavaScriptStringEncode(input), testEncoder.JavaScriptStringEncode(input));
                }
            }
        }

        [Fact]
        public void Default_ReturnsSingletonInstance()
        {
            // Act
            JavaScriptStringEncoder encoder1 = JavaScriptStringEncoder.Default;
            JavaScriptStringEncoder encoder2 = JavaScriptStringEncoder.Default;

            // Assert
            Assert.Same(encoder1, encoder2);
        }

        [Fact]
        public void JavaScriptStringEncode_AllRangesAllowed_StillEncodesForbiddenChars_Simple()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder(CodePointFilters.All);
            const string input = "Hello <>&\'\"+/\\\b\f\n\r\t there!";
            const string expected = @"Hello \u003C\u003E\u0026\u0027\u0022\u002B\/\\\b\f\n\r\t there!";

            // Act & assert
            Assert.Equal(expected, encoder.JavaScriptStringEncode(input));
        }

        [Fact]
        public void JavaScriptStringEncode_AllRangesAllowed_StillEncodesForbiddenChars_Extended()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder(CodePointFilters.All);

            // Act & assert - BMP chars
            for (int i = 0; i <= 0xFFFF; i++)
            {
                string input = new String((char)i, 1);
                string expected;
                if (IsSurrogateCodePoint(i))
                {
                    expected = "\uFFFD"; // unpaired surrogate -> Unicode replacement char
                }
                else
                {
                    if (input == "\b") { expected = @"\b"; }
                    else if (input == "\t") { expected = @"\t"; }
                    else if (input == "\n") { expected = @"\n"; }
                    else if (input == "\f") { expected = @"\f"; }
                    else if (input == "\r") { expected = @"\r"; }
                    else if (input == "\\") { expected = @"\\"; }
                    else if (input == "/") { expected = @"\/"; }
                    else
                    {
                        bool mustEncode = false;
                        switch (i)
                        {
                            case '<':
                            case '>':
                            case '&':
                            case '\"':
                            case '\'':
                            case '+':
                                mustEncode = true;
                                break;
                        }

                        if (i <= 0x001F || (0x007F <= i && i <= 0x9F))
                        {
                            mustEncode = true; // control char
                        }
                        else if (!UnicodeHelpers.IsCharacterDefined((char)i))
                        {
                            mustEncode = true; // undefined (or otherwise disallowed) char
                        }

                        if (mustEncode)
                        {
                            expected = String.Format(CultureInfo.InvariantCulture, @"\u{0:X4}", i);
                        }
                        else
                        {
                            expected = input; // no encoding
                        }
                    }
                }

                string retVal = encoder.JavaScriptStringEncode(input);
                Assert.Equal(expected, retVal);
            }

            // Act & assert - astral chars
            for (int i = 0x10000; i <= 0x10FFFF; i++)
            {
                string input = Char.ConvertFromUtf32(i);
                string expected = String.Format(CultureInfo.InvariantCulture, @"\u{0:X4}\u{1:X4}", (uint)input[0], (uint)input[1]);
                string retVal = encoder.JavaScriptStringEncode(input);
                Assert.Equal(expected, retVal);
            }
        }

        [Fact]
        public void JavaScriptStringEncode_BadSurrogates_ReturnsUnicodeReplacementChar()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder(CodePointFilters.All); // allow all codepoints

            // "a<unpaired leading>b<unpaired trailing>c<trailing before leading>d<unpaired trailing><valid>e<high at end of string>"
            const string input = "a\uD800b\uDFFFc\uDFFF\uD800d\uDFFF\uD800\uDFFFe\uD800";
            const string expected = "a\uFFFDb\uFFFDc\uFFFD\uFFFDd\uFFFD\\uD800\\uDFFFe\uFFFD"; // 'D800' 'DFFF' was preserved since it's valid

            // Act
            string retVal = encoder.JavaScriptStringEncode(input);

            // Assert
            Assert.Equal(expected, retVal);
        }

        [Fact]
        public void JavaScriptStringEncode_EmptyStringInput_ReturnsEmptyString()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder();

            // Act & assert
            Assert.Equal("", encoder.JavaScriptStringEncode(""));
        }

        [Fact]
        public void JavaScriptStringEncode_InputDoesNotRequireEncoding_ReturnsOriginalStringInstance()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder();
            string input = "Hello, there!";

            // Act & assert
            Assert.Same(input, encoder.JavaScriptStringEncode(input));
        }

        [Fact]
        public void JavaScriptStringEncode_NullInput_ReturnsNull()
        {
            // Arrange
            JavaScriptStringEncoder encoder = new JavaScriptStringEncoder();

            // Act & assert
            Assert.Null(encoder.JavaScriptStringEncode(null));
        }

        [Fact]
        public void JavaScriptStringEncode_WithCharsRequiringEncodingAtBeginning()
        {
            Assert.Equal(@"\u0026Hello, there!", new JavaScriptStringEncoder().JavaScriptStringEncode("&Hello, there!"));
        }

        [Fact]
        public void JavaScriptStringEncode_WithCharsRequiringEncodingAtEnd()
        {
            Assert.Equal(@"Hello, there!\u0026", new JavaScriptStringEncoder().JavaScriptStringEncode("Hello, there!&"));
        }

        [Fact]
        public void JavaScriptStringEncode_WithCharsRequiringEncodingInMiddle()
        {
            Assert.Equal(@"Hello, \u0026there!", new JavaScriptStringEncoder().JavaScriptStringEncode("Hello, &there!"));
        }

        [Fact]
        public void JavaScriptStringEncode_WithCharsRequiringEncodingInterspersed()
        {
            Assert.Equal(@"Hello, \u003Cthere\u003E!", new JavaScriptStringEncoder().JavaScriptStringEncode("Hello, <there>!"));
        }

        private static bool IsSurrogateCodePoint(int codePoint)
        {
            return (0xD800 <= codePoint && codePoint <= 0xDFFF);
        }
    }
}
