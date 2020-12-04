﻿using System;
using System.Collections.Generic;
using FasType.Models.Linguistics.Grammars;
using FasType.Services;

namespace FasType.Models.Linguistics
{
    public class LinguisticsDTO : ILinguisticsStorage
    {
        public GrammarType GenderType { get; set; }
        public GrammarType PluralType { get; set; }
        public GrammarType GenderPluralType { get; set; }
        public IEnumerable<SyllableAbbreviation> AbbreviationMethods { get; set; }

        public void Dispose() => throw new NotImplementedException();
        public bool Export(string filename) => throw new NotImplementedException();
        public bool Import(string filename) => throw new NotImplementedException();
    }
}