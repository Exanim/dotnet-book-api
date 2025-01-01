/*
 * Review Microservice
 *
 * This microservice is a part of our Intern playground API collection. Its intended function is to store reviews of products, and be able to search for reviews based on the author of the review and/or the product the review is written about. 
 *
 * The version of the OpenAPI document: 1.0
 * Contact: martin-tibor.sandor@capgemini.com
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class SystemApiController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>This operation can be used as a liveness probe to check if the system is alive</remarks>
        /// <response code="204">The service is available</response>
        /// <response code="500">Response when something unexpected occured server-side. Possible error codes: GenericError</response>
        [HttpGet]
        [Route("/healthz")]
        [SwaggerOperation("HealthzGet")]
        [SwaggerResponse(statusCode: 500, type: typeof(GenericErrorDto), description: "Response when something unexpected occured server-side. Possible error codes: GenericError")]
        public virtual IActionResult HealthzGet()
        {
            return StatusCode(204);
        }
    }
}