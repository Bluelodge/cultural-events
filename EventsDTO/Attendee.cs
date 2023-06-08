using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsDTO;

public class Attendee
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(200)]
    public string? LastName { get; set; }

    [Required]
    [StringLength(200)]
    public string? UserName { get; set; }

    [Required]
    [StringLength(255)]
    public string? EmailAddress { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

}
