using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

namespace Ardu.Common;

public class ArduComponent
{
    [Required]
    public string Name {get; set;}
    [Required]
    public string Image {get; set;}
    public bool KillOnExit {get; set;}
}
