using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IArgException
    {
        void ParkArgumentException(int ParkId);
        void SpotArgumentException(int SpotId);
        void EntryArgumentException(int EntryId);
        void OccupiedSpotException(int ParkId, int SpotId);
        void FreeSpotException(int ParkId, int SpotId, int EntryId);
        //void PaymentArgumentException(int PaymentId);

    }
}
