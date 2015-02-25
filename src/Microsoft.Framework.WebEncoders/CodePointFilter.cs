// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Framework.Internal;

namespace Microsoft.Framework.WebEncoders
{
    /// <summary>
    /// Represents a filter which allows only certain Unicode code points through.
    /// </summary>
    public sealed class CodePointFilter : ICodePointFilter
    {
        private AllowedCharsBitmap _allowedCharsBitmap;

        public CodePointFilter()
        {
            _allowedCharsBitmap = new AllowedCharsBitmap();
            AllowBlock(UnicodeBlocks.BasicLatin);
        }

        public CodePointFilter([NotNull] ICodePointFilter other)
        {
            CodePointFilter otherAsCodePointFilter = other as CodePointFilter;
            if (otherAsCodePointFilter != null)
            {
                otherAsCodePointFilter._allowedCharsBitmap.CloneInto(out this._allowedCharsBitmap);
            }
            else
            {
                _allowedCharsBitmap = new AllowedCharsBitmap();
                AllowFilter(other);
            }
        }

        public CodePointFilter(params UnicodeBlock[] allowedBlocks)
        {
            _allowedCharsBitmap = new AllowedCharsBitmap();
            AllowBlocks(allowedBlocks);
        }

        public CodePointFilter AllowBlock([NotNull] UnicodeBlock block)
        {
            int firstCodePoint = block.FirstCodePoint;
            int blockSize = block.BlockSize;
            for (int i = 0; i < blockSize; i++)
            {
                _allowedCharsBitmap.AllowCharacter((char)(firstCodePoint + i));
            }
            return this;
        }

        public CodePointFilter AllowBlocks(params UnicodeBlock[] blocks)
        {
            if (blocks != null)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    AllowBlock(blocks[i]);
                }
            }
            return this;
        }

        public CodePointFilter AllowChar(char c)
        {
            _allowedCharsBitmap.AllowCharacter(c);
            return this;
        }

        public CodePointFilter AllowChars(params char[] chars)
        {
            if (chars != null)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    _allowedCharsBitmap.AllowCharacter(chars[i]);
                }
            }
            return this;
        }

        public CodePointFilter AllowChars([NotNull] string chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                _allowedCharsBitmap.AllowCharacter(chars[i]);
            }
            return this;
        }

        public CodePointFilter AllowFilter([NotNull] ICodePointFilter filter)
        {
            foreach (var allowedCodePoint in filter.GetAllowedCodePoints())
            {
                // If the code point can't be represented as a BMP character, skip it.
                char codePointAsChar = (char)allowedCodePoint;
                if (allowedCodePoint == codePointAsChar)
                {
                    _allowedCharsBitmap.AllowCharacter(codePointAsChar);
                }
            }
            return this;
        }

        internal void CloneAllowedCharsBitmapInto(out AllowedCharsBitmap clone)
        {
            this._allowedCharsBitmap.CloneInto(out clone);
        }

        public CodePointFilter ForbidBlock([NotNull] UnicodeBlock block)
        {
            int firstCodePoint = block.FirstCodePoint;
            int blockSize = block.BlockSize;
            for (int i = 0; i < blockSize; i++)
            {
                _allowedCharsBitmap.ForbidCharacter((char)(firstCodePoint + i));
            }
            return this;
        }

        public CodePointFilter ForbidBlocks(params UnicodeBlock[] blocks)
        {
            if (blocks != null)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    ForbidBlock(blocks[i]);
                }
            }
            return this;
        }

        public CodePointFilter ForbidChar(char c)
        {
            _allowedCharsBitmap.ForbidCharacter(c);
            return this;
        }

        public CodePointFilter ForbidChars(params char[] chars)
        {
            if (chars != null)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    _allowedCharsBitmap.ForbidCharacter(chars[i]);
                }
            }
            return this;
        }

        public CodePointFilter ForbidChars([NotNull] string chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                _allowedCharsBitmap.ForbidCharacter(chars[i]);
            }
            return this;
        }

        public IEnumerable<int> GetAllowedCodePoints()
        {
            for (int i = 0; i < 0x10000; i++)
            {
                if (_allowedCharsBitmap.IsCharacterAllowed((char)i))
                {
                    yield return i;
                }
            }
        }

        internal static CodePointFilter Wrap(ICodePointFilter filter)
        {
            return (filter as CodePointFilter) ?? new CodePointFilter(filter);
        }
    }
}
