using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CsvRepository<T> : IRepository<T> where T : class
    {
        private CsvWriter mCsvWriter;
        public CsvRepository() {

            var writer = new StreamWriter($"Contacts_{DateTime.Now:MM_dd_yyyy}.csv");
            mCsvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        }

        public virtual async Task AddRangeAsync(IList<T> entities)
        {
            await mCsvWriter.WriteRecordsAsync(entities);
        }

        public virtual async Task SaveChangesAsync()
        {
            await mCsvWriter.FlushAsync();

        }
    }
}
