using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Model
{
    public class Customer
    {
        public int ID { get; set; }

        private string gender;

        //Only Male/Female/Unknown
        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                if (!value.Equals("Male") && !value.Equals("Female") && !value.Equals("Unknown"))
                {
                    throw new ArgumentException("The Gender is not valid!");
                }
                gender = value;
            }
        }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(75)]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(75)]
        public string Street { get; set; }

        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(75)]
        public string Town { get; set; }

        public List<Rental> Rentals { get; set; }
    }
}
