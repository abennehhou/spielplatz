using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using NUnit.Framework;
using PlaygroundApi.Middlewares;

namespace PlaygroundApi.UnitTests.Middlewares
{
    [TestFixture]
    public class LoggingMiddlewareTest
    {
        [TestCase("http", "localhost", "/test", "?param1=2", "POST", 400)]
        [TestCase("https", "my-domain", "/test1/test2", "", "GET", 200)]
        public async Task LoggingMiddleware_LogsContextInfo(string scheme, string host, string path,
            string queryString, string method, int statusCode)
        {
            // Arrange
            RequestDelegate next = (innerHttpContext) => Task.FromResult(0);
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();
            var contextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();
            var responseMock = new Mock<HttpResponse>();

            contextMock.SetupGet(x => x.Request).Returns(requestMock.Object);
            MockRequestGetDispayedUrl(requestMock, scheme, host, path, method, queryString);
            requestMock.Setup(x => x.Method).Returns(method);
            contextMock.SetupGet(x => x.Response).Returns(responseMock.Object);
            responseMock.SetupGet(x => x.StatusCode).Returns(statusCode);
            var allParts = new[] { scheme, host, path, queryString, method, statusCode.ToString() };

            // Mock logger.LogDebug
            loggerMock.Setup(m => m.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => allParts.All(x => v.ToString().Contains(x))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ));

            var logRequestMiddleware = new LoggingMiddleware(next: next, logger: loggerMock.Object);

            // Act
            await logRequestMiddleware.Invoke(contextMock.Object);

            // Assert
            loggerMock.VerifyAll();
            contextMock.VerifyAll();
            requestMock.VerifyAll();
            responseMock.VerifyAll();
        }

        [TestCase("http", "localhost", "/swagger", "", "POST", 400)]
        public async Task LoggingMiddleware_IgnoresLog_ForSwagger(string scheme, string host, string path,
            string queryString, string method, int statusCode)
        {
            // Arrange
            RequestDelegate next = (innerHttpContext) => Task.FromResult(0);
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();
            var contextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();

            contextMock.SetupGet(x => x.Request).Returns(requestMock.Object);
            MockRequestGetDispayedUrl(requestMock, scheme, host, path, method, queryString);

            var logRequestMiddleware = new LoggingMiddleware(next: next, logger: loggerMock.Object);

            // Act
            await logRequestMiddleware.Invoke(contextMock.Object);

            // Assert
            loggerMock.VerifyAll();
            contextMock.VerifyAll();
            requestMock.VerifyAll();
            loggerMock.Verify(m =>
                m.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Never);
        }

        private void MockRequestGetDispayedUrl(Mock<HttpRequest> requestMock, string scheme, string host, string path, string method, string queryString)
        {
            requestMock.Setup(x => x.Scheme).Returns(scheme);
            requestMock.Setup(x => x.Host).Returns(new HostString(host));
            requestMock.Setup(x => x.Path).Returns(new PathString(path));
            requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
            requestMock.Setup(x => x.QueryString).Returns(new QueryString(queryString));
        }
    }
}
