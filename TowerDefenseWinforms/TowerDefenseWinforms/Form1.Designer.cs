namespace TowerDefenseWinforms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.lblGold = new System.Windows.Forms.Label();
            this.lblWave = new System.Windows.Forms.Label();
            this.lstTowers = new System.Windows.Forms.ListBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnNextWave = new System.Windows.Forms.Button();
            this.btnBuyNormal = new System.Windows.Forms.Button();
            this.btnBuyEpic = new System.Windows.Forms.Button();
            this.btnBuyUnique = new System.Windows.Forms.Button();
            this.btnBuyLegendary = new System.Windows.Forms.Button();
            this.btnUpgradeTower = new System.Windows.Forms.Button();
            this.btnSellTower = new System.Windows.Forms.Button();
            this.cmbRarityFilter = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSelectedTower = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblPlayerHP = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnBuyFarm = new System.Windows.Forms.Button();
            this.btnUpgradePersonal = new System.Windows.Forms.Button();
            this.btnSpeed = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblGold
            // 
            this.lblGold.AutoSize = true;
            this.lblGold.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblGold.Location = new System.Drawing.Point(25, 55);
            this.lblGold.Name = "lblGold";
            this.lblGold.Size = new System.Drawing.Size(87, 20);
            this.lblGold.TabIndex = 0;
            this.lblGold.Text = "Gold : 0";
            this.lblGold.Click += new System.EventHandler(this.lblGold_Click);
            // 
            // lblWave
            // 
            this.lblWave.AutoSize = true;
            this.lblWave.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWave.Location = new System.Drawing.Point(24, 22);
            this.lblWave.Name = "lblWave";
            this.lblWave.Size = new System.Drawing.Size(97, 20);
            this.lblWave.TabIndex = 1;
            this.lblWave.Text = "Wave : 0";
            // 
            // lstTowers
            // 
            this.lstTowers.Font = new System.Drawing.Font("굴림", 15F);
            this.lstTowers.FormattingEnabled = true;
            this.lstTowers.ItemHeight = 20;
            this.lstTowers.Location = new System.Drawing.Point(292, 519);
            this.lstTowers.Name = "lstTowers";
            this.lstTowers.Size = new System.Drawing.Size(538, 64);
            this.lstTowers.TabIndex = 2;
            this.lstTowers.SelectedIndexChanged += new System.EventHandler(this.lstTowers_SelectedIndexChanged);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLog.Location = new System.Drawing.Point(292, 396);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(538, 52);
            this.txtLog.TabIndex = 3;
            // 
            // btnNextWave
            // 
            this.btnNextWave.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNextWave.Location = new System.Drawing.Point(851, 471);
            this.btnNextWave.Name = "btnNextWave";
            this.btnNextWave.Size = new System.Drawing.Size(97, 32);
            this.btnNextWave.TabIndex = 4;
            this.btnNextWave.Text = "시작";
            this.btnNextWave.UseVisualStyleBackColor = true;
            this.btnNextWave.Click += new System.EventHandler(this.btnNextWave_Click);
            // 
            // btnBuyNormal
            // 
            this.btnBuyNormal.BackColor = System.Drawing.Color.Lime;
            this.btnBuyNormal.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBuyNormal.Location = new System.Drawing.Point(292, 464);
            this.btnBuyNormal.Name = "btnBuyNormal";
            this.btnBuyNormal.Size = new System.Drawing.Size(98, 49);
            this.btnBuyNormal.TabIndex = 5;
            this.btnBuyNormal.Text = " 일반 타워   (15골드)";
            this.btnBuyNormal.UseVisualStyleBackColor = false;
            this.btnBuyNormal.Click += new System.EventHandler(this.btnBuyNormal_Click);
            // 
            // btnBuyEpic
            // 
            this.btnBuyEpic.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnBuyEpic.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBuyEpic.Location = new System.Drawing.Point(396, 464);
            this.btnBuyEpic.Name = "btnBuyEpic";
            this.btnBuyEpic.Size = new System.Drawing.Size(102, 48);
            this.btnBuyEpic.TabIndex = 6;
            this.btnBuyEpic.Text = "  에픽 타워    (30골드)";
            this.btnBuyEpic.UseVisualStyleBackColor = false;
            this.btnBuyEpic.Click += new System.EventHandler(this.btnBuyEpic_Click);
            // 
            // btnBuyUnique
            // 
            this.btnBuyUnique.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnBuyUnique.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBuyUnique.Location = new System.Drawing.Point(504, 464);
            this.btnBuyUnique.Name = "btnBuyUnique";
            this.btnBuyUnique.Size = new System.Drawing.Size(102, 48);
            this.btnBuyUnique.TabIndex = 7;
            this.btnBuyUnique.Text = " 유니크 타워  (70골드)";
            this.btnBuyUnique.UseVisualStyleBackColor = false;
            this.btnBuyUnique.Click += new System.EventHandler(this.btnBuyUnique_Click);
            // 
            // btnBuyLegendary
            // 
            this.btnBuyLegendary.BackColor = System.Drawing.Color.Yellow;
            this.btnBuyLegendary.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBuyLegendary.Location = new System.Drawing.Point(612, 463);
            this.btnBuyLegendary.Name = "btnBuyLegendary";
            this.btnBuyLegendary.Size = new System.Drawing.Size(110, 49);
            this.btnBuyLegendary.TabIndex = 8;
            this.btnBuyLegendary.Text = " 레전더리 타워  (150골드)";
            this.btnBuyLegendary.UseVisualStyleBackColor = false;
            this.btnBuyLegendary.Click += new System.EventHandler(this.btnBuyLegendary_Click);
            // 
            // btnUpgradeTower
            // 
            this.btnUpgradeTower.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpgradeTower.Location = new System.Drawing.Point(12, 487);
            this.btnUpgradeTower.Name = "btnUpgradeTower";
            this.btnUpgradeTower.Size = new System.Drawing.Size(109, 40);
            this.btnUpgradeTower.TabIndex = 9;
            this.btnUpgradeTower.Text = "등급 강화";
            this.btnUpgradeTower.UseVisualStyleBackColor = true;
            this.btnUpgradeTower.Click += new System.EventHandler(this.btnUpgradeTower_Click);
            // 
            // btnSellTower
            // 
            this.btnSellTower.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSellTower.Location = new System.Drawing.Point(12, 541);
            this.btnSellTower.Name = "btnSellTower";
            this.btnSellTower.Size = new System.Drawing.Size(109, 42);
            this.btnSellTower.TabIndex = 10;
            this.btnSellTower.Text = "판매";
            this.btnSellTower.UseVisualStyleBackColor = true;
            this.btnSellTower.Click += new System.EventHandler(this.btnSellTower_Click);
            // 
            // cmbRarityFilter
            // 
            this.cmbRarityFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRarityFilter.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbRarityFilter.FormattingEnabled = true;
            this.cmbRarityFilter.Location = new System.Drawing.Point(17, 407);
            this.cmbRarityFilter.Name = "cmbRarityFilter";
            this.cmbRarityFilter.Size = new System.Drawing.Size(236, 21);
            this.cmbRarityFilter.TabIndex = 11;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSearch.Location = new System.Drawing.Point(16, 445);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(237, 22);
            this.txtSearch.TabIndex = 12;
            // 
            // lblSelectedTower
            // 
            this.lblSelectedTower.AutoSize = true;
            this.lblSelectedTower.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTower.Location = new System.Drawing.Point(849, 335);
            this.lblSelectedTower.Name = "lblSelectedTower";
            this.lblSelectedTower.Size = new System.Drawing.Size(99, 15);
            this.lblSelectedTower.TabIndex = 13;
            this.lblSelectedTower.Text = "선택된 타워 없음";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.Location = new System.Drawing.Point(852, 532);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(97, 32);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "종료";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblPlayerHP
            // 
            this.lblPlayerHP.AutoSize = true;
            this.lblPlayerHP.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPlayerHP.Location = new System.Drawing.Point(25, 92);
            this.lblPlayerHP.Name = "lblPlayerHP";
            this.lblPlayerHP.Size = new System.Drawing.Size(96, 21);
            this.lblPlayerHP.TabIndex = 15;
            this.lblPlayerHP.Text = "HP : 500";
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRestart.Location = new System.Drawing.Point(24, 245);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(97, 32);
            this.btnRestart.TabIndex = 16;
            this.btnRestart.Text = "재시작";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Visible = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnBuyFarm
            // 
            this.btnBuyFarm.BackColor = System.Drawing.Color.DarkOrange;
            this.btnBuyFarm.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBuyFarm.Location = new System.Drawing.Point(728, 463);
            this.btnBuyFarm.Name = "btnBuyFarm";
            this.btnBuyFarm.Size = new System.Drawing.Size(102, 49);
            this.btnBuyFarm.TabIndex = 17;
            this.btnBuyFarm.Text = "  농장 구매    (30골드)";
            this.btnBuyFarm.UseVisualStyleBackColor = false;
            this.btnBuyFarm.Click += new System.EventHandler(this.btnBuyFarm_Click);
            // 
            // btnUpgradePersonal
            // 
            this.btnUpgradePersonal.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpgradePersonal.Location = new System.Drawing.Point(144, 485);
            this.btnUpgradePersonal.Name = "btnUpgradePersonal";
            this.btnUpgradePersonal.Size = new System.Drawing.Size(109, 42);
            this.btnUpgradePersonal.TabIndex = 18;
            this.btnUpgradePersonal.Text = "개별 강화";
            this.btnUpgradePersonal.UseVisualStyleBackColor = true;
            this.btnUpgradePersonal.Click += new System.EventHandler(this.btnUpgradePersonal_Click_1);
            // 
            // btnSpeed
            // 
            this.btnSpeed.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSpeed.Location = new System.Drawing.Point(874, 19);
            this.btnSpeed.Name = "btnSpeed";
            this.btnSpeed.Size = new System.Drawing.Size(75, 23);
            this.btnSpeed.TabIndex = 19;
            this.btnSpeed.Text = "x1";
            this.btnSpeed.UseVisualStyleBackColor = true;
            this.btnSpeed.Click += new System.EventHandler(this.btnSpeed_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 604);
            this.Controls.Add(this.btnSpeed);
            this.Controls.Add(this.btnUpgradePersonal);
            this.Controls.Add(this.btnBuyFarm);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.lblPlayerHP);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblSelectedTower);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cmbRarityFilter);
            this.Controls.Add(this.btnSellTower);
            this.Controls.Add(this.btnUpgradeTower);
            this.Controls.Add(this.btnBuyLegendary);
            this.Controls.Add(this.btnBuyUnique);
            this.Controls.Add(this.btnBuyEpic);
            this.Controls.Add(this.btnBuyNormal);
            this.Controls.Add(this.btnNextWave);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lstTowers);
            this.Controls.Add(this.lblWave);
            this.Controls.Add(this.lblGold);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGold;
        private System.Windows.Forms.Label lblWave;
        private System.Windows.Forms.ListBox lstTowers;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnNextWave;
        private System.Windows.Forms.Button btnBuyNormal;
        private System.Windows.Forms.Button btnBuyEpic;
        private System.Windows.Forms.Button btnBuyUnique;
        private System.Windows.Forms.Button btnBuyLegendary;
        private System.Windows.Forms.Button btnUpgradeTower;
        private System.Windows.Forms.Button btnSellTower;
        private System.Windows.Forms.ComboBox cmbRarityFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSelectedTower;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblPlayerHP;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnBuyFarm;
        private System.Windows.Forms.Button btnUpgradePersonal;
        private System.Windows.Forms.Button btnSpeed;
    }
}
