﻿using FasType.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FasType.Core.Models.Abbreviations
{
    [DebuggerDisplay("{" + nameof(ElementaryRepresentation) + "}")]
    public abstract class BaseAbbreviation
    {
        protected static readonly int _stringKeyLength = 2;
        protected static readonly string SpacedArrow = $" {Utils.Unicodes.Arrow} ";

        public Guid Key { get; private set; }
        public string ShortForm { get; private set; }
        public string FullForm { get; private set; }
        public ulong Used { get; private set; }

        public string StringKey => string.Concat(ShortForm.Take(_stringKeyLength));

        public BaseAbbreviation(string shortForm, string fullForm, ulong used)
        {
            ShortForm = shortForm;
            FullForm = fullForm;
            Used = used;
        }

        protected BaseAbbreviation(Guid key, string shortForm, string fullForm, ulong used) : this(shortForm, fullForm, used)
        {
            Key = key;
        }

        public string ElementaryRepresentation => GetElementaryRepresentation();
        //public string ComplexRepresentation => GetComplexRepresentation();

        public void UpdateUsed() => Used++;

        public abstract bool IsAbbreviation(string shortForm, ILinguisticsRepository linguistics);
        public abstract string? GetFullForm(string shortForm, ILinguisticsRepository linguistics);
        public abstract bool TryGetFullForm(string shortForm, ILinguisticsRepository linguistics, [NotNullWhen(true)] out string? fullForm);

        protected abstract string GetElementaryRepresentation();
        public abstract string GetComplexRepresentation(ILinguisticsRepository linguistics);
    }
}
