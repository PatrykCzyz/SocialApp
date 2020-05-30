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
using Bogus;
using TwitterMvc.Dtos;
using TwitterMvc.Helpers;
using TwitterMvc.Services.Interfaces;
using TwitterMvc.Tests.Helpers;

namespace TwitterMvc.Tests
{
    public class PostServiceTests
    {
        private AppDbContext _context;
        private IPostService _postService;
        private string _wrongUserId;
        private string _userId;
        private IErrorService _errorService;
        private FakeDataGenerator _fakeData;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SocialAppTestDb").Options;

            _context = new AppDbContext(options);
            _errorService = new ErrorService();
            _postService = new PostService(_context, _errorService);
            _fakeData = new FakeDataGenerator();

            var user = _fakeData.GetUser();
            _context.Add(user);
            _context.SaveChanges();

            _userId = user.Id;
            _wrongUserId = Guid.NewGuid().ToString();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        #region GetPosts
        
        [Test]
        public async Task GetPosts_Return_All_Posts_For_Given_UserId()
        {
            //Arrange
            var posts = _fakeData.GetPosts(_userId, 5);
            
            await _context.AddRangeAsync(posts);
            await _context.SaveChangesAsync();

            //Act
            var result = await _postService.GetPosts(_userId);

            //Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(posts.Count, result.Content.Count);
        }
        
        [Test]
        public async Task GetPosts_Return_Post_Data_Correctly()
        {
            //Arrange
            var postExpected = _fakeData.GetPosts(_userId, 1).First();

            await _context.AddAsync(postExpected);
            await _context.SaveChangesAsync();
        
            //Act
            var result = await _postService.GetPosts(_userId);
            
            Assert.True(result.Succeeded); // Assert
            
            var postActual = result.Content.FirstOrDefault();
        
            //Assert
            Assert.AreEqual(postExpected.Title, postActual.Title);
            Assert.AreEqual(postExpected.Content, postActual.Content);
            Assert.AreEqual(postExpected.DateTime, postActual.DateTime);
        }

        [Test]
        public async Task GetPosts_Return_Error_When_User_Wasnot_Provide()
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
            var result = await _postService.GetPosts(_wrongUserId);
        
            //Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }
        
        [Test]
        public async Task GetPosts_Return_Error_When_User_Dont_Have_Posts()
        {
            //Arrange
            
            //Act
            var result = await _postService.GetPosts(_userId);
            
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
            var result = await _postService.CreatePost(_userId, postDto);
            var actualPost = await _context.Posts.ToListAsync();
        
            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(1, actualPost.Count);
            Assert.AreEqual(postDto.Title, actualPost.First().Title);
            Assert.AreEqual(postDto.Content, actualPost.First().Content);
            Assert.AreEqual(_userId, actualPost.First().UserId);
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
            var result = await _postService.CreatePost(_wrongUserId, postDto);
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
            var result = await _postService.CreatePost(_userId, postDto);
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
            var result = await _postService.CreatePost(_userId, postDto);
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
            var result = await _postService.CreatePost(_userId, null);
            var posts = await _context.Posts.ToListAsync();
        
            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("PostDtoNotFilled"), result.ErrorMessage);
            Assert.AreEqual(0, posts.Count);
        }
        
        #endregion
        
        #region RemovePost
        
        [Test]
        public async Task RemovePost_Should_Remove_Post_Correctly()
        {
            // Arrange
            var post = new Post
            {
                Id = 3,
                Title = "Post title",
                Content = "Content of my post",
                DateTime = DateTime.Now,
                UserId = _userId
            };
            await _context.AddAsync(post);
            await _context.SaveChangesAsync();
        
            // Act
            var result = await _postService.RemovePost(_userId, post.Id);
            var posts = await _context.Posts.ToListAsync();
        
            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(0, posts.Count);
        }
        
        [Test]
        public void RemovePost_Should_Return_Error_When_User_Dont_Have_Post_With_Given_Id()
        {
            
        }
        
        #endregion
    }
}