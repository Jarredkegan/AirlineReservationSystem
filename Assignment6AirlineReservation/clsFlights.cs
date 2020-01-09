using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    class clsFlights
    {

        #region Attributes

        /// <summary>
        /// Holds the Flight_ID
        /// </summary>
        public int iID { get; set; }

        /// <summary>
        /// Holds the Flight_Number
        /// </summary>
        public int iFlightNumber { get; set; }

        /// <summary>
        /// Holds the Flight_Name
        /// </summary>
        public string sFlightName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Override ToString to display correctly in a combo box
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return iFlightNumber + " - " + sFlightName;
        }

        #endregion
    }
}
