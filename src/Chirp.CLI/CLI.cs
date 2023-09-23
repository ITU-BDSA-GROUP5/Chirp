namespace Chirp.CLI;

using SimpleDB;
using Chirp.CLI;

public class CLI {
    private IDatabaseRepository<Cheep> _db;
    private UserInterface _ui;

    public CLI (IDatabaseRepository<Cheep> db) {
        _db = db;
        _ui = new UserInterface();
    }

    public void Read()
    {
        IEnumerable<Cheep> cheeps = _db.Read();

        _ui.PrintCheeps(cheeps);
    }

    public void Cheep(string message)
    {
        string author = Environment.UserName;
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        _db.Store(new Cheep(author, message, time));
    }
}