﻿using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class PublishingHouseTests
    {
        [Test]
        public void PublishingHouse_IdProperty_ShouldHaveKeyAttribute()
        {
            var propertyInfo = typeof(PublishingHouse).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void PublishingHouse_NameProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(PublishingHouse).GetProperty("Name");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Name property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void PublishingHouse_YearFoundedProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(PublishingHouse).GetProperty("YearFounded");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "YearFounded property should have RequiredAttribute");
            Assert.AreEqual("Годината е задължителнa", requiredAttribute.ErrorMessage);
        }

        [TestCase(1, "Penguin Random House", 1935)]
        [TestCase(2, "HarperCollins", 1817)]
        [TestCase(3, "Simon & Schuster", 1924)]
        [TestCase(4, "Hachette Book Group", 1837)]
        [TestCase(5, "Macmillan Publishers", 1843)]
        public void PublishingHouse_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string name, int yearFounded)
        {
            var publishingHouse = new PublishingHouse
            {
                Id = id,
                Name = name,
                YearFounded = yearFounded,
                Books = new List<Book>()
            };

            Assert.AreEqual(id, publishingHouse.Id);
            Assert.AreEqual(name, publishingHouse.Name);
            Assert.AreEqual(yearFounded, publishingHouse.YearFounded);
            Assert.IsNotNull(publishingHouse.Books);
            Assert.IsEmpty(publishingHouse.Books);
        }

        [Test]
        public void PublishingHouse_BooksCollection_CanAddAndRemoveBooks()
        {
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "Test Publishing House",
                YearFounded = 2000,
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            publishingHouse.Books.Add(book1);
            publishingHouse.Books.Add(book2);

            Assert.AreEqual(2, publishingHouse.Books.Count);
            Assert.IsTrue(publishingHouse.Books.Contains(book1));
            Assert.IsTrue(publishingHouse.Books.Contains(book2));

            publishingHouse.Books.Remove(book1);

            Assert.AreEqual(1, publishingHouse.Books.Count);
            Assert.IsFalse(publishingHouse.Books.Contains(book1));
            Assert.IsTrue(publishingHouse.Books.Contains(book2));
        }

        [Test]
        public void PublishingHouse_Validation_InvalidWithEmptyName()
        {
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "", 
                YearFounded = 2000
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(publishingHouse);

            var isValid = Validator.TryValidateObject(publishingHouse, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("Name")),
                "Should have validation error for Name");
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"),
                "Should have the correct error message for Name");
        }

        

        [Test]
        public void PublishingHouse_Validation_ValidWithAllRequiredProperties()
        {
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "Test Publishing House",
                YearFounded = 2000,
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(publishingHouse);

            var isValid = Validator.TryValidateObject(publishingHouse, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void PublishingHouse_WithMultipleBooks_ShouldWorkCorrectly()
        {
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "Major Publisher",
                YearFounded = 1950,
                Books = new List<Book>()
            };

            for (int i = 1; i <= 5; i++)
            {
                publishingHouse.Books.Add(new Book { Id = i, Title = $"Book {i}" });
            }

            Assert.AreEqual(5, publishingHouse.Books.Count);

            for (int i = 1; i <= 5; i++)
            {
                Assert.IsTrue(publishingHouse.Books.Any(b => b.Id == i && b.Title == $"Book {i}"),
                    $"Book with ID {i} and title 'Book {i}' should be in the collection");
            }
        }

        [TestCase(1800)]
        [TestCase(1900)]
        [TestCase(2000)]
        [TestCase(2023)]
        public void PublishingHouse_DifferentYearFounded_ShouldBeValid(int year)
        {
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "Test Publishing House",
                YearFounded = year,
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(publishingHouse);

            var isValid = Validator.TryValidateObject(publishingHouse, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }
    }
}