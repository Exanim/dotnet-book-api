using Newtonsoft.Json;
using ReviewAPI.Models;
using System.Runtime.Serialization;
using System.Text;

namespace ReviewAPI.Models
{
    /// <summary>
    /// Represents a review object without the primary key, used to create a new entry in the database
    /// </summary>
    [DataContract]
    public class ReviewForCreationDto : IEquatable<ReviewForCreationDto>
    {
        /// <summary>
        /// The Id of the author of the review. Functionally a foreign key, but since users are stored in a different database, current implementation is TBD
        /// </summary>
        /// <value>The Id of the author of the review. Functionally a foreign key, but since users are stored in a different database, current implementation is TBD</value>
        /// <example>15</example>
        [DataMember(Name = "userId", EmitDefaultValue = true)]
        public int UserId { get; set; }

        /// <summary>
        /// The Id of the product being reviewed. Functionally a foreign key, but since products are stored in a different database, current implementation is TBD
        /// </summary>
        /// <value>The Id of the product being reviewed. Functionally a foreign key, but since products are stored in a different database, current implementation is TBD</value>
        /// <example>420</example>
        [DataMember(Name = "productId", EmitDefaultValue = true)]
        public int ProductId { get; set; }

        /// <summary>
        /// The review contained in the review object
        /// </summary>
        /// <value>The review contained in the review object</value>
        /// <example>The phone works fine, but the price seems overbearing given how little it improves on the previous model. Also, the camera seems to turn on sometimes by itself, is this something to worry about?</example>
        [DataMember(Name = "review", EmitDefaultValue = false)]
        public string ProductReview { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ReviewForCreationDto {\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  ProductReview: ").Append(ProductReview).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ReviewForCreationDto)obj);
        }

        /// <summary>
        /// Returns true if ReviewForCreationDto instances are equal
        /// </summary>
        /// <param name="other">Instance of ReviewForCreationDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReviewForCreationDto other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    UserId == other.UserId ||

                    UserId.Equals(other.UserId)
                ) &&
                (
                    ProductId == other.ProductId ||

                    ProductId.Equals(other.ProductId)
                ) &&
                (
                    ProductReview == other.ProductReview ||
                    ProductReview != null &&
                    ProductReview.Equals(other.ProductReview)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)

                hashCode = hashCode * 59 + UserId.GetHashCode();

                hashCode = hashCode * 59 + ProductId.GetHashCode();
                if (ProductReview != null)
                    hashCode = hashCode * 59 + ProductReview.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(ReviewForCreationDto left, ReviewForCreationDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReviewForCreationDto left, ReviewForCreationDto right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }

}
