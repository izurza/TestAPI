﻿using System;
using System.Collections.Generic;

namespace TestAPI.Models;

public partial class TodoItem
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    public string? Secret { get; set; }

    public byte[]? Image { get; set; }
    
}
