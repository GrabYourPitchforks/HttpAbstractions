// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Framework.WebEncoders
{
    public class CodePointFilterTests
    {
        [Fact]
        public void Ctor_Parameterless_DefaultsToBasicLatin()
        {
            // Act
            var filter = new CodePointFilter();

            // Assert
            for (int i = 0; i <= 0x007F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0080; i <= Char.MaxValue; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void Ctor_OtherCodePointFilterAsInterface()
        {
            // Arrange
            var originalFilter = new OddCodePointFilter();

            // Act
            var newFilter = new CodePointFilter(originalFilter);

            // Assert
            for (int i = 0; i <= Char.MaxValue; i++)
            {
                Assert.Equal((i % 2) == 1, newFilter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void Ctor_OtherCodePointFilterAsConcreteType_Clones()
        {
            // Arrange
            var originalFilter = new CodePointFilter(UnicodeBlocks.None).AllowChar('x');

            // Act
            var newFilter = new CodePointFilter(originalFilter).AllowChar('y');

            // Assert
            Assert.True(originalFilter.IsCharacterAllowed('x'));
            Assert.False(originalFilter.IsCharacterAllowed('y'));
            Assert.True(newFilter.IsCharacterAllowed('x'));
            Assert.True(newFilter.IsCharacterAllowed('y'));
        }

        [Fact]
        public void Ctor_UnicodeBlocks()
        {
            // Act
            var filter = new CodePointFilter(UnicodeBlocks.LatinExtendedA, UnicodeBlocks.LatinExtendedC);

            // Assert
            for (int i = 0; i < 0x0100; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0100; i <= 0x017F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0180; i < 0x2C60; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x2C60; i <= 0x2C7F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x2C80; i <= Char.MaxValue; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void AllowBlock()
        {
            // Arrange
            var filter = new CodePointFilter(UnicodeBlocks.None);

            // Act
            filter.AllowBlock(UnicodeBlocks.LatinExtendedA);

            // Assert
            for (int i = 0; i < 0x0100; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0100; i <= 0x017F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0180; i <= Char.MaxValue; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void AllowBlocks()
        {
            // Arrange
            var filter = new CodePointFilter(UnicodeBlocks.None);

            // Act
            filter.AllowBlocks(UnicodeBlocks.LatinExtendedA, UnicodeBlocks.LatinExtendedC);

            // Assert
            for (int i = 0; i < 0x0100; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0100; i <= 0x017F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0180; i < 0x2C60; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x2C60; i <= 0x2C7F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x2C80; i <= Char.MaxValue; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void AllowChar()
        {
            // Arrange
            var filter = new CodePointFilter();

            // Act
            filter.AllowChar('\u0100');

            // Assert
            Assert.True(filter.IsCharacterAllowed('\u0100'));
            Assert.False(filter.IsCharacterAllowed('\u0101'));
        }

        [Fact]
        public void AllowChars_Array()
        {
            // Arrange
            var filter = new CodePointFilter();

            // Act
            filter.AllowChars('\u0100', '\u0102');

            // Assert
            Assert.True(filter.IsCharacterAllowed('\u0100'));
            Assert.False(filter.IsCharacterAllowed('\u0101'));
            Assert.True(filter.IsCharacterAllowed('\u0102'));
            Assert.False(filter.IsCharacterAllowed('\u0103'));
        }

        [Fact]
        public void AllowChars_String()
        {
            // Arrange
            var filter = new CodePointFilter();

            // Act
            filter.AllowChars("\u0100\u0102");

            // Assert
            Assert.True(filter.IsCharacterAllowed('\u0100'));
            Assert.False(filter.IsCharacterAllowed('\u0101'));
            Assert.True(filter.IsCharacterAllowed('\u0102'));
            Assert.False(filter.IsCharacterAllowed('\u0103'));
        }

        [Fact]
        public void AllowFilter()
        {
            // Arrange
            var filter = new CodePointFilter(UnicodeBlocks.BasicLatin);

            // Act
            filter.AllowFilter(new OddCodePointFilter());

            // Assert
            for (int i = 0; i <= 0x007F; i++)
            {
                Assert.True(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0080; i <= Char.MaxValue; i++)
            {
                Assert.Equal((i % 2) == 1, filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void ForbidBlock()
        {
            // Arrange
            var filter = new CodePointFilter(new OddCodePointFilter());

            // Act
            filter.ForbidBlock(UnicodeBlocks.Specials);

            // Assert
            for (int i = 0; i <= 0xFFEF; i++)
            {
                Assert.Equal((i % 2) == 1, filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0xFFF0; i <= Char.MaxValue; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void ForbidBlocks()
        {
            // Arrange
            var filter = new CodePointFilter(new OddCodePointFilter());

            // Act
            filter.ForbidBlocks(UnicodeBlocks.BasicLatin, UnicodeBlocks.Specials);

            // Assert
            for (int i = 0; i <= 0x007F; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0x0080; i <= 0xFFEF; i++)
            {
                Assert.Equal((i % 2) == 1, filter.IsCharacterAllowed((char)i));
            }
            for (int i = 0xFFF0; i <= Char.MaxValue; i++)
            {
                Assert.False(filter.IsCharacterAllowed((char)i));
            }
        }

        [Fact]
        public void ForbidChar()
        {
            // Arrange
            var filter = new CodePointFilter(UnicodeBlocks.BasicLatin);

            // Act
            filter.ForbidChar('x');

            // Assert
            Assert.True(filter.IsCharacterAllowed('w'));
            Assert.False(filter.IsCharacterAllowed('x'));
            Assert.True(filter.IsCharacterAllowed('y'));
            Assert.True(filter.IsCharacterAllowed('z'));
        }

        [Fact]
        public void ForbidChars_Array()
        {
            // Arrange
            var filter = new CodePointFilter(UnicodeBlocks.BasicLatin);

            // Act
            filter.ForbidChars('x', 'z');

            // Assert
            Assert.True(filter.IsCharacterAllowed('w'));
            Assert.False(filter.IsCharacterAllowed('x'));
            Assert.True(filter.IsCharacterAllowed('y'));
            Assert.False(filter.IsCharacterAllowed('z'));
        }

        [Fact]
        public void ForbidChars_String()
        {
            // Arrange
            var filter = new CodePointFilter(UnicodeBlocks.BasicLatin);

            // Act
            filter.ForbidChars("xz");

            // Assert
            Assert.True(filter.IsCharacterAllowed('w'));
            Assert.False(filter.IsCharacterAllowed('x'));
            Assert.True(filter.IsCharacterAllowed('y'));
            Assert.False(filter.IsCharacterAllowed('z'));
        }

        [Fact]
        public void GetAllowedCodePoints()
        {
            // Arrange
            var expected = Enumerable.Range(UnicodeBlocks.BasicLatin.FirstCodePoint, UnicodeBlocks.BasicLatin.BlockSize)
                .Concat(Enumerable.Range(UnicodeBlocks.Specials.FirstCodePoint, UnicodeBlocks.Specials.BlockSize))
                .Except(new int[] { 'x' })
                .OrderBy(i => i)
                .ToArray();

            var filter = new CodePointFilter(UnicodeBlocks.BasicLatin, UnicodeBlocks.Specials);
            filter.ForbidChar('x');

            // Act
            var retVal = filter.GetAllowedCodePoints().OrderBy(i => i).ToArray();

            // Assert
            Assert.Equal<int>(expected, retVal);
        }

        // a code point filter which allows only odd code points through
        private sealed class OddCodePointFilter : ICodePointFilter
        {
            public IEnumerable<int> GetAllowedCodePoints()
            {
                for (int i = 1; i <= Char.MaxValue; i += 2)
                {
                    yield return i;
                }
            }
        }
    }
}
