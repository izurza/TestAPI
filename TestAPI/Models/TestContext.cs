using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestAPI.Models;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TestTable> TestTables { get; set; }

}
