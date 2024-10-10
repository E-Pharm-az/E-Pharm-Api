﻿namespace EPharm.Domain.Dtos.UserDto;

public class UpdateUserDto : EmailDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public int? Zip { get; set; }
 
}