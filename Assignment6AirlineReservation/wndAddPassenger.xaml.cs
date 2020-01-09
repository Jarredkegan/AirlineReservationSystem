using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for wndAddPassenger.xaml
    /// </summary>
    public partial class wndAddPassenger : Window
    {

        #region References

        clsFlightManager FlightManager;
        clsPassengers Passenger;
        MainWindow wndMainWindow;

        #endregion

        #region Attributes

        public bool saveMode;
        public string sTempID;

        #endregion

        #region Constructor

        /// <summary>
        /// constructor for the add passenger window
        /// </summary>
        public wndAddPassenger()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// only allows letters to be input
        /// </summary>
        /// <param name="sender">sent object</param>
        /// <param name="e">key argument</param>
        private void txtLetterInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Only allow letters to be entered
                if (!(e.Key >= Key.A && e.Key <= Key.Z))
                {
                    //Allow the user to use the backspace, delete, tab and enter
                    if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter))
                    {
                        //No other keys allowed besides numbers, backspace, delete, tab, and enter
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// exception handler that shows the error
        /// </summary>
        /// <param name="sClass">the class</param>
        /// <param name="sMethod">the method</param>
        /// <param name="sMessage">the error message</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// UI Button Event click that saves the new passengers information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                FlightManager = new clsFlightManager();
                Passenger = new clsPassengers();
                wndMainWindow = new MainWindow();

                //Enable add passenger save mode
                saveMode = true;

                //if flight name 767
                if (wndMainWindow.cbChooseFlight.ToString() == "412 - Boeing 767")
                {
                    //show 767
                    wndMainWindow.CanvasA380.Visibility = Visibility.Hidden;
                    wndMainWindow.Canvas767.Visibility = Visibility.Visible;

                    //then flight id is 2
                    wndMainWindow.iFlight_ID = 2;
                }
                else
                {
                    //show A380
                    wndMainWindow.Canvas767.Visibility = Visibility.Hidden;
                    wndMainWindow.CanvasA380.Visibility = Visibility.Visible;

                    //else flight id is 1
                    wndMainWindow.iFlight_ID = 1;
                }

                //Add passenger to database - first name, last name, and which flight they are on
                FlightManager.AddPassenger(txtFirstName.Text, txtLastName.Text, wndMainWindow.iFlight_ID);

                //Gets the newly added passengers ID
                Passenger.sID = FlightManager.GetPassengerID(txtFirstName.Text, txtLastName.Text);

                //temp variable to hold passengers id
                sTempID = Passenger.sID;

                //refresh
                wndMainWindow.cbChoosePassenger.Items.Clear();


                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Cancels the process for adding a new passenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //deactivate save new passenger mode
                saveMode = false;

                //close window
                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }

    #endregion
}
