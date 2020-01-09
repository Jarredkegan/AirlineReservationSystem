using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region References

        /// <summary>
        /// clsFlightManager class
        /// </summary>
        clsFlightManager clsFlightManager;

        /// <summary>
        /// Add Passenger Window
        /// </summary>
        wndAddPassenger wndAddPass;

        #endregion

        #region Attributes

        public int iFlight_ID;
        //public string sTempID { get; set; }
        public bool bChangeSeat = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                CanvasA380.Visibility = Visibility.Collapsed;
                Canvas767.Visibility = Visibility.Collapsed;

                //Start with change seat button off until user clicks on a label that contains a passenger
                cmdChangeSeat.IsEnabled = false;

                //Start with delete passenger button off until user clicks on a label that contains a passenger
                cmdDeletePassenger.IsEnabled = false;

                //new instance of clsFlightManager class
                clsFlightManager = new clsFlightManager();

                //bind flights to cbChooseFlight combobox
                cbChooseFlight.ItemsSource = clsFlightManager.GetFlights();

                //Clear passenger combobox each flight change.
                cbChoosePassenger.Items.Clear();

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the UI by getting all passengers and updating the seats
        /// </summary>
        public void cmdUpdateUI()
        {
            try
            {
                //bind passengers to cbChoosePassenger combobox
                cbChoosePassenger.ItemsSource = clsFlightManager.GetPassengers(iFlight_ID);

                //update the seats
                cmdUpdateSeats(iFlight_ID);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// UI Event - went user changes flight combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Name of flight chosen
                string selection = cbChooseFlight.SelectedItem.ToString();

                //Enable passenger combobox
                cbChoosePassenger.IsEnabled = true;

                //eneable commands
                gPassengerCommands.IsEnabled = true;

                //reset seat number label
                lblPassengersSeatNumber.Content = "";

                //if the name of the flight chosen equals
                if (selection == "412 - Boeing 767")
                {
                    //show 767
                    CanvasA380.Visibility = Visibility.Hidden;
                    Canvas767.Visibility = Visibility.Visible;
                    iFlight_ID = 2;

                    //Get passengers for this flight
                    cbChoosePassenger.ItemsSource = clsFlightManager.GetPassengers(iFlight_ID);

                    //update seats for this flight
                    cmdUpdateSeats(iFlight_ID);
                }
                else
                {
                    //show A380
                    Canvas767.Visibility = Visibility.Hidden;
                    CanvasA380.Visibility = Visibility.Visible;
                    iFlight_ID = 1;

                    //Get passengers for this flight
                    cbChoosePassenger.ItemsSource = clsFlightManager.GetPassengers(iFlight_ID);

                    //Update seats for this flight
                    cmdUpdateSeats(iFlight_ID);
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Loops through selected flight and find which seats are taken
        /// </summary>
        /// <param name="iFlight_ID"></param>
        public void cmdUpdateSeats(int iFlight_ID)
        {
            try
            {
                //if flight 767 is chosen
                if (iFlight_ID == 2)
                {
                    //get seats and assign the list to seats
                    var seats = clsFlightManager.GetSeats(iFlight_ID);

                    //for each person
                    foreach (var item in seats)
                    {
                        //for each label
                        foreach (Label label in c767_Seats.Children)
                        {
                            //if the passengers seat number equals a label, turn the label background to red
                            if (item.iSeat == Convert.ToInt32(label.Content))
                            {
                                label.Background = Brushes.Red;
                            }
                            //this if statement changes the old user click selection back to blue
                            else if (label.Background == Brushes.Lime)
                            {
                                label.Background = Brushes.Blue;
                            }
                            //Trying to find a way to turn an old seat change from red back to blue???
                            if (lblPassengersSeatNumber.Content.ToString() == "" && bChangeSeat == true)
                            {
                                label.Background = Brushes.Blue;
                            }
                        }
                    }

                }
                else
                {
                    //if flight A380 is chosen
                    var seats = clsFlightManager.GetSeats(iFlight_ID);
                    //for each person
                    foreach (var item in seats)
                    {
                        //for each label
                        foreach (Label label in cA380_Seats.Children)
                        {
                            //if the passengers seat number equals a label, turn the label background to red
                            if (item.iSeat == Convert.ToInt32(label.Content))
                            {
                                label.Background = Brushes.Red;
                            }
                            //this if statement changes the old user click selection back to blue
                            else if (label.Background == Brushes.Lime)
                            {
                                label.Background = Brushes.Blue;
                            }
                            //Trying to find a way to turn an old seat change from red back to blue???
                            if (lblPassengersSeatNumber.Content.ToString() == "" && bChangeSeat == true)
                            {
                                label.Background = Brushes.Blue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// AddPassenger click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //create new instance of add passenger window
                wndAddPass = new wndAddPassenger();

                //display it
                wndAddPass.ShowDialog();

                //if the user is in save passenger mode
                if (wndAddPass.saveMode)
                {
                    //new instance of clsFlightManager class
                    clsFlightManager = new clsFlightManager();

                    //disable buttons, forcing user to click on a label
                    cmdDisableButtons();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Enable all buttons
        /// </summary>
        private void cmdEnableButtons()
        {
            try
            {
                cmdAddPassenger.IsEnabled = true;
                cmdChangeSeat.IsEnabled = true;
                cmdDeletePassenger.IsEnabled = true;

                cbChooseFlight.IsEnabled = true;
                cbChoosePassenger.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Disable buttons, forcing user to click a label
        /// </summary>
        private void cmdDisableButtons()
        {
            try
            {
                cmdAddPassenger.IsEnabled = false;
                cmdChangeSeat.IsEnabled = false;
                cmdDeletePassenger.IsEnabled = false;

                cbChooseFlight.IsEnabled = false;
                cbChoosePassenger.IsEnabled = false;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Error Handle method
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// UI Event For a Seat Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Seat_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Label called bckgroundColor to be used for checking the labels background color
                Label bckgroundColor = (Label)sender;

                //start with fresh updated seats
                cmdUpdateSeats(iFlight_ID);

                //Check to see if someone is sitting in the selected seat
                CheckWhoIsSittingHere(bckgroundColor, sender);

                //Enable the change seat button
                cmdChangeSeat.IsEnabled = true;

                //Enable the delete passenger button
                cmdDeletePassenger.IsEnabled = true;

                //if add passanger save mode is true and wndAddPass instance is not null
                if (wndAddPass != null && wndAddPass.saveMode)
                {
                    //if the labels background is red, then the seat is already taken - return and pick a different seat
                    if (bckgroundColor.Background.ToString() == Brushes.Red.ToString())
                    {
                        lblError.Content = "Seat is already taken!";
                        return;
                    }

                    //userPick is the label the user clicked on
                    Label userPick = (Label)sender;

                    //set the passenger seat number label to be the same as the userPick label (This is the seat the new passenger selected)
                    lblPassengersSeatNumber.Content = userPick.Content;

                    //new instance of clsFlightManager
                    clsFlightManager = new clsFlightManager();

                    //Add the passenger link to database (first name, last name, and flight id have already been added at this point)
                    clsFlightManager.AddPassengerLink(iFlight_ID, Convert.ToInt32(wndAddPass.sTempID), Convert.ToInt32(userPick.Content));

                    //update the ui
                    cmdUpdateUI();

                    //Enable UI buttons
                    cmdEnableButtons();

                    //no longer need add passenger mode active
                    wndAddPass.saveMode = false;

                    //clean label error if there was one
                    lblError.Content = "";
                }
                //if Change seat mode is true
                if(bChangeSeat)
                {
                    //if the labels background is red, then the seat is already taken - return and pick a different seat

                    if (bckgroundColor.Background.ToString() == Brushes.Red.ToString())
                    {
                        lblError.Content = "Seat is already taken!";
                        return;
                    }

                    //Call change seat and pass the selected seat
                    ChangeSeat(sender);

                    //Enable UI buttons
                    cmdEnableButtons();

                    //Update the ui
                    cmdUpdateUI();

                    //no longer need save seat mode active
                    bChangeSeat = false;
                }

                //if button clicked is blue, change it to green - this acts as a "This button has been selected!" notifier
                if (bckgroundColor.Background.ToString() == Brushes.Blue.ToString())
                {
                    cmdUpdateSeats(iFlight_ID);
                    bckgroundColor.Background = Brushes.Lime;

                    //if the button is blue, then the seat is available. Can't change a seat that doesn't have a person attached to it
                    cmdChangeSeat.IsEnabled = false;

                    //if the button is blue, then the seat is available. Can't delete a passenger that doesn't have a seat
                    cmdDeletePassenger.IsEnabled = false;

                }
                //if button clicked is red, change it to green - this acts as a "This button has been selected!" notifier
                if (CheckWhoIsSittingHere(bckgroundColor, sender))
                {
                    bckgroundColor.Background = Brushes.Lime;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Checks to see who is seating in the selected seat, if anyone at all
        /// </summary>
        /// <param name="bckgroundColor"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool CheckWhoIsSittingHere(Label bckgroundColor, object sender)
        {
            try
            {
                //if labels background is red
                if (bckgroundColor.Background.ToString() == Brushes.Red.ToString())
                {
                    //for flight A380
                    if (CanvasA380.Visibility == Visibility.Visible)
                    {
                        //for each passenger in the selected flight
                        foreach (clsPassengers item in cbChoosePassenger.ItemsSource)
                        {
                            //check each seat label
                            foreach (Label label in cA380_Seats.Children)
                            {
                                Label userPick = (Label)sender;
                                //and see if a passengers seat number matches the seat the user has picked
                                if (item.iSeat.ToString() == userPick.Content.ToString())
                                {
                                    //if so, display who is sitting here and what seat they have
                                    lblPassengersSeatNumber.Content = userPick.Content;
                                    cbChoosePassenger.SelectedItem = item;
                                    return true;
                                }
                            }
                        }
                    }
                    //for flight 767
                    else
                    {
                        foreach (clsPassengers item in cbChoosePassenger.ItemsSource)
                        {
                            foreach (Label label in c767_Seats.Children)
                            {
                                Label userPick = (Label)sender;
                                if (item.iSeat.ToString() == userPick.Content.ToString())
                                {
                                    lblPassengersSeatNumber.Content = userPick.Content;
                                    cbChoosePassenger.SelectedItem = item;
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (bChangeSeat)
                {
                    return false;
                }
                else
                {
                    lblPassengersSeatNumber.Content = "";
                    cbChoosePassenger.SelectedItem = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// UI Event for deleting the selected passenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                //if a passenger has not been chosen, just return
                if (cbChoosePassenger.Text == "")
                {
                    return;
                }

                //grab the selected passengers first name out of the combobox
                string firstName = cbChoosePassenger.Text.Substring(0, cbChoosePassenger.Text.IndexOf(' '));

                //grab the selected passengers last name out of the combobox
                string LastName = cbChoosePassenger.Text.Substring(cbChoosePassenger.Text.IndexOf(' ') + 1);

                //get passengers ID from the database
                int tempID = Convert.ToInt32(clsFlightManager.GetPassengerID(firstName, LastName));

                //Delete the passengers link in database first
                clsFlightManager.DeletePassengerLink(iFlight_ID, tempID);

                //Delete passenger
                clsFlightManager.DeletePassenger(tempID);

                //update UI
                cmdUpdateUI();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// UI button click event to change a passengers seat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //put the program in change seat mode
                bChangeSeat = true;

                //Disable buttons to force user to click a label
                cmdDisableButtons();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method for changing a passengers seats
        /// </summary>
        /// <param name="sender"></param>
        public void ChangeSeat(object sender)
        {
            try
            {
                //get the flight name
                string selection = cbChooseFlight.SelectedItem.ToString();

                //needed for getting the label's content or seat number 
                Label Seat_Number = (Label)sender;

                //get passengers first name
                string firstName = cbChoosePassenger.Text.Substring(0, cbChoosePassenger.Text.IndexOf(' '));

                //get passengers last name
                string LastName = cbChoosePassenger.Text.Substring(cbChoosePassenger.Text.IndexOf(' ') + 1);

                //get the passengers ID
                int tempID = Convert.ToInt32(clsFlightManager.GetPassengerID(firstName, LastName));

                //if the flight is 737
                if (selection == "412 - Boeing 767")
                {
                    //Flight 737
                    iFlight_ID = 2;

                    //Change the passengers current seat in the database to the new selected seat number
                    clsFlightManager.UpdateSeatNumber(Seat_Number.Content.ToString(), iFlight_ID, tempID);
                }
                else
                {
                    //Flight A380
                    iFlight_ID = 1;

                    //Change the passengers current seat in the database to the new selected seat number
                    clsFlightManager.UpdateSeatNumber(Seat_Number.Content.ToString(), iFlight_ID, tempID);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        //private void cbChoosePassenger_SelectionChange(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {

        //        //needed for getting the label's content or seat number 
        //        Label Seat_Number = (Label)sender;

        //        //get passengers first name
        //        string firstName = cbChoosePassenger.Text.Substring(0, cbChoosePassenger.Text.IndexOf(' '));

        //        //get passengers last name
        //        string LastName = cbChoosePassenger.Text.Substring(cbChoosePassenger.Text.IndexOf(' ') + 1);

        //        //get the passengers ID
        //        int tempID = Convert.ToInt32(clsFlightManager.GetPassengerID(firstName, LastName));

        //        //CheckWhoIsSittingHere(clsFlightManager.GetPassengerSeatNumber(tempID), sender);


        //    }
        //    catch (Exception ex)
        //    {
        //        HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
        //            MethodInfo.GetCurrentMethod().Name, ex.Message);
        //    }
        //}

        #endregion
    }
}
