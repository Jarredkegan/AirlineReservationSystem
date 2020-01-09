using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    class clsPassengers
    {

        #region Attributes
        /// <summary>
        /// Passenger_ID
        /// </summary>
        public string sID { get; set; }

        /// <summary>
        /// Passenger's First_Name
        /// </summary>
        public string sFirstName { get; set; }

        /// <summary>
        /// Passenger's Last_Name
        /// </summary>
        public string sLastName { get; set; }

        /// <summary>
        /// Passanger's Seat Number
        /// </summary>
        public int iSeat { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Override ToString to display correctly in a combo box
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return sFirstName + " " + sLastName;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

    }
}
