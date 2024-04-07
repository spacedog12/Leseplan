using Path = System.IO.Path;

namespace Leseplan.Database;

public class DatabaseRepository
{
    static SQLiteAsyncConnection db;

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
            Console.WriteLine($"Init Exception: {ex}");
            throw;
        }
    }

    List<BiblePlan> bibleList = new();
    List<CatechismPlan> catechismList = new();

    // Retrieves all data from the Catechism table
    public async Task<List<CatechismPlan>?> GetCatechismPassages()
    {
        await Init();
        
        try
        {
            var table = await db.Table<CatechismPlan>().ToListAsync();

            foreach (var data in table)
            {
                Console.WriteLine($"Result: {data.CatechismPassage}");
            }
            return table;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
        }

        return new List<CatechismPlan>();
    }
}