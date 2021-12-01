using System;
using System.Collections.Generic;
using System.Text;

using Application.Interfaces;

namespace Application
{
    public class ArgException
    {
        public void NoParkException(int ParkId)
        {
            throw new ArgumentException(String.Format("ParkId {0} does not exist", ParkId), "ParkId");
        }

        public void NoParksCreatedException()
        {
            throw new ArgumentException(String.Format("No Parks yet Created"));
        }

        public void NoParkSpotException(int ParkId)
        {
            throw new ArgumentException(String.Format("No Spots are associated with ParkId {0}", ParkId), "ParkId");
        }

        public void NoEntryException(int SpotId)
        {
            throw new ArgumentException(String.Format("No Entries for Spot {0} ", SpotId), "SpotId");
        }

        public void InvalidDatesException()
        {
            throw new ArgumentException(String.Format("Dates inserted are invalid! FromDate must be older than ToDate"));
        }

        //public void PaymentArgumentException(int PaymentId)
        //{
        //    throw new ArgumentException(String.Format("PaymentId {0} does not exist", PaymentId), "PaymentId");
        //}
    }
}
