
using Coterie.Api.DTOs;
using Coterie.Api.Mappers;
using Coterie.Api.Models.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using Coterie.Api.Models.Requests;

namespace Coterie.UnitTests.MapperTests
{
    [TestFixture]
    public class QuoteMapperTests
    {
        [Test]
        public void TryMapToDomain_ValidInput_ReturnsMappedModel()
        {
            var dto = new QuoteRequestDto
            {
                Business = "Plumber",
                Revenue = 1000000,
                States = new List<string> { "TX", "OH" }
            };

            var success = QuoteMapper.TryMapToDomain(dto, out QuoteRequest domainModel, out List<string> errors);

            Assert.IsTrue(success);
            Assert.IsNotNull(domainModel);
            Assert.AreEqual(BusinessType.Plumber, domainModel.Business);
            Assert.AreEqual(2, domainModel.States.Count);
            Assert.AreEqual(StateCode.TX, domainModel.States[0]);
            Assert.AreEqual(StateCode.OH, domainModel.States[1]);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void TryMapToDomain_InvalidBusiness_ReturnsError()
        {
            var dto = new QuoteRequestDto
            {
                Business = "Mechanic",
                Revenue = 1000000,
                States = new List<string> { "TX" }
            };

            var success = QuoteMapper.TryMapToDomain(dto, out _, out List<string> errors);

            Assert.IsFalse(success);
            Assert.AreEqual(1, errors.Count);
            Assert.That(errors[0], Does.Contain($"Unsupported business type: 'Mechanic'"));
        }

        [Test]
        public void TryMapToDomain_UnsupportedState_ReturnsError()
        {
            var dto = new QuoteRequestDto
            {
                Business = "Plumber",
                Revenue = 1000000,
                States = new List<string> { "TX", "NEVADA" }
            };

            var success = QuoteMapper.TryMapToDomain(dto, out _, out List<string> errors);

            Assert.IsFalse(success);
            Assert.AreEqual(1, errors.Count);
            Assert.That(errors[0], Does.Contain($"Unsupported state: 'NEVADA'"));
        }

        [Test]
        public void TryMapToDomain_AllInvalidStates_ReturnsErrors()
        {
            var dto = new QuoteRequestDto
            {
                Business = "Plumber",
                Revenue = 1000000,
                States = new List<string> { "CALIFORNIA", "NEVADA" }
            };

            var success = QuoteMapper.TryMapToDomain(dto, out _, out List<string> errors);

            Assert.IsFalse(success);
            Assert.AreEqual(2, errors.Count);
        }
    }
}
