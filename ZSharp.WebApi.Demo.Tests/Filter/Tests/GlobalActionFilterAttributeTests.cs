using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using ZSharp.WebApi.Demo.Common.Filters;
using ZSharp.WebApi.Demo.Common.Attributes;

namespace ZSharp.WebApi.Demo.Tests.Filter.Tests
{
    public class GlobalActionFilterAttributeTests
    {
        protected readonly Mock<HttpActionDescriptor> ActionDescriptorMock = new Mock<HttpActionDescriptor>();
        protected readonly Mock<HttpControllerDescriptor> ControllerDescriptorMock = new Mock<HttpControllerDescriptor>();
        protected HttpActionContext HttpActionContext;
        protected GlobalActionFilterAttribute GlobalActionFilterAttribute;

        public GlobalActionFilterAttributeTests()
        {
            HttpActionContext = ContextUtil.CreateActionContext();
            GlobalActionFilterAttribute = new GlobalActionFilterAttribute();
        }
    }

    [Binding]
    [Scope(Scenario = @"HttpMethodNotMatched")]
    public class HttpMethodNotMatchedTest : GlobalActionFilterAttributeTests
    {
        [Given(@"非Post方式的请求")]
        public void Given()
        {
            HttpActionContext.Request.Method = HttpMethod.Get;
        }

        [When(@"执行OnActionExecuting方法")]
        public void When()
        {
            GlobalActionFilterAttribute.OnActionExecuting(HttpActionContext);
        }

        [Then(@"Response为空")]
        public void Then()
        {
            Assert.IsNull(HttpActionContext.Response);
        }
    }

    [Binding]
    [Scope(Scenario = @"BypassModelStateValidation")]
    public class BypassModelStateValidationTest : GlobalActionFilterAttributeTests
    {
        [Given(@"BypassModelStateValidationAttribute")]
        public void Given()
        {
            HttpActionContext.Request.Method = HttpMethod.Post;

            HttpActionContext.ActionDescriptor = ActionDescriptorMock.Object;
            ActionDescriptorMock.Setup(m => m.GetCustomAttributes<BypassModelStateValidationAttribute>()).Returns(new Collection<BypassModelStateValidationAttribute>(new[] { new BypassModelStateValidationAttribute() }));

            HttpActionContext.ControllerContext.ControllerDescriptor = ControllerDescriptorMock.Object;
            ControllerDescriptorMock.Setup(m => m.GetCustomAttributes<BypassModelStateValidationAttribute>()).Returns(new Collection<BypassModelStateValidationAttribute>());  
        }

        [When(@"执行OnActionExecuting方法")]
        public void When()
        {
            GlobalActionFilterAttribute.OnActionExecuting(HttpActionContext);
        }

        [Then(@"Response为空")]
        public void Then()
        {
            Assert.IsNull(HttpActionContext.Response);
        }
    }
    
    [Binding]
    [Scope(Scenario = @"ModelStateInvalid")]
    public class ModelStateInvalidTest : GlobalActionFilterAttributeTests
    {
        [Given(@"ModelState错误信息")]
        public void Given()
        {
            HttpActionContext.Request.Method = HttpMethod.Post;

            HttpActionContext.ActionDescriptor = ActionDescriptorMock.Object;
            ActionDescriptorMock.Setup(m => m.GetCustomAttributes<BypassModelStateValidationAttribute>()).Returns(new Collection<BypassModelStateValidationAttribute>());

            HttpActionContext.ControllerContext.ControllerDescriptor = ControllerDescriptorMock.Object;
            ControllerDescriptorMock.Setup(m => m.GetCustomAttributes<BypassModelStateValidationAttribute>()).Returns(new Collection<BypassModelStateValidationAttribute>());

            HttpActionContext.ModelState.AddModelError("stock.Name", "The Name field is required.");
        }

        [When(@"执行OnActionExecuting方法")]
        public void When()
        {
            GlobalActionFilterAttribute.OnActionExecuting(HttpActionContext);
        }

        [Then(@"返回Bad Request")]
        public void Then()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, HttpActionContext.Response.StatusCode);
        }
    }
}