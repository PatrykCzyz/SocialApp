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
using TwitterMvc.Dtos;
using TwitterMvc.Helpers;

namespace TwitterMvc.Tests
{
    public class PostServiceTests
    {
        private AppDbContext _context;
        private PostService _postService;
        private string userId = "1cd5f732-3a43-446b-978d-070bbd007a7d";
        private string wrongUserId = "7dd0e8d2-2059-4fa3-9ed9-d1968c872e0b";
        private ErrorService _errorService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SocialAppTestDb").Options;

            _context = new AppDbContext(options);
            _errorService = new ErrorService();
            _postService = new PostService(_context, _errorService);

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

        #region GetPosts
        
        [Test]
        public async Task GetPosts_Return_All_Users_Posts()
        {
            //Arrange
            var posts = new List<Post>
            {
                new Post
                {
                    Id = 5,
                    Title = "First post",
                    Content = "Test content for my first post.",
                    DateTime = DateTime.Now,
                    UserId = userId
                },
                new Post
                {
                    Id = 8,
                    Title = "Second post",
                    Content = "Test content for my second post.",
                    DateTime = DateTime.Now,
                    UserId = userId
                },
                new Post
                {
                    Id = 12,
                    Title = "Third post",
                    Content = "Test content for my third post.",
                    DateTime = DateTime.Now,
                    UserId = userId
                },
                new Post
                {
                    Id = 16,
                    Title = "Fourth post",
                    Content = "Test content for my fourth post.",
                    DateTime = DateTime.Now,
                    UserId = userId
                }
            };

            await _context.AddRangeAsync(posts);
            await _context.SaveChangesAsync();

            //Act
            var result = await _postService.GetPosts(userId);

            //Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(posts.Count, result.Content.Count);
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
            var result = await _postService.GetPosts(userId);
            Assert.True(result.Succeeded); // Assert
            
            var postActual = result.Content.FirstOrDefault();

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
            var result = await _postService.GetPosts(null);

            //Arrange
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        public async Task GetPosts_Return_Error_When_User_Doesnt_Exist()
        {
            //Arrange
            
            //Act
            var result = await _postService.GetPosts(wrongUserId);

            //Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        public async Task GetPosts_Return_Error_When_User_Dont_Have_Posts()
        {
            //Arrange
            
            //Act
            var result = await _postService.GetPosts(userId);
            
            //Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("NoPost"),result.ErrorMessage);
        }
        
        #endregion

        #region CreatePost

        [Test]
        public async Task CreatePost_Should_Create_Correctly()
        {
            // Arrange
            var postDto = new PostDto
            {
                Title = "First post",
                Content = "Test post content."
            };
            
            // Act
            var result = await _postService.CreatePost(userId, postDto);
            var actualPost = await _context.Posts.ToListAsync();
        
            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(1, actualPost.Count);
            Assert.AreEqual(postDto.Title, actualPost.First().Title);
            Assert.AreEqual(postDto.Content, actualPost.First().Content);
            Assert.AreEqual(userId, actualPost.First().UserId);
        }

        [Test]
        public async Task CreatePost_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arrange
            var postDto = new PostDto
            {
                Title = "My frist post",
                Content = "Content of my first post."
            };

            // Act
            var result = await _postService.CreatePost(wrongUserId, postDto);
            var posts = await _context.Posts.ToListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(0, posts.Count);
        }

        [Test]
        public async Task CreatePost_Should_Return_Error_When_User_Is_Null()
        {
            // Arrange
            var postDto = new PostDto
            {
                Title = "My first post",
                Content = "Content of my first post"
            };
            // Act
            var result = await _postService.CreatePost(null, postDto);
            var posts = await _context.Posts.ToListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }
        
        [Test]
        public async Task CreatePost_Should_Return_Error_When_Title_Is_Null()
        {
            // Arrange
            var postDto = new PostDto
            {
                Title = null,
                Content = "Content of my first post."
            };
            
            // Act
            var result = await _postService.CreatePost(userId, postDto);
            var posts = await _context.Posts.ToListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("PostDtoNotFilled"), result.ErrorMessage);
            Assert.AreEqual(0, posts.Count);
        }

        [Test]
        public async Task CreatePost_Should_Return_Error_When_Content_Is_Null()
        {
            // Arrange
            var postDto = new PostDto
            {
                Title = "My first post",
                Content = null
            };

            // Act
            var result = await _postService.CreatePost(userId, postDto);
            var posts = await _context.Posts.ToListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("PostDtoNotFilled"), result.ErrorMessage);
            Assert.AreEqual(0, posts.Count);
        }

        [Test]
        public async Task CreatePost_Should_Return_Error_When_PostDto_Is_Null()
        {
            // Arrange
            
            // Act
            var result = await _postService.CreatePost(userId, null);
            var posts = await _context.Posts.ToListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("PostDtoNotFilled"), result.ErrorMessage);
            Assert.AreEqual(0, posts.Count);
        }
        
        #endregion
    }
}