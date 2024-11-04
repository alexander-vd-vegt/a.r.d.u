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
    /// <summary>
    /// Must the container be killed when the supervisor exits?
    /// </summary>
    public bool KillOnExit {get; set;}
    /// <summary>
    /// ports to be exposed on the host, notation is host:container
    /// </summary>
    public Dictionary<string, string> Ports {get; set;}
}


public class ArduComponentStatus : ArduComponent
{
    public string Status {get; set;}
    public string State {get; set;}
}