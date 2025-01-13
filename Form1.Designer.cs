namespace AlterLinePictureAproximator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBoxSource = new PictureBox();
            buttonApproximate = new Button();
            pictureBoxAprox = new PictureBox();
            pictureBoxAtariAprox = new PictureBox();
            pictureBoxPalette = new PictureBox();
            pictureBoxAproxInverse = new PictureBox();
            pictureBoxAtariAproxInverse = new PictureBox();
            pictureBoxAtariMix = new PictureBox();
            buttonMixIt = new Button();
            pictureBoxCharMask = new PictureBox();
            comboBoxDither = new ComboBox();
            checkBoxUseDither = new CheckBox();
            comboBoxDistance = new ComboBox();
            label1 = new Label();
            buttonOpen = new Button();
            openFileDialog1 = new OpenFileDialog();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBoxAproxMix = new PictureBox();
            pictureBoxSrcReduced = new PictureBox();
            pictureBoxSrcData = new PictureBox();
            pictureBoxResult = new PictureBox();
            pictureBoxMasks = new PictureBox();
            buttonXex = new Button();
            checkBoxAutoscale = new CheckBox();
            checkBoxInterlace = new CheckBox();
            checkBoxAutoUpdate = new CheckBox();
            buttonAlpaCentauriAI = new Button();
            labelDiff = new Label();
            numericUpDownPopulation = new NumericUpDown();
            numericUpDownGeneration = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            listViewPopulation = new ListView();
            columnHeader1 = new ColumnHeader();
            buttonAlpaCentauriInit = new Button();
            checkBoxColorReduction = new CheckBox();
            comboBoxAverMethod = new ComboBox();
            label5 = new Label();
            toolTip1 = new ToolTip(components);
            groupBox1 = new GroupBox();
            checkBoxHepa = new CheckBox();
            numericUpDownHepaLuma = new NumericUpDown();
            label6 = new Label();
            label2 = new Label();
            numericUpDownHepaChroma = new NumericUpDown();
            progressBarAI = new ProgressBar();
            labelGenerationDone = new Label();
            button1 = new Button();
            labelPossibleColors = new Label();
            pictureBoxIcons = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAprox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAprox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPalette).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAproxInverse).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAproxInverse).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariMix).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharMask).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAproxMix).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcReduced).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcData).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMasks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPopulation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneration).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaLuma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaChroma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcons).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.Image = (Image)resources.GetObject("pictureBoxSource.Image");
            pictureBoxSource.Location = new Point(18, 20);
            pictureBoxSource.Margin = new Padding(4, 5, 4, 5);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Size = new Size(182, 254);
            pictureBoxSource.TabIndex = 0;
            pictureBoxSource.TabStop = false;
            // 
            // buttonApproximate
            // 
            buttonApproximate.Location = new Point(444, 20);
            buttonApproximate.Margin = new Padding(4, 5, 4, 5);
            buttonApproximate.Name = "buttonApproximate";
            buttonApproximate.Size = new Size(132, 121);
            buttonApproximate.TabIndex = 1;
            buttonApproximate.Text = "Approximate";
            buttonApproximate.UseVisualStyleBackColor = true;
            buttonApproximate.Click += ButtonApproximate_Click;
            // 
            // pictureBoxAprox
            // 
            pictureBoxAprox.Location = new Point(4, 241);
            pictureBoxAprox.Margin = new Padding(4, 5, 4, 5);
            pictureBoxAprox.Name = "pictureBoxAprox";
            pictureBoxAprox.Size = new Size(366, 226);
            pictureBoxAprox.TabIndex = 2;
            pictureBoxAprox.TabStop = false;
            // 
            // pictureBoxAtariAprox
            // 
            pictureBoxAtariAprox.Location = new Point(4, 5);
            pictureBoxAtariAprox.Margin = new Padding(4, 5, 4, 5);
            pictureBoxAtariAprox.Name = "pictureBoxAtariAprox";
            pictureBoxAtariAprox.Size = new Size(366, 226);
            pictureBoxAtariAprox.TabIndex = 3;
            pictureBoxAtariAprox.TabStop = false;
            // 
            // pictureBoxPalette
            // 
            pictureBoxPalette.ErrorImage = null;
            pictureBoxPalette.Image = (Image)resources.GetObject("pictureBoxPalette.Image");
            pictureBoxPalette.Location = new Point(18, 400);
            pictureBoxPalette.Margin = new Padding(4, 5, 4, 5);
            pictureBoxPalette.Name = "pictureBoxPalette";
            pictureBoxPalette.Size = new Size(171, 154);
            pictureBoxPalette.TabIndex = 4;
            pictureBoxPalette.TabStop = false;
            pictureBoxPalette.MouseDown += PictureBoxPalette_MouseDown;
            // 
            // pictureBoxAproxInverse
            // 
            pictureBoxAproxInverse.Location = new Point(378, 241);
            pictureBoxAproxInverse.Margin = new Padding(4, 5, 4, 5);
            pictureBoxAproxInverse.Name = "pictureBoxAproxInverse";
            pictureBoxAproxInverse.Size = new Size(366, 226);
            pictureBoxAproxInverse.TabIndex = 6;
            pictureBoxAproxInverse.TabStop = false;
            // 
            // pictureBoxAtariAproxInverse
            // 
            pictureBoxAtariAproxInverse.Location = new Point(378, 5);
            pictureBoxAtariAproxInverse.Margin = new Padding(4, 5, 4, 5);
            pictureBoxAtariAproxInverse.Name = "pictureBoxAtariAproxInverse";
            pictureBoxAtariAproxInverse.Size = new Size(366, 226);
            pictureBoxAtariAproxInverse.TabIndex = 7;
            pictureBoxAtariAproxInverse.TabStop = false;
            // 
            // pictureBoxAtariMix
            // 
            pictureBoxAtariMix.Location = new Point(752, 5);
            pictureBoxAtariMix.Margin = new Padding(4, 5, 4, 5);
            pictureBoxAtariMix.Name = "pictureBoxAtariMix";
            pictureBoxAtariMix.Size = new Size(366, 226);
            pictureBoxAtariMix.TabIndex = 8;
            pictureBoxAtariMix.TabStop = false;
            toolTip1.SetToolTip(pictureBoxAtariMix, "Result (non interlace view)");
            // 
            // buttonMixIt
            // 
            buttonMixIt.Location = new Point(444, 241);
            buttonMixIt.Margin = new Padding(4, 5, 4, 5);
            buttonMixIt.Name = "buttonMixIt";
            buttonMixIt.Size = new Size(132, 121);
            buttonMixIt.TabIndex = 9;
            buttonMixIt.Text = "Mix";
            buttonMixIt.UseVisualStyleBackColor = true;
            buttonMixIt.Click += ButtonMixIt_Click;
            // 
            // pictureBoxCharMask
            // 
            pictureBoxCharMask.Location = new Point(4, 477);
            pictureBoxCharMask.Margin = new Padding(4, 5, 4, 5);
            pictureBoxCharMask.Name = "pictureBoxCharMask";
            pictureBoxCharMask.Size = new Size(366, 226);
            pictureBoxCharMask.TabIndex = 10;
            pictureBoxCharMask.TabStop = false;
            toolTip1.SetToolTip(pictureBoxCharMask, "Char inverse mask");
            // 
            // comboBoxDither
            // 
            comboBoxDither.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDither.Enabled = false;
            comboBoxDither.FormattingEnabled = true;
            comboBoxDither.Items.AddRange(new object[] { "chess", "sierra", "F-S" });
            comboBoxDither.Location = new Point(239, 145);
            comboBoxDither.Margin = new Padding(4, 5, 4, 5);
            comboBoxDither.Name = "comboBoxDither";
            comboBoxDither.Size = new Size(154, 33);
            comboBoxDither.TabIndex = 13;
            // 
            // checkBoxUseDither
            // 
            checkBoxUseDither.AutoSize = true;
            checkBoxUseDither.Enabled = false;
            checkBoxUseDither.Location = new Point(239, 110);
            checkBoxUseDither.Margin = new Padding(4, 5, 4, 5);
            checkBoxUseDither.Name = "checkBoxUseDither";
            checkBoxUseDither.Size = new Size(143, 29);
            checkBoxUseDither.TabIndex = 14;
            checkBoxUseDither.Text = "Use dithering";
            toolTip1.SetToolTip(checkBoxUseDither, "cannot be used together with color reduction, its slower");
            checkBoxUseDither.UseVisualStyleBackColor = true;
            checkBoxUseDither.CheckedChanged += checkBoxUseDither_CheckedChanged;
            // 
            // comboBoxDistance
            // 
            comboBoxDistance.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDistance.FormattingEnabled = true;
            comboBoxDistance.Items.AddRange(new object[] { "RGB simple", "RGB euclid", "RGBYUV", "YUV euclid", "Weighted RGB" });
            comboBoxDistance.Location = new Point(239, 241);
            comboBoxDistance.Margin = new Padding(4, 5, 4, 5);
            comboBoxDistance.Name = "comboBoxDistance";
            comboBoxDistance.Size = new Size(154, 33);
            comboBoxDistance.TabIndex = 15;
            comboBoxDistance.SelectedIndexChanged += ComboBoxDistance_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(239, 211);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(151, 25);
            label1.TabIndex = 16;
            label1.Text = "Distance method:";
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(18, 350);
            buttonOpen.Margin = new Padding(4, 5, 4, 5);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(108, 39);
            buttonOpen.TabIndex = 17;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += ButtonOpen_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(pictureBoxAtariAprox);
            flowLayoutPanel1.Controls.Add(pictureBoxAtariAproxInverse);
            flowLayoutPanel1.Controls.Add(pictureBoxAtariMix);
            flowLayoutPanel1.Controls.Add(pictureBoxAprox);
            flowLayoutPanel1.Controls.Add(pictureBoxAproxInverse);
            flowLayoutPanel1.Controls.Add(pictureBoxAproxMix);
            flowLayoutPanel1.Controls.Add(pictureBoxCharMask);
            flowLayoutPanel1.Controls.Add(pictureBoxSrcReduced);
            flowLayoutPanel1.Controls.Add(pictureBoxSrcData);
            flowLayoutPanel1.Controls.Add(pictureBoxResult);
            flowLayoutPanel1.Controls.Add(pictureBoxMasks);
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.Location = new Point(587, 0);
            flowLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1142, 954);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // pictureBoxAproxMix
            // 
            pictureBoxAproxMix.Location = new Point(752, 241);
            pictureBoxAproxMix.Margin = new Padding(4, 5, 4, 5);
            pictureBoxAproxMix.Name = "pictureBoxAproxMix";
            pictureBoxAproxMix.Size = new Size(366, 226);
            pictureBoxAproxMix.TabIndex = 12;
            pictureBoxAproxMix.TabStop = false;
            toolTip1.SetToolTip(pictureBoxAproxMix, "Result (interlaced view)");
            // 
            // pictureBoxSrcReduced
            // 
            pictureBoxSrcReduced.BackColor = SystemColors.ActiveCaptionText;
            pictureBoxSrcReduced.Location = new Point(378, 477);
            pictureBoxSrcReduced.Margin = new Padding(4, 5, 4, 5);
            pictureBoxSrcReduced.Name = "pictureBoxSrcReduced";
            pictureBoxSrcReduced.Size = new Size(366, 226);
            pictureBoxSrcReduced.TabIndex = 13;
            pictureBoxSrcReduced.TabStop = false;
            toolTip1.SetToolTip(pictureBoxSrcReduced, "Source image reduced to 256 colors");
            // 
            // pictureBoxSrcData
            // 
            pictureBoxSrcData.Location = new Point(752, 477);
            pictureBoxSrcData.Margin = new Padding(4, 5, 4, 5);
            pictureBoxSrcData.Name = "pictureBoxSrcData";
            pictureBoxSrcData.Size = new Size(366, 226);
            pictureBoxSrcData.TabIndex = 11;
            pictureBoxSrcData.TabStop = false;
            toolTip1.SetToolTip(pictureBoxSrcData, "Source image");
            // 
            // pictureBoxResult
            // 
            pictureBoxResult.BackColor = Color.Red;
            pictureBoxResult.Location = new Point(4, 713);
            pictureBoxResult.Margin = new Padding(4, 5, 4, 5);
            pictureBoxResult.Name = "pictureBoxResult";
            pictureBoxResult.Size = new Size(366, 226);
            pictureBoxResult.TabIndex = 14;
            pictureBoxResult.TabStop = false;
            toolTip1.SetToolTip(pictureBoxResult, "new Result image");
            // 
            // pictureBoxMasks
            // 
            pictureBoxMasks.BackColor = Color.Lime;
            pictureBoxMasks.Location = new Point(378, 713);
            pictureBoxMasks.Margin = new Padding(4, 5, 4, 5);
            pictureBoxMasks.Name = "pictureBoxMasks";
            pictureBoxMasks.Size = new Size(366, 226);
            pictureBoxMasks.TabIndex = 15;
            pictureBoxMasks.TabStop = false;
            toolTip1.SetToolTip(pictureBoxMasks, "new Result image");
            // 
            // buttonXex
            // 
            buttonXex.Location = new Point(444, 469);
            buttonXex.Margin = new Padding(4, 5, 4, 5);
            buttonXex.Name = "buttonXex";
            buttonXex.Size = new Size(132, 116);
            buttonXex.TabIndex = 19;
            buttonXex.Text = "-> xex";
            buttonXex.UseVisualStyleBackColor = true;
            buttonXex.Click += buttonXex_Click;
            // 
            // checkBoxAutoscale
            // 
            checkBoxAutoscale.AutoSize = true;
            checkBoxAutoscale.Checked = true;
            checkBoxAutoscale.CheckState = CheckState.Checked;
            checkBoxAutoscale.Location = new Point(132, 355);
            checkBoxAutoscale.Margin = new Padding(4, 5, 4, 5);
            checkBoxAutoscale.Name = "checkBoxAutoscale";
            checkBoxAutoscale.Size = new Size(216, 29);
            checkBoxAutoscale.TabIndex = 20;
            checkBoxAutoscale.Text = "Autoscale (shrink only)";
            checkBoxAutoscale.UseVisualStyleBackColor = true;
            // 
            // checkBoxInterlace
            // 
            checkBoxInterlace.AutoSize = true;
            checkBoxInterlace.Checked = true;
            checkBoxInterlace.CheckState = CheckState.Checked;
            checkBoxInterlace.Location = new Point(444, 585);
            checkBoxInterlace.Margin = new Padding(4, 5, 4, 5);
            checkBoxInterlace.Name = "checkBoxInterlace";
            checkBoxInterlace.Size = new Size(104, 29);
            checkBoxInterlace.TabIndex = 21;
            checkBoxInterlace.Text = "Interlace";
            checkBoxInterlace.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoUpdate
            // 
            checkBoxAutoUpdate.AutoSize = true;
            checkBoxAutoUpdate.Checked = true;
            checkBoxAutoUpdate.CheckState = CheckState.Checked;
            checkBoxAutoUpdate.Location = new Point(21, 738);
            checkBoxAutoUpdate.Margin = new Padding(4, 5, 4, 5);
            checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            checkBoxAutoUpdate.Size = new Size(133, 29);
            checkBoxAutoUpdate.TabIndex = 22;
            checkBoxAutoUpdate.Text = "Autoupdate";
            checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // buttonAlpaCentauriAI
            // 
            buttonAlpaCentauriAI.Enabled = false;
            buttonAlpaCentauriAI.Location = new Point(18, 679);
            buttonAlpaCentauriAI.Margin = new Padding(4, 5, 4, 5);
            buttonAlpaCentauriAI.Name = "buttonAlpaCentauriAI";
            buttonAlpaCentauriAI.Size = new Size(182, 49);
            buttonAlpaCentauriAI.TabIndex = 23;
            buttonAlpaCentauriAI.Text = "Alpa Centauri AI";
            buttonAlpaCentauriAI.UseVisualStyleBackColor = true;
            buttonAlpaCentauriAI.Click += buttonAlpaCentauriAI_Click;
            // 
            // labelDiff
            // 
            labelDiff.AutoSize = true;
            labelDiff.Location = new Point(209, 636);
            labelDiff.Margin = new Padding(4, 0, 4, 0);
            labelDiff.Name = "labelDiff";
            labelDiff.Size = new Size(41, 25);
            labelDiff.TabIndex = 24;
            labelDiff.Text = "Diff";
            // 
            // numericUpDownPopulation
            // 
            numericUpDownPopulation.Location = new Point(135, 777);
            numericUpDownPopulation.Margin = new Padding(4, 5, 4, 5);
            numericUpDownPopulation.Maximum = new decimal(new int[] { 400, 0, 0, 0 });
            numericUpDownPopulation.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            numericUpDownPopulation.Name = "numericUpDownPopulation";
            numericUpDownPopulation.Size = new Size(81, 31);
            numericUpDownPopulation.TabIndex = 25;
            numericUpDownPopulation.Value = new decimal(new int[] { 40, 0, 0, 0 });
            // 
            // numericUpDownGeneration
            // 
            numericUpDownGeneration.Location = new Point(135, 826);
            numericUpDownGeneration.Margin = new Padding(4, 5, 4, 5);
            numericUpDownGeneration.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDownGeneration.Name = "numericUpDownGeneration";
            numericUpDownGeneration.Size = new Size(81, 31);
            numericUpDownGeneration.TabIndex = 26;
            numericUpDownGeneration.Value = new decimal(new int[] { 40, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 779);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(97, 25);
            label3.TabIndex = 27;
            label3.Text = "Population";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 828);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(106, 25);
            label4.TabIndex = 28;
            label4.Text = "Generations";
            // 
            // listViewPopulation
            // 
            listViewPopulation.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            listViewPopulation.Location = new Point(239, 666);
            listViewPopulation.Margin = new Padding(4, 5, 4, 5);
            listViewPopulation.MultiSelect = false;
            listViewPopulation.Name = "listViewPopulation";
            listViewPopulation.Size = new Size(336, 270);
            listViewPopulation.TabIndex = 29;
            listViewPopulation.UseCompatibleStateImageBehavior = false;
            listViewPopulation.View = View.Details;
            listViewPopulation.SelectedIndexChanged += listViewPopulation_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Results ordered by diff.ascending";
            columnHeader1.Width = 233;
            // 
            // buttonAlpaCentauriInit
            // 
            buttonAlpaCentauriInit.Location = new Point(18, 624);
            buttonAlpaCentauriInit.Margin = new Padding(4, 5, 4, 5);
            buttonAlpaCentauriInit.Name = "buttonAlpaCentauriInit";
            buttonAlpaCentauriInit.Size = new Size(182, 49);
            buttonAlpaCentauriInit.TabIndex = 30;
            buttonAlpaCentauriInit.Text = "Alpa Centauri Init";
            buttonAlpaCentauriInit.UseVisualStyleBackColor = true;
            buttonAlpaCentauriInit.Click += buttonAlpaCentauriInit_Click;
            // 
            // checkBoxColorReduction
            // 
            checkBoxColorReduction.AutoSize = true;
            checkBoxColorReduction.Checked = true;
            checkBoxColorReduction.CheckState = CheckState.Checked;
            checkBoxColorReduction.Location = new Point(239, 309);
            checkBoxColorReduction.Margin = new Padding(4, 5, 4, 5);
            checkBoxColorReduction.Name = "checkBoxColorReduction";
            checkBoxColorReduction.Size = new Size(161, 29);
            checkBoxColorReduction.TabIndex = 31;
            checkBoxColorReduction.Text = "Color reduction";
            checkBoxColorReduction.UseVisualStyleBackColor = true;
            checkBoxColorReduction.CheckedChanged += checkBoxColorReduction_CheckedChanged;
            // 
            // comboBoxAverMethod
            // 
            comboBoxAverMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAverMethod.FormattingEnabled = true;
            comboBoxAverMethod.Items.AddRange(new object[] { "RGB simple", "RGB euclid", "YUV euclid" });
            comboBoxAverMethod.Location = new Point(239, 50);
            comboBoxAverMethod.Margin = new Padding(4, 5, 4, 5);
            comboBoxAverMethod.Name = "comboBoxAverMethod";
            comboBoxAverMethod.Size = new Size(154, 33);
            comboBoxAverMethod.TabIndex = 32;
            comboBoxAverMethod.SelectedIndexChanged += ComboBoxAverMethod_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(239, 20);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(165, 25);
            label5.TabIndex = 33;
            label5.Text = "Averaging method:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBoxHepa);
            groupBox1.Controls.Add(numericUpDownHepaLuma);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(numericUpDownHepaChroma);
            groupBox1.Location = new Point(253, 392);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(173, 222);
            groupBox1.TabIndex = 37;
            groupBox1.TabStop = false;
            groupBox1.Text = "HEPA filter";
            toolTip1.SetToolTip(groupBox1, "Human Eye Perception Adjustment Filter");
            // 
            // checkBoxHepa
            // 
            checkBoxHepa.AutoSize = true;
            checkBoxHepa.Checked = true;
            checkBoxHepa.CheckState = CheckState.Checked;
            checkBoxHepa.Location = new Point(13, 46);
            checkBoxHepa.Margin = new Padding(4, 5, 4, 5);
            checkBoxHepa.Name = "checkBoxHepa";
            checkBoxHepa.Size = new Size(138, 29);
            checkBoxHepa.TabIndex = 42;
            checkBoxHepa.Text = "Enable HEPA";
            checkBoxHepa.UseVisualStyleBackColor = true;
            // 
            // numericUpDownHepaLuma
            // 
            numericUpDownHepaLuma.Location = new Point(14, 117);
            numericUpDownHepaLuma.Margin = new Padding(4, 5, 4, 5);
            numericUpDownHepaLuma.Maximum = new decimal(new int[] { 14, 0, 0, 0 });
            numericUpDownHepaLuma.Name = "numericUpDownHepaLuma";
            numericUpDownHepaLuma.Size = new Size(81, 31);
            numericUpDownHepaLuma.TabIndex = 41;
            numericUpDownHepaLuma.Value = new decimal(new int[] { 14, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 87);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(132, 25);
            label6.TabIndex = 40;
            label6.Text = "Luma max step";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 153);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(152, 25);
            label2.TabIndex = 39;
            label2.Text = "Chroma max step";
            // 
            // numericUpDownHepaChroma
            // 
            numericUpDownHepaChroma.Location = new Point(14, 183);
            numericUpDownHepaChroma.Margin = new Padding(4, 5, 4, 5);
            numericUpDownHepaChroma.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            numericUpDownHepaChroma.Name = "numericUpDownHepaChroma";
            numericUpDownHepaChroma.Size = new Size(81, 31);
            numericUpDownHepaChroma.TabIndex = 38;
            numericUpDownHepaChroma.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // progressBarAI
            // 
            progressBarAI.Location = new Point(18, 881);
            progressBarAI.Margin = new Padding(4, 5, 4, 5);
            progressBarAI.Name = "progressBarAI";
            progressBarAI.Size = new Size(198, 39);
            progressBarAI.Step = 1;
            progressBarAI.TabIndex = 34;
            // 
            // labelGenerationDone
            // 
            labelGenerationDone.AutoSize = true;
            labelGenerationDone.Location = new Point(96, 925);
            labelGenerationDone.Margin = new Padding(4, 0, 4, 0);
            labelGenerationDone.Name = "labelGenerationDone";
            labelGenerationDone.Size = new Size(22, 25);
            labelGenerationDone.TabIndex = 35;
            labelGenerationDone.Text = "0";
            // 
            // button1
            // 
            button1.Location = new Point(444, 372);
            button1.Margin = new Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new Size(131, 70);
            button1.TabIndex = 36;
            button1.Text = "NEW";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // labelPossibleColors
            // 
            labelPossibleColors.AutoSize = true;
            labelPossibleColors.Location = new Point(21, 577);
            labelPossibleColors.Margin = new Padding(4, 0, 4, 0);
            labelPossibleColors.Name = "labelPossibleColors";
            labelPossibleColors.Size = new Size(67, 25);
            labelPossibleColors.TabIndex = 39;
            labelPossibleColors.Text = "Colors:";
            // 
            // pictureBoxIcons
            // 
            pictureBoxIcons.Image = (Image)resources.GetObject("pictureBoxIcons.Image");
            pictureBoxIcons.InitialImage = null;
            pictureBoxIcons.Location = new Point(132, 559);
            pictureBoxIcons.Name = "pictureBoxIcons";
            pictureBoxIcons.Size = new Size(68, 43);
            pictureBoxIcons.TabIndex = 40;
            pictureBoxIcons.TabStop = false;
            pictureBoxIcons.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1729, 954);
            Controls.Add(pictureBoxIcons);
            Controls.Add(labelPossibleColors);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(labelGenerationDone);
            Controls.Add(progressBarAI);
            Controls.Add(label5);
            Controls.Add(comboBoxAverMethod);
            Controls.Add(checkBoxColorReduction);
            Controls.Add(buttonAlpaCentauriInit);
            Controls.Add(listViewPopulation);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(numericUpDownGeneration);
            Controls.Add(numericUpDownPopulation);
            Controls.Add(labelDiff);
            Controls.Add(buttonAlpaCentauriAI);
            Controls.Add(checkBoxAutoUpdate);
            Controls.Add(checkBoxInterlace);
            Controls.Add(checkBoxAutoscale);
            Controls.Add(buttonXex);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(buttonOpen);
            Controls.Add(label1);
            Controls.Add(comboBoxDistance);
            Controls.Add(checkBoxUseDither);
            Controls.Add(comboBoxDither);
            Controls.Add(buttonMixIt);
            Controls.Add(pictureBoxPalette);
            Controls.Add(buttonApproximate);
            Controls.Add(pictureBoxSource);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "AlterLinePictureApproximator (ALPA) v0.8 by MatoSimi";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAprox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAprox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPalette).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAproxInverse).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAproxInverse).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariMix).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharMask).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxAproxMix).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcReduced).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcData).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMasks).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPopulation).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneration).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaLuma).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaChroma).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcons).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxSource;
        private Button buttonApproximate;
        private PictureBox pictureBoxAprox;
        private PictureBox pictureBoxAtariAprox;
        private PictureBox pictureBoxPalette;
        private PictureBox pictureBoxAproxInverse;
        private PictureBox pictureBoxAtariAproxInverse;
        private PictureBox pictureBoxAtariMix;
        private Button buttonMixIt;
        private PictureBox pictureBoxCharMask;
        private CheckBox checkBoxSimpleAvg;
        private ComboBox comboBoxDither;
        private CheckBox checkBoxUseDither;
        private ComboBox comboBoxDistance;
        private Label label1;
        private Button buttonOpen;
        private OpenFileDialog openFileDialog1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonXex;
        private CheckBox checkBoxAutoscale;
        private CheckBox checkBoxInterlace;
        private CheckBox checkBoxAutoUpdate;
        private Button buttonAlpaCentauriAI;
        private Label labelDiff;
        private NumericUpDown numericUpDownPopulation;
        private NumericUpDown numericUpDownGeneration;
        private Label label3;
        private Label label4;
        private PictureBox pictureBoxAproxMix;
        private PictureBox pictureBoxSrcData;
        private PictureBox pictureBoxSrcReduced;
        private ListView listViewPopulation;
        private Button buttonAlpaCentauriInit;
        private ColumnHeader columnHeader1;
        private CheckBox checkBoxColorReduction;
        private ComboBox comboBoxAverMethod;
        private Label label5;
        private ToolTip toolTip1;
        private ProgressBar progressBarAI;
        private Label labelGenerationDone;
        private PictureBox pictureBoxResult;
        private Button button1;
        private PictureBox pictureBoxMasks;
        private GroupBox groupBox1;
        private NumericUpDown numericUpDownHepaLuma;
        private Label label6;
        private Label label2;
        private NumericUpDown numericUpDownHepaChroma;
        private CheckBox checkBoxHepa;
        private Label labelPossibleColors;
        private PictureBox pictureBoxIcons;
    }
}