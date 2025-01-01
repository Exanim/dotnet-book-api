using Newtonsoft.Json;
using RatingApi.Models;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace RatingApi.Models
{
    [DataContract]
    public class RatingForCreationDto : IEquatable<RatingForCreationDto>
    {
        /// <value>The Id of the author of the rating.</value>
        [DataMember(Name = "userId", EmitDefaultValue = true)]
        public int UserId { get; set; }

        /// <value>The Id of the product being rated.</value>
        [DataMember(Name = "productId", EmitDefaultValue = true)]
        public int ProductId { get; set; }

        /// <value>The rating in the rating object</value>
        [DataMember(Name = "rating", EmitDefaultValue = false)]
        public int RatingValue { get; set; }

        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RatingForCreationDto {\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  Rating: ").Append(RatingValue).Append("\n");
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
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RatingForCreationDto)obj);
        }

        /// <summary>
        /// Returns true if RatingForCreationDto instances are equal
        /// </summary>
        /// <param name="other">Instance of RatingForCreationDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RatingForCreationDto other)
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
                    RatingValue == other.RatingValue ||
                    RatingValue != null &&
                    RatingValue.Equals(other.RatingValue)
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
                if (RatingValue != null)
                    hashCode = hashCode * 59 + RatingValue.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(RatingForCreationDto left, RatingForCreationDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RatingForCreationDto left, RatingForCreationDto right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }

}