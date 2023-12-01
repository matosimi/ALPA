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
            progressBarAI = new ProgressBar();
            labelGenerationDone = new Label();
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
            ((System.ComponentModel.ISupportInitialize)numericUpDownPopulation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneration).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.Image = (Image)resources.GetObject("pictureBoxSource.Image");
            pictureBoxSource.Location = new Point(12, 12);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Size = new Size(128, 192);
            pictureBoxSource.TabIndex = 0;
            pictureBoxSource.TabStop = false;
            // 
            // buttonApproximate
            // 
            buttonApproximate.Location = new Point(311, 12);
            buttonApproximate.Name = "buttonApproximate";
            buttonApproximate.Size = new Size(93, 73);
            buttonApproximate.TabIndex = 1;
            buttonApproximate.Text = "Approximate";
            buttonApproximate.UseVisualStyleBackColor = true;
            buttonApproximate.Click += ButtonApproximate_Click;
            // 
            // pictureBoxAprox
            // 
            pictureBoxAprox.Location = new Point(3, 145);
            pictureBoxAprox.Name = "pictureBoxAprox";
            pictureBoxAprox.Size = new Size(256, 136);
            pictureBoxAprox.TabIndex = 2;
            pictureBoxAprox.TabStop = false;
            // 
            // pictureBoxAtariAprox
            // 
            pictureBoxAtariAprox.Location = new Point(3, 3);
            pictureBoxAtariAprox.Name = "pictureBoxAtariAprox";
            pictureBoxAtariAprox.Size = new Size(256, 136);
            pictureBoxAtariAprox.TabIndex = 3;
            pictureBoxAtariAprox.TabStop = false;
            // 
            // pictureBoxPalette
            // 
            pictureBoxPalette.ErrorImage = null;
            pictureBoxPalette.Image = AlterLinePictureApproximator.Properties.Resources.pal;
            pictureBoxPalette.Location = new Point(12, 240);
            pictureBoxPalette.Name = "pictureBoxPalette";
            pictureBoxPalette.Size = new Size(128, 97);
            pictureBoxPalette.TabIndex = 4;
            pictureBoxPalette.TabStop = false;
            pictureBoxPalette.MouseDown += pictureBoxPalette_MouseDown;
            // 
            // pictureBoxAproxInverse
            // 
            pictureBoxAproxInverse.Location = new Point(265, 145);
            pictureBoxAproxInverse.Name = "pictureBoxAproxInverse";
            pictureBoxAproxInverse.Size = new Size(256, 136);
            pictureBoxAproxInverse.TabIndex = 6;
            pictureBoxAproxInverse.TabStop = false;
            // 
            // pictureBoxAtariAproxInverse
            // 
            pictureBoxAtariAproxInverse.Location = new Point(265, 3);
            pictureBoxAtariAproxInverse.Name = "pictureBoxAtariAproxInverse";
            pictureBoxAtariAproxInverse.Size = new Size(256, 136);
            pictureBoxAtariAproxInverse.TabIndex = 7;
            pictureBoxAtariAproxInverse.TabStop = false;
            // 
            // pictureBoxAtariMix
            // 
            pictureBoxAtariMix.Location = new Point(527, 3);
            pictureBoxAtariMix.Name = "pictureBoxAtariMix";
            pictureBoxAtariMix.Size = new Size(256, 136);
            pictureBoxAtariMix.TabIndex = 8;
            pictureBoxAtariMix.TabStop = false;
            toolTip1.SetToolTip(pictureBoxAtariMix, "Result (non interlace view)");
            // 
            // buttonMixIt
            // 
            buttonMixIt.Location = new Point(311, 145);
            buttonMixIt.Name = "buttonMixIt";
            buttonMixIt.Size = new Size(93, 73);
            buttonMixIt.TabIndex = 9;
            buttonMixIt.Text = "Mix";
            buttonMixIt.UseVisualStyleBackColor = true;
            buttonMixIt.Click += ButtonMixIt_Click;
            // 
            // pictureBoxCharMask
            // 
            pictureBoxCharMask.Location = new Point(3, 287);
            pictureBoxCharMask.Name = "pictureBoxCharMask";
            pictureBoxCharMask.Size = new Size(256, 136);
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
            comboBoxDither.Location = new Point(167, 87);
            comboBoxDither.Name = "comboBoxDither";
            comboBoxDither.Size = new Size(109, 23);
            comboBoxDither.TabIndex = 13;
            // 
            // checkBoxUseDither
            // 
            checkBoxUseDither.AutoSize = true;
            checkBoxUseDither.Enabled = false;
            checkBoxUseDither.Location = new Point(167, 66);
            checkBoxUseDither.Name = "checkBoxUseDither";
            checkBoxUseDither.Size = new Size(96, 19);
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
            comboBoxDistance.Location = new Point(167, 145);
            comboBoxDistance.Name = "comboBoxDistance";
            comboBoxDistance.Size = new Size(109, 23);
            comboBoxDistance.TabIndex = 15;
            comboBoxDistance.SelectedIndexChanged += ComboBoxDistance_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(167, 127);
            label1.Name = "label1";
            label1.Size = new Size(100, 15);
            label1.TabIndex = 16;
            label1.Text = "Distance method:";
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(12, 210);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(75, 23);
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
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.Location = new Point(410, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(800, 572);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // pictureBoxAproxMix
            // 
            pictureBoxAproxMix.Location = new Point(527, 145);
            pictureBoxAproxMix.Name = "pictureBoxAproxMix";
            pictureBoxAproxMix.Size = new Size(256, 136);
            pictureBoxAproxMix.TabIndex = 12;
            pictureBoxAproxMix.TabStop = false;
            toolTip1.SetToolTip(pictureBoxAproxMix, "Result (interlaced view)");
            // 
            // pictureBoxSrcReduced
            // 
            pictureBoxSrcReduced.BackColor = SystemColors.ActiveCaptionText;
            pictureBoxSrcReduced.Location = new Point(265, 287);
            pictureBoxSrcReduced.Name = "pictureBoxSrcReduced";
            pictureBoxSrcReduced.Size = new Size(256, 136);
            pictureBoxSrcReduced.TabIndex = 13;
            pictureBoxSrcReduced.TabStop = false;
            toolTip1.SetToolTip(pictureBoxSrcReduced, "Source image reduced to 256 colors");
            // 
            // pictureBoxSrcData
            // 
            pictureBoxSrcData.Location = new Point(527, 287);
            pictureBoxSrcData.Name = "pictureBoxSrcData";
            pictureBoxSrcData.Size = new Size(256, 136);
            pictureBoxSrcData.TabIndex = 11;
            pictureBoxSrcData.TabStop = false;
            toolTip1.SetToolTip(pictureBoxSrcData, "Source image");
            // 
            // buttonXex
            // 
            buttonXex.Location = new Point(311, 281);
            buttonXex.Name = "buttonXex";
            buttonXex.Size = new Size(93, 70);
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
            checkBoxAutoscale.Location = new Point(93, 213);
            checkBoxAutoscale.Name = "checkBoxAutoscale";
            checkBoxAutoscale.Size = new Size(147, 19);
            checkBoxAutoscale.TabIndex = 20;
            checkBoxAutoscale.Text = "Autoscale (shrink only)";
            checkBoxAutoscale.UseVisualStyleBackColor = true;
            // 
            // checkBoxInterlace
            // 
            checkBoxInterlace.AutoSize = true;
            checkBoxInterlace.Checked = true;
            checkBoxInterlace.CheckState = CheckState.Checked;
            checkBoxInterlace.Location = new Point(311, 351);
            checkBoxInterlace.Name = "checkBoxInterlace";
            checkBoxInterlace.Size = new Size(71, 19);
            checkBoxInterlace.TabIndex = 21;
            checkBoxInterlace.Text = "Interlace";
            checkBoxInterlace.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoUpdate
            // 
            checkBoxAutoUpdate.AutoSize = true;
            checkBoxAutoUpdate.Location = new Point(146, 318);
            checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            checkBoxAutoUpdate.Size = new Size(89, 19);
            checkBoxAutoUpdate.TabIndex = 22;
            checkBoxAutoUpdate.Text = "Autoupdate";
            checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // buttonAlpaCentauriAI
            // 
            buttonAlpaCentauriAI.Enabled = false;
            buttonAlpaCentauriAI.Location = new Point(12, 384);
            buttonAlpaCentauriAI.Name = "buttonAlpaCentauriAI";
            buttonAlpaCentauriAI.Size = new Size(128, 29);
            buttonAlpaCentauriAI.TabIndex = 23;
            buttonAlpaCentauriAI.Text = "Alpa Centauri AI";
            buttonAlpaCentauriAI.UseVisualStyleBackColor = true;
            buttonAlpaCentauriAI.Click += buttonAlpaCentauriAI_Click;
            // 
            // labelDiff
            // 
            labelDiff.AutoSize = true;
            labelDiff.Location = new Point(145, 384);
            labelDiff.Name = "labelDiff";
            labelDiff.Size = new Size(26, 15);
            labelDiff.TabIndex = 24;
            labelDiff.Text = "Diff";
            // 
            // numericUpDownPopulation
            // 
            numericUpDownPopulation.Location = new Point(93, 427);
            numericUpDownPopulation.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDownPopulation.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            numericUpDownPopulation.Name = "numericUpDownPopulation";
            numericUpDownPopulation.Size = new Size(57, 23);
            numericUpDownPopulation.TabIndex = 25;
            numericUpDownPopulation.Value = new decimal(new int[] { 40, 0, 0, 0 });
            // 
            // numericUpDownGeneration
            // 
            numericUpDownGeneration.Location = new Point(93, 456);
            numericUpDownGeneration.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDownGeneration.Name = "numericUpDownGeneration";
            numericUpDownGeneration.Size = new Size(57, 23);
            numericUpDownGeneration.TabIndex = 26;
            numericUpDownGeneration.Value = new decimal(new int[] { 40, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 435);
            label3.Name = "label3";
            label3.Size = new Size(65, 15);
            label3.TabIndex = 27;
            label3.Text = "Population";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 458);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 28;
            label4.Text = "Generations";
            // 
            // listViewPopulation
            // 
            listViewPopulation.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            listViewPopulation.Location = new Point(167, 400);
            listViewPopulation.MultiSelect = false;
            listViewPopulation.Name = "listViewPopulation";
            listViewPopulation.Size = new Size(237, 164);
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
            buttonAlpaCentauriInit.Location = new Point(12, 351);
            buttonAlpaCentauriInit.Name = "buttonAlpaCentauriInit";
            buttonAlpaCentauriInit.Size = new Size(128, 29);
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
            checkBoxColorReduction.Location = new Point(167, 185);
            checkBoxColorReduction.Name = "checkBoxColorReduction";
            checkBoxColorReduction.Size = new Size(109, 19);
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
            comboBoxAverMethod.Location = new Point(167, 27);
            comboBoxAverMethod.Name = "comboBoxAverMethod";
            comboBoxAverMethod.Size = new Size(109, 23);
            comboBoxAverMethod.TabIndex = 32;
            comboBoxAverMethod.SelectedIndexChanged += ComboBoxAverMethod_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(167, 9);
            label5.Name = "label5";
            label5.Size = new Size(109, 15);
            label5.TabIndex = 33;
            label5.Text = "Averaging method:";
            // 
            // progressBarAI
            // 
            progressBarAI.Location = new Point(12, 506);
            progressBarAI.Name = "progressBarAI";
            progressBarAI.Size = new Size(138, 23);
            progressBarAI.Step = 1;
            progressBarAI.TabIndex = 34;
            // 
            // labelGenerationDone
            // 
            labelGenerationDone.AutoSize = true;
            labelGenerationDone.Location = new Point(67, 532);
            labelGenerationDone.Name = "labelGenerationDone";
            labelGenerationDone.Size = new Size(13, 15);
            labelGenerationDone.TabIndex = 35;
            labelGenerationDone.Text = "0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1210, 572);
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
            Name = "Form1";
            Text = "AlterLinePictureApproximator (ALPA) v0.6 by MatoSimi";
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
            ((System.ComponentModel.ISupportInitialize)numericUpDownPopulation).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneration).EndInit();
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
    }
}