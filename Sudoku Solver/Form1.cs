using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class frmSudoku : Form
    {
        public frmSudoku()
        {
            InitializeComponent();
        }

        TextBox[,] Grid;
        TextBox[] GridWritable;
        bool[,,] Writable;
        int[,] Result;
        int[,] StartMatrix;
        bool FirstRun = true;

        private void BtnSolve_Click(object sender, EventArgs e)
        {
            
            ReadData();
            FirstRun = false;
            CheckAllCells();
            SolveSudolku();
            
        }

        private void FrmSudoku_Load(object sender, EventArgs e)
        {
            Grid = new TextBox[9, 9];
            GridWritable = new TextBox[9];
            Writable = new bool[9, 9, 9];
            Result = new int[9, 9];
            StartMatrix = new int[9, 9];
            string txtname;
            int tabind = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    txtname = "txt_" + (i+1) + "_" + (j+1);
                    Grid[i, j] =(TextBox)this.Controls.Find(txtname ,true )[0];
                    Grid[i, j].TabIndex = tabind;
                    tabind++;
                    for (int k = 0; k < 9; k++)
                    {
                        Writable[i, j, k] = true;
                    }
                }
                txtname = "txtWritable_" + (i + 1);
                //Control[] asd = this.Controls.Find(txtname, true);
                GridWritable[i] = (TextBox)this.Controls.Find(txtname, true)[0];
            }
            AddViewer();
        }

        private void CheckRowCols(int RowIndex, int ColIndex)
        {
            for (int j = 0; j < 9; j++)
            {
                if (j != ColIndex)
                {
                    if (Result[RowIndex, j] !=0)
                    {
                        if (Result[RowIndex, j] != 0) Writable[RowIndex, ColIndex, Result[RowIndex, j]-1] = false;
                    } 
                }

                if (j != RowIndex)
                {
                    if (Result[j, ColIndex] != 0)
                    {
                        if (Result[j, ColIndex] != 0) Writable[RowIndex, ColIndex, Result[j, ColIndex]-1] = false;
                    }
                }

            }
        }

        private void CheckSquare(int RowIndex, int ColIndex)
        {
            int rowStart = RowIndex - (RowIndex % 3);
            int colStart = ColIndex - (ColIndex % 3);


            for (int i = rowStart; i < rowStart + 3; i++)
            {
                for (int j = colStart; j < colStart + 3; j++)
                {
                    if (j != ColIndex && i != RowIndex)
                    {
                        if(Result[i, j]!=0) Writable[RowIndex, ColIndex, Result[i, j]-1] = false;
                    }
                }
            }
        }

        private void CheckAllCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (Result[i, j]!=0)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            Writable[i, j, k] = false;
                        }
                    }
                    else
                    {
                        CheckRowCols(i, j);
                        CheckSquare(i, j);
                    }
                }
                Application.DoEvents();
            }
        }

        private void ReadData()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        StartMatrix[i, j] = int.Parse(Grid[i, j].Text);
                        Result[i, j] = int.Parse(Grid[i, j].Text);
                        if (FirstRun)
                        {
                            Grid[i, j].Enabled = false;
                            Grid[i, j].BackColor = Color.LightSeaGreen;
                        }
                    }
                    catch (Exception)
                    {
                        StartMatrix[i, j] = 0;
                        Result[i, j] = 0;
                    }
                    
                }
            }
        }

        private void AddViewer()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //Grid[i, j].Handle.
                    this.Grid[i, j].MouseHover += new System.EventHandler(TxtHover);
                    this.Grid[i, j].MouseLeave += new System.EventHandler(TxtMouseLeave);
                    this.Grid[i, j].TextChanged += new System.EventHandler(TxtChanged);
                }
            }
        }

        private void TxtChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                ReadData();
                CheckAllCells();
            }
        }

        private void TxtHover(object sender, EventArgs e)
        {
            //MessageBox.Show(((TextBox)sender).Name);

            writablePanel.Visible = true;
            int[] Indexes = findIndexes((TextBox)sender);
            for (int i = 0; i < 9; i++)
            {
                GridWritable[i].Visible = Writable[Indexes[0]-1, Indexes[1]-1, i];
            }
        }

        private void TxtMouseLeave(object sender, EventArgs e)
        {
            writablePanel.Visible = false;
        }

        private int[] findIndexes(TextBox RefTxtBox)
        {
            int[] tempReturn;
            tempReturn = new int[2];
            tempReturn[0] = int.Parse(RefTxtBox.Name.Substring(4, 1));
            tempReturn[1] = int.Parse(RefTxtBox.Name.Substring(6, 1));
            return tempReturn;
        }

        private void WriteAnsvers()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int c = 0;
                    int d = 0;
                    for (int k = 0; k < 9; k++)
                    {
                        if (Writable[i, j, k])
                        {
                            d = k+1;
                            c++;
                        }
                    }
                    if (c==1)
                    {
                        Grid[i, j].Text = d.ToString();
                    }
                }
                Application.DoEvents();
            }
        }

        private bool IsFinished()
        {
            bool tempreturn = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (Grid[i,j].TextLength ==0)
                    {
                        tempreturn = false;
                        break;
                    }
                }
                if (!tempreturn) break;
            }
            return tempreturn;
        }

        private void SolveSudolku()
        {
            while (!IsFinished())
            {
                WriteAnsvers();
                Application.DoEvents();
            }
        }

    }
}
