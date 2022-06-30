﻿using Conti.Tom.Publishing.Books.IsbnGenerator;
using Conti.Tom.Publishing.Books.IsbnGenerator.Interfaces;
using Conti.Tom.Publishing.Books.IsbnGenerator.Models;
using Conti.Tom.Publishing.Books.IsbnGenerator.Services;
using Conti.Tom.Publishing.Books.Warehouse.Interfaces;
using Conti.Tom.Publishing.Books.Warehouse.Models;
using Conti.Tom.Publishing.Store;

namespace Conti.Tom.Publishing.Books.Warehouse.Services
{
    public class BooksService : IBooksService
    {
        private IISBNService _ISBNService;
        private IStoreService _storeService;

        public BooksService(IISBNService iSBNService, IStoreService storeService)
        {
            _ISBNService = iSBNService;
            _storeService = storeService;
        }

        private Dictionary<ISBN, Book> _books = new Dictionary<ISBN, Book>();
        public ISBN CreateBook(string title, int pages, double price, Dictionary<string, Object> options)
        {
            if (title == null)
            {
                throw new ArgumentException("null title");
            }
            if (pages <= 0)
            {
                throw new ArgumentException("invalid pages");
            }
            if (price < 0)
            {
                throw new ArgumentException("invalid price");
            }
            if (options == null)
            {
                throw new ArgumentException("null options");
            }
            bool available = false;
            ISBN isbn = _ISBNService.Next();
            Book newBook;
            try
            {
                string? topic = options["topic"].ToString();
                newBook = new SpecialistBook(isbn, title, pages, price, available, topic!);
            }
            catch (Exception)
            {
                try
                {
                    string? subject = options["subject"].ToString();
                    int year = (int)options["year"];
                    newBook = new SchoolBook(isbn, title, pages, price, available, year, subject!);
                }
                catch (Exception)
                {
                    newBook = new Book(isbn, title, pages, price, available);
                }
            }
            this._books.Add(isbn, newBook);
            return isbn;
        }

        public Book FindBookByIsbn(ISBN isbn)
        {
            Book book = this._books[isbn];
            SetAvailability(book);
            return book;

        }

        private Book SetAvailability(Book book)
        {
            int isbnStock = this._storeService.GetStock("books", book.ISBN);
            if (isbnStock > 0)
            {
                book.Available = true;
            }
            else
            {
                book.Available = false;
            }
            return book;
        }
        public void DeleteBookByIsbn(ISBN isbn)
        {
            this._books.Remove(isbn);
        }

        public List<Book> FindBooksByTitle(string title)
        {
            return _books.Values.ToList().FindAll(x => x.Title == title).ConvertAll(book => SetAvailability(book));
        }

        public List<Book> FindBooksByPriceRange(double min, double max)
        {
            return _books.Values.ToList().FindAll(x => x.Price > min && x.Price < max).ConvertAll(book => SetAvailability(book));
        }

    }
}