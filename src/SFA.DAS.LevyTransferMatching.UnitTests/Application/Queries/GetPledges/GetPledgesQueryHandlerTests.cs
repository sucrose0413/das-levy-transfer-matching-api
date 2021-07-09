﻿using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    [TestFixture]
    public class GetPledgesQueryHandlerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Returns_Pledge_Details()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var dbContext = new LevyTransferMatchingDbContext(options);

            var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await dbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            var pledges = _fixture.CreateMany<Pledge>().ToArray();

            for (var i = 0; i < pledges.Length; i++)
            {
                pledges[i].EmployerAccount = employerAccounts[i];
            }
            
            await dbContext.Pledges.AddRangeAsync(pledges);

            await dbContext.SaveChangesAsync();

            var getPledgesQueryHandler = new GetPledgesQueryHandler(dbContext);

            var getPledgesQuery = new GetPledgesQuery();

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            pledges = await dbContext.Pledges.OrderByDescending(x => x.Amount).ToArrayAsync();

            for (var i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].Id, pledges[i].Id);
                Assert.AreEqual(result[i].AccountId, pledges[i].EmployerAccount.Id);
            }
        }
    }
}