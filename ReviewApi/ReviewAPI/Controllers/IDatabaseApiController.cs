using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ReviewAPI.Controllers
{
    [ApiController]
    [Route("/reviews")]
    public abstract class IDatabaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns all reviews in the database
        /// </summary>
        /// <param name="userId">Numeric ID of the author whose reviews we are looking for</param>
        /// <param name="productId">Numeric ID of the product about which we want reviews</param>
        /// <response code="200">Response array of type Review in successful case</response>
        [HttpGet]
        [SwaggerOperation("GetReviews")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ReviewDto>), description: "Response array of type Review in successful case")]
        public abstract Task<ActionResult> GetReviews([FromQuery(Name = "userId")] int? userId, [FromQuery(Name = "productId")] int? productId);

        /// <summary>
        /// Returns a specific review ID
        /// </summary>
        /// <param name="reviewId">Numeric ID of the review to get</param>
        /// <response code="200">Response object of type Review in successful case</response>
        /// <response code="400">Response in case the reviewId is malformed, such as a negative number. Possible error codes: InvalidId</response>
        /// <response code="404">Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound</response>
        [HttpGet("{reviewId}", Name = "GetReviewById")]
        [SwaggerOperation("GetReviewById")]
        [SwaggerResponse(statusCode: 200, type: typeof(ReviewDto), description: "Response object of type Review in successful case")]
        [SwaggerResponse(statusCode: 400, type: typeof(GenericErrorDto), description: "Response in case the reviewId is malformed, such as a negative number. Possible error codes: InvalidId")]
        [SwaggerResponse(statusCode: 404, type: typeof(GenericErrorDto), description: "Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound")]
        public abstract Task<IActionResult> GetReviewById([FromRoute(Name = "reviewId")][Required] int reviewId);

        /// <summary>
        /// Submit a new review
        /// </summary>
        /// <param name="reviewToUpload">The Review object we want to upload</param>
        /// <response code="201">Response if uploading the Review object to the database was successful</response>
        /// <response code="404">Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound, UserNotFound, ProductNotFound</response>
        [HttpPost]
        [SwaggerOperation("CreateReview")]
        [SwaggerResponse(statusCode: 201, type: typeof(ReviewDto), description: "The database entry has been successfully created")]
        [SwaggerResponse(statusCode: 404, type: typeof(GenericErrorDto), description: "Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound, UserNotFound, ProductNotFound")]
        public abstract Task<ActionResult> CreateReview([FromBody] ReviewForCreationDto reviewToUpload);


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Update an existing review</remarks>
        /// <param name="reviewId">Numeric ID of the review to get</param>
        /// <param name="reviewToUpdate">The new values of the Review object</param>
        /// <response code="204">The Review object in the database has been successfully updated</response>
        /// <response code="400">Response in case the reviewId is malformed, such as a negative number. Possible error codes: InvalidId</response>
        /// <response code="404">Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound</response>
        [HttpPut("{reviewId}")]
        [SwaggerOperation("UpdateReview")]
        [SwaggerResponse(statusCode: 204, description: "Entry has been successfully updated")]
        [SwaggerResponse(statusCode: 400, type: typeof(GenericErrorDto), description: "Response in case the reviewId is malformed, such as a negative number. Possible error codes: InvalidId")]
        [SwaggerResponse(statusCode: 404, type: typeof(GenericErrorDto), description: "Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound")]
        public abstract Task<IActionResult> UpdateReview([FromRoute(Name = "reviewId")][Required] int reviewId, [FromBody] ReviewForUpdateDto reviewToUpdate);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Delete an existing review</remarks>
        /// <param name="reviewId">Numeric ID of the review to get</param>
        /// <response code="204">The entry with the specified ID has been successfully deleted from the database</response>
        /// <response code="400">Response in case the reviewId is malformed, such as a negative number. Possible error codes: InvalidId</response>
        /// <response code="404">Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound</response>
        [HttpDelete("{reviewId}")]
        [SwaggerOperation("DeleteReview")]
        [SwaggerResponse(statusCode: 204, description: "Entry successfully deleted")]
        [SwaggerResponse(statusCode: 400, type: typeof(GenericErrorDto), description: "Response in case the reviewId is malformed, such as a negative number. Possible error codes: InvalidId")]
        [SwaggerResponse(statusCode: 404, type: typeof(GenericErrorDto), description: "Response when no element exists in the database with the given ID. Possible error codes: ReviewNotFound")]
        public abstract Task<IActionResult> DeleteReview([FromRoute(Name = "reviewId")][Required] int reviewId);



    }
}
