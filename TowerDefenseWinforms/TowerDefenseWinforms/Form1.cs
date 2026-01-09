using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace TowerDefenseWinforms
{
    public partial class Form1 : Form
    {
        private int currentWave = 0;

        private const int START_GOLD = 100;
        private int gold = START_GOLD;

        private int playerHP = 500;
        private const int MAX_PLAYER_HP = 500;

        private List<Tower> ownedTowers = new List<Tower>();
        private Random random = new Random();

        private Dictionary<TowerRarity, int> rarityLevels = new Dictionary<TowerRarity, int>()
        {
            { TowerRarity.Normal, 0 },
            { TowerRarity.Epic, 0 },
            { TowerRarity.Unique, 0 },
            { TowerRarity.Legendary, 0 }
        };

        private Timer farmTimer = new Timer();
        private float farmAcc = 0f;

        // ===== Field(맵/전투) =====
        private DoubleBufferedPanel pnlField;
        private GameMap map;
        private Timer gameTimer = new Timer();
        private const float DT = 0.016f;

        private List<MonsterInstance> activeMonsters = new List<MonsterInstance>();
        private List<TowerInstance> placedTowers = new List<TowerInstance>();

        private bool waveRunning = false;
        private bool isStageRunning = false;

        // ===== 밸런스 상수 =====
        private const float TOWER_GLOBAL_DAMAGE_MULT = 1.35f;
        private const float PERSONAL_PER_LEVEL = 0.15f;
        private const float AOE_DPS_PENALTY = 0.85f;
        private const float FAST_MULTI_HIT_BONUS = 1.10f;
        private const float MONSTER_GLOBAL_HP_MULT = 0.75f;

        // Panel 생성(디자이너 없을 때)
        private void EnsureFieldPanel()
        {
            if (pnlField != null) return;

            pnlField = new DoubleBufferedPanel();
            pnlField.Name = "pnlField";
            pnlField.Left = 150;
            pnlField.Top = 10;
            pnlField.Width = 680;
            pnlField.Height = 384;
            pnlField.BorderStyle = BorderStyle.FixedSingle;
            pnlField.BackColor = Color.Black;

            this.Controls.Add(pnlField);
        }

        public Form1()
        {
            InitializeComponent();
            EnsureFieldPanel();

            this.Shown -= Form1_Shown;
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                // 맵/경로
                map = new GameMap(20, 12, 32);
                map.Build(MapPreset.RandomFromPresets, seed: Environment.TickCount);

                // 이벤트 연결
                pnlField.Paint -= pnlField_Paint;
                pnlField.Paint += pnlField_Paint;

                pnlField.MouseClick -= pnlField_MouseClick;
                pnlField.MouseClick += pnlField_MouseClick;

                // 게임 루프
                gameTimer.Interval = 16;
                gameTimer.Tick -= GameTimer_Tick;
                gameTimer.Tick += GameTimer_Tick;

                // 농장 타이머
                farmTimer.Interval = 250;
                farmTimer.Tick -= FarmTimer_Tick;
                farmTimer.Tick += FarmTimer_Tick;

                // 배속 모드
                btnSpeed.Click -= btnSpeed_Click;
                btnSpeed.Click += btnSpeed_Click;


                // 버튼 (null 방어 추천)
                if (btnNextWave != null)
                {
                    btnNextWave.Click -= btnNextWave_Click;
                    btnNextWave.Click += btnNextWave_Click;
                }
                if (btnRestart != null)
                {
                    btnRestart.Click -= btnRestart_Click;
                    btnRestart.Click += btnRestart_Click;
                    btnRestart.Visible = false;
                }
                if (btnExit != null)
                {
                    btnExit.Click -= btnExit_Click;
                    btnExit.Click += btnExit_Click;
                }

                // 필터
                if (cmbRarityFilter != null)
                {
                    cmbRarityFilter.Items.Clear();
                    cmbRarityFilter.Items.Add("전체");
                    cmbRarityFilter.Items.Add(TowerRarity.Normal);
                    cmbRarityFilter.Items.Add(TowerRarity.Epic);
                    cmbRarityFilter.Items.Add(TowerRarity.Unique);
                    cmbRarityFilter.Items.Add(TowerRarity.Legendary);
                    cmbRarityFilter.SelectedIndex = 0;

                    cmbRarityFilter.SelectedIndexChanged -= FilterChanged;
                    cmbRarityFilter.SelectedIndexChanged += FilterChanged;
                }
                if (txtSearch != null)
                {
                    txtSearch.TextChanged -= FilterChanged;
                    txtSearch.TextChanged += FilterChanged;
                }

                // 시작
                gameTimer.Start();
                farmTimer.Start();
                pnlField.Invalidate();

                UpdateUI();
                Log("=== 타워 디펜스 시작 ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            this.Enabled = true;
            this.Activate();
            this.BringToFront();
            this.Focus();
            
            this.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;

        }

        // 몬스터 스케일링
        private float GetHpScale(int wave)
        {
            int w = wave;
            float early = 1f + 0.15f * (w - 1);
            float mid = 1f + 0.025f * (w - 1) * (w - 1);

            float t = (w <= 8) ? 0f : Math.Min(1f, (w - 8) / 20f);
            return Lerp(early, mid, t);
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private float GetTierExtraScale(MonsterTier tier, int wave)
        {
            if (tier == MonsterTier.Boss)
            {
                float k = 0.70f + 0.015f * wave;
                if (k > 1.0f) k = 1.0f;
                return k;
            }
            return 1.0f;
        }

        private float GetEpicChance(int wave)
        {
            if (wave <= 3) return 0f;
            if (wave <= 9) return 0.10f;
            if (wave <= 19) return 0.15f;
            return 0.22f;
        }

        private List<Monster> GenerateWaveMonsters(int wave, int normalCount, bool addElite, bool bossOnly)
        {
            List<Monster> list = new List<Monster>();

            if (bossOnly)
            {
                MonsterKind bk = MonsterDB.RandomFrom(MonsterDB.BossPool, random);
                list.Add(new Monster(bk, MonsterDB.Defs[bk]));
                return list;
            }

            float epicChance = GetEpicChance(wave);

            for (int i = 0; i < normalCount; i++)
            {
                bool spawnEpic = (random.NextDouble() < epicChance);
                MonsterKind k = spawnEpic
                    ? MonsterDB.RandomFrom(MonsterDB.EpicPool, random)
                    : MonsterDB.RandomFrom(MonsterDB.NormalPool, random);

                list.Add(new Monster(k, MonsterDB.Defs[k]));
            }

            if (addElite)
            {
                MonsterKind ek = MonsterDB.RandomFrom(MonsterDB.ElitePool, random);
                list.Add(new Monster(ek, MonsterDB.Defs[ek]));
            }

            return list;
        }

        // 타워 DPS
        private float GetTowerDPS(Tower t)
        {
            TowerDefinition def = TowerDB.Defs[t.Kind];

            float dps = t.Attack * def.AttacksPerSecond;

            if (def.IsAoE)
                dps *= AOE_DPS_PENALTY;

            if (def.AttacksPerSecond >= 4f)
                dps *= FAST_MULTI_HIT_BONUS;

            if (def.BonusVsEliteBoss > 0f)
                dps *= def.BonusVsEliteBoss;

            return dps;
        }

        // 웨이브 시작(실제 스폰)
        private void StartNextWave()
        {
            if (playerHP <= 0) return;
            if (waveRunning) { Log("이미 웨이브 진행 중입니다."); return; }

            currentWave++;
            Log("=== Wave " + currentWave + " 시작 ===");

            bool isBoss = (currentWave % 10 == 0);
            bool isElite = (currentWave % 5 == 0 && !isBoss);

            int normalCount = isBoss ? 0 : 20;

            List<Monster> monsters = GenerateWaveMonsters(
                currentWave,
                normalCount,
                isElite,
                isBoss
            );

            Log("스폰 목록:");
            for (int i = 0; i < monsters.Count; i++)
            {
                MonsterDefinition def = monsters[i].Def;
                Log("- " + def.Name + " (속도 " + def.Speed + ", 특성: " + def.AbilityText + ")");
            }

            SpawnMonsters(monsters);

            waveRunning = true;
            isStageRunning = true;

            UpdateUI();
        }

        private void SpawnMonsters(List<Monster> monsters)
        {
            activeMonsters.Clear();

            float scale = GetHpScale(currentWave);
            float spawnGap = map.TileSize * 0.9f;

            for (int i = 0; i < monsters.Count; i++)
            {
                MonsterDefinition def = monsters[i].Def;

                float tierScale = GetTierExtraScale(def.Tier, currentWave);
                float maxHp = def.BaseHP * scale * tierScale * MONSTER_GLOBAL_HP_MULT;

                MonsterInstance inst = new MonsterInstance(def, maxHp);

                PointF p0 = map.GetPathWorld(0);
                PointF p1 = map.GetPathWorld(1);
                Vector2 dir = Vector2.From(p0, p1).Normalized();

                inst.Pos = new PointF(p0.X - dir.X * spawnGap * i, p0.Y - dir.Y * spawnGap * i);
                inst.PathIndex = 0;

                activeMonsters.Add(inst);
            }

            Log("스폰: " + activeMonsters.Count + "마리");
        }

        // 게임 루프
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (pnlField == null || map == null) return;

            if (!waveRunning)
            {
                pnlField.Invalidate();
                return;
            }
            if (playerHP <= 0)
            {
                pnlField.Invalidate();
                return;
            }

            float scaledDt = DT * timeScale;

            MoveMonsters(scaledDt);
            TowersAttack(scaledDt);

            if (activeMonsters.Count == 0)
            {
                waveRunning = false;
                isStageRunning = false;

                ApplyInterest();
                Log("Wave " + currentWave + " 종료!");

                UpdateUI();
            }

            pnlField.Invalidate();
        }

        private void MoveMonsters(float dt)
        {
            for (int i = activeMonsters.Count - 1; i >= 0; i--)
            {
                MonsterInstance m = activeMonsters[i];

                int nextIndex = m.PathIndex + 1;
                if (nextIndex >= map.PathCount)
                {
                    MonsterReachedEnd(m);
                    activeMonsters.RemoveAt(i);
                    continue;
                }

                PointF target = map.GetPathWorld(nextIndex);

                float speed = m.Def.Speed;
                float worldSpeed = speed * map.TileSize;

                Vector2 to = Vector2.From(m.Pos, target);
                float dist = to.Length();
                if (dist < 0.001f)
                {
                    m.PathIndex++;
                    continue;
                }

                float step = worldSpeed * dt;
                if (step >= dist)
                {
                    m.Pos = target;
                    m.PathIndex++;
                }
                else
                {
                    Vector2 dir = to / dist;
                    m.Pos = new PointF(m.Pos.X + dir.X * step, m.Pos.Y + dir.Y * step);
                }
            }
        }

        private void MonsterReachedEnd(MonsterInstance m)
        {
            gold += m.Def.GoldReward;

            int damage = (int)Math.Ceiling(m.HP);
            playerHP -= damage;
            if (playerHP < 0) playerHP = 0;

            Log("도착: " + m.Def.Name + " / 골드 +" + m.Def.GoldReward + " / 피해 " + damage + " (HP " + playerHP + ")");

            if (playerHP <= 0)
            {
                UpdateUI();
                GameOver();
            }
            else
            {
                UpdateUI();
            }
        }

        private void TowersAttack(float dt)
        {
            if (placedTowers.Count == 0) return;
            if (activeMonsters.Count == 0) return;

            for (int t = 0; t < placedTowers.Count; t++)
            {
                TowerInstance tower = placedTowers[t];
                tower.Cooldown -= dt;
                if (tower.Cooldown > 0f) continue;

                TowerDefinition def = TowerDB.Defs[tower.Tower.Kind];

                float aps = def.AttacksPerSecond;
                if (aps <= 0f) continue;

                tower.Cooldown = 1f / aps;

                if (def.IsFarm || tower.Tower.Attack <= 0) continue;

                float range = def.Range * map.TileSize;
                float range2 = range * range;

                List<MonsterInstance> inRange = new List<MonsterInstance>();
                for (int i = 0; i < activeMonsters.Count; i++)
                {
                    MonsterInstance m = activeMonsters[i];
                    if (Dist2(tower.Pos, m.Pos) <= range2)
                        inRange.Add(m);
                }

                if (inRange.Count == 0) continue;

                if (!def.IsAoE)
                {
                    MonsterInstance target = SelectFrontMost(inRange);
                    DealDamage(tower, target, 1, def);
                }
                else
                {
                    inRange = inRange
                        .OrderByDescending(m => m.PathIndex)
                        .Take(8)
                        .ToList();

                    int n = inRange.Count;
                    for (int i = 0; i < n; i++)
                        DealDamage(tower, inRange[i], n, def);
                }
            }
        }

        private MonsterInstance SelectFrontMost(List<MonsterInstance> list)
        {
            MonsterInstance best = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                MonsterInstance m = list[i];
                if (m.PathIndex > best.PathIndex) best = m;
                else if (m.PathIndex == best.PathIndex)
                {
                    int idx = Math.Min(m.PathIndex + 1, map.PathCount - 1);
                    PointF tgt = map.GetPathWorld(idx);
                    float d1 = Dist2(m.Pos, tgt);
                    float d2 = Dist2(best.Pos, tgt);
                    if (d1 < d2) best = m;
                }
            }
            return best;
        }

        private void DealDamage(TowerInstance tower, MonsterInstance m, int hitCount, TowerDefinition def)
        {
            float dmg = tower.Tower.Attack;

            if (def.IsAoE && hitCount > 1)
                dmg = dmg / hitCount;

            if (def.BonusVsEliteBoss > 0f && (m.Def.Tier == MonsterTier.Elite || m.Def.Tier == MonsterTier.Boss))
                dmg *= def.BonusVsEliteBoss;

            if (m.Def.Kind == MonsterKind.Boss_Shield)
            {
                if (def.Range >= 6f)
                    dmg *= 0.4f;
            }

            m.HP -= dmg;

            if (m.HP <= 0f)
            {
                gold += m.Def.GoldReward;
                Log("처치: " + m.Def.Name + " / 골드 +" + m.Def.GoldReward);

                activeMonsters.Remove(m);
                UpdateUI();
            }
        }

        // 설치(클릭)
        private void pnlField_MouseClick(object sender, MouseEventArgs e)
        {
            Point cell = map.WorldToCell(e.Location);

            if (!map.InBounds(cell)) return;

            if (map.IsPathCell(cell))
            {
                Log("경로 위에는 설치할 수 없습니다.");
                return;
            }

            for (int i = 0; i < placedTowers.Count; i++)
            {
                if (placedTowers[i].Cell == cell)
                {
                    Log("이미 타워가 설치된 칸입니다.");
                    return;
                }
            }

            if (displayedTowers.Count == 0 || lstTowers.SelectedIndex < 0)
            {
                Log("설치할 타워를 리스트에서 선택하세요.");
                return;
            }

            Tower selected = displayedTowers[lstTowers.SelectedIndex];

            if (IsTowerPlaced(selected))
            {
                Log("이 타워는 이미 설치되어 있습니다. (구매한 타워 1개당 1회 설치)");
                return;
            }

            TowerInstance inst = new TowerInstance(selected, cell, map.CellToWorldCenter(cell));
            placedTowers.Add(inst);

            Log("📌 설치: " + TowerDB.Defs[selected.Kind].Name + " @ (" + cell.X + "," + cell.Y + ")");
            pnlField.Invalidate();
        }

        // 그리기
        private void pnlField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (map != null)
                map.Draw(g);

            for (int i = 0; i < placedTowers.Count; i++)
                placedTowers[i].Draw(g, map.TileSize);

            for (int i = 0; i < activeMonsters.Count; i++)
                activeMonsters[i].Draw(g, map.TileSize);

            if (map != null)
            {
                PointF s = map.CellToWorldCenter(map.StartCell);
                PointF epos = map.CellToWorldCenter(map.EndCell);
                float r = map.TileSize * 0.25f;

                Brush bs = new SolidBrush(Color.Lime);
                Brush be = new SolidBrush(Color.Red);

                g.FillEllipse(bs, s.X - r, s.Y - r, r * 2, r * 2);
                g.FillEllipse(be, epos.X - r, epos.Y - r, r * 2, r * 2);

                bs.Dispose();
                be.Dispose();
            }
        }

        private float Dist2(PointF a, PointF b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        // UI/로그/필터
        private void lstTowers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTowers.SelectedIndex < 0 || displayedTowers.Count == 0)
            {
                lblSelectedTower.Text = "선택된 타워 없음";
                return;
            }

            Tower t = displayedTowers[lstTowers.SelectedIndex];
            TowerDefinition def = TowerDB.Defs[t.Kind];

            lblSelectedTower.Text =
                "▶ " + def.Name + "\n" +
                "- 등급: " + t.Rarity + "\n" +
                "- 공격력: " + t.Attack + "\n" +
                "- DPS : " + (int)GetTowerDPS(t) + "\n" +
                "- 등급강화: " + t.RarityLevel + "강\n" +
                "- 개별강화: " + t.PersonalLevel + "강\n" +
                "- 공격속도: " + def.AttacksPerSecond + "/초\n" +
                "- 사거리: " + def.Range;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // HUD
            lblGold.Text = "Gold : " + gold;
            lblWave.Text = "Wave : " + currentWave;
            lblPlayerHP.Text = "HP : " + playerHP;

            int prevTopIndex = 0;
            if (lstTowers.Items.Count > 0) prevTopIndex = lstTowers.TopIndex;

            // 선택 저장
            Tower prevSelected = null;
            if (displayedTowers != null && lstTowers.SelectedIndex >= 0 && lstTowers.SelectedIndex < displayedTowers.Count)
                prevSelected = displayedTowers[lstTowers.SelectedIndex];

            lstTowers.BeginUpdate();
            try
            {
                lstTowers.Items.Clear();
                displayedTowers.Clear();

                if (ownedTowers.Count == 0)
                {
                    lstTowers.Items.Add("보유 타워 없음");
                    return;
                }

                IEnumerable<Tower> filtered = ownedTowers;

                if (cmbRarityFilter != null && cmbRarityFilter.SelectedIndex > 0)
                {
                    TowerRarity selectedRarity = (TowerRarity)cmbRarityFilter.SelectedItem;
                    filtered = filtered.Where(t => t.Rarity == selectedRarity);
                }

                string keyword = (txtSearch != null) ? txtSearch.Text.Trim() : "";
                if (keyword.Length > 0)
                    filtered = filtered.Where(t => TowerDB.Defs[t.Kind].Name.Contains(keyword));

                displayedTowers = filtered.ToList();

                if (displayedTowers.Count == 0)
                {
                    lstTowers.Items.Add("검색 결과 없음");
                    return;
                }

                for (int i = 0; i < displayedTowers.Count; i++)
                {
                    Tower t = displayedTowers[i];
                    TowerDefinition def = TowerDB.Defs[t.Kind];

                    lstTowers.Items.Add(
                        (i + 1) + ". " + def.Name +
                        " [" + t.Rarity + "] " +
                        "R" + t.RarityLevel + "/P" + t.PersonalLevel +
                        " ATK " + t.Attack
                    );
                }

                // 선택 복원
                if (prevSelected != null)
                {
                    int idx = displayedTowers.FindIndex(t => object.ReferenceEquals(t, prevSelected));
                    if (idx >= 0) lstTowers.SelectedIndex = idx;
                }

                if (lstTowers.Items.Count > 0)
                {
                    int maxTop = Math.Max(0, lstTowers.Items.Count - 1);
                    lstTowers.TopIndex = Math.Min(prevTopIndex, maxTop);
                }
            }
            finally
            {
                lstTowers.EndUpdate();
            }
        }

        private void Log(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
        }

        // 공격력/강화
        private int GetBaseAtk(TowerRarity rarity, int level)
        {
            float scale = 0.45f;

            float baseAtk = 0f;
            float per = 0f;

            if (rarity == TowerRarity.Normal) { baseAtk = 40f; per = 14f; }
            else if (rarity == TowerRarity.Epic) { baseAtk = 60f; per = 16f; }
            else if (rarity == TowerRarity.Unique) { baseAtk = 80f; per = 18f; }
            else if (rarity == TowerRarity.Legendary) { baseAtk = 100f; per = 20f; }

            float scaled = (baseAtk + per * level) * scale;

            int result = (int)Math.Round(scaled);
            if (result < 1) result = 1;
            return result;
        }

        private int CalcAttack(Tower tower)
        {
            TowerDefinition def = TowerDB.Defs[tower.Kind];

            float baseAtk = GetBaseAtk(tower.Rarity, tower.RarityLevel);
            float personalMult = 1f + PERSONAL_PER_LEVEL * tower.PersonalLevel;
            float scaled = baseAtk * def.DamageScale * personalMult * TOWER_GLOBAL_DAMAGE_MULT;

            int atk = (int)Math.Round(scaled);
            if (atk < 1) atk = 1;
            return atk;
        }

        private int GetRarityUpgradeCost(int currentLevel)
        {
            return 10 + currentLevel * 8;
        }

        private int GetPersonalUpgradeCost(Tower tower)
        {
            int baseCost = 10;
            if (tower.Rarity == TowerRarity.Epic) baseCost = 14;
            else if (tower.Rarity == TowerRarity.Unique) baseCost = 18;
            else if (tower.Rarity == TowerRarity.Legendary) baseCost = 22;

            return baseCost + tower.PersonalLevel * 8;
        }

        private void UpgradeRarityFromSelection()
        {
            if (displayedTowers.Count == 0 || lstTowers.SelectedIndex < 0)
            {
                Log("등급 강화를 하려면 타워를 선택하세요.");
                return;
            }

            Tower selected = displayedTowers[lstTowers.SelectedIndex];
            TowerRarity rarity = selected.Rarity;

            int cur = rarityLevels[rarity];
            if (cur >= 10)
            {
                Log("이미 10강입니다.");
                return;
            }

            int cost = GetRarityUpgradeCost(cur);
            if (gold < cost)
            {
                Log("골드 부족! (필요: " + cost + ", 보유: " + gold + ")");
                return;
            }

            gold -= cost;
            rarityLevels[rarity] = cur + 1;

            foreach (Tower t in ownedTowers)
            {
                if (t.Rarity != rarity) continue;
                t.RarityLevel = rarityLevels[rarity];
                t.Attack = CalcAttack(t);
            }

            Log("[" + rarity + "] 등급 강화! → " + rarityLevels[rarity] + "강");
            UpdateUI();
        }

        private void UpgradeSelectedPersonal()
        {
            if (displayedTowers.Count == 0 || lstTowers.SelectedIndex < 0)
            {
                Log("개별 강화를 하려면 타워를 선택하세요.");
                return;
            }

            Tower t = displayedTowers[lstTowers.SelectedIndex];

            if (t.PersonalLevel >= 5)
            {
                Log("이미 개인 강화 5강입니다.");
                return;
            }

            int cost = GetPersonalUpgradeCost(t);
            if (gold < cost)
            {
                Log("골드 부족! (필요: " + cost + ", 보유: " + gold + ")");
                return;
            }

            gold -= cost;
            t.PersonalLevel++;
            t.InvestedGold += cost;
            t.Attack = CalcAttack(t);

            Log("개별 강화 성공! P" + t.PersonalLevel + " (ATK " + t.Attack + ")");
            UpdateUI();
        }

        private void SellSelectedTower()
        {
            if (displayedTowers.Count == 0 || lstTowers.SelectedIndex < 0)
            {
                Log("판매할 타워를 선택하세요.");
                return;
            }

            Tower t = displayedTowers[lstTowers.SelectedIndex];

            int removedOnField = placedTowers.RemoveAll(pt => object.ReferenceEquals(pt.Tower, t));
            if (removedOnField > 0 && pnlField != null)
                pnlField.Invalidate();

            int refund = (int)Math.Floor(t.InvestedGold * 0.4);
            gold += refund;

            TowerDefinition def = TowerDB.Defs[t.Kind];
            Log(def.Name + " 판매! 환불 +" + refund + (removedOnField > 0 ? " (설치된 타워도 제거됨)" : ""));

            ownedTowers.Remove(t);
            UpdateUI();
        }

        // 골드/이자/농장
        private void ApplyInterest()
        {
            if (gold >= 10)
            {
                int interest = (int)(gold * 0.1);
                if (interest > 5) interest = 5;
                gold += interest;
                Log("[이자] +" + interest);
            }
        }

        private void FarmTimer_Tick(object sender, EventArgs e)
        {
            if (!isStageRunning) return;

            float dt = 0.25f * timeScale;
            float gps = 0f;


            foreach (Tower t in ownedTowers)
            {
                TowerDefinition def = TowerDB.Defs[t.Kind];
                if (!def.IsFarm) continue;

                gps += def.FarmBase + def.FarmPerLevel * t.PersonalLevel;
            }

            farmAcc += gps * dt;

            int gain = (int)Math.Floor(farmAcc);
            if (gain > 0)
            {
                farmAcc -= gain;
                gold += gain;
                UpdateUI();
            }
        }

        private TowerKind RandomKindByRarity(TowerRarity rarity)
        {
            List<TowerKind> list = new List<TowerKind>();

            foreach (var kv in TowerDB.Defs)
            {
                TowerDefinition def = kv.Value;
                if (def.Rarity != rarity) continue;
                if (def.IsFarm) continue;
                list.Add(kv.Key);
            }

            return list[random.Next(list.Count)];
        }

        private void TryBuyTowerByRarity(TowerRarity rarity)
        {
            TowerKind kind = RandomKindByRarity(rarity);
            TowerDefinition def = TowerDB.Defs[kind];

            int price = TowerDB.ShopPrice[rarity];
            if (gold < price)
            {
                Log("골드 부족! (필요: " + price + ", 보유: " + gold + ")");
                return;
            }

            gold -= price;

            int rarityLevel = rarityLevels[rarity];
            Tower tower = new Tower(kind, rarity, rarityLevel, 0, price);
            tower.Attack = CalcAttack(tower);

            ownedTowers.Add(tower);

            Log(def.Name + " 구매! (" + rarity + " R" + rarityLevel + " / P0, ATK " + tower.Attack + ")");
            UpdateUI();
        }

        private void TryBuyFarmTower()
        {
            TowerKind farmKind = TowerDB.Defs.First(kv => kv.Value.IsFarm).Key;
            TowerDefinition def = TowerDB.Defs[farmKind];

            int farmCount = ownedTowers.Count(t => TowerDB.Defs[t.Kind].IsFarm);
            if (farmCount >= def.FarmMaxCount)
            {
                Log("농장 타워는 최대 3개까지만 설치할 수 있습니다.");
                return;
            }

            int price = TowerDB.ShopPrice[TowerRarity.Epic];
            if (gold < price)
            {
                Log("골드 부족! (필요 : " + price + ", 보유 : " + gold + ")");
                return;
            }

            gold -= price;

            int rarityLevel = rarityLevels[TowerRarity.Epic];
            Tower tower = new Tower(farmKind, TowerRarity.Epic, rarityLevel, 0, price);
            tower.Attack = 0;

            ownedTowers.Add(tower);

            Log("농장 타워 구매! (골드 생산 전용)");
            UpdateUI();
        }

        // 종료/패배/재시작
        private void GameOver()
        {
            Log("패배!");
            MessageBox.Show("패배했습니다.", "게임 오버", MessageBoxButtons.OK, MessageBoxIcon.Error);

            isStageRunning = false;
            waveRunning = false;
            farmAcc = 0f;

            btnRestart.Visible = true;
            btnRestart.Enabled = true;
            btnRestart.BringToFront();
            btnRestart.Focus();

            btnNextWave.Enabled = false;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Log("[디버그] 재시작");
            RestartGame();
        }

        private void RestartGame()
        {
            isStageRunning = false;
            waveRunning = false;
            farmAcc = 0f;

            currentWave = 0;
            gold = START_GOLD;
            playerHP = MAX_PLAYER_HP;

            ownedTowers.Clear();
            displayedTowers.Clear();

            placedTowers.Clear();
            activeMonsters.Clear();

            rarityLevels[TowerRarity.Normal] = 0;
            rarityLevels[TowerRarity.Epic] = 0;
            rarityLevels[TowerRarity.Unique] = 0;
            rarityLevels[TowerRarity.Legendary] = 0;

            if (cmbRarityFilter != null) cmbRarityFilter.SelectedIndex = 0;
            if (txtSearch != null) txtSearch.Text = "";

            txtLog.Clear();
            lblSelectedTower.Text = "선택된 타워 없음";

            btnRestart.Visible = false;
            btnRestart.Enabled = true;

            btnNextWave.Enabled = true;
            btnNextWave.Focus();

            // 맵 다시 생성
            map.Build(MapPreset.RandomFromPresets, seed: Environment.TickCount);

            UpdateUI();
            if (pnlField != null) pnlField.Invalidate();

            Log("=== 게임 재시작 ===");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "정말 게임을 종료하시겠습니까?",
                "게임 종료",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
                Application.Exit();
        }

        // 입력/버튼 핸들러
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void btnNextWave_Click(object sender, EventArgs e)
        {
            StartNextWave();
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            MessageBox.Show("Form EnabledChanged: " + this.Enabled);
        }

        private void lblGold_Click(object sender, EventArgs e) { }
        private void btnBuyNormal_Click(object sender, EventArgs e) { TryBuyTowerByRarity(TowerRarity.Normal); }
        private void btnBuyEpic_Click(object sender, EventArgs e) { TryBuyTowerByRarity(TowerRarity.Epic); }
        private void btnBuyUnique_Click(object sender, EventArgs e) { TryBuyTowerByRarity(TowerRarity.Unique); }
        private void btnBuyLegendary_Click(object sender, EventArgs e) { TryBuyTowerByRarity(TowerRarity.Legendary); }
        private void btnUpgradeTower_Click(object sender, EventArgs e) { UpgradeRarityFromSelection(); }
        private void btnSellTower_Click(object sender, EventArgs e) { SellSelectedTower(); }
        private void btnUpgradePersonal_Click(object sender, EventArgs e) { UpgradeSelectedPersonal(); }
        
        private float timeScale = 1f;
        
        private void btnUpgradeRarity_Click(object sender, EventArgs e)
        {
            UpgradeRarityFromSelection();
        }

        private bool IsTowerPlaced(Tower tower)
        {
            return placedTowers.Any(pt => object.ReferenceEquals(pt.Tower, tower));
        }

        private void btnBuyFarm_Click(object sender, EventArgs e)
        {
            TryBuyFarmTower();
        }

        private void btnUpgradePersonal_Click_1(object sender, EventArgs e)
        {
            UpgradeSelectedPersonal();
        }

        private void button1_Click(object sender, EventArgs e) { }

        private List<Tower> displayedTowers = new List<Tower>();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSpeed_Click(object sender, EventArgs e)
        {
            if (timeScale < 1.5f)
            {
                timeScale = 2f;
                btnSpeed.Text = "x2";
                Log("[속도] 2배속");
            }
            else
            {
                timeScale = 1f;
                btnSpeed.Text = "x1";
                Log("[속도] 1배속");
            }
        }
    }
}
