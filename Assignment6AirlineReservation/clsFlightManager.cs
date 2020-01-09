using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    class clsFlightManager
    {

        #region References

        /// <summary>
        /// References to clsDataAccess class
        /// </summary>
        clsDataAccess db;
        clsPassengers passenger;
        //MainWindow wndMainWindow;

        #endregion

        #region Methods

        /// <summary>
        /// Returns a list of Flights
        /// </summary>
        /// <returns></returns>
        public List<clsFlights> GetFlights()
        {
            try
            {

                //local variable that counts the number of rows returned from database
                int iNumRetValues = 0;

                //Our ExecuteSQLStatement returns a data set.
                //Reference a dataset called ds
                DataSet ds;

                //Creates a new list instance of clsFlights class
                List<clsFlights> lstFlights = new List<clsFlights>();

                //SQL that gets all rows from table Flight
                string sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";


                //new instance of clsDataAccess creates an object called db
                db = new clsDataAccess();

                //ds holds our dataset retrieved from database
                ds = db.ExecuteSQLStatement(sSQL, ref iNumRetValues);

                //loop through the dataset ds and add the Flight_Number and Aircraft_Type to our list of Flights
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lstFlights.Add(new clsFlights { iFlightNumber = Convert.ToInt32(row["Flight_Number"]), sFlightName = Convert.ToString(row["Aircraft_Type"]) });
                }

                //returns a list of Flights
                return lstFlights;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Retruns a list of passengers
        /// </summary>
        /// <param name="iFlight_ID"></param>
        /// <returns></returns>
        public List<clsPassengers> GetPassengers(int iFlight_ID)
        {
            try
            {
                //local variable that counts the number of rows returned from database
                int iNumRetValues = 0;

                //Our ExecuteSQLStatement returns a data set.
                //Reference a dataset called ds
                DataSet ds;

                //creates a list new instance of clsPassenger class 
                List<clsPassengers> lstPassengers = new List<clsPassengers>();

                //SQL that gets the first and last name of every passenger on a specific flight
                string sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                              "FROM Passenger, Flight_Passenger_Link FPL " +
                              "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                              "Flight_ID = " + iFlight_ID;

                //new instance of clsDataAccess creates an object called db
                db = new clsDataAccess();

                //ds holds our dataset retrieved from database
                ds = db.ExecuteSQLStatement(sSQL, ref iNumRetValues);

                //loop through the dataset ds and add the first name and last names to our list of passengers
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lstPassengers.Add(new clsPassengers { sFirstName = Convert.ToString(row["First_Name"]), sLastName = Convert.ToString(row["Last_Name"]), iSeat = Convert.ToInt32(row["Seat_Number"]) });
                }

                //returns our list of passengers
                return lstPassengers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sets passengers Seat numbers and returns a list of passengers
        /// </summary>
        /// <param name="iFlight_ID"></param>
        /// <returns></returns>
        public List<clsPassengers> GetSeats(int iFlight_ID)
        {
            try
            {
                //local variable that counts the number of rows returned from database
                int iNumRetValues = 0;

                //Our ExecuteSQLStatement returns a data set.
                //Reference a dataset called ds
                DataSet ds;

                //creates a list new instance of clsPassenger class 
                List<clsPassengers> lstPassengers = new List<clsPassengers>();

                //SQL that gets the first and last name of every passenger on a specific flight
                string sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                              "FROM Passenger, Flight_Passenger_Link FPL " +
                              "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                              "Flight_ID = " + iFlight_ID;

                //new instance of clsDataAccess creates an object called db
                db = new clsDataAccess();

                //ds holds our dataset retrieved from database
                ds = db.ExecuteSQLStatement(sSQL, ref iNumRetValues);

                //loop through the dataset ds and add the first name and last names to our list of passengers
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lstPassengers.Add(new clsPassengers { iSeat = Convert.ToInt32(row["Seat_Number"]) });
                }

                //returns our list of passengers
                return lstPassengers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL for adding a new passenger
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="iFlightID"></param>
        public void AddPassenger(string FirstName, string LastName, int iFlightID)
        {
            try
            {
                //new instance of clsPassengers class
                passenger = new clsPassengers();

                //set passengers first name
                passenger.sFirstName = FirstName;

                //set passengers last name
                passenger.sLastName = LastName;

                //SQL that adds passenger to database
                string sSQL = "INSERT INTO PASSENGER(First_Name, Last_Name) VALUES('" + FirstName + "','" + LastName + "')";

                //new instance of clsDataAccess creates an object called db
                db = new clsDataAccess();

                //excute
                db.ExecuteNonQuery(sSQL);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// SQL for adding a new passengers data base table link
        /// </summary>
        /// <param name="iFlightID"></param>
        /// <param name="PassengerID"></param>
        /// <param name="iSeat"></param>
        public void AddPassengerLink(int iFlightID, int PassengerID, int iSeat)
        {

            try
            {
                //SQL statement for adding passenger link
                string sSQL = "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) VALUES( " + iFlightID + " , " + PassengerID + " , " + iSeat + ")";

                db = new clsDataAccess();

                //excute
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// SQL Statemnet for getting a passengers ID
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        public string GetPassengerID(string FirstName, string LastName)
        {
            try
            {
                //SQL statement for getting a passengers ID
                string sSQL = "SELECT Passenger_ID from Passenger where First_Name = '" + FirstName + "' AND Last_Name = '" + LastName + "'";

                db = new clsDataAccess();

                //excute
                string tempID = db.ExecuteScalarSQL(sSQL);

                //if the passenger is not null
                if (passenger != null)
                {
                    return passenger.sID = db.ExecuteScalarSQL(sSQL);
                }
                else
                {
                    return tempID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// SQL Statement for deleting a passengers database table link
        /// </summary>
        /// <param name="iFlightID"></param>
        /// <param name="PassengerID"></param>
        public void DeletePassengerLink(int iFlightID, int PassengerID)
        {
            //SQL Statement for deleting a passengers database table link
            string sSQL = "Delete FROM FLIGHT_PASSENGER_LINK " +
               "WHERE FLIGHT_ID = " + iFlightID + " AND " +
               "PASSENGER_ID = " + PassengerID;

            db = new clsDataAccess();

            //excute
            db.ExecuteNonQuery(sSQL);
        }

        /// <summary>
        /// SQL Statement for deleting a passenger by ID
        /// </summary>
        /// <param name="PassengerID"></param>
        public void DeletePassenger(int PassengerID)
        {

            //Delete the passenger by ID
            string sSQL = "Delete FROM PASSENGER WHERE PASSENGER_ID = " + PassengerID;

            db = new clsDataAccess();

            //excute
            db.ExecuteNonQuery(sSQL);

        }

        /// <summary>
        /// SQL Statement for getting passengers first name by passenger ID
        /// </summary>
        /// <param name="PassengerID"></param>
        /// <returns></returns>
        public string GetPassengerFirstName(int PassengerID)
        {
            try
            {
                //SQL Statement for getting passengers first name by passenger ID
                string sSQL = "SELECT First_Name from Passenger where Passenger_ID = " + PassengerID;

                db = new clsDataAccess();

                string temp = db.ExecuteScalarSQL(sSQL);

                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL Statement for getting a passengers last name by passenger ID
        /// </summary>
        /// <param name="PassengerID"></param>
        /// <returns></returns>
        public string GetPassengerLastName(int PassengerID)
        {
            try
            {
                //SQL Statement for geting a passengers last name by passenger ID
                string sSQL = "SELECT Last_Name from Passenger where Passenger_ID = " + PassengerID;

                db = new clsDataAccess();

                string temp = db.ExecuteScalarSQL(sSQL);

                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public string GetPassengerSeatNumber(int PassengerID)
        {

            //SQL Statement for getting passengers first name by passenger ID
            string sSQL = "SELECT Seat_Number from Passenger_Link where Passenger_ID = " + PassengerID;

            db = new clsDataAccess();

            string temp = db.ExecuteScalarSQL(sSQL);

            return temp;

        }

        /// <summary>
        /// SQL Statement for updating a passengers seat number
        /// </summary>
        /// <param name="Seat_Number"></param>
        /// <param name="Flight_ID"></param>
        /// <param name="Passenger_ID"></param>
        public void UpdateSeatNumber(string Seat_Number, int Flight_ID, int Passenger_ID )
        {

            try
            {
                //Updating seat numbers
                string sSQL = "UPDATE FLIGHT_PASSENGER_LINK SET Seat_Number = " + Seat_Number + " WHERE FLIGHT_ID = " + Flight_ID + " AND PASSENGER_ID = " + Passenger_ID;

                db = new clsDataAccess();

                //excute
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }


        }


        #endregion

    }
}

