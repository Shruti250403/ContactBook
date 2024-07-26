﻿using System.ComponentModel.DataAnnotations;

namespace ClientContactPhonebook.ViewModels
{
    public class ContactViewModel
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(15)]
        public string Company { get; set; }

        public string? Image { get; set; }
    }
}
