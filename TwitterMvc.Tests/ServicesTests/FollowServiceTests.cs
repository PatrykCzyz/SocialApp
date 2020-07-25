using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TwitterMvc.Data.Context;
using TwitterMvc.Helpers;
using TwitterMvc.Helpers.AutoMapper;
using TwitterMvc.Models;
using TwitterMvc.Services;
using TwitterMvc.Services.Interfaces;
using TwitterMvc.Tests.Helpers;

namespace TwitterMvc.Tests.ServicesTests
{
    public class FollowServiceTests
    {
        private AppDbContext _context;
        private IFollowService _followService;
        private string _wrongUserId;
        private string _userId;
        private IErrorService _errorService;
        private FakeDataGenerator _fakeData;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SocialAppTestDb").Options;

            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            _context = new AppDbContext(options);
            _errorService = new ErrorService();
            _followService = new FollowService(_context, _errorService, mapper);
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

        #region Follow
        [Test]
        [Category("Follow")]
        public async Task Follow_Should_FollowUser_Correctly()
        {
            // Arange
            var userToFollow = _fakeData.GetUser();
            await _context.AddAsync(userToFollow);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.Follow(_userId, userToFollow.Id);

            // Assert
            var following = await _context.Follows.Where(x => x.UserId == _userId).ToListAsync();

            Assert.True(result.Succeeded);
            Assert.AreEqual(1, following.Count);
            Assert.AreEqual(userToFollow.Id, following.First().FollowUserId);
        }
        
        [Test]
        [Category("Follow")]
        public async Task Follow_Should_Return_Error_When_UserToFollow_Doesnt_Exist()
        {
            // Arange

            // Act
            var result = await _followService.Follow(_userId, _wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("Follow")]
        public async Task Follow_Should_Return_Error_When_UserToFollow_Is_Null()
        {
            // Arange

            // Act
            var result = await _followService.Follow(_userId, null);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("Follow")]
        public async Task Follow_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arange

            // Act
            var result = await _followService.Follow(_wrongUserId, _userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("Follow")]
        public async Task Follow_Should_Return_Error_When_User_Is_Null()
        {
            // Arange

            // Act
            var result = await _followService.Follow(null, _userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("Follow")]
        public async Task Follow_Should_Return_Error_When_UserToFollow_Is_Already_Followed()
        {
            // Arange
            var followUser = _fakeData.GetUser();
            var follow = new Follow()
            {
                UserId = _userId,
                FollowUserId = followUser.Id
            };
            await _context.AddAsync(followUser);
            await _context.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.Follow(_userId, followUser.Id);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserIsAlreadyFollowed"), result.ErrorMessage);
        }
        #endregion

        #region UnFollow
        [Test]
        [Category("UnFollow")]
        public async Task UnFollow_Should_UnFollowUser_Correctly()
        {
            // Arange
            var followUser = _fakeData.GetUser();
            var follow = new Follow()
            {
                UserId = _userId,
                FollowUserId = followUser.Id
            };
            await _context.AddAsync(followUser);
            await _context.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.UnFollow(_userId, followUser.Id);

            // Assert
            var following = await _context.Follows.Where(x => x.UserId == _userId).ToListAsync();

            Assert.True(result.Succeeded);
            Assert.AreEqual(0, following.Count);
        }

        [Test]
        [Category("UnFollow")]
        public async Task UnFollow_Should_Return_Error_When_UserToUnFollow_Doesnt_Exist()
        {
            // Arange

            // Act
            var result = await _followService.UnFollow(_userId, _wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("UnFollow")]
        public async Task UnFollow_Should_Return_Error_When_UserToUnFollow_Is_Null()
        {
            // Arange

            // Act
            var result = await _followService.UnFollow(_userId, null);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("UnFollow")]
        public async Task UnFollow_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arange

            // Act
            var result = await _followService.UnFollow(_wrongUserId, _userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("UnFollow")]
        public async Task UnFollow_Should_Return_Error_When_User_Is_Null()
        {
            // Arange

            // Act
            var result = await _followService.UnFollow(null, _userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("UnFollow")]
        public async Task UnFollow_Should_Return_Error_When_UserToFollow_Is_Not_Followed()
        {
            // Arange
            var notFollowedUser = _fakeData.GetUser();
            await _context.AddAsync(notFollowedUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.UnFollow(_userId, notFollowedUser.Id);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserIsNotFollowed"), result.ErrorMessage);
        }
        #endregion

        #region GetFollowers
        [Test]
        [Category("GetFollowers")]
        public async Task GetFollowers_Should_Return_AllFollowers_Correctly()
        {
            // Arange
            var usersCount = 15;
            var users = _fakeData.GetUsers(usersCount);
            var follows = users.Select(x => new Follow() { UserId = x.Id, FollowUserId = _userId }).ToList();

            await _context.AddRangeAsync(users);
            await _context.AddRangeAsync(follows);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.GetFollowers(_userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(usersCount, result.Content.Count);
        }

        [Test]
        [Category("GetFollowers")]
        public async Task GetFollowers_Should_Return_Error_When_Dont_Have_Followers()
        {
            // Arange

            // Act
            var result = await _followService.GetFollowers(_userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("DontHaveFollowers"), result.ErrorMessage);
        }

        [Test]
        [Category("GetFollowers")]
        public async Task GetFollowers_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arange

            // Act
            var result = await _followService.GetFollowers(_wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("GetFollowers")]
        public async Task GetFollowers_Should_Return_Error_When_User_Is_Null()
        {
            // Arange

            // Act
            var result = await _followService.GetFollowers(null);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }
        #endregion

        #region GetFollowing
        [Test]
        [Category("GetFollowing")]
        public async Task GetFollowing_Should_Return_AllFollowing_Correctly()
        {
            // Arange
            var usersCount = 15;
            var users = _fakeData.GetUsers(usersCount);
            var follows = users.Select(x => new Follow() { UserId = _userId, FollowUserId = x.Id }).ToList();

            await _context.AddRangeAsync(users);
            await _context.AddRangeAsync(follows);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.GetFollowing(_userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(usersCount, result.Content.Count);
        }

        [Test]
        [Category("GetFollowing")]
        public async Task GetFollowing_Should_Return_Error_When_Dont_Follow_Anyone()
        {
            // Arange

            // Act
            var result = await _followService.GetFollowing(_userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("DontHaveFollowing"), result.ErrorMessage);
        }

        [Test]
        [Category("GetFollowing")]
        public async Task GetFollowing_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arange

            // Act
            var result = await _followService.GetFollowing(_wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("GetFollowing")]
        public async Task GetFollowing_Should_Return_Error_When_User_Is_Null()
        {
            // Arange

            // Act
            var result = await _followService.GetFollowing(null);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }
        #endregion

        #region Followed
        [Test]
        [Category("Followed")]
        public async Task Followed_Should_Return_True_When_User_Follow_SecondUser()
        {
            // Arrange
            var followedUser = _fakeData.GetUser();
            var follow = new Follow(_userId, followedUser.Id);
            await _context.AddAsync(followedUser);
            await _context.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.Followed(_userId, followedUser.Id);

            // Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Content);
        }

        [Test]
        [Category("Followed")]
        public async Task Followed_Should_Return_False_When_User_NotFollow_SecondUser()
        {
            // Arrange
            var followedUser = _fakeData.GetUser();
            await _context.AddAsync(followedUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.Followed(_userId, followedUser.Id);

            // Assert
            Assert.True(result.Succeeded);
            Assert.False(result.Content);
        }

        [Test]
        [Category("Followed")]
        public async Task Followed_Should_Return_Error_When_User_NotExist()
        {
            // Arrange
            var followedUser = _fakeData.GetUser();
            await _context.AddAsync(followedUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _followService.Followed(_wrongUserId, followedUser.Id);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("Followed")]
        public async Task Followed_Should_Return_Error_When_SecondUser_NotExist()
        {
            // Arrange

            // Act
            var result = await _followService.Followed(_userId, _wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }
        #endregion
    }
}
