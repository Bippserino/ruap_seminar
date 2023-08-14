using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ruap_seminar;

public class GridForm : Form
{
    private const int GridSize = 32;
    private Button[,] _grid = new Button[GridSize, GridSize];
    private bool _isPainting;
    private Panel gridPanel;
    private Button submitButton;
    private Button resetButton;
    public Label[] labels = new Label[11];

    public GridForm()
    {
        Text = "RUAP Seminar";
        this.Size = new Size(600, 420);
        gridPanel = new Panel();
        gridPanel.Size = new Size(322, 322);
        gridPanel.Location = new Point(10, 10);
        gridPanel.BorderStyle = BorderStyle.FixedSingle;
        gridPanel.AutoScroll = true;
        Controls.Add(gridPanel);
        Form.CheckForIllegalCrossThreadCalls = false;

        for (int row = 0; row < GridSize; row++)
        {
            for (int col = 0; col < GridSize; col++)
            {
                _grid[row, col] = new Button
                {
                    Size = new Size(10, 10),
                    Location = new Point(col * 10, row * 10),
                    Visible = true
                };

                _grid[row, col].MouseDown += Grid_MouseDown;
                _grid[row, col].MouseMove += Grid_MouseMove;
                _grid[row, col].MouseUp += Grid_MouseUp;
                _grid[row, col].BackColor = Color.Transparent;

                gridPanel.Controls.Add(_grid[row, col]);
            }
        }
        resetButton = new Button { Location = new Point(10, 340), Visible = true, Text = "Reset" };
        resetButton.Click += new EventHandler(this.ResetButton_MouseClick);
        this.Controls.Add(resetButton);

        submitButton = new Button { Location = new Point(255, 340), Visible = true, Text = "Submit" };
        submitButton.Click += new EventHandler(this.SubmitButton_MouseClick);
        this.Controls.Add(submitButton);

        for (int i = 0; i < 11; i++)
        {
            labels[i] = new Label();
            labels[i].Location = new Point(340, 10 + i * 30);
            labels[i].AutoSize = true;
            this.Controls.Add(labels[i]);
        }
    }

    private void Grid_MouseDown(object sender, MouseEventArgs e)
    {
        _isPainting = true;
        (sender as Button).BackColor = Color.Black;
    }

    private void Grid_MouseMove(object sender, MouseEventArgs e)
    {

        if (!_isPainting)
        {

            return;
        }
        var relativePosition = this.PointToClient(Cursor.Position);
        relativePosition.X -= 10;
        relativePosition.Y -= 10;
        int col = relativePosition.X / 10;
        int row = relativePosition.Y / 10;
        if (col < GridSize && col >= 0 && row < GridSize && row >= 0)
        {
            _grid[row, col].BackColor = Color.Black;
        }

    }

    private void Grid_MouseUp(object sender, MouseEventArgs e)
    {
        _isPainting = false;
    }

    private void ResetButton_MouseClick(object sender, EventArgs e)
    {
        for (int row = 0; row < GridSize; row++)
        {
            for (int col = 0; col < GridSize; col++)
            {
                _grid[row, col].BackColor = Color.Transparent;
            }
        }

        for (int i = 0; i < 11; i++)
        {
            labels[i].Text = "";
        }
    }

    public void SubmitButton_MouseClick(object sender, EventArgs e)
    {
        int[] grid = new int[GridSize * GridSize];

        for (int row = 0; row < GridSize; row++)
        {
            for (int col = 0; col < GridSize; col++)
            {
                grid[GridSize * row + col] = _grid[row, col].BackColor == Color.Black ? 1 : 0;

            }
        }
        Utilities.makeAPIrequest(Utilities.GetFormatedArray(grid), this);
    }

    static void Main()
    {
        Application.Run(new GridForm());
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // GridForm
            // 
            this.ClientSize = new System.Drawing.Size(582, 373);
            this.Name = "GridForm";
            this.ResumeLayout(false);

    }
}