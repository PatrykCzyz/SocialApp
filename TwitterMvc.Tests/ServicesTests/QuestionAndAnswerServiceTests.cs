using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TwitterMvc.Data.Context;
using TwitterMvc.Helpers;
using TwitterMvc.Helpers.AutoMapper;
using TwitterMvc.Helpers.ErrorHandler;
using TwitterMvc.Models;
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
            var actualQuestion =  (await _context.Questions.FirstOrDefaultAsync(x => x.SenderId == _userId));
            Assert.AreEqual(message, actualQuestion.Message);
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
            Assert.AreEqual(_errorService.GetError(Error.EmptyMessage), result.ErrorMessage);
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
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
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
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
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
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
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
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
        }
        #endregion

        #region AnswerToQuestion

        [Test]
        [Category("AnswerToQuestion")]
        public async Task AnswerToQuestion_Should_Answered_Correctly()
        {
            // Arrange
            var user = _fakeData.GetUser();
            var question = _fakeData.GetQuestion(user.Id, _userId);
            await _context.AddAsync(user);
            await _context.AddAsync(question);
            await _context.SaveChangesAsync();

            var answerText = "Some answer.";

            // Act
            var result = await _qnaService.AnswerToQuestion(_userId, question.Id, answerText);

            // Assert
            Assert.True(result.Succeeded);
            var actualAnswer = await _context.Answers.FirstOrDefaultAsync(x => x.QuestionId == question.Id);
            Assert.NotNull(actualAnswer);
            Assert.AreEqual(answerText, actualAnswer.Message);
        }

        [Test]
        [Category("AnswerToQuestion")]
        public async Task AnswerToQuestion_Should_Return_Error_When_Invalid_User()
        {
            // Arrange
            var reciever = _fakeData.GetUser();
            var question = _fakeData.GetQuestion(_userId, reciever.Id);
            await _context.AddAsync(reciever);
            await _context.AddAsync(question);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _qnaService.AnswerToQuestion(_userId, question.Id, "Some answer");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.QuestionDoesntExist), result.ErrorMessage);
            var actualAnswer = await _context.Answers.FirstOrDefaultAsync(x => x.QuestionId == question.Id);
            Assert.Null(actualAnswer);
        }
        
        [Test]
        [Category("AnswerToQuestion")]
        public async Task AnswerToQuestion_Should_Return_Error_When_Question_Doesnt_Exist()
        {
            // Arrange
            var questionId = 5;

            // Act
            var result = await _qnaService.AnswerToQuestion(_userId, questionId, "Some answer");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.QuestionDoesntExist), result.ErrorMessage);
            var actualAnswer = await _context.Answers.FirstOrDefaultAsync(x => x.QuestionId == questionId);
            Assert.Null(actualAnswer);
        }

        [TestCase(null)]
        [TestCase("")]
        [Category("AnswerToQuestion")]
        public async Task AnswerToQuestion_Should_Return_Error_When_AnswerText_Is_NullOrEmpty(string answer)
        {
            // Arrange
            var sender = _fakeData.GetUser();
            var question = _fakeData.GetQuestion(sender.Id, _userId);
            await _context.AddAsync(sender);
            await _context.AddAsync(question);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _qnaService.AnswerToQuestion(_userId, question.Id, answer);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.EmptyAnswer), result.ErrorMessage);
            var actualAnswer = await _context.Answers.FirstOrDefaultAsync(x => x.QuestionId == question.Id);
            Assert.Null(actualAnswer);
        }

        [TestCase(null)]
        [TestCase("")]
        [Category("AnswerToQuestion")]
        public async Task AnswerToQuestion_Should_Return_Error_When_User_Is_NullOrEmpty(string userId)
        {
            // Arrange
            var sender = _fakeData.GetUser();
            var question = _fakeData.GetQuestion(sender.Id, _userId);
            await _context.AddAsync(sender);
            await _context.AddAsync(question);
            await _context.SaveChangesAsync();

            // Act
            var result = await _qnaService.AnswerToQuestion(userId, question.Id, "Some answer");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
            var actualAnswer = await _context.Answers.FirstOrDefaultAsync(x => x.QuestionId == question.Id);
            Assert.Null(actualAnswer);
        }
        
        [Test]
        [Category("AnswerToQuestion")]
        public async Task AnswerToQuestion_Should_Return_Error_When_Question_Is_Already_Answered()
        {
            // Arrange
            var sender = _fakeData.GetUser();
            var question = _fakeData.GetQuestion(sender.Id, _userId);
            await _context.AddAsync(sender);
            await _context.AddAsync(question);
            var answer = new Answer()
            {
                AnsweredTime = DateTime.Now,
                Message = "Some answer",
                QuestionId = question.Id
            };
            await _context.AddAsync(answer);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _qnaService.AnswerToQuestion(_userId, question.Id, "Some other answer");

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.AlreadyAnswered), result.ErrorMessage);
        }
        
        #endregion

        #region GetQuestions

        [Test]
        [Category("GetQuestions")]
        public async Task GetQuestions_Should_GetQuestions_Correctly()
        {
            // Arrange
            var questionsWithoutAnswerId = new List<int>();
            
            var sender = _fakeData.GetUser();
            await _context.AddAsync(sender);
            for (var i = 0; i < 6; i++)
            {
                await _context.AddAsync(new Question
                {
                    Id = i,
                    SenderId = sender.Id,
                    ReceiverId = _userId,
                    Message = "Some question",
                    SentTime = DateTime.Now
                });

                if (i % 2 != 0)
                {
                    questionsWithoutAnswerId.Add(i);
                    continue;
                }
                
                await _context.AddAsync(new Answer
                {
                    Message = "Some answer",
                    AnsweredTime = DateTime.Now,
                    QuestionId = i
                });
            }

            // Act
            var result = await _qnaService.GetQuestions(_userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(questionsWithoutAnswerId.Count, result.Content.Count);
            result.Content.ForEach(x => Assert.True(questionsWithoutAnswerId.Contains(x.QuestionId)));
        }
        
        [Test]
        [Category("GetQuestions")]
        public async Task GetQuestions_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arrange
            

            // Act
            var result = await _qnaService.GetQuestions(_wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
        }
        
        [TestCase(null)]
        [TestCase("")]
        [Category("GetQuestions")]
        public async Task GetQuestions_Should_Return_Error_When_User_Is_NullOrEmpty(string userId)
        {
            // Arrange
            

            // Act
            var result = await _qnaService.GetQuestions(userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
        }
        
        #endregion

        #region GetAnsweredQuestions
        
        [Test]
        [Category("GetAnsweredQuestions")]
        public async Task GetAnsweredQuestions_Should_GetAnsweredQuestions_Correctly()
        {
            // Arrange
            var questionsWithAnswerCount = 0;
            
            var sender = _fakeData.GetUser();
            await _context.AddAsync(sender);
            for (var i = 0; i < 6; i++)
            {
                await _context.AddAsync(new Question
                {
                    Id = i,
                    SenderId = sender.Id,
                    ReceiverId = _userId,
                    Message = "Some question",
                    SentTime = DateTime.Now
                });

                if (i % 2 != 0) continue;
                
                await _context.AddAsync(new Answer
                {
                    Message = "Some answer",
                    AnsweredTime = DateTime.Now,
                    QuestionId = i
                });
                questionsWithAnswerCount++;
            }

            // Act
            var result = await _qnaService.GetAnsweredQuestions(_userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.AreEqual(questionsWithAnswerCount, result.Content.Count);
            result.Content.ForEach(x => Assert.True(x.QuestionMessage != null && x.AnswerMessage != null));
        }
        
        [Test]
        [Category("GetAnsweredQuestions")]
        public async Task GetAnsweredQuestions_Should_Return_Error_When_User_Doesnt_Exist()
        {
            // Arrange
            

            // Act
            var result = await _qnaService.GetAnsweredQuestions(_wrongUserId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
        }

        [TestCase(null)]
        [TestCase("")]
        [Category("GetAnsweredQuestions")]
        public async Task GetAnsweredQuestions_Should_Return_Error_When_User_Is_NullOrEmpty(string userId)
        {
            // Arrange
            

            // Act
            var result = await _qnaService.GetAnsweredQuestions(userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.AreEqual(_errorService.GetError(Error.UserDosentExist), result.ErrorMessage);
        }

        #endregion
    }
}