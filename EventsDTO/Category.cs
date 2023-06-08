using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventsDTO;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Name { get; set; }
}
