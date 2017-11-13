using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Assignment7 {
    public partial class Form1 : Form {

        /// <summary>
        /// Flag to determine what stage of the thread we are at
        /// </summary>
        Boolean flag = false;
        /// <summary>
        /// path to file destination
        /// </summary>
        String filePath = "C:\\Users\\Public\\";
        /// <summary>
        /// this variable will hold all the data to be outputted to the file.
        /// </summary>
        String output = "";

        public Form1() {
            InitializeComponent();
        }

        #region vars

        /// <summary>
        /// studentCount holds the number of students the user entered in
        /// </summary>
        public int studentCount = 0;
        /// <summary>
        /// assignmentCount holds the number of assignments the user entered in
        /// </summary>
        public int assignmentCount = 0;
        /// <summary>
        /// queueCount keeps track of the student currently selected
        /// </summary>
        public int queueCount = 0;
        /// <summary>
        /// header is the first line inputed into the richTextBox that will hold the results
        /// </summary>
        public String header = "";
        /// <summary>
        /// studentsArray will hold the students
        /// </summary>
        public String[] studentsArray;
        /// <summary>
        /// assignmentsArray will hold the assignment scores
        /// </summary>
        public int[,] assignmentsArray;

        #endregion

        #region methods

        /// <summary>
        /// setCounts(int, int) creates the array objects in the dimensions the user specified.
        /// The studentsArray is set with default values, and labels are assigned values.
        /// Buttons are enabled
        /// </summary>
        /// <param name="students"></param>
        /// <param name="assignments"></param>
        public void setCounts(int students, int assignments) {
            assignmentsArray = new int[assignments, students];
            studentsArray = new String[students];

            for (int i = 0; i < studentsArray.Length; i++) {
                studentsArray[i] = "Student #" + (i + 1);
            }

            setHeader(assignmentCount);
            lblAssignmentNum.Text = "(1 - " + assignmentCount + ")";
            setNameLabel(studentsArray[0]);
            queueCount = 0;
            btnSubmitCount.Enabled = false;

        }//end setCounts()

        /// <summary>
        /// setHeader(int) gets the numbers of assignments, and creates a header for the data that will be inputted later on.
        /// </summary>
        /// <param name="assignmentCount"></param>
        public void setHeader(int assignmentCount) {
            txtDisplay.Text = "";
            header = "Student\t\t";
            for (int i = 0; i < assignmentCount; i++) {
                header += "#" + (i + 1) + "\t";
            }
            header += "AVG\tGRADE";

            txtDisplay.Text = header;
        }//end setHeader()

        /// <summary>
        /// enableButtons enables the buttons that were disabled prior to entering valid number of students and assignments
        /// </summary>
        public void enableButtons() {
            btnResetScores.Enabled = true;
            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
            btnSave.Enabled = true;
            btnDisplay.Enabled = true;
            btnSubmitScore.Enabled = true;
            btnOutput.Enabled = true;
        }//end enableButtons()

        /// <summary>
        /// disableButtons() disables buttons on reset, and enables the submit count button
        /// </summary>
        public void disableButtons() {
            btnResetScores.Enabled = false;
            btnFirst.Enabled = false;
            btnPrev.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
            btnSave.Enabled = false;
            btnDisplay.Enabled = false;
            btnSubmitScore.Enabled = false;
        }//end disableButtons()

        /// <summary>
        /// gradeScale(int) takes in the average score, and returns a letter grade
        /// </summary>
        /// <param name="avg"></param>
        /// <returns></returns>
        public String gradeScale(int avg) {

            if (avg >= 93) { return "A"; } else if (avg >= 90) { return "A-"; } else if (avg >= 87) { return "B+"; } else if (avg >= 83) { return "B"; } else if (avg >= 80) { return "B-"; } else if (avg >= 77) { return "C+"; } else if (avg >= 73) { return "C"; } else if (avg >= 70) { return "C-"; } else if (avg >= 67) { return "D+"; } else if (avg >= 63) { return "D"; } else if (avg >= 60) { return "D-"; } else { return "F"; }
        }//end gradeScale()  

        /// <summary>
        /// sets the name label. probably unnecessary
        /// </summary>
        /// <param name="s"></param>
        public void setNameLabel(String s) {
            lblStudent.Text = s;
        }//end setNameLabel()

        #endregion

        #region clickEvents

        /// <summary>
        /// btnSubmitCount_Click() validates user input, calls setCounts(), calls enableButtons(), and sets label values.
        /// Incorrect user inputs will cause a message box to appear informing user their input was invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitCount_Click(object sender, EventArgs e) {
            DialogResult MyResult;
            try {
                studentCount = Int32.Parse(txtNumStudents.Text);
                assignmentCount = Int32.Parse(txtNumAssignments.Text);

                if (studentCount < 0 || studentCount > 10) {
                    //MyResult = MessageBox.Show("Please enter a number between 1 and 10", "Incorrect Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    throw new Exception("Exception thrown");
                }
                if (assignmentCount < 0 || assignmentCount > 99) {
                    // MyResult = MessageBox.Show("Please enter a number between 1 and 99", "Incorrect Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    throw new Exception("Exception thrown");
                }

                setCounts(studentCount, assignmentCount);
                enableButtons();
                txtNumAssignments.Text = "";
                txtNumStudents.Text = "";

            } catch (Exception ex) {
                MyResult = MessageBox.Show("\tInvalid input!\n\tStudents 1 - 10\n\tAssignments 1 - 99", "Incorrect Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine("EXCEPTION: btnSubmitCount_Click() " + ex);
            }//end try/catch
        }//end btnSubmitCount_Click()

        /// <summary>
        /// btnFirst_Click() sets queueCount = 0; and sets the name label to the value in the 0 index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, EventArgs e) {
            queueCount = 0;
            setNameLabel(studentsArray[queueCount]);
        }//end btnFirst_Click()

        /// <summary>
        ///  btnPrev_Click() decrements queue count, ensures queueCount will not be less than 0,
        ///  and sets the label in queueCount index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e) {
            queueCount--;
            if (queueCount < 0) { queueCount = 0; }
            setNameLabel(studentsArray[queueCount]);
        }//end btnPrev_Click()

        /// <summary>
        /// btnNext_Click() increments queueCount, ensures queueCount will not be greater than the number of students,
        /// and sets the label in queueCount index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e) {
            queueCount++;
            if (queueCount == studentsArray.Length) { queueCount = studentsArray.Length - 1; }
            setNameLabel(studentsArray[queueCount]);
        }//end btnNext_Click()

        /// <summary>
        /// btnLast_Click() sets queueCount to number of students -1,
        /// and sets the label in queueCount index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, EventArgs e) {
            queueCount = studentsArray.Length - 1;
            setNameLabel(studentsArray[queueCount]);
        }//end btnLast_Click()

        /// <summary>
        /// btnSave_Click() captures the value in text box, and sets the student array at index queueCount to that value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e) {

            studentsArray[queueCount] = txtStudent.Text;
            setNameLabel(studentsArray[queueCount]);
            txtStudent.Text = "";

        }//end btnSave_Click()

        /// <summary>
        ///  btnSubmitScore_Click() validates the user input, captures the assignment number. validates the score, captures value and
        ///  stores the value in the assignmentsArray at index [assignment-1, queueCount]
        ///  error message on invalid input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitScore_Click(object sender, EventArgs e) {
            DialogResult MyResult;
            try {
                int assignment = Int32.Parse(txtAssignment.Text);

                if (assignment < 1 || assignment > assignmentCount) {
                    MyResult = MessageBox.Show("\tAssignments 1 - " + (assignmentsArray.Length - 1), "Incorrect Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    throw new Exception("Exception thrown");
                }

                int score = Int32.Parse(txtScore.Text);

                assignmentsArray[(assignment - 1), queueCount] = score;
                txtAssignment.Text = "";
                txtScore.Text = "";

            } catch (Exception ex) {
                MyResult = MessageBox.Show("\tIncorrect Value", "Incorrect Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }//end btnSubmitScore_Click()

        /// <summary>
        /// btnDisplay_Click() cycles through the arrays and displays their values.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisplay_Click(object sender, EventArgs e) {
            int avg = 0;
            txtDisplay.Text = "";
            setHeader(assignmentCount);
            for (int i = 0; i < studentsArray.Length; i++) {

                if (studentsArray[i].Length < 8)//for formatting data
                    txtDisplay.Text += "\n" + studentsArray[i] + "\t\t";
                else
                    txtDisplay.Text += "\n" + studentsArray[i] + "\t";

                for (int k = 0; k < assignmentCount; k++) {
                    txtDisplay.Text += assignmentsArray[k, i] + "\t";
                    //add scores
                    avg += assignmentsArray[k, i];
                }//end inner for

                avg = avg / assignmentCount;
                txtDisplay.Text += avg + "%\t";
                txtDisplay.Text += gradeScale(avg);
                avg = 0;

            }//end outer for

        }//end btnDisplay_Click()

        /// <summary>
        ///  btnResetScores_Click() resets needed variables, disables buttons, enables submit count button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetScores_Click(object sender, EventArgs e) {
            txtDisplay.Text = "";
            setNameLabel("");
            txtStudent.Text = "";
            lblAssignmentNum.Text = "";
            assignmentCount = 0;
            studentCount = 0;
            disableButtons();
            btnSubmitCount.Enabled = true;

        }//end btnResetScores_Click()

        #endregion

        #region threads

        /// <summary>
        /// This method creates 1 string with all the info that will be written to a files
        /// </summary>
        /// <returns></returns>
        public String outputText() {
            try {
                for (int i = 0; i < studentCount; i++) {
                    output += studentsArray[i] + " ";
                    for (int k = 0; k < assignmentCount; k++) {
                        output += assignmentsArray[k, i] + " ";
                    }
                    output += "\n";
                }
                System.Console.WriteLine(output);
            } catch (Exception e) { System.Console.WriteLine(e.Message); }
            return output;
            }

        /// <summary>
        /// Output button on click listener. checks to see if the file exists. If not, execute thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutput_Click(object sender, EventArgs e) {
            try {
                if (File.Exists(filePath + txtFileName.Text + ".txt")) {
                    MessageBox.Show("File already exists, choose a different name");
                } else {
                    outputText();
                    lblProgress.Text = "Writing to file....";
                    btnOutput.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();
                }
            } catch (Exception ex) { System.Console.WriteLine(ex.Message); }
        }


        /// <summary>
        /// Thread work. Write output string to file. Sleep for 2 seconds, change label to Writing Complete!
        /// after another 3 seconds, remove text from label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            try {
                String path = filePath + txtFileName.Text + ".txt";
                System.IO.File.WriteAllText(@path, output);


                Thread.Sleep(2000);
                backgroundWorker1.ReportProgress(50);
                Thread.Sleep(3000);
                flag = true;//allow the label to be set to empty string
                backgroundWorker1.ReportProgress(100);
            } catch (Exception ex) { System.Console.WriteLine(ex.Message); }
        }


        /// <summary>
        /// background worker progress. set label to Writing Complete!
        /// if the flag is set to true, this means the 5 seconds has passed. We then enable button and remove text from label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            try {
                lblProgress.Text = "Writing Complete!";
                if (flag) {
                    lblProgress.Text = "";
                    btnOutput.Enabled = true;
                    flag = false;
                    output = "";
                }
            } catch (Exception ex) { System.Console.WriteLine(ex.Message); }
        }

        #endregion


    }//end class
}//end namespace
