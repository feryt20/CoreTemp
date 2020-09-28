using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.Api.Helpers
{
    public class NonWwwRewriteRule : IRule
    {
        public virtual void ApplyRule(RewriteContext context)
        {

            var request = context.HttpContext.Request;
            var path = request.Path.ToString().Trim();
            var response = context.HttpContext.Response;
            if (string.IsNullOrEmpty(path) || path == "/")
            {
                string redirectUrl = "https://qazvinbuy.ir";
                response.Headers[HeaderNames.Location] = redirectUrl;
                response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Result = RuleResult.EndResponse;
            }
            else
            {
                if (request.Host.Value.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                {
                    string redirectUrl = $"{request.Scheme}://{request.Host.Value.Replace("www.", "")}{request.Path}{request.QueryString}";
                    response.Headers[HeaderNames.Location] = redirectUrl;
                    response.StatusCode = StatusCodes.Status301MovedPermanently;
                    context.Result = RuleResult.EndResponse;
                }
            }



        }
    }
}
