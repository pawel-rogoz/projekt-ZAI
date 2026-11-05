using System.Net;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace IoTAnalyzer.API.Configurations;

public static class ResultConventionConfig
{
    public static MvcOptions AddApiResultConvention(this MvcOptions options) =>
        options.AddResultConvention(map => map
            .AddDefaultMap()
            .For(ResultStatus.NotFound, HttpStatusCode.NotFound, o => o.With(Problem(HttpStatusCode.NotFound, "Not Found")))
            .For(ResultStatus.Invalid, HttpStatusCode.UnprocessableEntity, o => o.With(Problem(HttpStatusCode.UnprocessableEntity, "Unprocessable Entity")))
            .For(ResultStatus.Conflict, HttpStatusCode.Conflict, o => o.With(Problem(HttpStatusCode.Conflict, "Conflict")))
            .For(ResultStatus.Unauthorized, HttpStatusCode.Unauthorized, o => o.With(Problem(HttpStatusCode.Unauthorized, "Unauthorized"))));

    private static Func<ControllerBase, Ardalis.Result.IResult, ProblemDetails> Problem(HttpStatusCode status, string title) =>
        (controller, result) =>
        {
            var problem = new ProblemDetails
            {
                Type = "about:blank",
                Title = title,
                Status = (int)status,
                Instance = controller.HttpContext.Request.Path,
            };

            var details = result.Errors.Concat(result.ValidationErrors.Select(e => e.ErrorMessage)).ToList();
            if (details.Count > 0)
                problem.Detail = string.Join(" ", details);

            if (result.ValidationErrors.Any())
                problem.Extensions["errors"] = result.ValidationErrors
                    .Select(e => new { field = e.Identifier, message = e.ErrorMessage })
                    .ToList();

            return problem;
        };
}
