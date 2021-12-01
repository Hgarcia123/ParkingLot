using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IParkingContext
    {

        //Properties
        DbSet<Park> Parks { get; }
        DbSet<ParkSpots> ParkSpots { get; }
        DbSet<Spot> Spots { get; }
        DbSet<Entry> Entries { get; }
        //DbSet<Payment> Payments { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        

    }
}
