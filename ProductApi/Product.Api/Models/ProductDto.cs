/*
 * Products Microservice
 *
 * A service to post and get product data
 *
 * The version of the OpenAPI document: 2.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
//using Org.OpenAPITools.Converters;

namespace Products.Api.Models
{ 
    /// <summary>
    /// Product data
    /// </summary>
    [DataContract]
    public partial class ProductDto : IEquatable<ProductDto>
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        /// <value>Name of the product</value>
        [Required]
        [DataMember(Name="productName", EmitDefaultValue=false)]
        public string ProductName { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProductDto {\n");
            sb.Append("  ProductName: ").Append(ProductName).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
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
            return obj.GetType() == GetType() && Equals((ProductDto)obj);
        }

        /// <summary>
        /// Returns true if ProductDto instances are equal
        /// </summary>
        /// <param name="other">Instance of ProductDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProductDto other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ProductName == other.ProductName ||
                    ProductName != null &&
                    ProductName.Equals(other.ProductName)
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
                    if (ProductName != null)
                    hashCode = hashCode * 59 + ProductName.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ProductDto left, ProductDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductDto left, ProductDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
