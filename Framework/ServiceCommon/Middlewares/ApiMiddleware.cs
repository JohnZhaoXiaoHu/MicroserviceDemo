﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceCommon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCommon
{
    /// <summary>
    /// https://elanderson.net/2017/02/log-requests-and-responses-in-asp-net-core/
    /// https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling
    /// </summary>
    public class ApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserPermissionCache _userPermissionCache;

        public ApiMiddleware(RequestDelegate next, IUserPermissionCache userPermissionCache)
        {
            _next = next;
            _userPermissionCache = userPermissionCache;
        }

        public async Task Invoke(HttpContext context)
        {
            Guid logID = Guid.NewGuid();
            DateTime requestTime = DateTime.UtcNow;
            string requestTenant = "", requestUser = "", requestBody = "";
            Exception invokeException = null;
            DateTime reponseTime = requestTime;
            string responseBody = "";

            try
            {
                if (context.Request.ContentType != null && !context.Request.ContentType.Contains("multipart/form-data"))
                {
                    requestBody = await GetRequestBody(context.Request);
                }
                if (context.Request.Path.HasValue && !context.Request.Path.Value.StartsWith("/swagger") && !context.Request.Path.Value.StartsWith("/api/heathcheck"))
                {
                    Claim userInfoClaim = context.User.FindFirst("current_user_info");
                    UserInfo currentUserInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoClaim.Value);
                    requestTenant = currentUserInfo.TenantCode;
                    requestUser = currentUserInfo.UserName;
                    context.Items.Add("CurrentUserInfo", currentUserInfo);

                    UserPermission userPermission = await _userPermissionCache.GetCurrentUserPermission();
                    context.Items.Add("CurrentUserPermission", userPermission);

                    var originalBodyStream = context.Response.Body;
                    using (var memoryResponseBody = new MemoryStream())
                    {
                        context.Response.Body = memoryResponseBody;
                        await _next(context);
                        responseBody = await GetResponseBody(context.Response);
                        await memoryResponseBody.CopyToAsync(originalBodyStream);
                    }
                }
                else
                {
                    await _next(context);
                }
               
            }
            catch (Exception ex)
            {
                invokeException = ex;
                context.Response.ContentType = "application/json";
                FriendlyException friendlyException = invokeException as FriendlyException;
                if (friendlyException != null)
                {
                    switch (friendlyException.ExceptionCode)
                    {
                        case 401:
                            friendlyException.ExceptionMessage = "Unauthorized user.";
                            break;
                        case 403:
                            friendlyException.ExceptionMessage = "Current user have no permission for this.";
                            break;
                        case 404:
                            friendlyException.ExceptionMessage = "This item does not exists.";
                            break;
                        case 409:
                            friendlyException.ExceptionMessage = "This item already exists.";
                            break;
                    }
                    context.Response.StatusCode = friendlyException.ExceptionCode;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        FriendlyExceptionMessage = friendlyException.ExceptionMessage
                    }));
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        FriendlyExceptionMessage = $"Internal Server Error, Error ID is {logID}, Please contact the administrator to handle."
                    }));
                }
            }
            finally
            {
                reponseTime = DateTime.UtcNow;
                if (context.Request.Path.HasValue && !context.Request.Path.Value.StartsWith("/swagger") && !context.Request.Path.Value.StartsWith("/api/heathcheck"))
                {
                    NlogHelper.LogApiRequestAndResponse(logID, context, requestTime, requestTenant, requestUser, requestBody, invokeException, reponseTime, responseBody);
                }
            }
           
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            //var body = request.Body;
            request.EnableRewind();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyText = Encoding.UTF8.GetString(buffer);
            //request.Body = body;
            request.Body.Position = 0;

            return bodyText;
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return bodyText;
        }
    }

    public static class ApiMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiMiddleware>();
        }
    }

    
}
