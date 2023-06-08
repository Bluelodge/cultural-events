using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventsDTO;

public class Organization
{
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string? Name { get; set; }

    [Required]
    [StringLength(500)]
    public string? CorporateName { get; set; }

    [StringLength(1000)]
    public string? WebSite { get; set; }

}
