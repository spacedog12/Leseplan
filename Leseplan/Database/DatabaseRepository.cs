namespace Leseplan.Database;

public class DatabaseRepository : IDisposable
{
    private static SQLiteAsyncConnection db;
    private bool disposed = false;

    public DatabaseRepository() { }

    private static async Task Init()
    {
        try
        {
            Console.WriteLine("Init Start");
            var dbPath = FileAccessHelper.GetLocalFilePath(Path.Combine("Leseplan", "Database", "ReadingPlanDB.db3"));
            Console.WriteLine($"DbPath after GetLocalFilePath: {dbPath}");

            if (!File.Exists(dbPath))
            {
                Console.WriteLine("File doesn't exist");
                await CopyDatabaseFromResources(dbPath);
            }

            if (db is null)
            {
                db = new SQLiteAsyncConnection(dbPath);
                await db.CreateTableAsync<BiblePlan>();
                await db.CreateTableAsync<CatechismPlan>();
            }

            Console.WriteLine("Init End");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database Error!", "Database couldn't be created", "OK");
            Debug.WriteLine($"Init Exception: {ex}");
            throw;
        }
    }

    private static async Task CopyDatabaseFromResources(string dbPath)
    {
        try
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            using var stream = assembly.GetManifestResourceStream("Leseplan.Database.ReadingPlanDB.db");
            if (stream is null) throw new Exception("Resource stream is null");
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            await File.WriteAllBytesAsync(dbPath, memoryStream.ToArray());
            Console.WriteLine("Database file copied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Init Error");
            Console.WriteLine($"Error copying database file: {ex}");
            throw;
        }
    }

    public async Task<List<CatechismPlan>> GetCatechismPassages()
    {
        await Init();
        try
        {
            var table = await db.Table<CatechismPlan>().ToListAsync();
            Debug.WriteLine("Catechism Table finished retrieving");
            return table;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
            return new List<CatechismPlan>();
        }
    }

    public async Task SetCatechismRead(int id)
    {
        await Init();
        try
        {
            string todaysDate = DatePickerHelper.GetTodaysDate();
            var query = await db.Table<CatechismPlan>().FirstOrDefaultAsync(i => i.CatechismId == id);

            if (query != null)
            {
                query.CatechismRead = !query.CatechismRead;
                query.CatechismDateRead = query.CatechismRead ? todaysDate : null;
                await db.UpdateAsync(query);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
        }
    }

    public async Task<(CatechismPlan nextQuery, CatechismPlan previousQuery)> GetNextUnreadCatechismPassage()
    {
        await Init();
        try
        {
            var nextQuery = await db.Table<CatechismPlan>()
                .Where(p => !p.CatechismRead)
                .OrderBy(i => i.CatechismId)
                .FirstOrDefaultAsync();

            CatechismPlan previousQuery = null;
            if (nextQuery != null)
            {
                previousQuery = await db.Table<CatechismPlan>()
                    .Where(p => p.CatechismId < nextQuery.CatechismId)
                    .OrderByDescending(i => i.CatechismId)
                    .FirstOrDefaultAsync();
            }

            return (nextQuery, previousQuery);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
            return (null, null);
        }
    }

    public async Task<List<BiblePlan>> GetBibleBooksByTestament(string pageTestament)
    {
        await Init();
        try
        {
            var query = await db.Table<BiblePlan>()
                .Where(p => p.Testament == pageTestament)
                .OrderBy(p => p.Chronology)
                .ToListAsync();

            Debug.WriteLine($"Query returned {query.Count} entries.");

            var groupedBooks = query
                .GroupBy(p => p.BibleBooks)
                .Select(p => p.First())
                .ToList();

            Debug.WriteLine($"Grouped into {groupedBooks.Count} groups.");
            /* foreach (var item in groupedBooks)
            {
                Console.WriteLine($"ID: {item.Id} | Testament: {item.Testament} | BibleBooks: {item.BibleBooks} | Chronology: {item.Chronology}");
            } */

            return groupedBooks;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in GetBibleBooksByTestament: {ex.Message}");
            Debug.WriteLine($"Exception in GetBibleBooksByTestament: {ex.StackTrace}");
            return new List<BiblePlan>();
        }
    }

    public async Task<List<BiblePlan>> GetBiblePassagesByBook(string bookName)
    {
        await Init();

        var query = await db.Table<BiblePlan>()
            .Where(p => p.BibleBooks == bookName)
            .OrderBy(p => p.Day)
            .ToListAsync();

        return query;
    }

    public async Task SetBiblePassageRead(int id)
    {
        await Init();

        string todaysDate = DatePickerHelper.GetTodaysDate();
        var query = await db.Table<BiblePlan>().FirstOrDefaultAsync(i => i.BibleId == id);

        if (query != null)
        {
            query.BibleRead = !query.BibleRead;
            query.BibleDateRead = query.BibleRead ? todaysDate : null;
            await db.UpdateAsync(query);

        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                db?.CloseAsync().Wait();
            }

            disposed = true;
        }
    }

    ~DatabaseRepository()
    {
        Dispose(false);
    }
}
