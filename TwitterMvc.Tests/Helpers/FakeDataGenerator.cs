using System;
using System.Collections.Generic;
using Bogus;
using TwitterMvc.Dtos;
using TwitterMvc.Enums;
using TwitterMvc.Models;

namespace TwitterMvc.Tests.Helpers
{
    public class FakeDataGenerator
    {
        public CustomUser GetUser()
        {
            var userFaker = new Faker<CustomUser>()
                .RuleFor(o => o.Id, f => Guid.NewGuid().ToString())
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Name, f => f.Person.FirstName)
                .RuleFor(o => o.Lastname, f => f.Person.LastName)
                .RuleFor(o => o.Age, f => f.Random.Int(13, 100))
                .RuleFor(o => o.Gender, f => f.PickRandom<GenderEnum>())
                .RuleFor(o => o.Country, f => f.Person.Address.City);

            return userFaker.Generate();
        }

        public List<CustomUser> GetUsers(int count)
        {
            var userFaker = new Faker<CustomUser>()
                .RuleFor(o => o.Id, f => Guid.NewGuid().ToString())
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Name, f => f.Person.FirstName)
                .RuleFor(o => o.Lastname, f => f.Person.LastName)
                .RuleFor(o => o.Age, f => f.Random.Int(13, 100))
                .RuleFor(o => o.Gender, f => f.PickRandom<GenderEnum>())
                .RuleFor(o => o.Country, f => f.Person.Address.City);

            return userFaker.Generate(count);
        }

        public List<Post> GetPosts(string userId, int count)
        {
            var postFaker = new Faker<Post>()
                .RuleFor(o => o.Id, f => f.IndexGlobal)
                .RuleFor(o => o.Title, f => f.Lorem.Sentence())
                .RuleFor(o => o.Content, f => f.Lorem.Letter(140))
                .RuleFor(o => o.DateTime, f => f.Date.Past())
                .RuleFor(o => o.UserId, f => userId);

            return postFaker.Generate(count);
        }

        public PostDto GetPostDto()
        {
            var postDtoFaker = new Faker<PostDto>()
                .RuleFor(o => o.Title, f => f.Lorem.Sentence())
                .RuleFor(o => o.Content, f => f.Lorem.Letter(140));

            return postDtoFaker.Generate();
        }

        public Question GetQuestion(string senderId, string recieverId)
        {
            var questionFaker = new Faker<Question>()
                .RuleFor(o => o.SenderId, f => senderId)
                .RuleFor(o => o.ReceiverId, f => recieverId)
                .RuleFor(o => o.Message, f => f.Lorem.Sentence())
                .RuleFor(o => o.SentTime, f => f.Date.Past());

            return questionFaker.Generate();
        }
    }
}