using Path = System.IO.Path;

namespace Leseplan.Database;

public class DatabaseRepository
{
    private static SQLiteAsyncConnection db;

    public DatabaseRepository()
    {
    }

    // Initialize Database
    static async Task Init()
    {
        try
        {
            Console.WriteLine($"Init Start");
            // Path to the SQLite database fil
            var DbPath = FileAccessHelper.GetLocalFilePath(Path.Combine("Leseplan", "Database", "ReadingPlanDB.db3"));
            // var DbPath = FileAccessHelper.GetLocalFilePath("ReadingPlanDB.db");
            Console.WriteLine($"DbPath after GetLocalFilePath: {DbPath}");

            // Check if Database file exists
            if (!File.Exists(DbPath))
            {
                Console.WriteLine($"File doesn't exist");

                // If not, copy the pre-populated database from resources
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                using (Stream? stream = assembly.GetManifestResourceStream("Leseplan.Database.ReadingPlanDB.db"))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        try
                        {
                            await stream.CopyToAsync(memoryStream);
                            Console.WriteLine($"DbPath: {DbPath}");
                            Directory.CreateDirectory(Path.GetDirectoryName(DbPath));
                            File.WriteAllBytes(DbPath, memoryStream.ToArray());
                            Console.WriteLine("Database file copied successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Init Error");
                            Console.WriteLine($"Error copying database file: {ex}");
                            throw;
                        }
                    }
                }
            }

            if (db != null)
                return;

            // Initialize the SQLiteAsyncConnection with the database path
            db = new SQLiteAsyncConnection(DbPath);

            // Create the tables if they do not exist
            await db.CreateTableAsync<BiblePlan>();
            await db.CreateTableAsync<CatechismPlan>();
            Console.WriteLine($"Init End");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database Error!", "Database couldn't be created", "OK");
            Debug.WriteLine($"Init Exception: {ex}");
            throw;
        }
    }

    // Retrieves all data from the Catechism table
    public async Task<List<CatechismPlan>?> GetCatechismPassages()
    {
        await Init();
        
        try
        {
            var table = await db.Table<CatechismPlan>().ToListAsync();

            /* foreach (var data in table)
            {
                Console.WriteLine($"Result: {data.CatechismPassage}");
            } */
            Debug.WriteLine($"Catechism Table finished getting");

            return table;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
        }

        return new List<CatechismPlan>();
    }

    // Updates the Catechism sections: catechismRead and dateRead
    public async Task SetCatechismRead(int id)
    {
        await Init();

        try
        {
            // Get's today's date
            string todaysDate = DatePickerHelper.GetTodaysDate();

            // get's the passage with the spezific id
            var query = await db.Table<CatechismPlan>()
                .Where(i => i.CatechismId == id)
                .FirstOrDefaultAsync();

            // updates the database
            if (query is not null)
            {
                // Checks if catechism is read or not and sets the value to true or false,
                // depending the database entry.
                if (query.CatechismRead is true)
                {
                    query.CatechismRead = false;
                    query.CatechismDateRead = null;
                }
                else
                {
                    query.CatechismRead = true;
                    query.CatechismDateRead = todaysDate;
                }

                await db.UpdateAsync(query);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
        }

    }

    // Get's the next catechism passage for the MainPage
    public async Task<(CatechismPlan, CatechismPlan)> GetNextUnreadCatechismPassage()
    {
        await Init();

        CatechismPlan nextQuery = null;
        CatechismPlan previousQuery = null;

        try
        {
            // Get's the next passage
            nextQuery = await db.Table<CatechismPlan>()
                .Where(p => p.CatechismRead == false)
                .OrderBy(i => i.CatechismId)
                .FirstOrDefaultAsync();

            if (nextQuery is not null)
            {
                previousQuery = await db.Table<CatechismPlan>()
                .Where(p => p.CatechismId < nextQuery.CatechismId)
                .OrderByDescending(i => i.CatechismId)
                .FirstOrDefaultAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
        }

        return (nextQuery, previousQuery);
    }
}