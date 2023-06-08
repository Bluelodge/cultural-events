using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventsDTO;

public class Event
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Title { get; set; }
}
