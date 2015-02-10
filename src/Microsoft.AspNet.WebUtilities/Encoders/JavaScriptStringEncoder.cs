﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Microsoft.AspNet.WebUtilities.Encoders
{
    /// <summary>
    /// A class which can perform JavaScript string escaping given an allow list of characters which
    /// can be represented unescaped.
    /// </summary>
    /// <remarks>
    /// Once constructed, instances of this class are thread-safe for multiple callers.
    /// </remarks>
    public sealed class JavaScriptStringEncoder : IJavaScriptStringEncoder
    {
        // The default JavaScript string encoder (Basic Latin), instantiated on demand
        private static JavaScriptStringEncoder _defaultEncoder;

        // The inner encoder, responsible for the actual encoding routines
        private readonly JavaScriptStringUnicodeEncoder _innerUnicodeEncoder;

        /// <summary>
        /// Instantiates an encoder using the 'Basic Latin' code table as the allow list.
        /// </summary>
        public JavaScriptStringEncoder()
            : this(JavaScriptStringUnicodeEncoder.BasicLatin)
        {
        }

        /// <summary>
        /// Instantiates an encoder using a custom allow list of characters.
        /// </summary>
        public JavaScriptStringEncoder(params ICodePointFilter[] filters)
            : this(new JavaScriptStringUnicodeEncoder(filters))
        {
        }

        private JavaScriptStringEncoder(JavaScriptStringUnicodeEncoder innerEncoder)
        {
            Debug.Assert(innerEncoder != null);
            _innerUnicodeEncoder = innerEncoder;
        }

        /// <summary>
        /// A default instance of the JavaScriptStringEncoder, equivalent to allowing only
        /// the 'Basic Latin' character range.
        /// </summary>
        public static JavaScriptStringEncoder Default
        {
            get
            {
                JavaScriptStringEncoder defaultEncoder = Volatile.Read(ref _defaultEncoder);
                if (defaultEncoder == null)
                {
                    defaultEncoder = new JavaScriptStringEncoder();
                    Volatile.Write(ref _defaultEncoder, defaultEncoder);
                }
                return defaultEncoder;
            }
        }

        /// <summary>
        /// Everybody's favorite JavaScriptStringEncode routine.
        /// </summary>
        public string JavaScriptStringEncode(string value)
        {
            return _innerUnicodeEncoder.Encode(value);
        }

        private sealed class JavaScriptStringUnicodeEncoder : UnicodeEncoderBase
        {
            // A singleton instance of the basic latin encoder.
            private static JavaScriptStringUnicodeEncoder _basicLatinSingleton;

            // The worst case encoding is 6 output chars per input char: [input] U+FFFF -> [output] "\uFFFF"
            // We don't need to worry about astral code points since they're represented as encoded
            // surrogate pairs in the output.
            private const int MaxOutputCharsPerInputChar = 6;

            internal JavaScriptStringUnicodeEncoder(ICodePointFilter[] filters)
                : base(filters, MaxOutputCharsPerInputChar)
            {
                // The only interesting characters above and beyond what the base encoder
                // already covers are the solidus and reverse solidus.
                ForbidCharacter('\\');
                ForbidCharacter('/');
            }

            internal static JavaScriptStringUnicodeEncoder BasicLatin
            {
                get
                {
                    JavaScriptStringUnicodeEncoder encoder = Volatile.Read(ref _basicLatinSingleton);
                    if (encoder == null)
                    {
                        encoder = new JavaScriptStringUnicodeEncoder(new[] { CodePointFilters.BasicLatin });
                        Volatile.Write(ref _basicLatinSingleton, encoder);
                    }
                    return encoder;
                }
            }

            // Writes a scalar value as a JavaScript-escaped character (or sequence of characters).
            // See ECMA-262, Sec. 7.8.4, and ECMA-404, Sec. 9
            // http://www.ecma-international.org/ecma-262/5.1/#sec-7.8.4
            // http://www.ecma-international.org/publications/files/ECMA-ST/ECMA-404.pdf
            protected override void WriteEncodedScalar(StringBuilder builder, uint value)
            {
                // ECMA-262 allows encoding U+000B as "\v", but ECMA-404 does not.
                // Both ECMA-262 and ECMA-404 allow encoding U+002F SOLIDUS as "\/".
                // (In ECMA-262 this character is a NonEscape character.)
                // HTML-specific characters (including apostrophe and quotes) will
                // be written out as numeric entities for defense-in-depth.
                // See UnicodeEncoderBase ctor comments for more info.

                if (value == (uint)'\b') { builder.Append(@"\b"); }
                else if (value == (uint)'\t') { builder.Append(@"\t"); }
                else if (value == (uint)'\n') { builder.Append(@"\n"); }
                else if (value == (uint)'\f') { builder.Append(@"\f"); }
                else if (value == (uint)'\r') { builder.Append(@"\r"); }
                else if (value == (uint)'/') { builder.Append(@"\/"); }
                else if (value == (uint)'\\') { builder.Append(@"\\"); }
                else { WriteEncodedScalarAsNumericEntity(builder, value); }
            }

            // Writes a scalar value as an JavaScript-escaped character (or sequence of characters).
            private static void WriteEncodedScalarAsNumericEntity(StringBuilder builder, uint value)
            {
                if (UnicodeHelpers.IsSupplementaryCodePoint((int)value))
                {
                    // Convert this back to UTF-16 and write out both characters.
                    char leadingSurrogate, trailingSurrogate;
                    UnicodeHelpers.GetUtf16SurrogatePairFromAstralScalarValue((int)value, out leadingSurrogate, out trailingSurrogate);
                    WriteEncodedSingleCharacter(builder, leadingSurrogate);
                    WriteEncodedSingleCharacter(builder, trailingSurrogate);
                }
                else
                {
                    // This is only a single character.
                    WriteEncodedSingleCharacter(builder, value);
                }
            }

            // Writes an encoded scalar value (in the BMP) as a JavaScript-escaped character.
            private static void WriteEncodedSingleCharacter(StringBuilder builder, uint value)
            {
                Debug.Assert(!UnicodeHelpers.IsSupplementaryCodePoint((int)value), "The incoming value should've been in the BMP.");

                // Encode this as 6 chars "\uFFFF".
                builder.Append('\\');
                builder.Append('u');
                builder.Append(HexUtil.IntToChar(value >> 12));
                builder.Append(HexUtil.IntToChar((value >> 8) & 0xFU));
                builder.Append(HexUtil.IntToChar((value >> 4) & 0xFU));
                builder.Append(HexUtil.IntToChar(value & 0xFU));
            }
        }
    }
}
