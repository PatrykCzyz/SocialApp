using NUnit.Framework;
using TwitterMvc.Data.Context;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Models;
using System;
using TwitterMvc.Enums;
using System.Collections.Generic;
using TwitterMvc.Services;
using System.Threading.Tasks;
using System.Linq;

namespace TwitterMvc.Tests
{
    public class PostServiceTests
    {
        private AppDbContext _context;
        private PostService _postService;
        private string userId = "1cd5f732-3a43-446b-978d-070bbd007a7d";

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


            _postService = new PostService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        #region GetPosts
        [Test]
        public async Task GetPosts_Return_All_Users_Posts()
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
            var postsActual = await _postService.GetPosts(userId);

            //Assert
            Assert.AreEqual(posts.Count, postsActual.Result.Count);
        }

        [Test]
        public async Task GetPosts_Return_Post_Data_Correctly()
        {
            //Arrange
            var postExpected = new Post
            {
                Id = 3,
                Title = "My Post",
                Content = "Content of my post.",
                DateTime = DateTime.Now,
                UserId = userId
            };

            await _context.AddAsync(postExpected);
            await _context.SaveChangesAsync();

            //Act
            var postActual = (await _postService.GetPosts(userId)).Result.First();

            //Assert
            Assert.AreEqual(postExpected.Title, postActual.Title);
            Assert.AreEqual(postExpected.Content, postActual.Content);
            Assert.AreEqual(postExpected.DateTime, postActual.DateTime);
        }

        [Test]
        public async Task GetPosts_Return_Error_When_User_Wasnt_Provide()
        {
            //Arrange

            //Act
            var posts = await _postService.GetPosts(null);

            //Arrange
            Assert.NotNull(posts.Error);
            Assert.AreEqual(posts.Error, "User doesn't exist.");
        }

        [Test]
        public async Task GetPosts_Return_Error_When_User_Doesnt_Exist()
        {
            //Arrange
            var id = "7dd0e8d2-2059-4fa3-9ed9-d1968c872e0b";

            //Act
            var posts = await _postService.GetPosts(id);

            //Assert
            Assert.NotNull(posts.Error);
            Assert.AreEqual(posts.Error, "User doesn't exist.");
        }

        [Test]
        public async Task GetPosts_Return_Error_When_User_Dont_Have_Posts()
        {
            //Arrange
            
            //Act
            var posts = await _postService.GetPosts(userId);
            
            //Assert
            Assert.Null(posts.Result);
            Assert.NotNull(posts.Error);
            Assert.AreEqual("There is no post yet!",posts.Error);
        }
        #endregion
    }
}