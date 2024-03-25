namespace Notes.Models;

internal class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    

    public Note()
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt"; 
        Text = "";
    }

    public void Save() =>
    File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));
   
    public static Note Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
            };
    }

    public static IEnumerable<Note> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.notes.txt")

                // Each file name is used to load a note
                .Select(filename => Note.Load(Path.GetFileName(filename)));
    }
}