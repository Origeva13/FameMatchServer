using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class FameMatchDbContext : DbContext
{
    public Casted? GetCasted(string email)
    {
        return this.Casteds.Where(s=>s.User.UserEmail == email).FirstOrDefault();
    }
    public Castor? GetCastor(string email)
    {
        return this.Castors.Where(s=>s.User.UserEmail==email).FirstOrDefault();
    }
}

