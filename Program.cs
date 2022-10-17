// finds files that end in "*.prt.2" and deletes them if there's a newer one
// also renames those, so there's only "*.1" files left over

const string path = "./testfolder";

string[] all_files = Directory.GetFiles(path);

List<Creo_File> files = new List<Creo_File>();

foreach (string file in all_files)
{
    if(file.Contains("prt"))
    {
        // get ending and its length
        int numlength = file.Length - 1 - file.LastIndexOf('.');
        string ending = file.Substring(file.Length - numlength);
        int namelength = file.IndexOf('.', file.LastIndexOf('/')) - file.LastIndexOf('/') - 1;
        string filename = file.Substring(file.LastIndexOf('/') + 1, namelength);

        files.Add(new Creo_File(file, filename, Int16.Parse(ending), numlength));
    }
}

    // go through all files and check for duplicate names, delete all except 
    // the one with the highest ending number
    List<Creo_File> files_to_delete = new List<Creo_File>();
    foreach (Creo_File file in files)
    {
        
        foreach (Creo_File other_file in files)
        {
            if ((file.filename == other_file.filename) && (file != other_file))
            {
                // same name but not checking against itself
                if (file.num < other_file.num)
                {
                    files_to_delete.Add(file);
                }
            }
        }
    }

    foreach (Creo_File file in files_to_delete)
    {
        file.Delete_File();
        files.Remove(file);
    }


    // second pass, check files for numbers > 1
    foreach (Creo_File file in files)
    {
        if (file.num > 1)
        {
            file.Rename_File();
        }
    }


class Creo_File{

    public string path;
    public string filename;
    public int num;
    public int numlength;

    public Creo_File(string p, string f, int n, int l)
    {
        this.path = p;
        this.filename = f;
        this.num = n;
        this.numlength = l;
    }


    // delete the file, return 1 if successfull
    public void Delete_File()
    {
        File.Delete(this.path);
        Console.WriteLine("deleted " + this.path);
    }

    // rename remaining file to "*.1"
    public void Rename_File()
    {
        string newpath = this.path.Substring(0, this.path.Length - this.numlength);
        newpath += '1';
        File.Move(this.path, newpath);
        Console.WriteLine("renamed " + this.path + " to " + newpath);
    }
}