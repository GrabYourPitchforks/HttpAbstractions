// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Framework.WebEncoders
{
    public class CodePointFiltersTests
    {
        [Fact]
        public void Range_None()
        {
            // Act & assert
            int[] codePoints = CodePointFilters.None.GetAllowedCodePoints().ToArray();
            Assert.Empty(codePoints);
        }

        [Fact]
        public void Range_All()
        {
            RunRangeTest(CodePointFilters.All, first: '\u0000', last: '\uFFFF');
        }

        [Fact]
        public void Range_BasicLatin()
        {
            RunRangeTest(CodePointFilters.BasicLatin, first: '\u0000', last: '\u007F');
        }

        [Fact]
        public void Range_Latin1Supplement()
        {
            RunRangeTest(CodePointFilters.Latin1Supplement, first: '\u0080', last: '\u00FF');
        }

        [Fact]
        public void Range_LatinExtendedA()
        {
            RunRangeTest(CodePointFilters.LatinExtendedA, first: '\u0100', last: '\u017F');
        }

        [Fact]
        public void Range_LatinExtendedB()
        {
            RunRangeTest(CodePointFilters.LatinExtendedB, first: '\u0180', last: '\u024F');
        }

        [Fact]
        public void Range_IPAExtensions()
        {
            RunRangeTest(CodePointFilters.IPAExtensions, first: '\u0250', last: '\u02AF');
        }

        [Fact]
        public void Range_SpacingModifierLetters()
        {
            RunRangeTest(CodePointFilters.SpacingModifierLetters, first: '\u02B0', last: '\u02FF');
        }

        [Fact]
        public void Range_CombiningDiacriticalMarks()
        {
            RunRangeTest(CodePointFilters.CombiningDiacriticalMarks, first: '\u0300', last: '\u036F');
        }

        [Fact]
        public void Range_GreekandCoptic()
        {
            RunRangeTest(CodePointFilters.GreekandCoptic, first: '\u0370', last: '\u03FF');
        }

        [Fact]
        public void Range_Cyrillic()
        {
            RunRangeTest(CodePointFilters.Cyrillic, first: '\u0400', last: '\u04FF');
        }

        [Fact]
        public void Range_CyrillicSupplement()
        {
            RunRangeTest(CodePointFilters.CyrillicSupplement, first: '\u0500', last: '\u052F');
        }

        [Fact]
        public void Range_Armenian()
        {
            RunRangeTest(CodePointFilters.Armenian, first: '\u0530', last: '\u058F');
        }

        [Fact]
        public void Range_Hebrew()
        {
            RunRangeTest(CodePointFilters.Hebrew, first: '\u0590', last: '\u05FF');
        }

        [Fact]
        public void Range_Arabic()
        {
            RunRangeTest(CodePointFilters.Arabic, first: '\u0600', last: '\u06FF');
        }

        [Fact]
        public void Range_Syriac()
        {
            RunRangeTest(CodePointFilters.Syriac, first: '\u0700', last: '\u074F');
        }

        [Fact]
        public void Range_ArabicSupplement()
        {
            RunRangeTest(CodePointFilters.ArabicSupplement, first: '\u0750', last: '\u077F');
        }

        [Fact]
        public void Range_Thaana()
        {
            RunRangeTest(CodePointFilters.Thaana, first: '\u0780', last: '\u07BF');
        }

        [Fact]
        public void Range_NKo()
        {
            RunRangeTest(CodePointFilters.NKo, first: '\u07C0', last: '\u07FF');
        }

        [Fact]
        public void Range_Samaritan()
        {
            RunRangeTest(CodePointFilters.Samaritan, first: '\u0800', last: '\u083F');
        }

        [Fact]
        public void Range_Mandaic()
        {
            RunRangeTest(CodePointFilters.Mandaic, first: '\u0840', last: '\u085F');
        }

        [Fact]
        public void Range_ArabicExtendedA()
        {
            RunRangeTest(CodePointFilters.ArabicExtendedA, first: '\u08A0', last: '\u08FF');
        }

        [Fact]
        public void Range_Devanagari()
        {
            RunRangeTest(CodePointFilters.Devanagari, first: '\u0900', last: '\u097F');
        }

        [Fact]
        public void Range_Bengali()
        {
            RunRangeTest(CodePointFilters.Bengali, first: '\u0980', last: '\u09FF');
        }

        [Fact]
        public void Range_Gurmukhi()
        {
            RunRangeTest(CodePointFilters.Gurmukhi, first: '\u0A00', last: '\u0A7F');
        }

        [Fact]
        public void Range_Gujarati()
        {
            RunRangeTest(CodePointFilters.Gujarati, first: '\u0A80', last: '\u0AFF');
        }

        [Fact]
        public void Range_Oriya()
        {
            RunRangeTest(CodePointFilters.Oriya, first: '\u0B00', last: '\u0B7F');
        }

        [Fact]
        public void Range_Tamil()
        {
            RunRangeTest(CodePointFilters.Tamil, first: '\u0B80', last: '\u0BFF');
        }

        [Fact]
        public void Range_Telugu()
        {
            RunRangeTest(CodePointFilters.Telugu, first: '\u0C00', last: '\u0C7F');
        }

        [Fact]
        public void Range_Kannada()
        {
            RunRangeTest(CodePointFilters.Kannada, first: '\u0C80', last: '\u0CFF');
        }

        [Fact]
        public void Range_Malayalam()
        {
            RunRangeTest(CodePointFilters.Malayalam, first: '\u0D00', last: '\u0D7F');
        }

        [Fact]
        public void Range_Sinhala()
        {
            RunRangeTest(CodePointFilters.Sinhala, first: '\u0D80', last: '\u0DFF');
        }

        [Fact]
        public void Range_Thai()
        {
            RunRangeTest(CodePointFilters.Thai, first: '\u0E00', last: '\u0E7F');
        }

        [Fact]
        public void Range_Lao()
        {
            RunRangeTest(CodePointFilters.Lao, first: '\u0E80', last: '\u0EFF');
        }

        [Fact]
        public void Range_Tibetan()
        {
            RunRangeTest(CodePointFilters.Tibetan, first: '\u0F00', last: '\u0FFF');
        }

        [Fact]
        public void Range_Myanmar()
        {
            RunRangeTest(CodePointFilters.Myanmar, first: '\u1000', last: '\u109F');
        }

        [Fact]
        public void Range_Georgian()
        {
            RunRangeTest(CodePointFilters.Georgian, first: '\u10A0', last: '\u10FF');
        }

        [Fact]
        public void Range_HangulJamo()
        {
            RunRangeTest(CodePointFilters.HangulJamo, first: '\u1100', last: '\u11FF');
        }

        [Fact]
        public void Range_Ethiopic()
        {
            RunRangeTest(CodePointFilters.Ethiopic, first: '\u1200', last: '\u137F');
        }

        [Fact]
        public void Range_EthiopicSupplement()
        {
            RunRangeTest(CodePointFilters.EthiopicSupplement, first: '\u1380', last: '\u139F');
        }

        [Fact]
        public void Range_Cherokee()
        {
            RunRangeTest(CodePointFilters.Cherokee, first: '\u13A0', last: '\u13FF');
        }

        [Fact]
        public void Range_UnifiedCanadianAboriginalSyllabics()
        {
            RunRangeTest(CodePointFilters.UnifiedCanadianAboriginalSyllabics, first: '\u1400', last: '\u167F');
        }

        [Fact]
        public void Range_Ogham()
        {
            RunRangeTest(CodePointFilters.Ogham, first: '\u1680', last: '\u169F');
        }

        [Fact]
        public void Range_Runic()
        {
            RunRangeTest(CodePointFilters.Runic, first: '\u16A0', last: '\u16FF');
        }

        [Fact]
        public void Range_Tagalog()
        {
            RunRangeTest(CodePointFilters.Tagalog, first: '\u1700', last: '\u171F');
        }

        [Fact]
        public void Range_Hanunoo()
        {
            RunRangeTest(CodePointFilters.Hanunoo, first: '\u1720', last: '\u173F');
        }

        [Fact]
        public void Range_Buhid()
        {
            RunRangeTest(CodePointFilters.Buhid, first: '\u1740', last: '\u175F');
        }

        [Fact]
        public void Range_Tagbanwa()
        {
            RunRangeTest(CodePointFilters.Tagbanwa, first: '\u1760', last: '\u177F');
        }

        [Fact]
        public void Range_Khmer()
        {
            RunRangeTest(CodePointFilters.Khmer, first: '\u1780', last: '\u17FF');
        }

        [Fact]
        public void Range_Mongolian()
        {
            RunRangeTest(CodePointFilters.Mongolian, first: '\u1800', last: '\u18AF');
        }

        [Fact]
        public void Range_UnifiedCanadianAboriginalSyllabicsExtended()
        {
            RunRangeTest(CodePointFilters.UnifiedCanadianAboriginalSyllabicsExtended, first: '\u18B0', last: '\u18FF');
        }

        [Fact]
        public void Range_Limbu()
        {
            RunRangeTest(CodePointFilters.Limbu, first: '\u1900', last: '\u194F');
        }

        [Fact]
        public void Range_TaiLe()
        {
            RunRangeTest(CodePointFilters.TaiLe, first: '\u1950', last: '\u197F');
        }

        [Fact]
        public void Range_NewTaiLue()
        {
            RunRangeTest(CodePointFilters.NewTaiLue, first: '\u1980', last: '\u19DF');
        }

        [Fact]
        public void Range_KhmerSymbols()
        {
            RunRangeTest(CodePointFilters.KhmerSymbols, first: '\u19E0', last: '\u19FF');
        }

        [Fact]
        public void Range_Buginese()
        {
            RunRangeTest(CodePointFilters.Buginese, first: '\u1A00', last: '\u1A1F');
        }

        [Fact]
        public void Range_TaiTham()
        {
            RunRangeTest(CodePointFilters.TaiTham, first: '\u1A20', last: '\u1AAF');
        }

        [Fact]
        public void Range_CombiningDiacriticalMarksExtended()
        {
            RunRangeTest(CodePointFilters.CombiningDiacriticalMarksExtended, first: '\u1AB0', last: '\u1AFF');
        }

        [Fact]
        public void Range_Balinese()
        {
            RunRangeTest(CodePointFilters.Balinese, first: '\u1B00', last: '\u1B7F');
        }

        [Fact]
        public void Range_Sundanese()
        {
            RunRangeTest(CodePointFilters.Sundanese, first: '\u1B80', last: '\u1BBF');
        }

        [Fact]
        public void Range_Batak()
        {
            RunRangeTest(CodePointFilters.Batak, first: '\u1BC0', last: '\u1BFF');
        }

        [Fact]
        public void Range_Lepcha()
        {
            RunRangeTest(CodePointFilters.Lepcha, first: '\u1C00', last: '\u1C4F');
        }

        [Fact]
        public void Range_OlChiki()
        {
            RunRangeTest(CodePointFilters.OlChiki, first: '\u1C50', last: '\u1C7F');
        }

        [Fact]
        public void Range_SundaneseSupplement()
        {
            RunRangeTest(CodePointFilters.SundaneseSupplement, first: '\u1CC0', last: '\u1CCF');
        }

        [Fact]
        public void Range_VedicExtensions()
        {
            RunRangeTest(CodePointFilters.VedicExtensions, first: '\u1CD0', last: '\u1CFF');
        }

        [Fact]
        public void Range_PhoneticExtensions()
        {
            RunRangeTest(CodePointFilters.PhoneticExtensions, first: '\u1D00', last: '\u1D7F');
        }

        [Fact]
        public void Range_PhoneticExtensionsSupplement()
        {
            RunRangeTest(CodePointFilters.PhoneticExtensionsSupplement, first: '\u1D80', last: '\u1DBF');
        }

        [Fact]
        public void Range_CombiningDiacriticalMarksSupplement()
        {
            RunRangeTest(CodePointFilters.CombiningDiacriticalMarksSupplement, first: '\u1DC0', last: '\u1DFF');
        }

        [Fact]
        public void Range_LatinExtendedAdditional()
        {
            RunRangeTest(CodePointFilters.LatinExtendedAdditional, first: '\u1E00', last: '\u1EFF');
        }

        [Fact]
        public void Range_GreekExtended()
        {
            RunRangeTest(CodePointFilters.GreekExtended, first: '\u1F00', last: '\u1FFF');
        }

        [Fact]
        public void Range_GeneralPunctuation()
        {
            RunRangeTest(CodePointFilters.GeneralPunctuation, first: '\u2000', last: '\u206F');
        }

        [Fact]
        public void Range_SuperscriptsandSubscripts()
        {
            RunRangeTest(CodePointFilters.SuperscriptsandSubscripts, first: '\u2070', last: '\u209F');
        }

        [Fact]
        public void Range_CurrencySymbols()
        {
            RunRangeTest(CodePointFilters.CurrencySymbols, first: '\u20A0', last: '\u20CF');
        }

        [Fact]
        public void Range_CombiningDiacriticalMarksforSymbols()
        {
            RunRangeTest(CodePointFilters.CombiningDiacriticalMarksforSymbols, first: '\u20D0', last: '\u20FF');
        }

        [Fact]
        public void Range_LetterlikeSymbols()
        {
            RunRangeTest(CodePointFilters.LetterlikeSymbols, first: '\u2100', last: '\u214F');
        }

        [Fact]
        public void Range_NumberForms()
        {
            RunRangeTest(CodePointFilters.NumberForms, first: '\u2150', last: '\u218F');
        }

        [Fact]
        public void Range_Arrows()
        {
            RunRangeTest(CodePointFilters.Arrows, first: '\u2190', last: '\u21FF');
        }

        [Fact]
        public void Range_MathematicalOperators()
        {
            RunRangeTest(CodePointFilters.MathematicalOperators, first: '\u2200', last: '\u22FF');
        }

        [Fact]
        public void Range_MiscellaneousTechnical()
        {
            RunRangeTest(CodePointFilters.MiscellaneousTechnical, first: '\u2300', last: '\u23FF');
        }

        [Fact]
        public void Range_ControlPictures()
        {
            RunRangeTest(CodePointFilters.ControlPictures, first: '\u2400', last: '\u243F');
        }

        [Fact]
        public void Range_OpticalCharacterRecognition()
        {
            RunRangeTest(CodePointFilters.OpticalCharacterRecognition, first: '\u2440', last: '\u245F');
        }

        [Fact]
        public void Range_EnclosedAlphanumerics()
        {
            RunRangeTest(CodePointFilters.EnclosedAlphanumerics, first: '\u2460', last: '\u24FF');
        }

        [Fact]
        public void Range_BoxDrawing()
        {
            RunRangeTest(CodePointFilters.BoxDrawing, first: '\u2500', last: '\u257F');
        }

        [Fact]
        public void Range_BlockElements()
        {
            RunRangeTest(CodePointFilters.BlockElements, first: '\u2580', last: '\u259F');
        }

        [Fact]
        public void Range_GeometricShapes()
        {
            RunRangeTest(CodePointFilters.GeometricShapes, first: '\u25A0', last: '\u25FF');
        }

        [Fact]
        public void Range_MiscellaneousSymbols()
        {
            RunRangeTest(CodePointFilters.MiscellaneousSymbols, first: '\u2600', last: '\u26FF');
        }

        [Fact]
        public void Range_Dingbats()
        {
            RunRangeTest(CodePointFilters.Dingbats, first: '\u2700', last: '\u27BF');
        }

        [Fact]
        public void Range_MiscellaneousMathematicalSymbolsA()
        {
            RunRangeTest(CodePointFilters.MiscellaneousMathematicalSymbolsA, first: '\u27C0', last: '\u27EF');
        }

        [Fact]
        public void Range_SupplementalArrowsA()
        {
            RunRangeTest(CodePointFilters.SupplementalArrowsA, first: '\u27F0', last: '\u27FF');
        }

        [Fact]
        public void Range_BraillePatterns()
        {
            RunRangeTest(CodePointFilters.BraillePatterns, first: '\u2800', last: '\u28FF');
        }

        [Fact]
        public void Range_SupplementalArrowsB()
        {
            RunRangeTest(CodePointFilters.SupplementalArrowsB, first: '\u2900', last: '\u297F');
        }

        [Fact]
        public void Range_MiscellaneousMathematicalSymbolsB()
        {
            RunRangeTest(CodePointFilters.MiscellaneousMathematicalSymbolsB, first: '\u2980', last: '\u29FF');
        }

        [Fact]
        public void Range_SupplementalMathematicalOperators()
        {
            RunRangeTest(CodePointFilters.SupplementalMathematicalOperators, first: '\u2A00', last: '\u2AFF');
        }

        [Fact]
        public void Range_MiscellaneousSymbolsandArrows()
        {
            RunRangeTest(CodePointFilters.MiscellaneousSymbolsandArrows, first: '\u2B00', last: '\u2BFF');
        }

        [Fact]
        public void Range_Glagolitic()
        {
            RunRangeTest(CodePointFilters.Glagolitic, first: '\u2C00', last: '\u2C5F');
        }

        [Fact]
        public void Range_LatinExtendedC()
        {
            RunRangeTest(CodePointFilters.LatinExtendedC, first: '\u2C60', last: '\u2C7F');
        }

        [Fact]
        public void Range_Coptic()
        {
            RunRangeTest(CodePointFilters.Coptic, first: '\u2C80', last: '\u2CFF');
        }

        [Fact]
        public void Range_GeorgianSupplement()
        {
            RunRangeTest(CodePointFilters.GeorgianSupplement, first: '\u2D00', last: '\u2D2F');
        }

        [Fact]
        public void Range_Tifinagh()
        {
            RunRangeTest(CodePointFilters.Tifinagh, first: '\u2D30', last: '\u2D7F');
        }

        [Fact]
        public void Range_EthiopicExtended()
        {
            RunRangeTest(CodePointFilters.EthiopicExtended, first: '\u2D80', last: '\u2DDF');
        }

        [Fact]
        public void Range_CyrillicExtendedA()
        {
            RunRangeTest(CodePointFilters.CyrillicExtendedA, first: '\u2DE0', last: '\u2DFF');
        }

        [Fact]
        public void Range_SupplementalPunctuation()
        {
            RunRangeTest(CodePointFilters.SupplementalPunctuation, first: '\u2E00', last: '\u2E7F');
        }

        [Fact]
        public void Range_CJKRadicalsSupplement()
        {
            RunRangeTest(CodePointFilters.CJKRadicalsSupplement, first: '\u2E80', last: '\u2EFF');
        }

        [Fact]
        public void Range_KangxiRadicals()
        {
            RunRangeTest(CodePointFilters.KangxiRadicals, first: '\u2F00', last: '\u2FDF');
        }

        [Fact]
        public void Range_IdeographicDescriptionCharacters()
        {
            RunRangeTest(CodePointFilters.IdeographicDescriptionCharacters, first: '\u2FF0', last: '\u2FFF');
        }

        [Fact]
        public void Range_CJKSymbolsandPunctuation()
        {
            RunRangeTest(CodePointFilters.CJKSymbolsandPunctuation, first: '\u3000', last: '\u303F');
        }

        [Fact]
        public void Range_Hiragana()
        {
            RunRangeTest(CodePointFilters.Hiragana, first: '\u3040', last: '\u309F');
        }

        [Fact]
        public void Range_Katakana()
        {
            RunRangeTest(CodePointFilters.Katakana, first: '\u30A0', last: '\u30FF');
        }

        [Fact]
        public void Range_Bopomofo()
        {
            RunRangeTest(CodePointFilters.Bopomofo, first: '\u3100', last: '\u312F');
        }

        [Fact]
        public void Range_HangulCompatibilityJamo()
        {
            RunRangeTest(CodePointFilters.HangulCompatibilityJamo, first: '\u3130', last: '\u318F');
        }

        [Fact]
        public void Range_Kanbun()
        {
            RunRangeTest(CodePointFilters.Kanbun, first: '\u3190', last: '\u319F');
        }

        [Fact]
        public void Range_BopomofoExtended()
        {
            RunRangeTest(CodePointFilters.BopomofoExtended, first: '\u31A0', last: '\u31BF');
        }

        [Fact]
        public void Range_CJKStrokes()
        {
            RunRangeTest(CodePointFilters.CJKStrokes, first: '\u31C0', last: '\u31EF');
        }

        [Fact]
        public void Range_KatakanaPhoneticExtensions()
        {
            RunRangeTest(CodePointFilters.KatakanaPhoneticExtensions, first: '\u31F0', last: '\u31FF');
        }

        [Fact]
        public void Range_EnclosedCJKLettersandMonths()
        {
            RunRangeTest(CodePointFilters.EnclosedCJKLettersandMonths, first: '\u3200', last: '\u32FF');
        }

        [Fact]
        public void Range_CJKCompatibility()
        {
            RunRangeTest(CodePointFilters.CJKCompatibility, first: '\u3300', last: '\u33FF');
        }

        [Fact]
        public void Range_CJKUnifiedIdeographsExtensionA()
        {
            RunRangeTest(CodePointFilters.CJKUnifiedIdeographsExtensionA, first: '\u3400', last: '\u4DBF');
        }

        [Fact]
        public void Range_YijingHexagramSymbols()
        {
            RunRangeTest(CodePointFilters.YijingHexagramSymbols, first: '\u4DC0', last: '\u4DFF');
        }

        [Fact]
        public void Range_CJKUnifiedIdeographs()
        {
            RunRangeTest(CodePointFilters.CJKUnifiedIdeographs, first: '\u4E00', last: '\u9FFF');
        }

        [Fact]
        public void Range_YiSyllables()
        {
            RunRangeTest(CodePointFilters.YiSyllables, first: '\uA000', last: '\uA48F');
        }

        [Fact]
        public void Range_YiRadicals()
        {
            RunRangeTest(CodePointFilters.YiRadicals, first: '\uA490', last: '\uA4CF');
        }

        [Fact]
        public void Range_Lisu()
        {
            RunRangeTest(CodePointFilters.Lisu, first: '\uA4D0', last: '\uA4FF');
        }

        [Fact]
        public void Range_Vai()
        {
            RunRangeTest(CodePointFilters.Vai, first: '\uA500', last: '\uA63F');
        }

        [Fact]
        public void Range_CyrillicExtendedB()
        {
            RunRangeTest(CodePointFilters.CyrillicExtendedB, first: '\uA640', last: '\uA69F');
        }

        [Fact]
        public void Range_Bamum()
        {
            RunRangeTest(CodePointFilters.Bamum, first: '\uA6A0', last: '\uA6FF');
        }

        [Fact]
        public void Range_ModifierToneLetters()
        {
            RunRangeTest(CodePointFilters.ModifierToneLetters, first: '\uA700', last: '\uA71F');
        }

        [Fact]
        public void Range_LatinExtendedD()
        {
            RunRangeTest(CodePointFilters.LatinExtendedD, first: '\uA720', last: '\uA7FF');
        }

        [Fact]
        public void Range_SylotiNagri()
        {
            RunRangeTest(CodePointFilters.SylotiNagri, first: '\uA800', last: '\uA82F');
        }

        [Fact]
        public void Range_CommonIndicNumberForms()
        {
            RunRangeTest(CodePointFilters.CommonIndicNumberForms, first: '\uA830', last: '\uA83F');
        }

        [Fact]
        public void Range_Phagspa()
        {
            RunRangeTest(CodePointFilters.Phagspa, first: '\uA840', last: '\uA87F');
        }

        [Fact]
        public void Range_Saurashtra()
        {
            RunRangeTest(CodePointFilters.Saurashtra, first: '\uA880', last: '\uA8DF');
        }

        [Fact]
        public void Range_DevanagariExtended()
        {
            RunRangeTest(CodePointFilters.DevanagariExtended, first: '\uA8E0', last: '\uA8FF');
        }

        [Fact]
        public void Range_KayahLi()
        {
            RunRangeTest(CodePointFilters.KayahLi, first: '\uA900', last: '\uA92F');
        }

        [Fact]
        public void Range_Rejang()
        {
            RunRangeTest(CodePointFilters.Rejang, first: '\uA930', last: '\uA95F');
        }

        [Fact]
        public void Range_HangulJamoExtendedA()
        {
            RunRangeTest(CodePointFilters.HangulJamoExtendedA, first: '\uA960', last: '\uA97F');
        }

        [Fact]
        public void Range_Javanese()
        {
            RunRangeTest(CodePointFilters.Javanese, first: '\uA980', last: '\uA9DF');
        }

        [Fact]
        public void Range_MyanmarExtendedB()
        {
            RunRangeTest(CodePointFilters.MyanmarExtendedB, first: '\uA9E0', last: '\uA9FF');
        }

        [Fact]
        public void Range_Cham()
        {
            RunRangeTest(CodePointFilters.Cham, first: '\uAA00', last: '\uAA5F');
        }

        [Fact]
        public void Range_MyanmarExtendedA()
        {
            RunRangeTest(CodePointFilters.MyanmarExtendedA, first: '\uAA60', last: '\uAA7F');
        }

        [Fact]
        public void Range_TaiViet()
        {
            RunRangeTest(CodePointFilters.TaiViet, first: '\uAA80', last: '\uAADF');
        }

        [Fact]
        public void Range_MeeteiMayekExtensions()
        {
            RunRangeTest(CodePointFilters.MeeteiMayekExtensions, first: '\uAAE0', last: '\uAAFF');
        }

        [Fact]
        public void Range_EthiopicExtendedA()
        {
            RunRangeTest(CodePointFilters.EthiopicExtendedA, first: '\uAB00', last: '\uAB2F');
        }

        [Fact]
        public void Range_LatinExtendedE()
        {
            RunRangeTest(CodePointFilters.LatinExtendedE, first: '\uAB30', last: '\uAB6F');
        }

        [Fact]
        public void Range_MeeteiMayek()
        {
            RunRangeTest(CodePointFilters.MeeteiMayek, first: '\uABC0', last: '\uABFF');
        }

        [Fact]
        public void Range_HangulSyllables()
        {
            RunRangeTest(CodePointFilters.HangulSyllables, first: '\uAC00', last: '\uD7AF');
        }

        [Fact]
        public void Range_HangulJamoExtendedB()
        {
            RunRangeTest(CodePointFilters.HangulJamoExtendedB, first: '\uD7B0', last: '\uD7FF');
        }

        [Fact]
        public void Range_CJKCompatibilityIdeographs()
        {
            RunRangeTest(CodePointFilters.CJKCompatibilityIdeographs, first: '\uF900', last: '\uFAFF');
        }

        [Fact]
        public void Range_AlphabeticPresentationForms()
        {
            RunRangeTest(CodePointFilters.AlphabeticPresentationForms, first: '\uFB00', last: '\uFB4F');
        }

        [Fact]
        public void Range_ArabicPresentationFormsA()
        {
            RunRangeTest(CodePointFilters.ArabicPresentationFormsA, first: '\uFB50', last: '\uFDFF');
        }

        [Fact]
        public void Range_VariationSelectors()
        {
            RunRangeTest(CodePointFilters.VariationSelectors, first: '\uFE00', last: '\uFE0F');
        }

        [Fact]
        public void Range_VerticalForms()
        {
            RunRangeTest(CodePointFilters.VerticalForms, first: '\uFE10', last: '\uFE1F');
        }

        [Fact]
        public void Range_CombiningHalfMarks()
        {
            RunRangeTest(CodePointFilters.CombiningHalfMarks, first: '\uFE20', last: '\uFE2F');
        }

        [Fact]
        public void Range_CJKCompatibilityForms()
        {
            RunRangeTest(CodePointFilters.CJKCompatibilityForms, first: '\uFE30', last: '\uFE4F');
        }

        [Fact]
        public void Range_SmallFormVariants()
        {
            RunRangeTest(CodePointFilters.SmallFormVariants, first: '\uFE50', last: '\uFE6F');
        }

        [Fact]
        public void Range_ArabicPresentationFormsB()
        {
            RunRangeTest(CodePointFilters.ArabicPresentationFormsB, first: '\uFE70', last: '\uFEFF');
        }

        [Fact]
        public void Range_HalfwidthandFullwidthForms()
        {
            RunRangeTest(CodePointFilters.HalfwidthandFullwidthForms, first: '\uFF00', last: '\uFFEF');
        }

        [Fact]
        public void Range_Specials()
        {
            RunRangeTest(CodePointFilters.Specials, first: '\uFFF0', last: '\uFFFF');
        }

        private static void RunRangeTest(ICodePointFilter filter, char first, char last)
        {
            // All defined chars in the range should be added to the set
            HashSet<char> charsStillToSee = new HashSet<char>();
            for (int i = first; i <= last; i++)
            {
                if (UnicodeHelpers.IsCharacterDefined((char)i))
                {
                    charsStillToSee.Add((char)i);
                }
            }

            // Each range should have *some* valid chars, otherwise the range is useless.
            Assert.NotEqual(0, charsStillToSee.Count);

            // Now iterate through the filter, checking that each code point is represented
            // in the hash set and occurs exactly once.
            foreach (int codePoint in filter.GetAllowedCodePoints())
            {
                char c = checked((char)codePoint);
                Assert.True(charsStillToSee.Remove(c), "Filter provided a code point that wasn't in the allow list.");
            }

            // Finally, the hash set should be emptied at this point.
            Assert.Equal(0, charsStillToSee.Count);
        }
    }
}
