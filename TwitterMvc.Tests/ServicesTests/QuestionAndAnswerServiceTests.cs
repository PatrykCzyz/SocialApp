using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TwitterMvc.Data.Context;
using TwitterMvc.Helpers;
using TwitterMvc.Helpers.AutoMapper;
using TwitterMvc.Services;
using TwitterMvc.Services.Interfaces;
using TwitterMvc.Tests.Helpers;

namespace TwitterMvc.Tests.ServicesTests
{
    public class QuestionAndAnswerServiceTests
    {
        private AppDbContext _context;
        private IQuestionAndAnswerService _qnaService;
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
            _qnaService = new QuestionAndAnswerService(_context, _errorService, mapper);
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

        #region SendQuestion
        [Test]
        [Category("SendQuestion")]
        public async Task SendQuestion_Should_Sent_Correctly()
        {
            // Arrange
            var user = _fakeData.GetUser();
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            var message = "Test question!";

            // Act
            var result = await _qnaService.SendQuestion(_userId, user.Id, message);

            // Assert
            Assert.True(result.Succeeded);
            var actualMessage =  (await _context.Questions.FirstOrDefaultAsync(x => x.SenderId == _userId)).Message;
            Assert.AreEqual(message, actualMessage);
        }

        [TestCase(null)]
        [TestCase("")]
        [Category("SendQuestion")]
        public async Task SendQuestion_Should_Return_Error_When_Message_IsEmptyOrNull(string message)
        {
            // Arrange
            var user = _fakeData.GetUser();
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _qnaService.SendQuestion(_userId, user.Id, message);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("EmptyMessage"), result.ErrorMessage);
        }

        [Test]
        [Category("SendQuestion")]
        public async Task SendQuestion_Should_Return_Error_When_Reciever_IsNull()
        {
            // Arrange

            // Act
            var result = await _qnaService.SendQuestion(_userId, null, "some text");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("SendQuestion")]
        public async Task SendQuestion_Should_Return_Error_When_Reciever_DoesntExist()
        {
            // Arrange

            // Act
            var result = await _qnaService.SendQuestion(_userId, _wrongUserId, "some text");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("SendQuestion")]
        public async Task SendQuestion_Should_Return_Error_When_Sender_IsNull()
        {
            // Arrange

            // Act
            var result = await _qnaService.SendQuestion(null, _userId, "some text");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }

        [Test]
        [Category("SendQuestion")]
        public async Task SendQuestion_Should_Return_Error_When_Sender_DoesntExist()
        {
            // Arrange

            // Act
            var result = await _qnaService.SendQuestion(_wrongUserId, _userId, "some text");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError("UserDosentExist"), result.ErrorMessage);
        }
        #endregion

        #region AnswerToQuestion
        #endregion

        #region GetQuestions
        #endregion

        #region GetAnsweredQuestions
        #endregion
    }
}