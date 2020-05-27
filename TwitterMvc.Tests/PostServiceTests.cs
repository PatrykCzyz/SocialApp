using NUnit.Framework;
using TwitterMvc.Data.Context;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Models;
using System;
using TwitterMvc.Enums;
using System.Collections.Generic;
using TwitterMvc.Services;
using System.Threading.Tasks;

namespace TwitterMvc.Tests
{
    public class Post_Service_Tests
    {
        private AppDbContext _context;
        private string userId = Guid.NewGuid().ToString();

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SocialAppTestDb").Options;

            _context = new AppDbContext(options);

            _context.Add(new CustomUser
            {
                Id = userId,
                UserName = "Test",
                Email = "test@example.com",
                Name = "John",
                Lastname = "Tester",
                Age = 33,
                Gender = GenderEnum.Male,
                Country = "Poland"
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task Return_All_Users_Posts()
        {
            //Arrange
            var posts = new List<Post>();

            posts.Add(new Post
            {
                Id = 5,
                Title = "First post",
                Content = "Test content for my first post.",
                DateTime = DateTime.Now,
                UserId = userId
            });

            posts.Add(new Post
            {
                Id = 8,
                Title = "Second post",
                Content = "Test content for my second post.",
                DateTime = DateTime.Now,
                UserId = userId
            });

            posts.Add(new Post
            {
                Id = 12,
                Title = "Third post",
                Content = "Test content for my third post.",
                DateTime = DateTime.Now,
                UserId = userId
            });

            posts.Add(new Post
            {
                Id = 16,
                Title = "Fourth post",
                Content = "Test content for my fourth post.",
                DateTime = DateTime.Now,
                UserId = userId
            });

            await _context.AddRangeAsync(posts);
            await _context.SaveChangesAsync();

            //Act
            var postService = new PostService(_context);

            var result = await postService.GetPosts(userId);

            //Assert
            Assert.AreEqual(posts.Count, result.Count);
        }
    }
}