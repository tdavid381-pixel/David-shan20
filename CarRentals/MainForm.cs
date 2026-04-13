using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarRentals;

public sealed class MainForm : Form
{
    private readonly Label titleLabel;
    private readonly Button btnRegister;
    private readonly Button btnReport;
    private readonly Button btnRent;
    private readonly Button btnRefresh;
    private readonly Button btnReturn;
    private readonly DataGridView customersGrid;

    private readonly List<Customer> customers = new();

    public MainForm()
    {
        Text = "Car Rentals";
        ClientSize = new Size(979, 741);
        StartPosition = FormStartPosition.CenterScreen;

        titleLabel = new Label
        {
            AutoSize = true,
            Location = new Point(300, 52),
            Text = "Car Rental System"
        };

        btnRegister = BuildButton("Register", new Point(192, 139), BtnRegister_Click);
        btnReport = BuildButton("Report", new Point(425, 139), BtnReport_Click);
        btnRent = BuildButton("Rent", new Point(192, 250), BtnRent_Click);
        btnRefresh = BuildButton("Refresh", new Point(425, 250), BtnRefresh_Click);
        btnReturn = BuildButton("Return", new Point(192, 355), BtnReturn_Click);

        customersGrid = new DataGridView
        {
            Location = new Point(192, 458),
            Size = new Size(600, 246),
            ReadOnly = true,
            AutoGenerateColumns = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false
        };

        Controls.AddRange([
            titleLabel,
            btnRegister,
            btnReport,
            btnRent,
            btnRefresh,
            btnReturn,
            customersGrid
        ]);

        Load += MainForm_Load;
    }

    private static Button BuildButton(string text, Point location, EventHandler onClick)
    {
        var button = new Button
        {
            Text = text,
            Location = location,
            Size = new Size(134, 50),
            UseVisualStyleBackColor = true
        };

        button.Click += onClick;
        return button;
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        InitializeApp();
        RefreshAll();
    }

    private void InitializeApp()
    {
        customers.Clear();
    }

    private void RefreshAll()
    {
        customersGrid.DataSource = null;
        customersGrid.DataSource = customers
            .Select(c => new
            {
                c.Id,
                c.FullName,
                c.Phone,
                c.LicenseNumber
            })
            .ToList();
    }

    private void SaveData()
    {
        // Placeholder for persistence implementation.
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        using Form regForm = new()
        {
            Text = "Register Customer",
            Size = new Size(350, 250),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        Label lblName = new() { Text = "Full Name:", Location = new Point(20, 20), AutoSize = true };
        Label lblPhone = new() { Text = "Phone:", Location = new Point(20, 60), AutoSize = true };
        Label lblLicense = new() { Text = "License:", Location = new Point(20, 100), AutoSize = true };

        TextBox txtName = new() { Location = new Point(110, 20), Width = 190 };
        TextBox txtPhone = new() { Location = new Point(110, 60), Width = 190 };
        TextBox txtLicense = new() { Location = new Point(110, 100), Width = 190 };

        Button btnSave = new() { Text = "Register", Location = new Point(60, 150), Width = 100 };
        Button btnCancel = new() { Text = "Cancel", Location = new Point(180, 150), Width = 100 };

        regForm.Controls.AddRange([
            lblName, txtName,
            lblPhone, txtPhone,
            lblLicense, txtLicense,
            btnSave, btnCancel
        ]);

        btnSave.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtLicense.Text))
            {
                MessageBox.Show("Fill all fields!");
                return;
            }

            Customer newCustomer = new()
            {
                Id = customers.Count == 0 ? 1 : customers.Max(c => c.Id) + 1,
                FullName = txtName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                LicenseNumber = txtLicense.Text.Trim()
            };

            customers.Add(newCustomer);
            SaveData();
            RefreshAll();

            MessageBox.Show("Customer registered successfully!");
            regForm.Close();
        };

        btnCancel.Click += (_, _) => regForm.Close();

        regForm.ShowDialog(this);
    }

    private void BtnReport_Click(object? sender, EventArgs e)
    {
        MessageBox.Show($"Total registered customers: {customers.Count}", "Report");
    }

    private void BtnRent_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("Rent workflow is not implemented yet.", "Rent");
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        RefreshAll();
    }

    private void BtnReturn_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("Return workflow is not implemented yet.", "Return");
    }
}
