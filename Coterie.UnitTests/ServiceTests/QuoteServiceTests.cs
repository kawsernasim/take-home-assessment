
using System;
using Coterie.Api.Models.Configs;
using Coterie.Api.Models.Enums;
using Coterie.Api.Services;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using Coterie.Api.Models.Requests;
using Microsoft.Extensions.Logging;
using Moq;

namespace Coterie.UnitTests.ServiceTests
{
    [TestFixture]
    public class QuoteServiceTests
    {
        private QuoteService _quoteService;
        private Mock<ILogger<QuoteService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<QuoteService>>();
            var quoteFactors = new QuoteFactorConfigs()
            {
                BasePremium = 1000,
                HazardFactor = 1.2m,
                BusinessFactors = new Dictionary<BusinessType, decimal>
                {
                    { BusinessType.Plumber, 1.1m },
                    { BusinessType.Architect, 1.3m }
                },
                StateFactors = new Dictionary<StateCode, decimal>
                {
                    { StateCode.TX, 1.0m },
                    { StateCode.FL, 1.2m },
                    { StateCode.OH, 1.1m }
                }
            };

            var options = Options.Create(quoteFactors);
            _quoteService = new QuoteService(options, _mockLogger.Object);
        }

        [Test]
        public void GetQuote_ValidInput_ReturnsCorrectPremiums()
        {
            var request = new QuoteRequest
            {
                Business = BusinessType.Plumber,
                Revenue = 6000000,
                States = new List<StateCode> { StateCode.TX, StateCode.OH }
            };

            var result = _quoteService.CalculateQuote(request);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(2, result.Data?.Premiums.Count);
            Assert.AreEqual(StateCode.TX, result.Data.Premiums[0].State);
            Assert.AreEqual(StateCode.OH, result.Data.Premiums[1].State);
            Assert.AreEqual(1320m, result.Data.Premiums[0].Premium);
            Assert.AreEqual(1452m, result.Data.Premiums[1].Premium);
        }

        [Test]
        public void GetQuote_UnsupportedBusiness_ReturnsError()
        {
            var request = new QuoteRequest
            {
                Business = (BusinessType)999,
                Revenue = 1000000,
                States = new List<StateCode> { StateCode.TX }
            };

            var result = _quoteService.CalculateQuote(request);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNull(result.Data);
        }

        [Test]
        public void GetQuote_UnsupportedState_ReturnsError()
        {
            var request = new QuoteRequest
            {
                Business = BusinessType.Plumber,
                Revenue = 1000000,
                States = new List<StateCode> { (StateCode)125 }
            };

            var result = _quoteService.CalculateQuote(request);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNull(result.Data);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.That(result.Errors[0], Does.Contain("Unsupported state: 125"));
        }
        
        [Test]
        public void GetQuote_MixedValidAndInvalidStates_ReturnsError()
        {
            var request = new QuoteRequest
            {
                Business = BusinessType.Plumber,
                Revenue = 1000000,
                States = new List<StateCode> { StateCode.TX, (StateCode)123 }
            };

            var result = _quoteService.CalculateQuote(request);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNull(result.Data);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.That(result.Errors[0], Does.Contain("Unsupported state: 123"));
        }
        [Test]
        public void GetQuote_EmptyStatesList_ReturnsError()
        {
            var request = new QuoteRequest
            {
                Business = BusinessType.Plumber,
                Revenue = 1000000,
                States = new List<StateCode>()
            };

            var result = _quoteService.CalculateQuote(request);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNull(result.Data);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void GetQuote_NullStatesList_ReturnsError()
        {
            var request = new QuoteRequest
            {
                Business = BusinessType.Plumber,
                Revenue = 1000000,
                States = null
            };

            Assert.Throws<NullReferenceException>(() => _quoteService.CalculateQuote(request));
        }
    }
}
