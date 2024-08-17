using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace EmailBodyFunction
{
    public class EmailBody
    {
        private readonly ILogger<EmailBody> _logger;

        public EmailBody(ILogger<EmailBody> logger)
        {
            _logger = logger;
        }

        [Function("EmailBody")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            //Parse query parameter
            string emailBodyContent = await new StreamReader(req.Body).ReadToEndAsync();

            //Replace HTML with other characters
            string updatedBody = Regex.Replace(emailBodyContent, "<.*?>",string.Empty);
            updatedBody = updatedBody.Replace("\\r\\n", " ");
            updatedBody = updatedBody.Replace(@"&nbsp;", " ");

            //Return cleaned text
            return (ActionResult)new OkObjectResult(new { updatedBody });
        }
    }
}
