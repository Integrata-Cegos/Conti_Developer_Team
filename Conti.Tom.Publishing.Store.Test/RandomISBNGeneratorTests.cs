﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conti.Tom.Publishing.Tests
{
    internal class RandomISBNGeneratorTests
    {
        private string ISBN = "Test-Isbn";
        private string CountryCode = "-test";
        private IISBNService? _isbnGenerator;
        [SetUp]
        public void init()
        {
            RandomISBNService isbnService = new RandomISBNService();
            isbnService.Prefix = ISBN;
            isbnService.CountryCode = CountryCode;
            _isbnGenerator = isbnService;
        }

        [Test]
        public void NextGeneratesNonEmptyIsbn()
        {
            ISBN isbn = _isbnGenerator.Next();
            Assert.NotNull(isbn);
        }
        [Test]
        public void NextGeneratesUniqueIsbns()
        {
            ISBN isbn1 = _isbnGenerator.Next();
            ISBN isbn2 = _isbnGenerator.Next();
            Assert.AreNotEqual(isbn1, isbn2);
        }

        [Test]
        public void GeneratedIsbnStartsWithTestIsbn()
        {
            ISBN isbn = _isbnGenerator.Next();
            string isbnAsString = isbn.ToString();
            Assert.True(isbnAsString.StartsWith(ISBN));
        }
    }
}