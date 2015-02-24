// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Framework.WebEncoders
{
    internal sealed class CustomCodePointFilter : ICodePointFilter
    {
        private readonly int[] _allowedCodePoints;

        public CustomCodePointFilter(params int[] allowedCodePoints)
        {
            _allowedCodePoints = allowedCodePoints;
        }

        public IEnumerable<int> GetAllowedCodePoints()
        {
            return _allowedCodePoints;
        }
    }
}
