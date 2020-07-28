using System;
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
        private IQuestionAndAnswerService _followService;
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
            _followService = new QuestionAndAnswerService(_context, _errorService, mapper);
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
        #endregion

        #region AnswerToQuestion
        #endregion

        #region GetQuestions
        #endregion

        #region GetAnsweredQuestions
        #endregion
    }
}