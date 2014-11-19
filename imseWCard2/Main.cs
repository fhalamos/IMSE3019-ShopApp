﻿//=======================================================================================================================
//   IMSE Department, University of Hong Kong 
//
//   Created by  : Bill Chan
//   Date        : 12 / 1 / 2012
//   Remark      :                   
//      1. The reference imseWSC1K should be added.
//
//      2. The aim of this application example is to demonstrate the use of the 
//         imseWSC1K class library to do payment transection (credit and debit). 
//
//      3. Block 2 of sector 15, i.e. block 0x3E, is used to store the value for
//         transactions.
//
//=======================================================================================================================


//=======================================================================================================================
//   Parking System Project - IMSE3019 
//
//   Created by  : Felipe Alamos
//   Date        : 12 / 1 / 2012
//   Remark      :                   
//      1. The project is based in the project developed by Bill Chan, mentioned aboved.
//
//      2. The 3 blocks of Sector 15 are used to save the amounts of the 3 biggest purchases, i.e. blocks 0x3C, 0x3D, 0x3E, is used to store the value for
//         transactions.
//
//=======================================================================================================================



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using imseWSC1K;  // the class library for contactless smart card

namespace imseWCard2
{
    public partial class Main : Form
    {
        // create an object to handle the card
        private wSCard CADw = new wSCard();

        // true if a smart card is connected. Otherwise, false. 
        private bool connected = false;

        // true if a transaction is done. Reset to false if a connection is lost.
        private bool transectionDone = false;

        // @fhalamos: Block 0, 1 and 2 of sector 15, i.e. block 0x3E, are used to save the biggest purchases
        private int sector = 15;
        private int block0 = 0x3C;
        private int block1 = 0x3D;
        private int block2 = 0x3E;

        public Main()
        {
            InitializeComponent();
        }

        private void btn100_Click(object sender, EventArgs e)
        {
            textBoxCAmt.Text = "100";
        }

        private void btn200_Click(object sender, EventArgs e)
        {
            textBoxCAmt.Text = "200";
        }

        private void btn500_Click(object sender, EventArgs e)
        {
            textBoxCAmt.Text = "500";
        }

        private void btn800_Click(object sender, EventArgs e)
        {
            textBoxCAmt.Text = "800";
        }

        private void btn1000_Click(object sender, EventArgs e)
        {
            textBoxCAmt.Text = "1000";
        }

        private void btn2000_Click(object sender, EventArgs e)
        {
            textBoxCAmt.Text = "2000";
        }

        private void addDigit(string d)
        { // Append a digit to the text box textBoxDAmt
          // The value should contains not more than 2 decimal points.
            string s = textBoxDAmt.Text;
            if (s.Contains("."))
            {
                if (s.IndexOf(".") + 2 < s.Length)
                    return;
            }
            if (s.Length==1 && s[0]=='0')
                textBoxDAmt.Text = d;
            else
                textBoxDAmt.Text = s + d;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            addDigit("1");
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            addDigit("2");
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            addDigit("3");
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            addDigit("4");
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            addDigit("5");
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            addDigit("6");
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            addDigit("7");
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            addDigit("8");
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            addDigit("9");
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            addDigit("0");
        }

        private void btnCLR_Click(object sender, EventArgs e)
        {
            textBoxDAmt.Text = "0";
        }

        private void btnDOT_Click(object sender, EventArgs e)
        {
            string s = textBoxDAmt.Text;
            if (s.Contains(".")==false)
                textBoxDAmt.Text = s + ".";
        }

        private void radioButtonDebitReady_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDebitReady.Checked == false)
                return;

            /* The value in the text box textBoxDAmt represents the amount to be
             * debit from the card in dollar. If radio button "ready" is selected
             * and the value is positive, disable input buttons and then enable 
             * the timer.
             * */

            double value = 0.0;

            if (textBoxDAmt.Text.Length > 0)
                value = Convert.ToDouble(textBoxDAmt.Text);

            if (value <= 0)
            {
                radioButtonDebitChange.Select();
                return;
            }

            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btnDOT.Enabled = false;
            btn0.Enabled = false;
            btnCLR.Enabled = false;
            timer3.Enabled = true;
        }

        private void radioButtonDebitChange_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDebitChange.Checked == false)
                return;

            /* If radio button "change" is selected, enable input buttons.
             * and disable timer.
             * */

            btn1.Enabled = true;
            btn2.Enabled = true;
            btn3.Enabled = true;
            btn4.Enabled = true;
            btn5.Enabled = true;
            btn6.Enabled = true;
            btn7.Enabled = true;
            btn8.Enabled = true;
            btn9.Enabled = true;
            btnDOT.Enabled = true;
            btn0.Enabled = true;
            btnCLR.Enabled = true;
            textBoxDAmt.Text = "0";
            timer3.Enabled = false;
        }

        private void radioButtonCreditReady_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreditReady.Checked == false)
                return;

            /* The value in the text box textBoxCAmt represents the amount to be
             * credit to the card in dollar. If radio button "ready" is selected 
             * and the value is positive, disable input buttons.
             * */
            double value = 0.0;

            if (textBoxCAmt.Text.Length > 0)
                value = Convert.ToDouble(textBoxCAmt.Text);

            if (value <= 0)
            {
                radioButtonCreditChange.Select();
                return;
            }
            
            btn100.Enabled = false;
            btn200.Enabled = false;
            btn500.Enabled = false;
            btn800.Enabled = false;
            btn1000.Enabled = false;
            btn2000.Enabled = false;
            btnCredit.Enabled = true;
        }

        private void radioButtonCreditChange_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreditChange.Checked == false)
                return;
            /* If radio button "change" is selected, enable input buttons.
             * */
            btn100.Enabled = true;
            btn200.Enabled = true;
            btn500.Enabled = true;
            btn800.Enabled = true;
            btn1000.Enabled = true;
            btn2000.Enabled = true;
            textBoxCAmt.Text = "0";
            btnCredit.Enabled = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!CADw.initialize())  // initialize the object CADw
            {
                textBoxMsg.Text = "Reader initialization failed!";
                return;
            }
            textBoxMsg.Text = "Reader initialization successful.";
            timer1.Enabled = true;
            timer2.Enabled = false;
            timer3.Enabled = false;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* initialize timers and other values when the user switch
             * between the three main functions: configure, credit and debit.
             * Timer1, timer2 and timer3 are used for configure, credit and debit
             * respectively.
             * */

            switch (tabControl.SelectedIndex)
            {
                case 0:
                    timer1.Enabled = true;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    transectionDone = false;
                    labelAmt.Text = "";
                    break;
                case 1:
                    timer1.Enabled = false;
                    timer2.Enabled = true;
                    timer3.Enabled = false;
                    transectionDone = false;
                    radioButtonCreditChange.Checked = true;
                    break;
                case 2:
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = true;
                    radioButtonDebitChange.Checked = true;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!CADw.connect())
            {
                // Connection lost.
                if (connected)
                {
                    textBoxMsg.Text = "Lose connection!";
                    labelAmt.Text = "";
                }
                btnConfigGo.Enabled = false;
                connected = false;
                return;
            }
            else
            {
                // Enable "Go" button if connected.
                if (!connected)
                {
                    connected = true;
                    textBoxMsg.Text = "Connected.";
                    btnConfigGo.Enabled = true;
                } 
                
                // Authenticate before reading or writing to a sector
                if (CADw.authenticate(sector) == false)
                {
                    textBoxMsg.Text ="Authentication error!";
                    return;
                }

                // Read the value card
                //@fhalamos: modify so that label shows the sum of the 3 amounts
                long amount0 = 0;
                if (CADw.readValueBlock(block0, ref amount0) == false)
                {
                    textBoxMsg.Text = "Read value error!";
                    return;
                }

                long amount1 = 0;
                if (CADw.readValueBlock(block1, ref amount1) == false)
                {
                    textBoxMsg.Text = "Read value error!";
                    return;
                }
                
                long amount2 = 0;
                if (CADw.readValueBlock(block2, ref amount2) == false)
                {
                    textBoxMsg.Text = "Read value error!";
                    return;
                }

                // Display the value 
                labelAmt.Text = toDollar(amount0+amount1+amount2);
            }
        }

        private string toDollar(long amount)
        {
            /* The value stored in the card is in cents. Convert the value to dollar
             * */

            string str = "";

            if (amount <= 0)
                str = "0";
            else if (amount < 10)
                str = "0.0" + amount.ToString();
            else if (amount < 100)
                str = "0." + amount.ToString();
            else
            {
                int dollar = (int)amount / 100;
                int cent = (int)amount % 100;

                str = dollar.ToString();
                str += ".";
                if (cent == 0)
                    str += "00";
                else
                    str += cent.ToString();
            }
            return str;
        }

        private void btnConfigGo_Click(object sender, EventArgs e)
        {
            /* Initialize a card's value
             * */
            long Amount;
            int tempInt;

            // Check whether the value in the text box is valid.
            if (!int.TryParse(textBoxConfigAmt.Text, out tempInt))
            {
                textBoxConfigAmt.Focus();
                textBoxConfigAmt.Text = "0";
                return;
            }
            // convert to cents and store into card
            // @fhalamos: Also add value already in card
            Amount = 100*Convert.ToInt32(textBoxConfigAmt.Text);
            CADw.updateValueBlock(block0, Amount);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!CADw.connect())
            {
                // Connection lost.
                if (connected)
                {
                    textBoxMsg.Text = "Lose connection!";
                    textBoxCBalance.Text = "";
                    radioButtonCreditChange.Select();
                }
                connected = false;
                return;
            }
            else
            {
                // Connected
                if (!connected)
                {
                    connected = true;
                    textBoxMsg.Text = "Connected.";
                } 

                // Authenticate before reading or writing
                if (CADw.authenticate(sector) == false)
                {
                    textBoxMsg.Text = "Authentication error!";
                    return;
                }

                //Read value from the card
                long amount = 0;
                if (CADw.readValueBlock(block2, ref amount) == false)
                {
                    textBoxMsg.Text = "Read value error!";
                    return;
                }

                // Display the value
                textBoxCBalance.Text = toDollar(amount);


            }
        }

        private void btnCredit_Click(object sender, EventArgs e)
        {
            /* Add value to the card
             * */

            // Check whether the value in the text box is valid.
            double value = 0.0;
            if (textBoxCAmt.Text.Length > 0)
                value = Convert.ToDouble(textBoxCAmt.Text);

            if (value <= 0)
            {
                radioButtonCreditChange.Select();
                return;
            }
            /* The value in the text box is in dollar. Convert to cents and 
             * add to the card.
             * */
            int amount = Convert.ToInt32(100.0 * value);
            if (CADw.incValueBlock(block2, amount) == false)
                textBoxMsg.Text = "Value increment error!";
        }

        private void timer3_Tick(object sender, EventArgs e)
        {            
            if (!CADw.connect())
            {
                // Connection lost.
                if (connected)
                {
                    textBoxMsg.Text = "Lose connection!";
                    textBoxDBalance.Text = "";
                    transectionDone = false;
                }
                connected = false;
                return;
            }
            else
            {
                // Connected
                if (!connected)
                {
                    connected = true;
                    textBoxDBalance.Text = "";
                    textBoxMsg.Text = "Connected.";
                } 
                
                // Authenticate before reading or writing
                if (CADw.authenticate(sector) == false)
                {
                    textBoxMsg.Text = "Authentication error!";
                    return;
                }

                // Read value from the card
                long amount = 0;
                if (CADw.readValueBlock(block2, ref amount) == false)
                {
                    textBoxMsg.Text = "Read value error!";
                    return;
                }

                // If ready to debit and the transaction is not done in previous timer cycle
                
                if (radioButtonDebitReady.Checked == true && transectionDone == false)
                {
                    // Check whether the value in the text box is valid
                    double value = 0.0;
                    if (textBoxDAmt.Text.Length > 0)
                        value = Convert.ToDouble(textBoxDAmt.Text);

                    if (value <= 0)
                    {
                        radioButtonDebitChange.Checked = true;
                        return;
                    }

                    long newAmount = Convert.ToInt32(100.0 * value);

                    // Check whether there are enough money in the card
                    if (newAmount > amount)
                    {
                        textBoxMsg.Text = "Balance not enough!";
                        return;
                    }

                    // Subtract theamount from the card.
                    if (CADw.decValueBlock(block2, newAmount) == false)
                    {
                        textBoxMsg.Text = "Value decrement error!";
                        return;
                    }
                    transectionDone = true;
                }

                // Read value from the card
                if (CADw.readValueBlock(block2, ref amount) == false)
                {
                    textBoxMsg.Text = "Read value error!";
                    return;
                }

                // Display the value
                textBoxDBalance.Text = toDollar(amount);
            }

        }

        private void btnCredit_EnabledChanged(object sender, EventArgs e)
        {
            /* Change colour of the credit button.
             * Pink if ready to credit. Otherwise, blue.
             * */

            if (btnCredit.Enabled == true)
            {
                btnCredit.BackColor = Color.Pink;
            }
            else
            {
                btnCredit.BackColor = Color.LightBlue;
            }
        }

        private void textBoxMsg_TextChanged(object sender, EventArgs e)
        {

        }






    }
}