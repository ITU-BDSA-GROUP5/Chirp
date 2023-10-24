﻿using System.Linq;
using System.Xml.Linq;

namespace Chirp.Razor.Repositories
{
    public interface IAuthorRepository
    {
        public List<Author> getAuthorByName(string name);
        public List<Author> getAuthorByEmail(string email);
        public void createNewAuthor(Guid id, string name, string email);
        public int getHumanReadableId(Guid id);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _context;

        public AuthorRepository(ChirpDBContext context)
        {
            _context = context;
        }

        public void createNewAuthor(Guid id, string name, string email)
        {
            var existing = getAuthorByEmail(email);
            if (existing.Any())
            {
                //Console.WriteLine("Author " + email + " already exists");
                return;
            } 
            _context.Authors.Add(new Author { AuthorId = getHumanReadableId(id), Name = name, Email = email, Cheeps = new List<Cheep>() });
            _context.SaveChanges();
        }

        public int getHumanReadableId(Guid id) { return id.GetHashCode(); }

        public List<Author> getAuthorByEmail(string email)
        {
            return _context.Authors
                .Take(_context.Authors.Count<Author>())
                .OrderByDescending(a => a.AuthorId)
                .Where(a => a.Email == email)
                .ToList();
        }

        public List<Author> getAuthorByName(string name)
        {
            return _context.Authors
                .Take(_context.Authors.Count<Author>())
                .OrderByDescending(a => a.AuthorId)
                .Where(a => a.Name == name)
                .ToList();
        }
    }
}
