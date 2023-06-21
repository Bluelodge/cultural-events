using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsDTO;

public class Guest
{
    public int Id { get; set; }

    [Required]
    [StringLength(400)]
    public string? FullName { get; set; }

    [Required]
    [StringLength(400)]
    public string? Position { get; set; }

    [Required]
    [StringLength(4000)]
    public string? Bio { get; set; }

    [StringLength(1000)]
    public string? Social { get; set; }

    [StringLength(1000)]
    public string? WebSite { get; set; }

}
