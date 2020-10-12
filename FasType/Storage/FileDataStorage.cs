﻿using FasType.Converters;
using FasType.Models;
using FasType.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FasType.Storage
{
    public class FileDataStorage : IDataStorage
    {
        readonly string _filepath;
        List<IAbbreviation> _allAbbreviations;
        readonly JsonSerializerOptions serializerOptions;

        public int Count => AllAbbreviations.Count;

        ILookup<string, IAbbreviation> AbbreviationsLookup { get; set; }
        IList<IAbbreviation> AllAbbreviations
        {
            get => _allAbbreviations;
            set
            {
                _allAbbreviations = value.ToList();
                AbbreviationsLookup = _allAbbreviations.ToLookup(a => string.Concat(a.ShortForm.Take(2)), a => a);
            }
        }
        
        public FileDataStorage(IConfiguration _configuration)
        {
            _filepath = _configuration["DataFilePath"];

            serializerOptions = new JsonSerializerOptions();
#if DEBUG
            serializerOptions.WriteIndented = true;
#endif
            serializerOptions.Converters.Add(new IAbbreviationConverter());
            serializerOptions.Converters.Add(new IEnumerableConverter(serializerOptions));

            Load();
        }

        protected bool Load()
        {
            using var stream = new FileStream(_filepath, FileMode.OpenOrCreate, FileAccess.Read);
            using var reader = new StreamReader(stream);
            string content = reader.ReadToEnd();

            AllAbbreviations = JsonSerializer.Deserialize<IList<IAbbreviation>>(content, serializerOptions);
            Log.Information("Abbreviations Data Storage Loaded.");

            return true;
        }

        protected async Task<bool> LoadAsync()
        {
            using var stream = new FileStream(_filepath, FileMode.OpenOrCreate, FileAccess.Read);

            AllAbbreviations = await JsonSerializer.DeserializeAsync<IList<IAbbreviation>>(stream, serializerOptions);
            Log.Information("Abbreviations Data Storage Loaded.");

            return true;
        }

        protected bool Save()
        {
            using var stream = new FileStream(_filepath, FileMode.OpenOrCreate, FileAccess.Write);
            using var writer = new StreamWriter(stream);
            var ser = JsonSerializer.Serialize(AllAbbreviations, serializerOptions);

            writer.Write(ser);
            
            Log.Information("Abbreviations Data Storage Saved.");

            return true;
        }

        protected async Task<bool> SaveAsync()
        {
            using var stream = new FileStream(_filepath, FileMode.OpenOrCreate, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, AllAbbreviations, serializerOptions);

            Log.Information("Abbreviations Data Storage Saved.");

            return true;
        }

        public bool Add(IAbbreviation abbrev)
        {
            AllAbbreviations.Add(abbrev);

            return Save() && Load();
        }

        public async Task<bool> AddAsync(IAbbreviation abbrev)
        {
            AllAbbreviations.Add(abbrev);

            return await SaveAsync() && await LoadAsync();
        }

        public IAbbreviation GetAbbreviation(string shortForm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAbbreviation> GetAbbreviations(string shortForm)
        {
            var approx = AbbreviationsLookup[string.Concat(shortForm.Take(2))];
            var matching = approx.Where(a => a.IsAbbreviation(shortForm)).ToList();
            return matching;
        }

        void ICollection<IAbbreviation>.Add(IAbbreviation item) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public bool Contains(IAbbreviation item) => AllAbbreviations.Contains(item);
        public void CopyTo(IAbbreviation[] array, int arrayIndex) => AllAbbreviations.CopyTo(array, arrayIndex);
        public bool Remove(IAbbreviation item) => throw new NotImplementedException();
        public IEnumerator<IAbbreviation> GetEnumerator() => AllAbbreviations.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => AllAbbreviations.GetEnumerator();
    }
}
