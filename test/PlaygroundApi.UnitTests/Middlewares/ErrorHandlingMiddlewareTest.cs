using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PlaygroundApi.Domain.Exceptions;
using PlaygroundApi.Middlewares;

namespace PlaygroundApi.UnitTests.Middlewares
{
    [TestFixture]
    public class ErrorHandlingMiddlewareTest
    {
        private Faker _faker;

        [SetUp]
        public void Initialize()
        {
            _faker = new Faker();
        }

        [Test]
        public async Task ErrorHandlingMiddleware_ReturnsError500_WithUnknownException()
        {
            var exception = _faker.System.Exception();
            var expectedErrorMessage = exception.Message;
            await VerifyErrorHandling(exception, expectedErrorMessage, HttpStatusCode.InternalServerError, ApiErrorCode.InternalError);
        }

        [Test]
        public async Task ErrorHandlingMiddleware_ReturnsError400_WithValidationApiException()
        {
            var apiErrorCode = _faker.PickRandom<ApiErrorCode>();
            var expectedErrorMessage = _faker.Lorem.Sentence();
            var exception = new ValidationApiException(apiErrorCode, expectedErrorMessage);
            await VerifyErrorHandling(exception, expectedErrorMessage, HttpStatusCode.BadRequest, apiErrorCode);
        }

        [Test]
        public async Task ErrorHandlingMiddleware_ReturnsError404_WithResourceNotFoundInnerException()
        {
            var apiErrorCode = _faker.PickRandom<ApiErrorCode>();
            var expectedErrorMessage = _faker.Lorem.Sentence();
            var exception = new AggregateException("Aggregate exception", new ResourceNotFoundApiException(apiErrorCode, expectedErrorMessage));
            await VerifyErrorHandling(exception, expectedErrorMessage, HttpStatusCode.NotFound, apiErrorCode);
        }

        private async Task VerifyErrorHandling(Exception exception, string expectedErrorMessage, HttpStatusCode expectedResponseCode, ApiErrorCode expectedApiErrorCode)
        {
            // Arrange
            RequestDelegate next = (innerHttpContext) => Task.FromException<Exception>(exception);
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var contextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            var body = new MemoryStream();
            int responseCode = 0;
            loggerMock.Setup(m => m.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains(expectedErrorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ));
            contextMock.SetupGet(x => x.Response).Returns(responseMock.Object);
            responseMock.SetupGet(x => x.Body).Returns(body);
            responseMock.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(y => responseCode = y);

            var middleware = new ErrorHandlingMiddleware(next: next, logger: loggerMock.Object);

            // Act
            await middleware.Invoke(contextMock.Object);
            body.Position = 0;
            var serializedResult = Encoding.UTF8.GetString(body.ToArray());
            body.Dispose();

            // Assert
            loggerMock.VerifyAll();
            contextMock.VerifyAll();
            responseMock.VerifyAll();
            Assert.That(responseCode, Is.EqualTo((int)expectedResponseCode));
            Assert.That(serializedResult, Is.Not.Null);
            dynamic myobject = JsonConvert.DeserializeObject(serializedResult);
            string errorMessage = myobject.Error;
            ApiErrorCode apiErrorCode = myobject.ApiErrorCode;
            Assert.That(errorMessage, Is.EqualTo(expectedErrorMessage));
            Assert.That(apiErrorCode, Is.EqualTo(expectedApiErrorCode));
        }
    }
}
