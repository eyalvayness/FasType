﻿using FasType.Core.Contexts;
using FasType.Core.Models.Dictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FasType.Core.Services
{
    public interface IDictionaryRepository : IGenericRepository<BaseDictionaryElement, string>
    {
        T? GetById<T>(string id) where T : BaseDictionaryElement;
        IEnumerable<BaseDictionaryElement> GetElementsFromRegex(string regexPattern);
    }

    public class DictionaryRepository : GenericRepository<BaseDictionaryElement, string, DictionaryDbContext>, IDictionaryRepository
    {
        public DictionaryRepository(DictionaryDbContext context) : base(context)
        { }

        public T? GetById<T>(string id) where T : BaseDictionaryElement => GetById(id) as T;
        public IEnumerable<BaseDictionaryElement> GetElementsFromRegex(string regexPattern) => Set.FromSqlRaw("SELECT * FROM Dictionary WHERE regexp(LOWER(FullForm), {0})", regexPattern);
    }
}
