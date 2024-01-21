// ﻿using EFConsole;
// using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Text;

namespace mysqlefcore
{
  class Program
  {
    static void Main(string[] args)
    {
      while (true)
      {
        Console.WriteLine("Library Management System");
        Console.WriteLine("--------------------------");
        Console.Write("1.Insert\n2.Print\n3.Update\n4.Remove\n5.Exit ");
        Console.Write("Enter your choice: ");
        string? choice = Console.ReadLine();
        switch (choice)
        {
          case "1":
            InsertData();
            break;
          case "2":
            PrintData();
            break;
          case "3":
            UpdateData();
            break;
          case "4":
            RemoveData();
            break;
          case "5":
            Environment.Exit(0);
            break;
          default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
        }
      }
    }

    private static void InsertData()
    {
      using (var context = new LibraryContext())
      {
        // Creates the database if not exists
        context.Database.EnsureCreated();
        Console.WriteLine("Enter Publisher name:");
        // Adds a publisher
        var publisher = new Publisher
        {

          Name = Console.ReadLine()
        };
        context.Publisher.Add(publisher);
        // Adds some books
        Console.WriteLine("Enter ISBN number");
        string? bid = Console.ReadLine();
        Console.WriteLine("Enter Title of the book");
        string? bname = Console.ReadLine();
        Console.WriteLine("Enter author of the book");
        string? authorname = Console.ReadLine();
        Console.WriteLine("Enter Language of the book");
        string? Lang = Console.ReadLine();
        Console.WriteLine("How many pages in that book");
        int pno = Convert.ToInt32(Console.ReadLine());
        context.Book.Add(new Book
        {
          ISBN = bid,
          Title = bname,
          author = authorname,
          Language = Lang,
          Pages = pno,
          Publisher = publisher
        });
        // Saves changes
        context.SaveChanges();
      }
    }
    private static void PrintData()
    {
      using (var context = new LibraryContext())
      {
        var books = context.Book
          .Include(p => p.Publisher);
        foreach (var book in books)
        {
          var data = new StringBuilder();
          data.AppendLine($"ISBN: {book.ISBN}");
          data.AppendLine($"Title: {book.Title}");
          data.AppendLine($"Publisher: {book.Publisher.Name}");
          data.AppendLine($"Language: {book.Language}");
          data.AppendLine($"Pages: {book.Pages}");
          Console.WriteLine(data.ToString());
        }
      }
    }
    private static void UpdateData()
    {
      using (var context = new LibraryContext())
      {
        Console.WriteLine("Current Books:");
        PrintData();
        Console.Write("Enter ISBN of the book to update: ");
        string? isbnToUpdate = Console.ReadLine();
        var bookToUpdate = context.Book.FirstOrDefault(b => b.ISBN == isbnToUpdate);
        if (bookToUpdate != null)
        {
          Console.WriteLine("Updating  values into the tables: ");
          Console.WriteLine("What you want to update");
          Console.WriteLine("1. Title\n2.author\n3.Language\n4.Pages");
          string? updatechoice = Console.ReadLine();
          string? updatedata = null;
          switch (updatechoice)
          {
            case "1":
              updatedata = "Title";
              Console.WriteLine($"Current Title: {bookToUpdate.Title}");
              Console.Write("Enter new title: ");
              string? newTitle = Console.ReadLine();
              bookToUpdate.Title = newTitle;
              break;
            case "2":
              updatedata = "author";

              Console.WriteLine($"Current author: {bookToUpdate.author}");
              Console.Write("Enter new author: ");
              string? newauthor = Console.ReadLine();
              bookToUpdate.author = newauthor;
              break;
            case "3":
              updatedata = "Language";
              Console.WriteLine($"Current Language: {bookToUpdate.Language}");
              Console.Write("Enter new language: ");
              string? newLanguage = Console.ReadLine();
              bookToUpdate.Language = newLanguage;
              break;
            case "4":
              Console.WriteLine($"Current Pages: {bookToUpdate.Pages}");
              Console.Write("Enter new pages: ");
              int newPages;
              while (!int.TryParse(Console.ReadLine(), out newPages))
              {
                Console.WriteLine("Invalid input. Please enter a valid integer for pages.");
                Console.Write("Enter valid new pages: ");
              }
              bookToUpdate.Pages = newPages;
              break;

          }
          context.SaveChanges();
        }

        else
        {
          Console.WriteLine($"Book with ISBN {isbnToUpdate} not found.");
        }
      }
    }

    private static void RemoveData()
    {
      using (var context = new LibraryContext())
      {
        // Display current data
        Console.WriteLine("Current Books:");
        PrintData();

        // Get ISBN from user for the book to remove
        Console.Write("Enter ISBN of the book to remove: ");
        string? isbnToRemove = Console.ReadLine();

        // Retrieve the book by ISBN for removal
        var bookToRemove = context.Book.FirstOrDefault(b => b.ISBN == isbnToRemove);

        if (bookToRemove != null)
        {
          // Display details of the book to be removed
          Console.WriteLine($"Removing Book - ISBN: {bookToRemove.ISBN}, Title: {bookToRemove.Title}");

          // Remove the book
          context.Book.Remove(bookToRemove);

          // Save changes
          context.SaveChanges();

          Console.WriteLine("Removal successful!");
        }
        else
        {
          Console.WriteLine($"Book with ISBN {isbnToRemove} not found.");
        }
      }
    }
  }
}
