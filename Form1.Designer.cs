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
            pictureBoxPalette = new PictureBox();
            pictureBoxIdealDither = new PictureBoxWithInterpolationMode();
            comboBoxDither = new ComboBox();
            checkBoxUseDither = new CheckBox();
            comboBoxDistance = new ComboBox();
            label1 = new Label();
            buttonOpen = new Button();
            openFileDialog1 = new OpenFileDialog();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBoxSrcData = new PictureBoxWithInterpolationMode();
            pictureBoxSrcReduced = new PictureBoxWithInterpolationMode();
            pictureBoxResult = new PictureBoxWithInterpolationMode();
            pictureBoxMasks = new PictureBoxWithInterpolationMode();
            pictureBoxResultLines = new PictureBoxWithInterpolationMode();
            pictureBoxPaletteCollection = new PictureBoxWithInterpolationMode();
            groupBoxLSG = new GroupBox();
            labelG = new Label();
            labelS = new Label();
            labelL = new Label();
            trackBarSaturation = new TrackBar();
            trackBarGamma = new TrackBar();
            trackBarLightness = new TrackBar();
            groupBoxDivide = new GroupBox();
            labelDividerLength = new Label();
            labelDividerStart = new Label();
            checkBoxDividerEnabled = new CheckBox();
            numericUpDownDividerLength = new NumericUpDown();
            numericUpDownDividerStart = new NumericUpDown();
            buttonXex = new Button();
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
            label8 = new Label();
            label6 = new Label();
            comboBoxLightness = new ComboBox();
            checkBoxHepa = new CheckBox();
            numericUpDownHepaLuma = new NumericUpDown();
            label2 = new Label();
            numericUpDownHepaChroma = new NumericUpDown();
            numericUpDownDitherStrength = new NumericUpDown();
            buttonImportColors = new Button();
            checkBoxHalfWidthFallback = new CheckBox();
            progressBarAI = new ProgressBar();
            labelGenerationDone = new Label();
            buttonGenerate = new Button();
            labelPossibleColors = new Label();
            pictureBoxIcons = new PictureBox();
            labelOutputSize = new Label();
            splitContainer1 = new SplitContainer();
            panel1 = new Panel();
            comboBoxCharsPerLine = new ComboBox();
            checkBoxAutoGenerate = new CheckBox();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPalette).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIdealDither).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcData).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcReduced).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMasks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResultLines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPaletteCollection).BeginInit();
            groupBoxLSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarSaturation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarGamma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarLightness).BeginInit();
            groupBoxDivide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDividerLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDividerStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPopulation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneration).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaLuma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaChroma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDitherStrength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcons).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.Image = (Image)resources.GetObject("pictureBoxSource.Image");
            pictureBoxSource.Location = new Point(13, 5);
            pictureBoxSource.Margin = new Padding(4, 5, 4, 5);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Size = new Size(320, 224);
            pictureBoxSource.TabIndex = 0;
            pictureBoxSource.TabStop = false;
            // 
            // pictureBoxPalette
            // 
            pictureBoxPalette.ErrorImage = null;
            pictureBoxPalette.Image = (Image)resources.GetObject("pictureBoxPalette.Image");
            pictureBoxPalette.Location = new Point(13, 394);
            pictureBoxPalette.Margin = new Padding(4, 5, 4, 5);
            pictureBoxPalette.Name = "pictureBoxPalette";
            pictureBoxPalette.Size = new Size(171, 154);
            pictureBoxPalette.TabIndex = 4;
            pictureBoxPalette.TabStop = false;
            pictureBoxPalette.MouseDown += PictureBoxPalette_MouseDown;
            // 
            // pictureBoxIdealDither
            // 
            pictureBoxIdealDither.BackColor = Color.Fuchsia;
            pictureBoxIdealDither.Location = new Point(544, 5);
            pictureBoxIdealDither.Margin = new Padding(4, 5, 4, 5);
            pictureBoxIdealDither.Name = "pictureBoxIdealDither";
            pictureBoxIdealDither.Size = new Size(100, 80);
            pictureBoxIdealDither.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxIdealDither.TabIndex = 8;
            pictureBoxIdealDither.TabStop = false;
            toolTip1.SetToolTip(pictureBoxIdealDither, "Ideal dithered image");
            // 
            // comboBoxDither
            // 
            comboBoxDither.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDither.FormattingEnabled = true;
            comboBoxDither.Items.AddRange(new object[] { "chess", "sierra lite", "F-S" });
            comboBoxDither.Location = new Point(389, 130);
            comboBoxDither.Margin = new Padding(4, 5, 4, 5);
            comboBoxDither.Name = "comboBoxDither";
            comboBoxDither.Size = new Size(154, 33);
            comboBoxDither.TabIndex = 13;
            comboBoxDither.SelectedIndexChanged += ComboBoxDither_SelectedIndexChanged;
            // 
            // checkBoxUseDither
            // 
            checkBoxUseDither.AutoSize = true;
            checkBoxUseDither.Location = new Point(389, 93);
            checkBoxUseDither.Margin = new Padding(4, 5, 4, 5);
            checkBoxUseDither.Name = "checkBoxUseDither";
            checkBoxUseDither.Size = new Size(143, 29);
            checkBoxUseDither.TabIndex = 14;
            checkBoxUseDither.Text = "Use dithering";
            toolTip1.SetToolTip(checkBoxUseDither, "cannot be used together with color reduction, its slower");
            checkBoxUseDither.UseVisualStyleBackColor = true;
            checkBoxUseDither.CheckedChanged += CheckBoxUseDither_CheckedChanged;
            // 
            // comboBoxDistance
            // 
            comboBoxDistance.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDistance.FormattingEnabled = true;
            comboBoxDistance.Items.AddRange(new object[] { "RGB simple", "RGB euclid", "RGBYUV", "YUV euclid", "Weighted RGB" });
            comboBoxDistance.Location = new Point(389, 235);
            comboBoxDistance.Margin = new Padding(4, 5, 4, 5);
            comboBoxDistance.Name = "comboBoxDistance";
            comboBoxDistance.Size = new Size(154, 33);
            comboBoxDistance.TabIndex = 15;
            comboBoxDistance.SelectedIndexChanged += ComboBoxDistance_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(389, 205);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(151, 25);
            label1.TabIndex = 16;
            label1.Text = "Distance method:";
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(13, 303);
            buttonOpen.Margin = new Padding(4, 5, 4, 5);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(171, 39);
            buttonOpen.TabIndex = 17;
            buttonOpen.Text = "Open Picture";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += ButtonOpen_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(pictureBoxSrcData);
            flowLayoutPanel1.Controls.Add(pictureBoxSrcReduced);
            flowLayoutPanel1.Controls.Add(pictureBoxResult);
            flowLayoutPanel1.Controls.Add(pictureBoxMasks);
            flowLayoutPanel1.Controls.Add(pictureBoxResultLines);
            flowLayoutPanel1.Controls.Add(pictureBoxIdealDither);
            flowLayoutPanel1.Controls.Add(pictureBoxPaletteCollection);
            flowLayoutPanel1.Controls.Add(groupBoxLSG);
            flowLayoutPanel1.Controls.Add(groupBoxDivide);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(827, 954);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // pictureBoxSrcData
            // 
            pictureBoxSrcData.BackColor = Color.Blue;
            pictureBoxSrcData.Location = new Point(4, 5);
            pictureBoxSrcData.Margin = new Padding(4, 5, 4, 5);
            pictureBoxSrcData.Name = "pictureBoxSrcData";
            pictureBoxSrcData.Size = new Size(100, 80);
            pictureBoxSrcData.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSrcData.TabIndex = 11;
            pictureBoxSrcData.TabStop = false;
            toolTip1.SetToolTip(pictureBoxSrcData, "Source image");
            // 
            // pictureBoxSrcReduced
            // 
            pictureBoxSrcReduced.BackColor = SystemColors.ActiveCaptionText;
            pictureBoxSrcReduced.Location = new Point(112, 5);
            pictureBoxSrcReduced.Margin = new Padding(4, 5, 4, 5);
            pictureBoxSrcReduced.Name = "pictureBoxSrcReduced";
            pictureBoxSrcReduced.Size = new Size(100, 80);
            pictureBoxSrcReduced.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSrcReduced.TabIndex = 13;
            pictureBoxSrcReduced.TabStop = false;
            toolTip1.SetToolTip(pictureBoxSrcReduced, "Source image reduced to 256 colors");
            // 
            // pictureBoxResult
            // 
            pictureBoxResult.BackColor = Color.Red;
            pictureBoxResult.Location = new Point(220, 5);
            pictureBoxResult.Margin = new Padding(4, 5, 4, 5);
            pictureBoxResult.Name = "pictureBoxResult";
            pictureBoxResult.Size = new Size(100, 80);
            pictureBoxResult.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxResult.TabIndex = 14;
            pictureBoxResult.TabStop = false;
            toolTip1.SetToolTip(pictureBoxResult, "new Result image");
            // 
            // pictureBoxMasks
            // 
            pictureBoxMasks.BackColor = Color.Lime;
            pictureBoxMasks.Location = new Point(328, 5);
            pictureBoxMasks.Margin = new Padding(4, 5, 4, 5);
            pictureBoxMasks.Name = "pictureBoxMasks";
            pictureBoxMasks.Size = new Size(100, 80);
            pictureBoxMasks.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxMasks.TabIndex = 15;
            pictureBoxMasks.TabStop = false;
            toolTip1.SetToolTip(pictureBoxMasks, "char & pmg masks");
            // 
            // pictureBoxResultLines
            // 
            pictureBoxResultLines.BackColor = Color.Yellow;
            pictureBoxResultLines.Location = new Point(436, 5);
            pictureBoxResultLines.Margin = new Padding(4, 5, 4, 5);
            pictureBoxResultLines.Name = "pictureBoxResultLines";
            pictureBoxResultLines.Size = new Size(100, 80);
            pictureBoxResultLines.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxResultLines.TabIndex = 16;
            pictureBoxResultLines.TabStop = false;
            toolTip1.SetToolTip(pictureBoxResultLines, "Result without blending");
            // 
            // pictureBoxPaletteCollection
            // 
            pictureBoxPaletteCollection.BackColor = Color.FromArgb(255, 128, 0);
            pictureBoxPaletteCollection.Location = new Point(652, 5);
            pictureBoxPaletteCollection.Margin = new Padding(4, 5, 4, 5);
            pictureBoxPaletteCollection.Name = "pictureBoxPaletteCollection";
            pictureBoxPaletteCollection.Size = new Size(100, 80);
            pictureBoxPaletteCollection.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPaletteCollection.TabIndex = 51;
            pictureBoxPaletteCollection.TabStop = false;
            toolTip1.SetToolTip(pictureBoxPaletteCollection, "Palette Collection");
            // 
            // groupBoxLSG
            // 
            groupBoxLSG.Controls.Add(labelG);
            groupBoxLSG.Controls.Add(labelS);
            groupBoxLSG.Controls.Add(labelL);
            groupBoxLSG.Controls.Add(trackBarSaturation);
            groupBoxLSG.Controls.Add(trackBarGamma);
            groupBoxLSG.Controls.Add(trackBarLightness);
            groupBoxLSG.Location = new Point(3, 93);
            groupBoxLSG.Name = "groupBoxLSG";
            groupBoxLSG.Size = new Size(382, 269);
            groupBoxLSG.TabIndex = 49;
            groupBoxLSG.TabStop = false;
            groupBoxLSG.Text = "Lightness Saturation Gamma";
            // 
            // labelG
            // 
            labelG.AutoSize = true;
            labelG.Location = new Point(16, 186);
            labelG.Margin = new Padding(4, 0, 4, 0);
            labelG.Name = "labelG";
            labelG.Size = new Size(24, 25);
            labelG.TabIndex = 51;
            labelG.Text = "G";
            labelG.Click += label11_Click;
            // 
            // labelS
            // 
            labelS.AutoSize = true;
            labelS.Location = new Point(16, 112);
            labelS.Margin = new Padding(4, 0, 4, 0);
            labelS.Name = "labelS";
            labelS.Size = new Size(22, 25);
            labelS.TabIndex = 50;
            labelS.Text = "S";
            // 
            // labelL
            // 
            labelL.AutoSize = true;
            labelL.Location = new Point(16, 45);
            labelL.Margin = new Padding(4, 0, 4, 0);
            labelL.Name = "labelL";
            labelL.Size = new Size(20, 25);
            labelL.TabIndex = 49;
            labelL.Text = "L";
            // 
            // trackBarSaturation
            // 
            trackBarSaturation.Location = new Point(42, 105);
            trackBarSaturation.Maximum = 20;
            trackBarSaturation.Name = "trackBarSaturation";
            trackBarSaturation.Size = new Size(333, 69);
            trackBarSaturation.TabIndex = 48;
            trackBarSaturation.TickFrequency = 10;
            trackBarSaturation.TickStyle = TickStyle.None;
            trackBarSaturation.Value = 10;
            trackBarSaturation.Scroll += AdjustmentControls_Changed;
            // 
            // trackBarGamma
            // 
            trackBarGamma.Location = new Point(42, 186);
            trackBarGamma.Maximum = 50;
            trackBarGamma.Minimum = 1;
            trackBarGamma.Name = "trackBarGamma";
            trackBarGamma.Size = new Size(333, 69);
            trackBarGamma.TabIndex = 48;
            trackBarGamma.TickStyle = TickStyle.TopLeft;
            toolTip1.SetToolTip(trackBarGamma, "Gamma");
            trackBarGamma.Value = 10;
            trackBarGamma.Scroll += AdjustmentControls_Changed;
            // 
            // trackBarLightness
            // 
            trackBarLightness.Location = new Point(41, 30);
            trackBarLightness.Maximum = 20;
            trackBarLightness.Name = "trackBarLightness";
            trackBarLightness.Size = new Size(334, 69);
            trackBarLightness.TabIndex = 47;
            trackBarLightness.Value = 10;
            trackBarLightness.Scroll += AdjustmentControls_Changed;
            // 
            // groupBoxDivide
            // 
            groupBoxDivide.Controls.Add(labelDividerLength);
            groupBoxDivide.Controls.Add(labelDividerStart);
            groupBoxDivide.Controls.Add(checkBoxDividerEnabled);
            groupBoxDivide.Controls.Add(numericUpDownDividerLength);
            groupBoxDivide.Controls.Add(numericUpDownDividerStart);
            groupBoxDivide.Location = new Point(391, 93);
            groupBoxDivide.Name = "groupBoxDivide";
            groupBoxDivide.Size = new Size(300, 211);
            groupBoxDivide.TabIndex = 50;
            groupBoxDivide.TabStop = false;
            groupBoxDivide.Text = "Vertical Divider";
            // 
            // labelDividerLength
            // 
            labelDividerLength.AutoSize = true;
            labelDividerLength.Location = new Point(7, 114);
            labelDividerLength.Margin = new Padding(4, 0, 4, 0);
            labelDividerLength.Name = "labelDividerLength";
            labelDividerLength.Size = new Size(70, 25);
            labelDividerLength.TabIndex = 46;
            labelDividerLength.Text = "Length:";
            // 
            // labelDividerStart
            // 
            labelDividerStart.AutoSize = true;
            labelDividerStart.Location = new Point(7, 70);
            labelDividerStart.Margin = new Padding(4, 0, 4, 0);
            labelDividerStart.Name = "labelDividerStart";
            labelDividerStart.Size = new Size(52, 25);
            labelDividerStart.TabIndex = 45;
            labelDividerStart.Text = "Start:";
            // 
            // checkBoxDividerEnabled
            // 
            checkBoxDividerEnabled.AutoSize = true;
            checkBoxDividerEnabled.Location = new Point(7, 30);
            checkBoxDividerEnabled.Margin = new Padding(4, 5, 4, 5);
            checkBoxDividerEnabled.Name = "checkBoxDividerEnabled";
            checkBoxDividerEnabled.Size = new Size(101, 29);
            checkBoxDividerEnabled.TabIndex = 44;
            checkBoxDividerEnabled.Text = "Enabled";
            checkBoxDividerEnabled.UseVisualStyleBackColor = true;
            // 
            // numericUpDownDividerLength
            // 
            numericUpDownDividerLength.Location = new Point(79, 112);
            numericUpDownDividerLength.Margin = new Padding(4, 5, 4, 5);
            numericUpDownDividerLength.Maximum = new decimal(new int[] { 224, 0, 0, 0 });
            numericUpDownDividerLength.Name = "numericUpDownDividerLength";
            numericUpDownDividerLength.Size = new Size(81, 31);
            numericUpDownDividerLength.TabIndex = 43;
            numericUpDownDividerLength.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // numericUpDownDividerStart
            // 
            numericUpDownDividerStart.Location = new Point(79, 68);
            numericUpDownDividerStart.Margin = new Padding(4, 5, 4, 5);
            numericUpDownDividerStart.Maximum = new decimal(new int[] { 224, 0, 0, 0 });
            numericUpDownDividerStart.Name = "numericUpDownDividerStart";
            numericUpDownDividerStart.Size = new Size(81, 31);
            numericUpDownDividerStart.TabIndex = 42;
            numericUpDownDividerStart.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // buttonXex
            // 
            buttonXex.Location = new Point(488, 697);
            buttonXex.Margin = new Padding(4, 5, 4, 5);
            buttonXex.Name = "buttonXex";
            buttonXex.Size = new Size(83, 79);
            buttonXex.TabIndex = 19;
            buttonXex.Text = "-> xex";
            buttonXex.UseVisualStyleBackColor = true;
            buttonXex.Click += ButtonXex_Click;
            // 
            // checkBoxInterlace
            // 
            checkBoxInterlace.AutoSize = true;
            checkBoxInterlace.Checked = true;
            checkBoxInterlace.CheckState = CheckState.Checked;
            checkBoxInterlace.Location = new Point(470, 786);
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
            checkBoxAutoUpdate.Location = new Point(16, 732);
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
            buttonAlpaCentauriAI.Location = new Point(13, 673);
            buttonAlpaCentauriAI.Margin = new Padding(4, 5, 4, 5);
            buttonAlpaCentauriAI.Name = "buttonAlpaCentauriAI";
            buttonAlpaCentauriAI.Size = new Size(182, 49);
            buttonAlpaCentauriAI.TabIndex = 23;
            buttonAlpaCentauriAI.Text = "Alpa Centauri AI";
            buttonAlpaCentauriAI.UseVisualStyleBackColor = true;
            buttonAlpaCentauriAI.Click += ButtonAlpaCentauriAI_Click;
            // 
            // labelDiff
            // 
            labelDiff.AutoSize = true;
            labelDiff.Location = new Point(204, 630);
            labelDiff.Margin = new Padding(4, 0, 4, 0);
            labelDiff.Name = "labelDiff";
            labelDiff.Size = new Size(41, 25);
            labelDiff.TabIndex = 24;
            labelDiff.Text = "Diff";
            // 
            // numericUpDownPopulation
            // 
            numericUpDownPopulation.Increment = new decimal(new int[] { 4, 0, 0, 0 });
            numericUpDownPopulation.Location = new Point(130, 771);
            numericUpDownPopulation.Margin = new Padding(4, 5, 4, 5);
            numericUpDownPopulation.Maximum = new decimal(new int[] { 800, 0, 0, 0 });
            numericUpDownPopulation.Minimum = new decimal(new int[] { 8, 0, 0, 0 });
            numericUpDownPopulation.Name = "numericUpDownPopulation";
            numericUpDownPopulation.Size = new Size(81, 31);
            numericUpDownPopulation.TabIndex = 25;
            numericUpDownPopulation.Value = new decimal(new int[] { 40, 0, 0, 0 });
            // 
            // numericUpDownGeneration
            // 
            numericUpDownGeneration.Location = new Point(130, 820);
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
            label3.Location = new Point(16, 773);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(97, 25);
            label3.TabIndex = 27;
            label3.Text = "Population";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 822);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(106, 25);
            label4.TabIndex = 28;
            label4.Text = "Generations";
            // 
            // listViewPopulation
            // 
            listViewPopulation.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            listViewPopulation.Location = new Point(234, 660);
            listViewPopulation.Margin = new Padding(4, 5, 4, 5);
            listViewPopulation.MultiSelect = false;
            listViewPopulation.Name = "listViewPopulation";
            listViewPopulation.Size = new Size(228, 270);
            listViewPopulation.TabIndex = 29;
            listViewPopulation.UseCompatibleStateImageBehavior = false;
            listViewPopulation.View = View.Details;
            listViewPopulation.SelectedIndexChanged += ListViewPopulation_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Results ordered by diff.ascending";
            columnHeader1.Width = 233;
            // 
            // buttonAlpaCentauriInit
            // 
            buttonAlpaCentauriInit.Location = new Point(13, 618);
            buttonAlpaCentauriInit.Margin = new Padding(4, 5, 4, 5);
            buttonAlpaCentauriInit.Name = "buttonAlpaCentauriInit";
            buttonAlpaCentauriInit.Size = new Size(182, 49);
            buttonAlpaCentauriInit.TabIndex = 30;
            buttonAlpaCentauriInit.Text = "Alpa Centauri Init";
            buttonAlpaCentauriInit.UseVisualStyleBackColor = true;
            buttonAlpaCentauriInit.Click += ButtonAlpaCentauriInit_Click;
            // 
            // checkBoxColorReduction
            // 
            checkBoxColorReduction.AutoSize = true;
            checkBoxColorReduction.Checked = true;
            checkBoxColorReduction.CheckState = CheckState.Checked;
            checkBoxColorReduction.Location = new Point(204, 313);
            checkBoxColorReduction.Margin = new Padding(4, 5, 4, 5);
            checkBoxColorReduction.Name = "checkBoxColorReduction";
            checkBoxColorReduction.Size = new Size(161, 29);
            checkBoxColorReduction.TabIndex = 31;
            checkBoxColorReduction.Text = "Color reduction";
            toolTip1.SetToolTip(checkBoxColorReduction, "Use 256colors instead of 24bit");
            checkBoxColorReduction.UseVisualStyleBackColor = true;
            checkBoxColorReduction.CheckedChanged += CheckBoxColorReduction_CheckedChanged;
            // 
            // comboBoxAverMethod
            // 
            comboBoxAverMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAverMethod.FormattingEnabled = true;
            comboBoxAverMethod.Items.AddRange(new object[] { "RGB simple", "RGB euclid", "YUV euclid" });
            comboBoxAverMethod.Location = new Point(389, 35);
            comboBoxAverMethod.Margin = new Padding(4, 5, 4, 5);
            comboBoxAverMethod.Name = "comboBoxAverMethod";
            comboBoxAverMethod.Size = new Size(154, 33);
            comboBoxAverMethod.TabIndex = 32;
            comboBoxAverMethod.SelectedIndexChanged += ComboBoxAverMethod_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(389, 5);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(165, 25);
            label5.TabIndex = 33;
            label5.Text = "Averaging method:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(comboBoxLightness);
            groupBox1.Controls.Add(checkBoxHepa);
            groupBox1.Controls.Add(numericUpDownHepaLuma);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(numericUpDownHepaChroma);
            groupBox1.Location = new Point(248, 386);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(322, 222);
            groupBox1.TabIndex = 37;
            groupBox1.TabStop = false;
            groupBox1.Text = "HEPA filter";
            toolTip1.SetToolTip(groupBox1, "Human Eye Perceptual Adjustment Filter");
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(7, 66);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(238, 25);
            label8.TabIndex = 45;
            label8.Text = "Brightness calculation mode:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 137);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(171, 25);
            label6.TabIndex = 44;
            label6.Text = "Brightness max step";
            // 
            // comboBoxLightness
            // 
            comboBoxLightness.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLightness.FormattingEnabled = true;
            comboBoxLightness.Items.AddRange(new object[] { "Atari Lightness ($0x) ", "Luminance (0..255) ", "L* perceived lightness (0..100)" });
            comboBoxLightness.Location = new Point(7, 96);
            comboBoxLightness.Margin = new Padding(4, 5, 4, 5);
            comboBoxLightness.Name = "comboBoxLightness";
            comboBoxLightness.Size = new Size(308, 33);
            comboBoxLightness.TabIndex = 43;
            comboBoxLightness.SelectedIndexChanged += ComboBoxLightness_SelectedIndexChanged;
            // 
            // checkBoxHepa
            // 
            checkBoxHepa.AutoSize = true;
            checkBoxHepa.Location = new Point(14, 32);
            checkBoxHepa.Margin = new Padding(4, 5, 4, 5);
            checkBoxHepa.Name = "checkBoxHepa";
            checkBoxHepa.Size = new Size(138, 29);
            checkBoxHepa.TabIndex = 42;
            checkBoxHepa.Text = "Enable HEPA";
            checkBoxHepa.UseVisualStyleBackColor = true;
            // 
            // numericUpDownHepaLuma
            // 
            numericUpDownHepaLuma.Location = new Point(234, 135);
            numericUpDownHepaLuma.Margin = new Padding(4, 5, 4, 5);
            numericUpDownHepaLuma.Maximum = new decimal(new int[] { 14, 0, 0, 0 });
            numericUpDownHepaLuma.Name = "numericUpDownHepaLuma";
            numericUpDownHepaLuma.Size = new Size(81, 31);
            numericUpDownHepaLuma.TabIndex = 41;
            numericUpDownHepaLuma.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 185);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(175, 25);
            label2.TabIndex = 39;
            label2.Text = "Color ($x0) max step";
            // 
            // numericUpDownHepaChroma
            // 
            numericUpDownHepaChroma.Location = new Point(234, 183);
            numericUpDownHepaChroma.Margin = new Padding(4, 5, 4, 5);
            numericUpDownHepaChroma.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            numericUpDownHepaChroma.Name = "numericUpDownHepaChroma";
            numericUpDownHepaChroma.Size = new Size(81, 31);
            numericUpDownHepaChroma.TabIndex = 38;
            numericUpDownHepaChroma.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // numericUpDownDitherStrength
            // 
            numericUpDownDitherStrength.Location = new Point(479, 171);
            numericUpDownDitherStrength.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownDitherStrength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDitherStrength.Name = "numericUpDownDitherStrength";
            numericUpDownDitherStrength.Size = new Size(64, 31);
            numericUpDownDitherStrength.TabIndex = 42;
            toolTip1.SetToolTip(numericUpDownDitherStrength, "10=max,1=min");
            numericUpDownDitherStrength.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownDitherStrength.ValueChanged += NumericUpDownDitherStrength_ValueChanged;
            // 
            // buttonImportColors
            // 
            buttonImportColors.Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonImportColors.Location = new Point(192, 394);
            buttonImportColors.Margin = new Padding(4, 5, 4, 5);
            buttonImportColors.Name = "buttonImportColors";
            buttonImportColors.Size = new Size(49, 121);
            buttonImportColors.TabIndex = 45;
            buttonImportColors.Text = "";
            toolTip1.SetToolTip(buttonImportColors, "Import colors (color2.dat)");
            buttonImportColors.UseVisualStyleBackColor = true;
            buttonImportColors.Click += ButtonImportColors_Click;
            // 
            // checkBoxHalfWidthFallback
            // 
            checkBoxHalfWidthFallback.AutoSize = true;
            checkBoxHalfWidthFallback.Location = new Point(13, 342);
            checkBoxHalfWidthFallback.Margin = new Padding(4, 5, 4, 5);
            checkBoxHalfWidthFallback.Name = "checkBoxHalfWidthFallback";
            checkBoxHalfWidthFallback.Size = new Size(179, 29);
            checkBoxHalfWidthFallback.TabIndex = 48;
            checkBoxHalfWidthFallback.Text = "Halfwidth fallback";
            toolTip1.SetToolTip(checkBoxHalfWidthFallback, "Use 256colors instead of 24bit");
            checkBoxHalfWidthFallback.UseVisualStyleBackColor = true;
            // 
            // progressBarAI
            // 
            progressBarAI.Location = new Point(13, 875);
            progressBarAI.Margin = new Padding(4, 5, 4, 5);
            progressBarAI.Name = "progressBarAI";
            progressBarAI.Size = new Size(198, 39);
            progressBarAI.Step = 1;
            progressBarAI.TabIndex = 34;
            // 
            // labelGenerationDone
            // 
            labelGenerationDone.AutoSize = true;
            labelGenerationDone.Location = new Point(91, 919);
            labelGenerationDone.Margin = new Padding(4, 0, 4, 0);
            labelGenerationDone.Name = "labelGenerationDone";
            labelGenerationDone.Size = new Size(22, 25);
            labelGenerationDone.TabIndex = 35;
            labelGenerationDone.Text = "0";
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new Point(389, 278);
            buttonGenerate.Margin = new Padding(4, 5, 4, 5);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(181, 109);
            buttonGenerate.TabIndex = 36;
            buttonGenerate.Text = "Generate";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += ButtonGenerate_Click;
            // 
            // labelPossibleColors
            // 
            labelPossibleColors.AutoSize = true;
            labelPossibleColors.Location = new Point(16, 571);
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
            pictureBoxIcons.Location = new Point(127, 553);
            pictureBoxIcons.Name = "pictureBoxIcons";
            pictureBoxIcons.Size = new Size(68, 43);
            pictureBoxIcons.TabIndex = 40;
            pictureBoxIcons.TabStop = false;
            pictureBoxIcons.Visible = false;
            // 
            // labelOutputSize
            // 
            labelOutputSize.AutoSize = true;
            labelOutputSize.Location = new Point(13, 258);
            labelOutputSize.Margin = new Padding(4, 0, 4, 0);
            labelOutputSize.Name = "labelOutputSize";
            labelOutputSize.Size = new Size(191, 25);
            labelOutputSize.TabIndex = 41;
            labelOutputSize.Text = "Output: 256 * 192 (24)";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Size = new Size(1414, 954);
            splitContainer1.SplitterDistance = 583;
            splitContainer1.TabIndex = 42;
            // 
            // panel1
            // 
            panel1.Controls.Add(checkBoxHalfWidthFallback);
            panel1.Controls.Add(comboBoxCharsPerLine);
            panel1.Controls.Add(checkBoxAutoGenerate);
            panel1.Controls.Add(buttonImportColors);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(numericUpDownDitherStrength);
            panel1.Controls.Add(pictureBoxSource);
            panel1.Controls.Add(labelOutputSize);
            panel1.Controls.Add(pictureBoxIcons);
            panel1.Controls.Add(pictureBoxPalette);
            panel1.Controls.Add(labelPossibleColors);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(comboBoxDither);
            panel1.Controls.Add(buttonGenerate);
            panel1.Controls.Add(checkBoxUseDither);
            panel1.Controls.Add(labelGenerationDone);
            panel1.Controls.Add(comboBoxDistance);
            panel1.Controls.Add(progressBarAI);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(buttonOpen);
            panel1.Controls.Add(comboBoxAverMethod);
            panel1.Controls.Add(buttonXex);
            panel1.Controls.Add(checkBoxColorReduction);
            panel1.Controls.Add(checkBoxInterlace);
            panel1.Controls.Add(buttonAlpaCentauriInit);
            panel1.Controls.Add(checkBoxAutoUpdate);
            panel1.Controls.Add(listViewPopulation);
            panel1.Controls.Add(buttonAlpaCentauriAI);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(labelDiff);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(numericUpDownPopulation);
            panel1.Controls.Add(numericUpDownGeneration);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(583, 954);
            panel1.TabIndex = 0;
            // 
            // comboBoxCharsPerLine
            // 
            comboBoxCharsPerLine.FormattingEnabled = true;
            comboBoxCharsPerLine.Items.AddRange(new object[] { "32 (narrow)", "40 (normal)" });
            comboBoxCharsPerLine.Location = new Point(211, 255);
            comboBoxCharsPerLine.Name = "comboBoxCharsPerLine";
            comboBoxCharsPerLine.Size = new Size(154, 33);
            comboBoxCharsPerLine.TabIndex = 47;
            comboBoxCharsPerLine.SelectedIndexChanged += ComboBoxCharsPerLine_SelectedIndexChanged;
            // 
            // checkBoxAutoGenerate
            // 
            checkBoxAutoGenerate.AutoSize = true;
            checkBoxAutoGenerate.BackColor = SystemColors.ButtonHighlight;
            checkBoxAutoGenerate.Location = new Point(488, 352);
            checkBoxAutoGenerate.Margin = new Padding(4, 5, 4, 5);
            checkBoxAutoGenerate.Name = "checkBoxAutoGenerate";
            checkBoxAutoGenerate.Size = new Size(77, 29);
            checkBoxAutoGenerate.TabIndex = 46;
            checkBoxAutoGenerate.Text = "Auto";
            checkBoxAutoGenerate.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(389, 173);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(83, 25);
            label7.TabIndex = 43;
            label7.Text = "Strength:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1414, 954);
            Controls.Add(splitContainer1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "appText";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPalette).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIdealDither).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcData).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrcReduced).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMasks).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResultLines).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPaletteCollection).EndInit();
            groupBoxLSG.ResumeLayout(false);
            groupBoxLSG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarSaturation).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarGamma).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarLightness).EndInit();
            groupBoxDivide.ResumeLayout(false);
            groupBoxDivide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDividerLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDividerStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPopulation).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneration).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaLuma).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHepaChroma).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDitherStrength).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcons).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxSource;
        private PictureBox pictureBoxPalette;
        private CheckBox checkBoxSimpleAvg;
        private ComboBox comboBoxDither;
        private CheckBox checkBoxUseDither;
        private ComboBox comboBoxDistance;
        private Label label1;
        private Button buttonOpen;
        private OpenFileDialog openFileDialog1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonXex;
        private CheckBox checkBoxInterlace;
        private CheckBox checkBoxAutoUpdate;
        private Button buttonAlpaCentauriAI;
        private Label labelDiff;
        private NumericUpDown numericUpDownPopulation;
        private NumericUpDown numericUpDownGeneration;
        private Label label3;
        private Label label4;
        private ListView listViewPopulation;
        private Button buttonAlpaCentauriInit;
        private ColumnHeader columnHeader1;
        private CheckBox checkBoxColorReduction;
        private ComboBox comboBoxAverMethod;
        private Label label5;
        private ToolTip toolTip1;
        private ProgressBar progressBarAI;
        private Label labelGenerationDone;
        private Button buttonGenerate;
        private GroupBox groupBox1;
        private NumericUpDown numericUpDownHepaLuma;
        private Label label2;
        private NumericUpDown numericUpDownHepaChroma;
        private CheckBox checkBoxHepa;
        private Label labelPossibleColors;
        private PictureBox pictureBoxIcons;
        private Label labelOutputSize;
        private SplitContainer splitContainer1;
        private Panel panel1;
        private PictureBoxWithInterpolationMode pictureBoxIdealDither;
        private PictureBoxWithInterpolationMode pictureBoxSrcReduced;
        private PictureBoxWithInterpolationMode pictureBoxResult;
        private PictureBoxWithInterpolationMode pictureBoxMasks;
        private PictureBoxWithInterpolationMode pictureBoxResultLines;
        private PictureBoxWithInterpolationMode pictureBoxSrcData;
        private NumericUpDown numericUpDownDitherStrength;
        private Label label7;
        private Button buttonImportColors;
        private ComboBox comboBoxLightness;
        private Label label8;
        private Label label6;
        private CheckBox checkBoxAutoGenerate;
        private TrackBar trackBarLightness;
        private TrackBar trackBarGamma;
        private ComboBox comboBoxCharsPerLine;
        private TrackBar trackBarSaturation;
        private GroupBox groupBoxLSG;
        private Label labelG;
        private Label labelS;
        private Label labelL;
        private CheckBox checkBoxHalfWidthFallback;
        private GroupBox groupBoxDivide;
        private NumericUpDown numericUpDownDividerStart;
        private NumericUpDown numericUpDownDividerLength;
        private Label labelDividerLength;
        private Label labelDividerStart;
        private CheckBox checkBoxDividerEnabled;
        private PictureBoxWithInterpolationMode pictureBoxPaletteCollection;
    }
}